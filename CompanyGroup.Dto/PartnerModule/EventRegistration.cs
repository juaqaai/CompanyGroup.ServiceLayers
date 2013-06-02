using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// eseményregisztráció
    /// { 
    ///     EventId: '123', 
    ///     EventName: 'event 0012', 
    ///     Data: {
    ///         PersonName: '', 
    ///         CompanyName: '', 
    ///         Email: '', 
    ///         Phone: ''
    ///     }
    /// }
    /// </summary>
    public class EventRegistration
    {
        public EventRegistration() : this(String.Empty, String.Empty, new Dictionary<string, string>()) { }

        public EventRegistration(string eventId, string eventName, Dictionary<string, string> data)
        {
            this.EventId = eventId;

            this.EventName = eventName;

            this.Data = data;
        }

        public string EventId { get; set; }

        public string EventName { get; set; }

        public Dictionary<string, string> Data { get; set; }
    }
}
