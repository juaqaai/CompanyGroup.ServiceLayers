using System;
using System.Linq;
using System.Collections.Generic;

namespace CompanyGroup.WebApi.Models
{

    public class ApiMessage
    {
        public string Message { get; set; }

        public bool IsCallbackError { get; set; }

        public List<string> Errors { get; set; }

        public ApiMessage() : this(String.Empty) {}

        public ApiMessage(string message)
        {
            this.IsCallbackError = false;

            this.Errors = new List<string>();

            this.Message = message;
        }

        public ApiMessage(System.Web.Http.ModelBinding.ModelStateDictionary modelState)
        {
            this.IsCallbackError = true;

            this.Message = "Model is invalid.";

            foreach (KeyValuePair<string, System.Web.Http.ModelBinding.ModelState> modelItem in modelState)
            {
                System.Web.Http.ModelBinding.ModelError modelError = modelItem.Value.Errors.FirstOrDefault();

                if (!string.IsNullOrEmpty(modelError.ErrorMessage))
                {
                    this.Errors.Add(modelItem.Key + ": " + ParseModelStateErrorMessage(modelError.ErrorMessage));
                }
                else
                {
                    this.Errors.Add(modelItem.Key + ": " + ParseModelStateErrorMessage(modelError.Exception.Message));
                }
            }
        }

        private string ParseModelStateErrorMessage(string msg)
        {
            int period = msg.IndexOf('.');

            if (period < 0 || period > msg.Length - 1)
                return msg;

            return msg.Substring(0, period);
        }
    }
}