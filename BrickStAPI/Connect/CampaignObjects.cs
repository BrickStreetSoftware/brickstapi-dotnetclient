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
    public class Campaign
    {
        private static DateTime javaEpoch = new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime();

        // Campaign type codes
        public const int STANDARD_CAMPAIGN = 1;
        public const int PERIODIC_CAMPAIGN = 2;    // recurring
        public const int EVENT_CAMPAIGN = 3;
        public const int CURRICULUM_CAMPAIGN = 4;    // ???
        public const int SUBSCRIBE_CAMPAIGN = 5;
        public const int UNSUBSCRIBE_CAMPAIGN = 6;

        [JsonProperty("id")]
        public long? Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public int? Type { get; set; }

        // ID fields
        [JsonProperty("departmentID")]
        public long? DepartmentID { get; set; }
        [JsonProperty("languageID")]
        public long? LanguageID { get; set; }
        [JsonProperty("mailFarmID")]
        public long? MailFarmID { get; set; }

        [JsonProperty("conversationID")]
        public long? ConversationID { get; set; }
        [JsonProperty("audienceModelID")]
        public long? AudienceModelID { get; set; }
        [JsonProperty("segmentModelID")]
        public long? SegmentModelID { get; set; }

        private DateTime? _expireDate;

        [JsonProperty("expirationDate")]
        public long? ExpirationDateJava
        {
            get
            {
                if (_expireDate == null)
                {
                    return null;
                }
                long javaVal = JavaDateUtil.serializeDateTime((DateTime)_expireDate);
                return javaVal;
            }
            set
            {
                _expireDate = JavaDateUtil.deserializeDateTime((long)value);
            }
        }
        // Aliased Expiration Date property.  Used by .NET code.
        // Serialize/Deserialize code uses the ExpirationDateRaw property.
        [JsonIgnore]
        public DateTime? ExpirationDate 
        {
            get { return _expireDate; }
            set { _expireDate = value; }
        }
    }

    public class EventCampaign : Campaign
    {
        [JsonProperty("bounce")]
        public bool? Bounce { get; set; }
        [JsonProperty("event")]
        public EventMaster Event { get; set; }
    }

    //
    // CAMPAIGN INTERACTIONS
    //
    // Campaign Interaction == Sending Content to a Segment
    //

    public class CampaignInteraction
    {
        // Interaction Type Codes
        public const int TYPE_MAIN_MESSAGE = 2;
        public const int TYPE_AUTOMATED_MAIN_MESSAGE = 4;
        public const int TYPE_TRIAL_MESSAGE = 1;
        public const int TYPE_FOLLOWUP_MESSAGE = 3;

        // Priority Codes
	    public const int PRIORITY_NORMAL = 1;
	    public const int PRIORITY_HIGH	= 2;

        // Status Codes
        public const int STATUS_CREATING = 4;
        public const int STATUS_READY_FOR_LAUNCH = 1;
        public const int STATUS_LAUNCHED = 3;
        public const int STATUS_EXPIRED = 5;
        public const int STATUS_DISABLED = 6;
        public const int STATUS_EXPORTABLE = 7;
        // This status is set when a disabled interaction is TERMINATED
        // via the admin tool.
        public const int STATUS_TERMINATED = 9;
        
        // Sending Mode
        public const int MODE_EMAIL_ONLY = 0;
        public const int MODE_SMS_ONLY = 1;
        public const int MODE_SMS_PREF_OVER_EMAIL = 2;
        public const int MODE_DIRECTMAIL_ONLY = 3;

        [JsonProperty("id")]
        public long? Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; }
        // this is required
        [JsonProperty("campaignID")]
        public long CampaignID { get; set; }

        [JsonProperty("segmentID")]
        public long SegmentID { get; set; }
        [JsonProperty("contentID")]
        public long ContentID { get; set; }
        [JsonProperty("activeDeploymentID")]
        public long ActiveDeploymentID { get; set; }
        [JsonProperty("mailFarmID")]
        public long MailFarmID { get; set; }
        [JsonProperty("sendHandlerID")]
        public long DeliveryChannelID { get; set; }

        [JsonProperty("sendingMode")]
        public int? SendingMode { get; set; }
        [JsonProperty("priority")]
        public int? Priority { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("definitionStatus")]
        public int DefinitionStatus { get; set; }
        [JsonProperty("statusName")]
        public string StatusName { get; set; }
        [JsonProperty("expirationDate")]
        public DateTime ExpirationDate { get; set; }

        // RECURRING CAMPAIGNS
        [JsonProperty("polling")]
        public bool Polling { get; set; }
        [JsonProperty("pollingInterval")]
        public int PollingInterval { get; set; }
        [JsonProperty("externalExpirationDate")]
        public DateTime ExternalExpirationDate { get; set; }
        [JsonProperty("externalExpirationReference")]
        public string ExternalExpirationReference { get; set; }

        // SMPP-Specific
        [JsonProperty("smppServerID")]
        public long? SmppServerID { get; set; }
        [JsonProperty("smppSenderID")]
        public long? SmppSenderID { get; set; }
        [JsonProperty("smppMsgMode")]
        public int? SmppMsgMode { get; set; }
        [JsonProperty("smppDeliveryReceipt")]
        public bool? SmppDeliveryReceipt { get; set; }
    }

    public class RolloutAction
    {
        public const string ACTION_COUNT = "count";
        public const string ACTION_COUNT_STATUS = "count_status";
        public const string ACTION_PREPARE_LAUNCH = "prepare_launch";
        public const string ACTION_LAUNCH = "launch";
        public const string ACTION_CAN_ROLLOUT = "can_rollout";

        [JsonProperty("action")]
        public string Action { get; set; }
        [JsonProperty("effectiveDate")]
        public DateTime EffectiveDate { get; set; }
        [JsonProperty("expirationDate")]
        public DateTime ExpirationDate { get; set; }

        [JsonProperty("frequencyType")]
        public int? FrequencyType { get; set; }

        [JsonProperty("repeatEvery")]
        public int? RepeatEvery { get; set; }
        [JsonProperty("repeatEveryWhat")]
        public string RepeatEveryWhat { get; set; }
        [JsonProperty("repeatEveryWeek")]
        public int? RepeatEveryWeek { get; set; }
        [JsonProperty("dayOfWeek")]
        public List<int> DayOfWeek { get; set; }
        [JsonProperty("dayOfTheMonth")]
        public int? DayOfTheMonth { get; set; }

        public RolloutAction()
        {
            DayOfWeek = new List<int>();
        }
    }

    public class RolloutActionResponse
    {
        public const string STATUS_OK = "OK";
        public const string STATUS_ROLLOUT_NOT_ALLOWED = "ROLLOUT_NOT_ALLOWED";
        public const string STATUS_ALREADY_LAUNCHED = "ALREADY_LAUNCHED";
        public const string STATUS_ROLLOUT_ALLOWED = "ROLLOUT_ALLOWED";
        public const string STATUS_NOT_SUPPORTED = "NOT_SUPPORTED";
        
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("countStatus")]
        public int? CountStatus { get; set; }
        [JsonProperty("definitionStatus")]
        public int? DefinitionStatus { get; set; }
    }

    //
    // CONTENT
    //

    public class Sender
    {
        [JsonProperty("id")]
        public long? Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
        [JsonProperty("defaultSender")]
        public bool? DefaultSender { get; set; }
        
        // required
        [JsonProperty("departmentID")]
        public long DepartmentID { get; set; }
    }

    public class CampaignContent
    {
        public const int TYPE_STATIC = 1;
        public const int TYPE_DYNAMIC = 2;
        public const int TYPE_XSL = 3;
        public const int TYPE_SCRIPTED = 4;

        [JsonProperty("id")]
        public long? Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        
        // this is required
        [JsonProperty("campaignID")]
        public long CampaignID { get; set; }
        [JsonProperty("senderID")]
        public long SenderID { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("subject")]
        public string Subject { get; set; }
        [JsonProperty("languageID")]
        public long LanguageID { get; set; }

        //
        // content urls
        //

        [JsonProperty("htmlContentURL")]
        public string HtmlContentURL { get; set; }
        [JsonProperty("textContentURL")]
        public string TextContentURL { get; set; }
        [JsonProperty("shortTextContentURL")]
        public string ShortTextContentURL { get; set; }
        [JsonProperty("referralContentURL")]
        public string ReferralContentURL { get; set; }

        //
        // MISC SETTINGS
        //

        [JsonProperty("archive")]
        public bool Archive { get; set; }
        [JsonProperty("requiresTransportEncrypton")]
        public bool RequireTLS { get; set; }
        [JsonProperty("listUnsubscribeHeader")]
        public bool ListUnsubscribeHeader { get; set; }
        [JsonProperty("reparseContentFlag")]
        public bool ReparseContent { get; set; }
        [JsonProperty("sendModeOverride")]
        public long SendModeOverride { get; set; }

    }

}
