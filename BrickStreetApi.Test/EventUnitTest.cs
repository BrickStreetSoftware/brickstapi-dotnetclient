using System;
using System.Configuration;
using System.Net;
using System.Text;
using BrickStreetAPI;
using BrickStreetAPI.Connect;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrickStreetApi.Test
{
    [TestClass]
    public class EventUnitTest
    {
        public long ConnectDepartmentID { get; set; }

        public BrickStreetConnect makeClient()
        {
            string apiBaseUrl = ConfigurationManager.AppSettings["BrickStreetApiHttps"];
            string apiBaseUser = ConfigurationManager.AppSettings["BrickStreetApiUser"];
            string apiBasePass = ConfigurationManager.AppSettings["BrickStreetApiPass"];

            string apiBaseDept = ConfigurationManager.AppSettings["BrickStreetApiDept"];
            ConnectDepartmentID = long.Parse(apiBaseDept);

            BrickStreetConnect c = new BrickStreetConnect(apiBaseUrl, apiBaseUser, apiBasePass);
            return c;
        }

        [TestMethod]
        public void EventReceiptTest()
        {
            BrickStreetConnect brickst = makeClient();
            HttpStatusCode status;
            string statusMessage = null;

            //
            // verify that conversation exists
            //
            // CONV NAME: "TEST ERECEIPT CONVERSATION"
            //
            // SPENTLY: SHOULD CREATE A CONVERSATION OBJECT FOR EACH SPENTLY ACCOUNT
            Conversation conv = brickst.GetConversationByName("TEST ERECEIPT CONVERSATION", out status, out statusMessage);
            if (status == HttpStatusCode.NotFound)
            {
                // conversation not found; create it
                Assert.IsNull(conv, "Expected null conversation");

                conv = new Conversation();
                conv.Name = "TEST ERECEIPT CONVERSATION";
                conv.DepartmentID = ConnectDepartmentID;

                Conversation conv2 = brickst.AddConversation(conv, out status, out statusMessage);
                Assert.AreEqual(HttpStatusCode.OK, status);
                Assert.IsNotNull(conv2);
                Assert.IsNotNull(conv2.Id);
                Assert.AreEqual(conv.Name, conv2.Name);
                conv = conv2;
            }
            Assert.IsNotNull(conv);

            //
            // create new campaign
            //
            string campaignName = "TEST ERECEIPT " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();

            EventMaster eventMaster = new EventMaster()
            {
                Name = campaignName,
                IncludeXML = true
            };
            // NO EVENT PARAMS

            EventCampaign camp = new EventCampaign()
            {
                Name = campaignName,
                Type = Campaign.EVENT_CAMPAIGN,
                ConversationID = conv.Id,
                DepartmentID = ConnectDepartmentID,
                Event = eventMaster
            };

            EventCampaign savedCamp = brickst.AddEventCampaign(camp, out status, out statusMessage);
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(savedCamp);
            Assert.IsNotNull(savedCamp.Id);
            Assert.AreEqual(ConnectDepartmentID, savedCamp.DepartmentID);
            camp = savedCamp;

            //
            // create content
            //
            CampaignContent cont = new CampaignContent()
            {
                Name = campaignName + " CONTENT",
                Type = CampaignContent.TYPE_XSL,
                Subject = "TEST SUBJ LINE FOR " + campaignName,
                //
                // NOTE: THIS URL SHOULD BE THE URL OF THE RECEIPT TEMPLATE XSL !!!
                ///
                HtmlContentURL = "http://content.brickst.net/eNotify/spently/eReceipt/spently-receipt2.xsl",
                CampaignID = (long)camp.Id
            };
            CampaignContent cont2 = brickst.AddCampaignContent(cont, out status, out statusMessage);
            if (statusMessage != null)
            {
                Console.WriteLine("ERROR SAVING CONTENT: " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(cont2);
            Assert.IsNotNull(cont2.Id);
            cont = cont2;

            //
            // create message interaction to use the content
            //
            CampaignInteraction inter = new CampaignInteraction()
            {
                Name = "Test Interaction 1",
                Type = CampaignInteraction.TYPE_MAIN_MESSAGE,
                CampaignID = (long)camp.Id,
                ContentID = (long)cont.Id,
                SegmentID = 1
            };
            CampaignInteraction inter2 = brickst.AddMessage(inter, out status, out statusMessage);
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(inter2);
            Assert.IsNotNull(inter2.Id);
            inter = inter2;

            //
            // Prepare to Launch the Interaction
            //
            RolloutAction raction = new RolloutAction()
            {
                Action = RolloutAction.ACTION_PREPARE_LAUNCH,
                EffectiveDate = DateTime.Now.ToUniversalTime()
            };
            RolloutActionResponse raresp = brickst.DoRolloutAction((long)camp.Id, (long)inter.Id, raction, out status, out statusMessage);
            if (statusMessage != null)
            {
                Console.WriteLine("ERROR ROLLOUT ACTION 1: " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(raresp);
            Assert.AreEqual(RolloutActionResponse.STATUS_OK, raresp.Status);

            raction = new RolloutAction()
            {
                Action = RolloutAction.ACTION_LAUNCH
            };
            raresp = brickst.DoRolloutAction((long)camp.Id, (long)inter.Id, raction, out status, out statusMessage);
            if (statusMessage != null)
            {
                Console.WriteLine("ERROR ROLLOUT ACTION 2: " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(raresp);
            Assert.AreEqual(RolloutActionResponse.STATUS_OK, raresp.Status);

            //
            // event campaign is active; create a customer and submit events
            //
            string altCustId = "TEST" + DateTime.Now.Ticks.ToString();
            string email = altCustId + "@cmaeda.com";
            Customer cust = new Customer
            {
                AltCustomerId = altCustId,
                StatusCode = 1,
                EmailAddress = email,
                FirstName = "Test",
                LastName = "Testman"
            };
            Customer dbCust = brickst.AddCustomer(cust, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNull(statusMessage);
            Assert.IsNotNull(dbCust);
            Assert.IsNotNull(dbCust.Id);
            Assert.AreEqual(cust.AltCustomerId, dbCust.AltCustomerId);
            Assert.AreEqual(cust.EmailAddress, dbCust.EmailAddress);
            Assert.AreEqual(cust.LastName, dbCust.LastName);
            Assert.AreEqual(cust.FirstName, dbCust.FirstName);
            cust = dbCust;

            //
            // have customer, submit event
            //
            StringBuilder bld = new StringBuilder();
            bld.Append("<purchase>");
            bld.Append("<customer_name>");
            bld.Append(cust.FirstName).Append(" ").Append(cust.LastName);
            bld.Append("</customer_name>");
            bld.Append("<line_items>");
            // LINE ITEM...
            bld.Append("<item>");
            bld.Append("<item_name>Hat</item_name>");
            bld.Append("<item_quantity>1</item_quantity>");
            bld.Append("<item_price>9.95</item_price>");
            bld.Append("</item>");
            // LINE ITEM...
            bld.Append("<item>");
            bld.Append("<item_name>Shoes</item_name>");
            bld.Append("<item_quantity>1</item_quantity>");
            bld.Append("<item_price>19.95</item_price>");
            bld.Append("</item>");
            bld.Append("</line_items>");
            bld.Append("</purchase>");
            string xml = bld.ToString();

            Event tstEvent = new Event
            {
                EventName = campaignName,
                CustomerId = (long)cust.Id,
                Xml = xml,
                Subscribe = true
            };
            Event dbEvent = brickst.AddEvent(tstEvent, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNull(statusMessage);
            Assert.IsNotNull(dbEvent);
            Assert.IsNotNull(dbEvent.Id);
            Assert.AreEqual(tstEvent.EventName, dbEvent.EventName);
            Assert.AreEqual(tstEvent.CustomerId, dbEvent.CustomerId);
            Assert.AreEqual("1", dbEvent.Status);
            Console.WriteLine("SUBMITTED EVENT " + dbEvent.Id);
        }
    }
}
