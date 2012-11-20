using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{

    public class ApiMessage
    {
        public string Message { get; set; }

        public bool IsCallbackError { get; set; }

        public List<string> Errors { get; set; }

        public ApiMessage() : this(String.Empty)
        {
        }

        public ApiMessage(string message)
        {
            IsCallbackError = true;

            Errors = new List<string>();

            Message = message;
        }
    }
}