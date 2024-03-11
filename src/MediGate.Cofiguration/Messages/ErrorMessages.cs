using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediGate.Cofiguration.Messages
{
    public static class ErrorMessages
    {
        public static class Generic
        {
            public static string ObjectNotFound = "Object Not Found";
            public static string InvalidRequest = "Invalid Request";
            public static string TypeBadRequest = "Bad Request";
            public static string InvalidPayload = "Invalid Payload";
            public static string UnableToProcess = "Unable to process request";
            public static string SomethingWentWrong = "Something went wrong, Please try again later!";
        }

        public static class Profile
        {
            public static string UserNotFound = "Profile not found";

        }

        public static class Users
        {
            public static string UserNotFound = "User not found";

        }
    }
}