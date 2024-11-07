
using System;

namespace webapp_sub1.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }  // Property to hold the ID of the request that caused the error

        // Computed property to check if RequestId has a value
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
