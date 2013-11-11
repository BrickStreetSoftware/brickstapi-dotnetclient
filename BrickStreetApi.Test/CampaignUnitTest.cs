/*
 * Brick Street Connect Web Services API Client
 * Copyright (c) 2013 Brick Street Software, Inc.
 * http://brickstreetsoftware.com
 * This code open source and governed by the Apache Software License.
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BrickStreetAPI;
using BrickStreetAPI.Connect;


namespace BrickStreetApi.Test
{
    [TestClass]
    public class CampaignUnitTest
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
        public void SendersGetAll()
        {
            BrickStreetConnect brickst = makeClient();

            HttpStatusCode status;
            List<Sender> senders = brickst.GetSenders(out status);

            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(senders);
        }

        [TestMethod]
        public void CampaignGetByName()
        {
            BrickStreetConnect brickst = makeClient();

            HttpStatusCode status;
            string statusMessage;
            EventCampaign ecamp = brickst.GetEventCampaignByName("Receipt Store 1", out status, out statusMessage);

            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(ecamp);
        }

        [TestMethod]
        public void ReadCampaignWithExpirationDate()
        {
            BrickStreetConnect brickst = makeClient();

            HttpStatusCode status;
            string statusMessage;
            EventCampaign ec = brickst.GetEventCampaign(1021, out status, out statusMessage);
            Assert.AreEqual(1021, ec.Id);
        }

        [TestMethod]
        public void CampaignEventTriggeredForReceipt()
        {
            BrickStreetConnect brickst = makeClient();
            //
            // verify that conversation exists
            //
            // CONV NAME: "TEST ERECEIPT CONVERSATION"
            //
            // SPENTLY: SHOULD CREATE A CONVERSATION OBJECT FOR EACH SPENTLY ACCOUNT
            HttpStatusCode status;
            string statusMessage = null;
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

            //
            // create event params
            //
            EventParameterMaster evParam;

            evParam = new EventParameterMaster()
            {
                Name = "shopName",
                DataType = EventParameterMaster.TYPE_STRING
            };
            eventMaster.Parameters.Add(evParam);

            evParam = new EventParameterMaster()
            {
                Name = "shopAddress",
                DataType = EventParameterMaster.TYPE_STRING
            };
            eventMaster.Parameters.Add(evParam);

            evParam = new EventParameterMaster()
            {
                Name = "shopPhone",
                DataType = EventParameterMaster.TYPE_STRING
            };
            eventMaster.Parameters.Add(evParam);

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
                //HtmlContentURL = "http://content.brickst.net/eNotify/spently/eReceipt/spently-receipt2.xsl",
                HtmlContentURL = "http://www.geocities.jp/bssjapantest01/ereceipt/proj1.xsl",
                CampaignID = (long) camp.Id
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
        }


    }
}
