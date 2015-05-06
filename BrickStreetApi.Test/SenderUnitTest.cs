using System;
using System.Configuration;
using System.Net;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BrickStreetAPI;
using BrickStreetAPI.Connect;


namespace BrickStreetApi.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class SenderUnitTest
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

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void SendersGetAll()
        {
            BrickStreetConnect brickst = makeClient();

            HttpStatusCode status;
            String statusMessage;
            List<Sender> senders = brickst.GetSenders(out status, out statusMessage);

            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(senders);

            foreach (Sender s in senders)
            {
                long id = s.Id.Value;
                Sender fetched = brickst.GetSender(id, out status, out statusMessage);
                Assert.AreEqual(HttpStatusCode.OK, status);
            }
        }

        [TestMethod]
        public void SenderCRU()
        {
            BrickStreetConnect brickst = makeClient();

            HttpStatusCode status;
            String statusMessage;

            Sender s1 = new Sender();
            s1.Name = "My Test Sender";
            s1.EmailAddress = "mytestsender";
            s1.DepartmentID = ConnectDepartmentID;
            s1.DefaultSender = false;

            Sender s2 = brickst.AddSender(s1, out status, out statusMessage);

            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(s2);
            Assert.IsTrue(s2.Id.HasValue);
            Assert.AreEqual(s2.Name, s1.Name);
            Assert.AreEqual(s2.EmailAddress, s1.EmailAddress);
            Assert.AreEqual(s2.DepartmentID, s1.DepartmentID);
            Assert.AreEqual(s2.DefaultSender, s1.DefaultSender);

            s2.EmailAddress = "testsender";

            Sender s3 = brickst.UpdateSender(s2, out status, out statusMessage);

            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(s3);
            Assert.IsTrue(s3.Id.HasValue);
            Assert.AreEqual(s3.Id.Value, s2.Id.Value);
            Assert.AreEqual(s3.Name, s2.Name);
            Assert.AreEqual(s3.EmailAddress, s2.EmailAddress);
            Assert.AreEqual(s3.DepartmentID, s2.DepartmentID);
            Assert.AreEqual(s3.DefaultSender, s2.DefaultSender);
        }

        [TestMethod]
        public void SenderDomainsGetAll()
        {
            BrickStreetConnect brickst = makeClient();

            HttpStatusCode status;
            String statusMessage;

            List<SenderDomain> domains = brickst.GetSenderDomains(out status, out statusMessage);
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(domains);

            foreach (SenderDomain d in domains)
            {
                long id = d.Id.Value;
                SenderDomain fetched = brickst.GetSenderDomain(id, out status, out statusMessage);
                Assert.AreEqual(HttpStatusCode.OK, status);
            }
        }

        [TestMethod]
        public void SenderDomainCRU()
        {
            BrickStreetConnect brickst = makeClient();

            HttpStatusCode status;
            String statusMessage;

            SenderDomain s1 = new SenderDomain();
            s1.Name = "example.com";
            s1.SigningAlg = 0;
            s1.DefaultDomain = false;

            SenderDomain s2 = brickst.AddSenderDomain(s1, out status, out statusMessage);

            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(s2);
            Assert.IsTrue(s2.Id.HasValue);
            Assert.AreEqual(s2.Name, s1.Name);
            Assert.AreEqual(s2.SigningAlg, s1.SigningAlg);  
            Assert.AreEqual(s2.DefaultDomain, s1.DefaultDomain);

            s2.Name = "example2.com";

            SenderDomain s3 = brickst.UpdateSenderDomain(s2, out status, out statusMessage);

            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNotNull(s3);
            Assert.IsTrue(s3.Id.HasValue);
            Assert.AreEqual(s3.Id.Value, s2.Id.Value);
            Assert.AreEqual(s3.Name, s2.Name);
            Assert.AreEqual(s3.SigningAlg, s2.SigningAlg);
            Assert.AreEqual(s3.DefaultDomain, s2.DefaultDomain);
        }

    }
}
