/*
 * Brick Street Connect Web Services API Client
 * Copyright (c) 2013 Brick Street Software, Inc.
 * http://brickstreetsoftware.com
 * This code open source and governed by the Apache Software License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

// import connect objects
using BrickStreetAPI.Connect;

namespace BrickStreetAPI
{
    public class BrickStreetConnect
    {
        public string ApiBaseUrl { get; set; }
        public string ApiUser { get; set; }
        public string ApiPass { get; set; }
        public bool UrlEncodePathParams { get; set; }


        #region "Constructors"

        public BrickStreetConnect(string url, string username, string password)
        {
            ApiBaseUrl = url;
            ApiUser = username;
            ApiPass = password;
            UrlEncodePathParams = false;
        }

        private RestClient createClient()
        {
            RestClient restClient = new RestClient
            {
                BaseUrl = ApiBaseUrl,
                Authenticator = new HttpBasicAuthenticator(ApiUser, ApiPass)
            };
            return restClient;
        }

        #endregion


        #region "Departments"

        public List<Department> GetDepartments(out HttpStatusCode status, out string statusMessage)
        {
            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/department",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };

            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            List<Department> objs = JsonConvert.DeserializeObject<List<Department>>(content);
            return objs;
        }

        public Department GetDepartment(long id, out HttpStatusCode status, out string statusMessage)
        {
            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/department/id/{id}",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };
            restRequest.AddParameter("id", id.ToString(), ParameterType.UrlSegment);

            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            Department obj = JsonConvert.DeserializeObject<Department>(content);
            return obj;
        }


        #endregion
        

        #region "Conversation"

        public List<Conversation> GetConversations(out HttpStatusCode status, out string statusMessage)
        {
            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/conversation",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };

            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            List<Conversation> convs = JsonConvert.DeserializeObject<List<Conversation>>(content);
            return convs;
        }

        public Conversation GetConversation(long id, out HttpStatusCode status, out string statusMessage)
        {
            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/conversation/id/{id}",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };
            restRequest.AddParameter("id", id.ToString(), ParameterType.UrlSegment);

            //
            // NOTE: The JSON deserializer built into the RestSharp
            // package does not work well with our classes, so we use Newtonsoft instead.
            //
            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            Conversation obj = JsonConvert.DeserializeObject<Conversation>(content);
            return obj;
        }

        public Conversation GetConversationByName(string name, out HttpStatusCode status, out string statusMessage)
        {
            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/conversation/name/{name}",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };
            // encode name for use in url path
            string encodedName = name;
            if (UrlEncodePathParams)
            {
                //encodedName = HttpUtility.UrlPathEncode(encodedName);
                encodedName = Uri.EscapeUriString(encodedName);
            }
            restRequest.AddParameter("name", encodedName, ParameterType.UrlSegment);

            //
            // NOTE: The JSON deserializer built into the RestSharp
            // package does not work well, so we use Newtonsoft instead.
            //
            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            Conversation obj = JsonConvert.DeserializeObject<Conversation>(content);
            return obj;
        }
       
        public Conversation AddConversation(Conversation conversation, out HttpStatusCode status, out string statusMessage)
        {
            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/conversation",
                RequestFormat = DataFormat.Json,
                Method = Method.POST
            };
            // we handle our own serialization instead of calling AddBody
            JsonSerializerSettings serSettings = new JsonSerializerSettings();
            serSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            serSettings.NullValueHandling = NullValueHandling.Ignore;

            string serialized = JsonConvert.SerializeObject(conversation, serSettings);
            restRequest.AddParameter("application/json", serialized, ParameterType.RequestBody);

            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            serialized = restResponse.Content;
            Conversation jobj = JsonConvert.DeserializeObject<Conversation>(serialized);
            return jobj;
        }

        
        #endregion

        #region "Customer"

        private Customer _GetCustomer(string uri, string paramName, string paramVal, out HttpStatusCode status, out string statusMessage)
        {
            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = uri,
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };
            restRequest.AddParameter(paramName, paramVal, ParameterType.UrlSegment);

            //
            // NOTE: The JSON deserializer built into the RestSharp
            // package does not work well with our classes, so we use Newtonsoft instead.
            //
            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            Customer obj = JsonConvert.DeserializeObject<Customer>(content);
            return obj;
        }
        
        public Customer GetCustomer(long id, out HttpStatusCode status, out string statusMessage)
        {
            string uri = "data/customer/id/{id}";
            string paramName = "id";
            string paramVal = id.ToString();

            Customer cust = _GetCustomer(uri, paramName, paramVal, out status, out statusMessage);
            return cust;
        }

        public Customer GetCustomerByEmail(string email, out HttpStatusCode status, out string statusMessage)
        {
            string uri = "data/customer/email/{email}";
            string paramName = "email";
            string paramVal = email;
            if (UrlEncodePathParams)
            {
                //paramVal = HttpUtility.UrlPathEncode(paramVal);
                paramVal = Uri.EscapeUriString(paramVal);
            }

            Customer cust = _GetCustomer(uri, paramName, paramVal, out status, out statusMessage);
            return cust;
        }

        public Customer GetCustomerByAltId(string altid, out HttpStatusCode status, out string statusMessage)
        {
            string uri = "data/customer/altid/{altid}";
            string paramName = "altid";
            string paramVal = altid;
            if (UrlEncodePathParams)
            {
                //paramVal = HttpUtility.UrlPathEncode(paramVal);
                paramVal = Uri.EscapeUriString(paramVal);
            }

            Customer cust = _GetCustomer(uri, paramName, paramVal, out status, out statusMessage);
            return cust;
        }

        public Customer UpdateCustomer(Customer newCust, out HttpStatusCode status, out string statusMessage)
        {
            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/customer/id/{id}",
                RequestFormat = DataFormat.Json,
                Method = Method.PUT
            };
            long newCustId = (long)newCust.Id;
            string paramVal = newCustId.ToString();
            restRequest.AddParameter("id", paramVal, ParameterType.UrlSegment);

            // we handle our own serialization instead of calling AddBody
            JsonSerializerSettings serSettings = new JsonSerializerSettings();
            serSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            serSettings.NullValueHandling = NullValueHandling.Ignore;
            string serialized = JsonConvert.SerializeObject(newCust, serSettings);
            restRequest.AddParameter("application/json", serialized, ParameterType.RequestBody);

            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            serialized = restResponse.Content;
            Customer obj = JsonConvert.DeserializeObject<Customer>(serialized);
            return obj;
        }

        public Customer AddCustomer(Customer newCust, out HttpStatusCode status, out string statusMessage)
        {
            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/customer",
                RequestFormat = DataFormat.Json,
                Method = Method.POST
            };
            // we handle our own serialization instead of calling AddBody
            JsonSerializerSettings serSettings = new JsonSerializerSettings();
            serSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            serSettings.NullValueHandling = NullValueHandling.Ignore;

            string serialized = JsonConvert.SerializeObject(newCust, serSettings);
            restRequest.AddParameter("application/json", serialized, ParameterType.RequestBody);

            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            serialized = restResponse.Content;
            Customer obj = JsonConvert.DeserializeObject<Customer>(serialized);
            return obj;
        }

        public CustomerAttribute GetCustomerAttribute(string attrName, out HttpStatusCode status, out string statusMessage)
        {
            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "metadata/customer/{attrName}",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };
            string paramVal = attrName;
            if (UrlEncodePathParams)
            {
                //paramVal = HttpUtility.UrlPathEncode(paramVal);
                paramVal = Uri.EscapeUriString(paramVal);
            }
            restRequest.AddParameter("attrName", paramVal, ParameterType.UrlSegment);
            
            //
            // NOTE: The JSON deserializer built into the RestSharp
            // package does not work well with our classes, so we use Newtonsoft instead.
            //
            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            CustomerAttribute obj = JsonConvert.DeserializeObject<CustomerAttribute>(content);
            return obj;
        }

        #endregion

        #region "Events"

        public Event GetEvent(long id, out HttpStatusCode status, out string statusMessage)
        {
            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/event/id/{id}",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };
            restRequest.AddParameter("id", id.ToString(), ParameterType.UrlSegment);

            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            Event obj = JsonConvert.DeserializeObject<Event>(content);
            return obj;
        }

        public List<Event> GetEventsForCustomer(long customerId, out HttpStatusCode status, out string statusMessage)
        {
            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/event/customer/{customer_id}",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };
            restRequest.AddParameter("customer_id", customerId.ToString(), ParameterType.UrlSegment);

            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            List<Event> obj = JsonConvert.DeserializeObject<List<Event>>(content);
            return obj;
        }

        public Event AddEvent(Event newEvent, out HttpStatusCode status, out string statusMessage)
        {
            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/event",
                RequestFormat = DataFormat.Json,
                Method = Method.POST
            };

            // we handle our own serialization instead of calling AddBody
            JsonSerializerSettings serSettings = new JsonSerializerSettings();
            serSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            serSettings.NullValueHandling = NullValueHandling.Ignore;
            string serialized = JsonConvert.SerializeObject(newEvent, serSettings);
            restRequest.AddParameter("application/json", serialized, ParameterType.RequestBody);

            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            serialized = restResponse.Content;
            Event obj = JsonConvert.DeserializeObject<Event>(serialized);
            return obj;
        }

        #endregion

        #region "Senders"

        public List<Sender> GetSenders(out HttpStatusCode status)
        {
            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/sender",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };

            //
            // NOTE: The JSON deserializer built into the RestSharp
            // package does not work well, so we use Newtonsoft instead.
            //
            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                return null;
            }

            string content = restResponse.Content;
            List<Sender> senders = JsonConvert.DeserializeObject<List<Sender>>(content);
            return senders;
        }                

        #endregion

        #region "Campaigns"

        public EventCampaign AddEventCampaign(EventCampaign camp, out HttpStatusCode status, out string statusMessage)
        {
            RestClient restClient = createClient();
                
            RestRequest restRequest = new RestRequest
            {
                Resource = "data/campaign/type/event",
                RequestFormat = DataFormat.Json,
                Method = Method.POST
            };
            // we handle our own serialization instead of calling AddBody
            JsonSerializerSettings serSettings = new JsonSerializerSettings();
            serSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            serSettings.NullValueHandling = NullValueHandling.Ignore;

            string serialized = JsonConvert.SerializeObject(camp, serSettings);
            restRequest.AddParameter("application/json", serialized, ParameterType.RequestBody);

            //
            // NOTE: The JSON deserializer built into the RestSharp
            // package does not work well, so we use Newtonsoft instead.
            //
            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            EventCampaign camp2 = JsonConvert.DeserializeObject<EventCampaign>(content);
            return camp2;
        }

        // get campaign by id
        public EventCampaign GetEventCampaign(long id, out HttpStatusCode status, out string statusMessage)
        {
            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/campaign/id/{id}",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };
            restRequest.AddParameter("id", id.ToString(), ParameterType.UrlSegment);

            //
            // NOTE: The JSON deserializer built into the RestSharp
            // package does not work well, so we use Newtonsoft instead.
            //
            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            EventCampaign camp = JsonConvert.DeserializeObject<EventCampaign>(content);
            return camp;
        }

        public EventCampaign GetEventCampaignByName(string name, out HttpStatusCode status, out string statusMessage)
        {
            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/campaign/name/{name}",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };
            string encodedName = name;
            if (UrlEncodePathParams)
            {
                //encodedName = HttpUtility.UrlPathEncode(encodedName);
                encodedName = Uri.EscapeUriString(encodedName);
            }
            restRequest.AddParameter("name", encodedName, ParameterType.UrlSegment);

            //
            // NOTE: The JSON deserializer built into the RestSharp
            // package does not work well, so we use Newtonsoft instead.
            //
            IRestResponse restResponse = restClient.Execute(restRequest);                        
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            EventCampaign camp = JsonConvert.DeserializeObject<EventCampaign>(content);
            return camp;
        }

        #endregion

        #region "Content"

        public List<CampaignContent> GetCampaignContent(long campaignID, out HttpStatusCode status, out string statusMessage)
        {
            if (campaignID <= 0)
            {
                throw new Exception("Invalid Campaign ID");
            }

            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/campaign/campaign_id/{campaign_id}/content",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };
            restRequest.AddUrlSegment("campaign_id", campaignID.ToString());

            //
            // NOTE: The JSON deserializer built into the RestSharp
            // package does not work well, so we use Newtonsoft instead.
            //
            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            List<CampaignContent> cont = JsonConvert.DeserializeObject<List<CampaignContent>>(content);
            return cont;
        }

        public CampaignContent GetCampaignContent(long campaignID, long contentID, out HttpStatusCode status, out string statusMessage)
        {
            if (campaignID <= 0)
            {
                throw new Exception("Invalid Campaign ID");
            }
            if (contentID <= 0)
            {
                throw new Exception("Invalid Content ID");
            }

            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/campaign/campaign_id/{campaign_id}/content/id/{content_id}",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };
            restRequest.AddUrlSegment("campaign_id", campaignID.ToString());
            restRequest.AddUrlSegment("content_id", contentID.ToString());

            //
            // NOTE: The JSON deserializer built into the RestSharp
            // package does not work well with our classes, so we use Newtonsoft instead.
            //
            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            CampaignContent cont = JsonConvert.DeserializeObject<CampaignContent>(content);
            return cont;
        }        
        
        public CampaignContent AddCampaignContent(CampaignContent cont, out HttpStatusCode status, out string statusMessage)
        {
            long campaignID = cont.CampaignID;
            if (campaignID <= 0)
            {
                throw new Exception("Invalid Campaign ID in Content Object");
            }

            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/campaign/campaign_id/{campaign_id}/content",
                RequestFormat = DataFormat.Json,
                Method = Method.POST
            };
            restRequest.AddUrlSegment("campaign_id", campaignID.ToString());

            // we handle our own serialization instead of calling AddBody
            JsonSerializerSettings serSettings = new JsonSerializerSettings();
            serSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            serSettings.NullValueHandling = NullValueHandling.Ignore;

            string serialized = JsonConvert.SerializeObject(cont, serSettings);
            restRequest.AddParameter("application/json", serialized, ParameterType.RequestBody);

            //
            // NOTE: The JSON deserializer built into the RestSharp
            // package does not work well, so we use Newtonsoft instead.
            //
            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            CampaignContent cont2 = JsonConvert.DeserializeObject<CampaignContent>(content);
            return cont2;
        }

        public CampaignContent UpdateCampaignContent(CampaignContent cont, out HttpStatusCode status, out string statusMessage)
        {
            long campaignID = cont.CampaignID;
            if (campaignID <= 0)
            {
                throw new Exception("Invalid Campaign ID in Content Object");
            }
            long? contentID = cont.Id;
            if (contentID == null)
            {
                throw new Exception("Content ID must be non-null; use AddCampaignContent instead");
            }
            if (contentID == null && contentID <= 0)
            {
                throw new Exception("Invalid ID in Content Object");
            }

            RestClient restClient = createClient();
            RestRequest restRequest = new RestRequest
            {
                Resource = "data/campaign/campaign_id/{campaign_id}/content/id/{content_id}",
                RequestFormat = DataFormat.Json,
                Method = Method.PUT
            };
            restRequest.AddUrlSegment("campaign_id", campaignID.ToString());
            restRequest.AddUrlSegment("content_id", contentID.ToString());

            // we handle our own serialization instead of calling AddBody
            JsonSerializerSettings serSettings = new JsonSerializerSettings();
            serSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            serSettings.NullValueHandling = NullValueHandling.Ignore;

            string serialized = JsonConvert.SerializeObject(cont, serSettings);
            restRequest.AddParameter("application/json", serialized, ParameterType.RequestBody);

            //
            // NOTE: The JSON deserializer built into the RestSharp
            // package does not work well, so we use Newtonsoft instead.
            //
            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            CampaignContent cont2 = JsonConvert.DeserializeObject<CampaignContent>(content);
            return cont2;
        }

        #endregion

        #region "Campaign Interactions"

        public CampaignInteraction AddMessage(CampaignInteraction inter, out HttpStatusCode status, out string statusMessage)
        {
            long campaignID = inter.CampaignID;
            if (campaignID <= 0)
            {
                throw new Exception("Invalid Campaign ID in Interaction Object");
            }

            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/campaign/campaign_id/{campaign_id}/message",
                RequestFormat = DataFormat.Json,
                Method = Method.POST
            };
            restRequest.AddUrlSegment("campaign_id", campaignID.ToString());

            // we handle our own serialization instead of calling AddBody
            JsonSerializerSettings serSettings = new JsonSerializerSettings();
            serSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            serSettings.NullValueHandling = NullValueHandling.Ignore;

            string serialized = JsonConvert.SerializeObject(inter, serSettings);
            restRequest.AddParameter("application/json", serialized, ParameterType.RequestBody);

            //
            // NOTE: The JSON deserializer built into the RestSharp
            // package does not work well, so we use Newtonsoft instead.
            //
            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            CampaignInteraction inter2 = JsonConvert.DeserializeObject<CampaignInteraction>(content);
            return inter2;
        }

        public List<CampaignInteraction> GetMessages(long campaignID, out HttpStatusCode status, out string statusMessage)
        {
            if (campaignID <= 0)
            {
                throw new Exception("Invalid Campaign ID");
            }

            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/campaign/campaign_id/{campaign_id}/message",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };
            restRequest.AddUrlSegment("campaign_id", campaignID.ToString());

            //
            // NOTE: The JSON deserializer built into the RestSharp
            // package does not work well, so we use Newtonsoft instead.
            //
            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            List<CampaignInteraction> cont = JsonConvert.DeserializeObject<List<CampaignInteraction>>(content);
            return cont;
        }

        public CampaignInteraction GetMessage(long campaignID, long interactionID, out HttpStatusCode status, out string statusMessage)
        {
            if (campaignID <= 0)
            {
                throw new Exception("Invalid Campaign ID");
            }
            if (interactionID <= 0)
            {
                throw new Exception("Invalid Interaction ID");
            }

            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/campaign/campaign_id/{campaign_id}/message/id/{message_id}",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };
            restRequest.AddUrlSegment("campaign_id", campaignID.ToString());
            restRequest.AddUrlSegment("message_id", interactionID.ToString());

            //
            // NOTE: The JSON deserializer built into the RestSharp
            // package does not work well, so we use Newtonsoft instead.
            //
            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            CampaignInteraction cont = JsonConvert.DeserializeObject<CampaignInteraction>(content);
            return cont;
        }

        public CampaignInteraction UpdateMessage(CampaignInteraction inter, out HttpStatusCode status, out string statusMessage)
        {
            long interactionID = (long) inter.Id;
            long campaignID = inter.CampaignID;

            if (campaignID <= 0)
            {
                throw new Exception("Invalid Campaign ID");
            }
            if (interactionID <= 0)
            {
                throw new Exception("Invalid Interaction ID");
            }

            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/campaign/campaign_id/{campaign_id}/message/id/{message_id}",
                RequestFormat = DataFormat.Json,
                Method = Method.PUT
            };
            restRequest.AddUrlSegment("campaign_id", campaignID.ToString());
            restRequest.AddUrlSegment("message_id", interactionID.ToString());

            //
            // NOTE: The JSON deserializer built into the RestSharp
            // package does not work well, so we use Newtonsoft instead.
            //
            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }

            string content = restResponse.Content;
            CampaignInteraction cont = JsonConvert.DeserializeObject<CampaignInteraction>(content);
            return cont;
        }

        
        public RolloutActionResponse DoRolloutAction(long campaignID, long interactionID, RolloutAction ra,
            out HttpStatusCode status, out string statusMessage)
        {
            if (campaignID <= 0)
            {
                throw new Exception("Invalid Campaign ID");
            }
            if (interactionID <= 0)
            {
                throw new Exception("Invalid Interaction ID");
            }

            RestClient restClient = createClient();

            RestRequest restRequest = new RestRequest
            {
                Resource = "data/campaign/campaign_id/{campaign_id}/message/id/{inter_id}/rollout",
                RequestFormat = DataFormat.Json,
                Method = Method.PUT
            };
            restRequest.AddUrlSegment("campaign_id", campaignID.ToString());
            restRequest.AddUrlSegment("inter_id", interactionID.ToString());

            // we handle our own serialization instead of calling AddBody
            JsonSerializerSettings serSettings = new JsonSerializerSettings();
            serSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            serSettings.NullValueHandling = NullValueHandling.Ignore;

            string serialized = JsonConvert.SerializeObject(ra, serSettings);
            restRequest.AddParameter("application/json", serialized, ParameterType.RequestBody);

            //
            // NOTE: The JSON deserializer built into the RestSharp
            // package does not work well, so we use Newtonsoft instead.
            //
            IRestResponse restResponse = restClient.Execute(restRequest);
            status = restResponse.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                statusMessage = restResponse.Content;
                return null;
            }
            else
            {
                statusMessage = null;
            }                

            string content = restResponse.Content;
            RolloutActionResponse raresp = JsonConvert.DeserializeObject<RolloutActionResponse>(content);
            return raresp;
        }

        #endregion
    }
}
