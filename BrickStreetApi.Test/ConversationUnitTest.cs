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

