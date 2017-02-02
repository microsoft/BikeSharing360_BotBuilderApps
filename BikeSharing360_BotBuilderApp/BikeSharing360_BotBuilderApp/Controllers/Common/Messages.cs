using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeSharing360_BotBuilderApp.Common
{
    public class Messages
    {
        public const string CustomerAskTime = "Hi {0}, sorry to hear that! I am here to help you, how long ago did this happen?";
        public const string CustomerHangon = "OK, let me see if I can figure out where your bike went…hang on.";
        public const string CustomerReplyLocation = "{0} you were at {1} and I now see your bike currently is at {2}: ";
        public const string CustomerAskLocation = "Where exactly are you right now? ";
        public const string CustomerReportPolice = "OK, got it. Let me report this to the police on your behalf and have a customer service representative bring you a new bike. Hang tight!";
        public const string CustomerReplyCaseNumber = "Your help desk incident number is {0}";
        public const string CustomerReplyETA = "Hi {0}, a customer service representative is on their way!  ETA is {1} minutes… ";
        public const string CustomerReplyWithoutETA = "Hi {0}, a customer service representative is on their way!";

        public const string EmployeeMsg1 = "{0}, I see that you are currently available to dispatch a bike to a customer who had their bike stolen. Here is the customer information and where the bike needs to be delivered: ";
        public const string EmployeeMsg2 = "DELIVERY ADDRESS: **{0} ({1})** ";
        public const string EmployeeMsg3 = "Please tell me when you are on your way so I can update the customer.";

        public const string CustomerDefault = "Sorry, I did not understand '{0}'. What can I do for you?";

        public const string CannotFindUser = "Cannot find user";
        public const string CannotFindBike = "Cannot find bike";
        public const string CannotFindEmployee = "Cannot find employee";
        public const string NoValidTime = "Time is invalid";
    }
}