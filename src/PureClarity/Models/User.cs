
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using PureClarity.Models;

namespace PureClarity.Models
{
    public class User: PCModelBase
    {
        
        /// <summary>
        /// Unique user id. Must be unique across all users
        /// </summary>
        public string UserId { get => Id; set => Id = value; }

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

        public User(string id)
        {
            UserId = id ?? throw new ArgumentNullException(nameof(id));
            CustomFields = new Dictionary<string, JToken>();
        }        
        
    }
}