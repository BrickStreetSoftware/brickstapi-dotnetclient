using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Runtime.CompilerServices;
using BrickStreetAPI;
using BrickStreetAPI.Connect;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrickStreetApi.Test
{
    [TestClass]
    public class EventCampaignTest
    {
        public BrickStreetConnect makeClient()
        {
            string apiBaseUrl = ConfigurationManager.AppSettings["BrickStreetApiHttps"];
            string apiBaseUser = ConfigurationManager.AppSettings["BrickStreetApiUser"];
            string apiBasePass = ConfigurationManager.AppSettings["BrickStreetApiPass"];

            BrickStreetConnect c = new BrickStreetConnect(apiBaseUrl, apiBaseUser, apiBasePass);
            return c;
        }

        [TestMethod]
        public void ExistingEventCampaign()
        {
            //
            //Test Parameters
            //
            string altCustId = ConfigurationManager.AppSettings["AltCustId"];
            string eventName = ConfigurationManager.AppSettings["EventName"];
            string tokenName = ConfigurationManager.AppSettings["TokenName"];
            string tokenValue = ConfigurationManager.AppSettings["TokenValue"];
            //

            BrickStreetConnect brickst = makeClient();
            HttpStatusCode status;
            string statusMessage;
            string timecode = DateTime.Now.ToLongTimeString();

            Customer customer = brickst.GetCustomerByAltId(altCustId, out status, out statusMessage);

            if (customer == null)
            {
                string randomEmail = "user" + timecode + "@example.com";
                customer = new Customer();
                customer.EmailAddress = randomEmail;
                customer.AltCustomerId = altCustId;

                Customer cust2 = brickst.AddCustomer(customer, out status, out statusMessage);
                if (status != HttpStatusCode.OK)
                {
                    Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
                    throw new Exception("null customer received from add customer");
                }
            }

            long custId = Convert.ToInt32(customer.Id);

            if (!string.IsNullOrEmpty(tokenName) && !string.IsNullOrEmpty(tokenValue))
            {
                //fetch attribute metadata
                BrickStAPI.Connect.Attribute attrDef = brickst.GetCustomerAttribute(tokenName, out status, out statusMessage);
                if (status != HttpStatusCode.OK)
                {
                    Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
                }

                string attrType = attrDef.Type;
                bool doupdate = false;

                CustomerAttribute attr = customer.GetChannelAddress(tokenName);

                if (string.Compare("attribute", attrType, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (attr == null)
                    {
                        attr = new CustomerAttribute();
                        attr.Name = attrDef.Name;
                        attr.Type = attrDef.Type;
                        attr.DataType = attrDef.DataType;
                        attr.Value = tokenValue;
                        customer.Attributes.Add(attr);
                        doupdate = true;
                    }
                    else
                    {
                        // update if new
                        if (!tokenValue.Equals(attr.Value))
                        {
                            attr.Value = tokenValue;
                            doupdate = true;
                        }
                    }
                }
                else if (string.Compare("preference", attrType, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (attr == null)
                    {
                        attr = new CustomerAttribute();
                        attr.Name = attrDef.Name;
                        attr.Type = attrDef.Type;
                        attr.DataType = attrDef.DataType;
                        // start with 1 value
                        attr.PreferenceValues = new String[1];
                        attr.PreferenceValues[0] = tokenValue;
                        customer.ChannelAddresses.Add(attr);
                        doupdate = true;
                    }
                    else
                    {
                        // existing preference record
                        // add the token value if it is not already there
                        // the push channel code will automatically remove invalid device tokens
                        String[] vals = attr.PreferenceValues;
                        bool valuefound = false;
                        for (int i = 0; i < vals.Length; i++)
                        {
                            String val = vals[i];
                            if (tokenValue.Equals(val))
                            {
                                valuefound = true;
                                break;
                            }
                        }

                        if (!valuefound)
                        {
                            String[] newVals = new String[vals.Length + 1];
                            Array.Copy(vals, 0, newVals, 0, vals.Length);
                            newVals[newVals.Length - 1] = tokenValue;
                            attr.PreferenceValues = newVals;
                            doupdate = true;
                        }
                    }
                }


                // save updated customer record if necessary
                if (doupdate)
                {
                    Customer custSave2 = brickst.UpdateCustomer(customer, out status, out statusMessage);
                    if (custSave2 == null)
                    {
                        throw new Exception("null customer received from updateCustomer");
                    }
                    customer = custSave2;
                }
            }

            //
            //create an event record
            //
            BrickStreetAPI.Connect.Event eventObj = new BrickStreetAPI.Connect.Event();
            eventObj.EventName = eventName;
            eventObj.CustomerId = custId;
            eventObj.Subscribe = true;
            eventObj.Parameters = new List<EventParameter>();

            DateTime now = new DateTime();

            //event parameter: Message
            EventParameter ep1 = new EventParameter();
            ep1.ParameterName = "Message";
            ep1.ParameterValue = "Test at " + now.ToString();
            ep1.Encrypted = false;
            eventObj.Parameters.Add(ep1);

            //event parameter:Badge
            EventParameter ep2 = new EventParameter();
            ep2.ParameterName = "Badge";
            ep2.ParameterValue = "42";
            ep2.Encrypted = false;
            eventObj.Parameters.Add(ep2);

            //event parameter: AlertID
            EventParameter ep3 = new EventParameter();
            ep3.ParameterName = "AlertID";
            ep3.ParameterValue = timecode;
            ep3.Encrypted = false;
            eventObj.Parameters.Add(ep3);

            //
            //submit event to connect
            //
            BrickStreetAPI.Connect.Event posted = brickst.AddEvent(eventObj, out status, out statusMessage);

            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
                throw new Exception("addEvent returned null");
            }

            long eventQueueId = Convert.ToInt32(posted.Id);
            long eventId = Convert.ToInt32(posted.EventId);

            Console.WriteLine("Posted Event;ID=" + eventQueueId + " for CustomerID=" + custId + " and Event ID=" + eventId);

        }
    }
}
