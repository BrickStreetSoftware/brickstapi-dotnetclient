using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BrickStreetAPI.Connect
{
    public class CustomerAttribute
    {
        public const string TYPE_ATTRIBUTE = "attribute";
        public const string TYPE_PREFERENCE = "preference";

        public const string TYPE_CHANNEL = "channel";
        public const string TYPE_MULTIADDRESS = "multiaddress";

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("dataType")]
        public int DataType { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("preferenceValues")]
        public string[] PreferenceValues { get; set; }
    }

    public class Customer
    {
        [JsonProperty("id")]
        public long? Id { get; set; }
        [JsonProperty("altCustomerID")]
        public string AltCustomerId { get; set; }
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
        [JsonProperty("smsNumber")]
        public string SMSNumber { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("middleName")]
        public string MiddleName { get; set; }
        [JsonProperty("salutation")]
        public string Salutation { get; set; }
        [JsonProperty("organization")]
        public string Organization { get; set; }

        [JsonProperty("addressLine1")]
        public string AddressLine1 { get; set; }
        [JsonProperty("addressLine2")]
        public string AddressLine2 { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("province")]
        public string Province { get; set; }
        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("birthDate")]
        public DateTime? BirthDate { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("altPhone")]
        public string AltPhone { get; set; }
        [JsonProperty("fax")]
        public string Fax { get; set; }

        [JsonProperty("attributes")]
        public List<CustomerAttribute> Attributes { get; set; }

        [JsonProperty("channelAddresses")]
        public List<CustomerAttribute> ChannelAddresses { get; set; }

        public Customer()
        {
            Attributes = new List<CustomerAttribute>();
            ChannelAddresses = new List<CustomerAttribute>();
        }

        public CustomerAttribute GetAttribute(string attrName)
        {
            return Attributes.FirstOrDefault(attr => System.String.Compare(attr.Name, attrName, System.StringComparison.OrdinalIgnoreCase) == 0);
        }

        public CustomerAttribute GetChannelAddress(string attrName)
        {
            return
                ChannelAddresses.FirstOrDefault(
                    attr => System.String.Compare(attr.Name, attrName, System.StringComparison.OrdinalIgnoreCase) == 0);
        }
    }
}
