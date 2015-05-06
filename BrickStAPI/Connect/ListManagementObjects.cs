/*
 * Brick Street Connect Web Services API Client
 * Copyright (c) 2013 Brick Street Software, Inc.
 * http://brickstreetsoftware.com
 * This code open source and governed by the Apache Software License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BrickStreetAPI.Connect
{
    // conversations represent mailing lists
    // customers subscribe and unsubscribe 
    public class Conversation
    {
        [JsonProperty("id")]
        public long? Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }

        // this is required
        [JsonProperty("departmentID")]
        public long DepartmentID { get; set; }

        [JsonProperty("senderID")]
        public long? SenderID { get; set; }
        [JsonProperty("mailFarmID")]
        public long? MailFarmID { get; set; }
        [JsonProperty("senderDomain")]
        public long? SenderDomain { get; set; }

        [JsonProperty("signingEnabled")]
        public bool? SigningEnabled { get; set; }

        [JsonProperty("referFriendDefaultText")]
        public string ReferFriendDefaultText { get; set; }
        [JsonProperty("unsubscribeDefaultText")]
        public string UnsubscribeDefaultText { get; set; }
    }

    public class SenderDomain
    {
        [JsonProperty("id")]
        public long? Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("defaultDomain")]
        public bool? DefaultDomain { get; set; }
        [JsonProperty("signingAlg")]
        public int SigningAlg { get; set; }
        [JsonProperty("selector")]
        public string Selector { get; set; }
        [JsonProperty("privateKey")]
        public string PrivateKey { get; set; }
    }

}
