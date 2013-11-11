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
    // This object describes an event.
    // Event objects represent the individual customer events.
    public class EventMaster
    {
        [JsonProperty("id")]
        public long? Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("includeXml")]
        public bool? IncludeXML { get; set; }

        [JsonProperty("parameters")]
        public List<EventParameterMaster> Parameters { get; set; }

        public EventMaster()
        {
            Parameters = new List<EventParameterMaster>();
        }
    }

    public class EventParameterMaster
    {
        // DATA TYPES (cf common.bl.cnversation.EventParameter.getPossibleDataTypes in Connect src)
        // NOTE: The codes are originally from java.sql.Types in JDBC.
        public static int TYPE_SINGLE_CHARACTER = 1;
        public static int TYPE_NUMBER = 3;
        public static int TYPE_STRING = 12;
        public static int TYPE_DATE = 93;
        public static int TYPE_URL = 3000;

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("dataType")]
        public int DataType { get; set; }

        // Should always be TRUE
        [JsonProperty("forwarded")]
        public bool Forwarded { get; set; }

        // XXX what about bounded?

        public EventParameterMaster()
        {
            Forwarded = true;
        }
    }

    public class Event
    {
        private static DateTime javaEpoch = new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime();

        [JsonProperty("id")]
        public long? Id { get; set; }
        [JsonProperty("eventName")]
        public string EventName { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("customerId")]
        public long CustomerId { get; set; }
        [JsonProperty("externalId")]
        public long? ExternalId { get; set; }
        [JsonProperty("xml")]
        public string Xml { get; set; }
        [JsonProperty("eventId")]
        public long? EventId { get; set; }

        [JsonProperty("sendEmail")]
        public bool? SendEmail { get; set; }
        [JsonProperty("archive")]
        public bool? Archive { get; set; }
        [JsonProperty("subscribe")]
        public bool? Subscribe { get; set; }

        [JsonProperty("parameters")]
        public List<EventParameter> Parameters { get; set; }
        [JsonProperty("attachments")]
        public List<EventAttachment> Attachments { get; set; }

        // XXX date handling for java dates
        // We implement custom serialize/deserialize logic by having two properties
        // that refer to the same instance var.  One property is invisible to JSON.
        private DateTime? _effectiveDate;
        private DateTime? _expirationDate;

        // Used by .NET code; invisible to JSON
        [JsonIgnore]
        public DateTime? EffectiveDate 
        {
            get { return _effectiveDate; }
            set { _effectiveDate = value; }
        }
        [JsonIgnore]
        public DateTime? ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }

        // Java Date serialize/deserialize code
        [JsonProperty("effectiveDate")]
        public long? EffectiveDateJava
        {
            get
            {
                if (_effectiveDate == null)
                {
                    return null;
                }
                long javaVal = JavaDateUtil.serializeDateTime((DateTime)_effectiveDate);
                return javaVal;
            }
            set
            {
                _effectiveDate = JavaDateUtil.deserializeDateTime((long)value);
            }
        }

        [JsonProperty("expirationDate")]
        public long? ExpirationDateJava
        {
            get
            {
                if (_expirationDate == null)
                {
                    return null;
                }
                long javaVal = JavaDateUtil.serializeDateTime((DateTime)_expirationDate);
                return javaVal;
            }
            set
            {
                _expirationDate = JavaDateUtil.deserializeDateTime((long)value);
            }
        }

        public Event()
        {
            Parameters = new List<EventParameter>();
            Attachments = new List<EventAttachment>();
        }
    }

    public class EventParameter
    {
        [JsonProperty("parameterName")]
        public string ParameterName { get; set; }
        [JsonProperty("parameterValue")]
        public string ParameterValue { get; set; }
        [JsonProperty("encrypted")]
        public bool Encrypted { get; set; }
    }

    public class EventAttachment
    {
        [JsonProperty("attachmentName")]
        public string AttachmentName { get; set; }
        [JsonProperty("contentUrl")]
        public string ContentUrl { get; set; }
        [JsonProperty("contentData")]
        public string ContentData { get; set; }
        [JsonProperty("contentEncoding")]
        public string ContentEncoding { get; set; }
        [JsonProperty("fileName")]
        public string FileName { get; set; }
        [JsonProperty("position")]
        public long? Position { get; set; }
        [JsonProperty("purge")]
        public bool? Purge { get; set; }
    }
}
