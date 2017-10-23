
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using PureClarity.Models;

namespace PureClarity.Models
{
    public class ProcessedUser
    {

        /// <summary>
        /// Unique user id. Must be unique across all users
        /// </summary>
        public string UserId;

        public string Email;
        public string FirstName;
        public string LastName;
        public string Salutation;
        public string DOB;
        public string Gender;
        public string City;
        public string State;
        public string Country;

        /// <summary>
        /// Optional. Custom attributes for use in segmentation and rules
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> CustomFields { get; set; }
    }
}