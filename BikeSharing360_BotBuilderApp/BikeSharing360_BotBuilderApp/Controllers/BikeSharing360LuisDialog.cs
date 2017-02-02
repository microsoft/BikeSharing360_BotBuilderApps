using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using BikeSharing360_BotBuilderApp.Common;

namespace BikeSharing360_BotBuilderApp
{
    [Serializable]
    [LuisModel("_YourModelId_", "_YourSubscriptionKey_")]
    public class BikeSharing360LuisDialog : LuisDialog<object>
    {
        private const string EntityDateTime = "builtin.datetime.time";
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            try
            {
                string state;
                bool found = context.UserData.TryGetValue("state", out state);
                if (found && state == "reportloss")
                {
                    EntityRecommendation datetimeEntityRecommendation;
                    if (result.TryFindEntity(EntityDateTime, out datetimeEntityRecommendation))
                    {
                        string bikeid;
                        found = context.UserData.TryGetValue("bikeid", out bikeid);
                        if (!found)
                        {
                            await context.PostAsync(Messages.CannotFindBike);
                            context.Wait(this.MessageReceived);
                            return;
                        }
                        context.UserData.SetValue("state", "reporttime");
                        string message = Messages.CustomerHangon;
                        await context.PostAsync(message);

                        DateTime time;
                        var succeed = DateTime.TryParse(datetimeEntityRecommendation.Resolution["time"], out time);
                        if (!succeed)
                        {
                            await context.PostAsync(Messages.NoValidTime);
                            context.Wait(this.MessageReceived);
                            return;
                        }
                        var oldLoc = await Backend.Bike.LocateBike(bikeid, time);
                        var currentLoc = await Backend.Bike.LocateBike(bikeid);
                        context.UserData.SetValue("lat", oldLoc.latitude);
                        context.UserData.SetValue("lon", oldLoc.longitude);
                        context.UserData.SetValue("addr", oldLoc.name);
                        message = string.Format(Messages.CustomerReplyLocation, result.Query.ToLower(), oldLoc.name, currentLoc.name);
                        var richmessage = context.MakeMessage();
                        richmessage.Text = message;
                        var mapurl = await Backend.BingMapHelper.StaticMapWithRoute(oldLoc, currentLoc);
                        if (mapurl != "")
                        {
                            var heroCard = new HeroCard
                            {
                                Images = new List<CardImage> { new CardImage(mapurl) }
                            };
                            richmessage.Attachments.Add(heroCard.ToAttachment());
                        }
                        await context.PostAsync(richmessage);
                        message = Messages.CustomerAskLocation;
                        await context.PostAsync(message);
                        context.Wait(this.MessageReceived);
                        return;
                    }
                    else
                    {
                        context.Wait(this.MessageReceived);
                        return;
                    }
                }
                else
                {
                    string message = string.Format(Messages.CustomerDefault, result.Query);

                    await context.PostAsync(message);

                    context.Wait(this.MessageReceived);
                }
            }
            catch (Exception)
            {
                context.Wait(this.MessageReceived);
            }
        }

        [LuisIntent("reportloss")]
        public async Task ReportLoss(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            try
            {
                string name = "";
                string id = "";
                var message = await activity;

                var from = message.From.Id;
                Backend.User user = await Backend.User.LookupUser(from, Backend.ConnectorType.Skype);
                if (user == null)
                {
                    await context.PostAsync(Messages.CannotFindUser);
                    context.Wait(this.MessageReceived);
                    return;
                }
                name = message.From.Name;
                id = user.userId;

                //look up bikes
                var bikes = await Backend.Bike.LookupBikesWithUser(id);
                if (bikes != null && bikes.Count == 1)
                    context.UserData.SetValue("bikeid", bikes[0].bikeid);
                else
                {
                    await context.PostAsync(Messages.CannotFindBike);
                    context.Wait(this.MessageReceived);
                    return;
                }

                context.UserData.SetValue("id", id);
                context.UserData.SetValue("state", "reportloss");
                string reply = string.Format(Messages.CustomerAskTime, name);
                await context.PostAsync(reply);
                context.Wait(this.MessageReceived);
            }
            catch (Exception)
            {
                context.Wait(this.MessageReceived);
            }
        }

        [LuisIntent("confirmlocation")]
        public async Task ConfirmLocation(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            try
            {
                string state = "";
                if (context.UserData.TryGetValue("state", out state) && state == "reporttime")
                {
                    var message = await activity;
                    bool found = false;
                    double lat, lon;
                    string addr;
                    string id = "";
                    found = context.UserData.TryGetValue("lat", out lat);
                    if (!found)
                    {
                        context.Wait(this.MessageReceived);
                        return;
                    }
                    found = context.UserData.TryGetValue("lon", out lon);
                    if (!found)
                    {
                        context.Wait(this.MessageReceived);
                        return;
                    }
                    found = context.UserData.TryGetValue("addr", out addr);
                    if (!found)
                    {
                        context.Wait(this.MessageReceived);
                        return;
                    }
                    found = context.UserData.TryGetValue("id", out id);
                    if (!found)
                    {
                        context.Wait(this.MessageReceived);
                        return;
                    }
                    context.UserData.SetValue("state", "");
                    string msg = Messages.CustomerReportPolice;
                    await context.PostAsync(msg);

                    string ticketnumber = await Backend.CustomerService.FileCase(id, Backend.IncidentType.lost, lat, lon);
                    msg = string.Format(Messages.CustomerReplyCaseNumber, ticketnumber);
                    await context.PostAsync(msg);

                    //send message to employee
                    var employee = await Backend.CustomerService.GetAvailableEmployee();
                    BotUser user = new BotUser();
                    user.id = message.From.Id;
                    user.name = message.From.Name;
                    user.serviceUrl = message.ServiceUrl;
                    user.conversationId = message.Conversation.Id;
                    user.location = new GeoLocation();
                    user.location.latitude = lat;
                    user.location.longitude = lon;
                    user.location.name = addr;
                    employee.customer = user;
                    await Backend.CustomerService.SaveInformation(employee);
                    var userAccount = new ChannelAccount(name: employee.name, id: employee.id);
                    var connector = new ConnectorClient(new Uri(employee.serviceUrl));
                    IMessageActivity mm = Activity.CreateMessageActivity();
                    mm.From = message.Recipient;
                    mm.Recipient = userAccount;
                    mm.Text = string.Format(Messages.EmployeeMsg1, employee.name);
                    mm.Locale = "en-Us";
                    await connector.Conversations.SendToConversationAsync((Activity)mm, employee.conversationId);
                    mm.Text = "NAME: **" + user.name + "**";
                    await connector.Conversations.SendToConversationAsync((Activity)mm, employee.conversationId);
                    mm.Text = "INCIDENT NUMBER: **" + ticketnumber + "**";
                    await connector.Conversations.SendToConversationAsync((Activity)mm, employee.conversationId);
                    var deliveraddr = await Backend.BingMapHelper.PointToAddress(lat, lon);
                    mm.Text = string.Format(Messages.EmployeeMsg2, addr, deliveraddr);
                    var mapurl = await Backend.BingMapHelper.StaticMapWith1Pin(lat, lon, addr);
                    if (mapurl != "")
                    {
                        var heroCard = new HeroCard
                        {
                            Images = new List<CardImage> { new CardImage(mapurl) }
                        };
                        mm.Attachments = new List<Attachment>();
                        mm.Attachments.Add(heroCard.ToAttachment());
                    }
                    await connector.Conversations.SendToConversationAsync((Activity)mm, employee.conversationId);
                    mm.Text = Messages.EmployeeMsg3;
                    mm.Attachments = null;
                    await connector.Conversations.SendToConversationAsync((Activity)mm, employee.conversationId);
                    context.Wait(this.MessageReceived);
                }
                else
                {
                    string message = string.Format(Messages.CustomerDefault, result.Query);

                    await context.PostAsync(message);

                    context.Wait(this.MessageReceived);
                    return;
                }
            }
            catch (Exception)
            {
                context.Wait(this.MessageReceived);
            }
        }

        [LuisIntent("confirmontheway")]
        public async Task ConfirmOnTheWay(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            try
            {
                var message = await activity;
                Employee employee = null;
                BotUser user;
                string id = message.From.Id;
                employee = await Backend.CustomerService.LoadInformation(id);
                if (employee == null)
                {
                    var msg = "Cannot find the employee";
                    await context.PostAsync(msg);
                    context.Wait(this.MessageReceived);
                    return;
                }
                user = employee.customer;
                var userAccount = new ChannelAccount(name: user.name, id: user.id);
                var connector = new ConnectorClient(new Uri(user.serviceUrl));
                IMessageActivity mm = Activity.CreateMessageActivity();
                mm.From = message.Recipient;
                mm.Recipient = userAccount;
                int eta = await Backend.BingMapHelper.GetETA(employee.location.latitude, employee.location.longitude,
                    employee.customer.location.latitude, employee.customer.location.longitude);
                if (eta > 0)
                    mm.Text = string.Format(Messages.CustomerReplyETA, user.name, (eta / 60).ToString());
                else
                    mm.Text = string.Format(Messages.CustomerReplyWithoutETA, user.name);
                mm.Locale = "en-Us";
                await connector.Conversations.SendToConversationAsync((Activity)mm, user.conversationId);
                context.Wait(this.MessageReceived);
            }
            catch (Exception)
            {
                context.Wait(this.MessageReceived);
            }
        }
    }
}
