using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;

using BrickStreetAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BrickStreetAPI.Connect;

namespace BrickStreetApi.Test
{
    [TestClass]
    public class ConversationUnitTest
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
        public void ConversationAddGet()
        {
            BrickStreetConnect brickStreetConnect = makeClient();
            HttpStatusCode status;
            string statusMessage;

            string cname = "TEST" + DateTime.Now.Ticks.ToString();
            Conversation conv = new Conversation
            {
                Name = cname,
                DepartmentID = ConnectDepartmentID
            };
            Conversation dbConv = brickStreetConnect.AddConversation(conv, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(dbConv);
            Assert.IsNotNull(dbConv.Id);
            Assert.AreEqual(cname, dbConv.Name);

            //
            // test get methods
            //

            conv = brickStreetConnect.GetConversation((long)dbConv.Id, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(conv);
            Assert.IsNotNull(conv.Id);
            Assert.AreEqual(dbConv.Name, conv.Name);

            conv = brickStreetConnect.GetConversationByName(cname, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(conv);
            Assert.IsNotNull(conv.Id);
            Assert.AreEqual(dbConv.Name, conv.Name);        
        }

        [TestMethod]
        public void ConversationAddGetSenders()
        {
            BrickStreetConnect brickStreetConnect = makeClient();
            HttpStatusCode status;
            string statusMessage;

            // get sender name and domain
            Sender defSender = null;
            SenderDomain defDomain = null;
            
            //
            // get default sender
            //
            List<Sender> senders = brickStreetConnect.GetSenders(out status, out statusMessage);
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(senders);
            foreach (Sender s in senders)
            {
                Sender fetched = brickStreetConnect.GetSender(s.Id.Value, out status, out statusMessage);
                Assert.AreEqual(HttpStatusCode.OK, status);
                Assert.IsNotNull(fetched);
                Assert.IsTrue(fetched.DefaultSender.HasValue);
                if (fetched.DefaultSender.Value)
                {
                    defSender = fetched;
                    break;
                }
            }
            Assert.IsNotNull(defSender);

            //
            // get default sender domain
            //
            List<SenderDomain> domains = brickStreetConnect.GetSenderDomains(out status, out statusMessage);
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(domains);
            foreach (SenderDomain d in domains)
            {
                SenderDomain fetched = brickStreetConnect.GetSenderDomain(d.Id.Value, out status, out statusMessage);
                Assert.AreEqual(HttpStatusCode.OK, status);
                Assert.IsNotNull(fetched);
                Assert.IsTrue(fetched.DefaultDomain.HasValue);
                if (fetched.DefaultDomain.Value)
                {
                    defDomain = fetched;
                    break;
                }
            }
            Assert.IsNotNull(defDomain);
                

            string cname = "TEST" + DateTime.Now.Ticks.ToString();
            Conversation conv = new Conversation
            {
                Name = cname,
                DepartmentID = ConnectDepartmentID,
                SenderID = defSender.Id.Value,
                SenderDomain = defDomain.Id.Value
            };

            Conversation dbConv = brickStreetConnect.AddConversation(conv, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(dbConv);
            Assert.IsNotNull(dbConv.Id);
            Assert.AreEqual(cname, dbConv.Name);
            Assert.AreEqual(dbConv.SenderID.Value, defSender.Id.Value);
            Assert.AreEqual(dbConv.SenderDomain.Value, defDomain.Id.Value);

            //
            // test get methods
            //

            conv = brickStreetConnect.GetConversation((long)dbConv.Id, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(conv);
            Assert.IsNotNull(conv.Id);
            Assert.AreEqual(dbConv.Name, conv.Name);
            Assert.AreEqual(dbConv.SenderID.Value, defSender.Id.Value);
            Assert.AreEqual(dbConv.SenderDomain.Value, defDomain.Id.Value);

            conv = brickStreetConnect.GetConversationByName(cname, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(conv);
            Assert.IsNotNull(conv.Id);
            Assert.AreEqual(dbConv.Name, conv.Name);
            Assert.AreEqual(dbConv.SenderID.Value, defSender.Id.Value);
            Assert.AreEqual(dbConv.SenderDomain.Value, defDomain.Id.Value);
        }

        [TestMethod]
        public void ConversationAddGetSpace()
        {
            BrickStreetConnect brickStreetConnect = makeClient();
            HttpStatusCode status;
            string statusMessage;

            // NB: space in name
            string cname = "TEST " + DateTime.Now.Ticks.ToString() + " TEST TEST";
            Conversation conv = new Conversation
            {
                Name = cname,
                DepartmentID = ConnectDepartmentID
            };
            Conversation dbConv = brickStreetConnect.AddConversation(conv, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(dbConv);
            Assert.IsNotNull(dbConv.Id);
            Assert.AreEqual(cname, dbConv.Name);

            //
            // test get methods
            //

            conv = brickStreetConnect.GetConversation((long)dbConv.Id, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(conv);
            Assert.IsNotNull(conv.Id);
            Assert.AreEqual(dbConv.Name, conv.Name);

            conv = brickStreetConnect.GetConversationByName(cname, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(conv);
            Assert.IsNotNull(conv.Id);
            Assert.AreEqual(dbConv.Name, conv.Name);
        }

        [TestMethod]
        public void ConversationAddGetSpace2()
        {
            BrickStreetConnect brickStreetConnect = makeClient();
            HttpStatusCode status;
            string statusMessage;

            // NB: space in name

            string aname = "First Name";
            BrickStAPI.Connect.Attribute attr = brickStreetConnect.GetCustomerAttribute(aname, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(attr);
            Assert.IsNotNull(attr.Name);
            Assert.AreEqual(aname, attr.Name);
            
            
            string cname = "generic Spently Test Store";
            Conversation conv = brickStreetConnect.GetConversationByName(cname, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(conv);
            Assert.IsNotNull(conv.Id);
            Assert.AreEqual(cname, conv.Name);
        }

    }
}

