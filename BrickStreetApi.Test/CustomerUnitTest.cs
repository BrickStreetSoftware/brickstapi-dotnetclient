using System;
using System.Configuration;
using System.Net;
using BrickStreetAPI;
using BrickStreetAPI.Connect;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BrickStreetApi.Test
{
    [TestClass]
    public class CustomerUnitTest
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
        public void CustomerApiTest()
        {
            string altCustId = "TEST" + DateTime.Now.Ticks.ToString();
            string email = altCustId + "@example.com";
            Customer cust = new Customer
            {
                AltCustomerId = altCustId,
                StatusCode = 1,
                EmailAddress = email,
                FirstName = "Test",
                LastName = "Testman"
            };

            HttpStatusCode status;
            string statusMessage;
            BrickStreetConnect brickStreetConnect = makeClient();

            //
            // create new customer record
            //

            Customer dbCust = brickStreetConnect.AddCustomer(cust, out status, out statusMessage);
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

            //
            // now try the get methods on the new customer
            //

            Customer getCust = brickStreetConnect.GetCustomer((long)dbCust.Id, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNull(statusMessage);
            Assert.IsNotNull(getCust);
            Assert.IsNotNull(getCust.Id);
            Assert.AreEqual(dbCust.AltCustomerId, getCust.AltCustomerId);
            Assert.AreEqual(dbCust.EmailAddress, getCust.EmailAddress);
            Assert.AreEqual(dbCust.LastName, getCust.LastName);
            Assert.AreEqual(dbCust.FirstName, getCust.FirstName);

            getCust = brickStreetConnect.GetCustomerByAltId(altCustId, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNull(statusMessage);
            Assert.IsNotNull(getCust);
            Assert.IsNotNull(getCust.Id);
            Assert.AreEqual(dbCust.AltCustomerId, getCust.AltCustomerId);
            Assert.AreEqual(dbCust.EmailAddress, getCust.EmailAddress);
            Assert.AreEqual(dbCust.LastName, getCust.LastName);
            Assert.AreEqual(dbCust.FirstName, getCust.FirstName);

            getCust = brickStreetConnect.GetCustomerByEmail(email, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNull(statusMessage);
            Assert.IsNotNull(getCust);
            Assert.IsNotNull(getCust.Id);
            Assert.AreEqual(dbCust.AltCustomerId, getCust.AltCustomerId);
            Assert.AreEqual(dbCust.EmailAddress, getCust.EmailAddress);
            Assert.AreEqual(dbCust.LastName, getCust.LastName);
            Assert.AreEqual(dbCust.FirstName, getCust.FirstName);

            //
            // update the customer
            //
            getCust.FirstName = "Testy";
            getCust.LastName = "Testyman";
            getCust.AddressLine1 = "123 Oak St";
            getCust.City = "Oakland";
            getCust.State = "CA";
            dbCust = brickStreetConnect.UpdateCustomer(getCust, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNull(statusMessage);
            Assert.IsNotNull(dbCust);
            Assert.IsNotNull(dbCust.Id);
            Assert.AreEqual((long)getCust.Id, (long)dbCust.Id);
            Assert.AreEqual(getCust.AltCustomerId, dbCust.AltCustomerId);
            Assert.AreEqual(getCust.EmailAddress, dbCust.EmailAddress);
            Assert.AreEqual(getCust.LastName, dbCust.LastName);
            Assert.AreEqual(getCust.FirstName, dbCust.FirstName);
            Assert.AreEqual(getCust.AddressLine1, dbCust.AddressLine1);
            Assert.AreEqual(getCust.City, dbCust.City);
            Assert.AreEqual(getCust.State, dbCust.State);

            //
            // dbCust is valid; recheck GET methods
            //

            getCust = brickStreetConnect.GetCustomer((long)dbCust.Id, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNull(statusMessage);
            Assert.IsNotNull(getCust);
            Assert.IsNotNull(getCust.Id);
            Assert.AreEqual(dbCust.AltCustomerId, getCust.AltCustomerId);
            Assert.AreEqual(dbCust.EmailAddress, getCust.EmailAddress);
            Assert.AreEqual(dbCust.LastName, getCust.LastName);
            Assert.AreEqual(dbCust.FirstName, getCust.FirstName);

            getCust = brickStreetConnect.GetCustomerByAltId(altCustId, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNull(statusMessage);
            Assert.IsNotNull(getCust);
            Assert.IsNotNull(getCust.Id);
            Assert.AreEqual(dbCust.AltCustomerId, getCust.AltCustomerId);
            Assert.AreEqual(dbCust.EmailAddress, getCust.EmailAddress);
            Assert.AreEqual(dbCust.LastName, getCust.LastName);
            Assert.AreEqual(dbCust.FirstName, getCust.FirstName);

            getCust = brickStreetConnect.GetCustomerByEmail(email, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNull(statusMessage);
            Assert.IsNotNull(getCust);
            Assert.IsNotNull(getCust.Id);
            Assert.AreEqual(dbCust.AltCustomerId, getCust.AltCustomerId);
            Assert.AreEqual(dbCust.EmailAddress, getCust.EmailAddress);
            Assert.AreEqual(dbCust.LastName, getCust.LastName);
            Assert.AreEqual(dbCust.FirstName, getCust.FirstName);        
        }

        [TestMethod]
        public void CustomerApiTestSpace()
        {
            // NB: space in value
            string altCustId = "TEST " + DateTime.Now.Ticks.ToString() + " TEST TEST";
            string email = altCustId + "@example.com";
            Customer cust = new Customer
            {
                AltCustomerId = altCustId,
                StatusCode = 1,
                EmailAddress = email,
                FirstName = "Test",
                LastName = "Testman"
            };

            HttpStatusCode status;
            string statusMessage;
            BrickStreetConnect brickStreetConnect = makeClient();

            //
            // create new customer record
            //

            Customer dbCust = brickStreetConnect.AddCustomer(cust, out status, out statusMessage);
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

            //
            // now try the get methods on the new customer
            //

            Customer getCust = brickStreetConnect.GetCustomer((long)dbCust.Id, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNull(statusMessage);
            Assert.IsNotNull(getCust);
            Assert.IsNotNull(getCust.Id);
            Assert.AreEqual(dbCust.AltCustomerId, getCust.AltCustomerId);
            Assert.AreEqual(dbCust.EmailAddress, getCust.EmailAddress);
            Assert.AreEqual(dbCust.LastName, getCust.LastName);
            Assert.AreEqual(dbCust.FirstName, getCust.FirstName);

            getCust = brickStreetConnect.GetCustomerByAltId(altCustId, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNull(statusMessage);
            Assert.IsNotNull(getCust);
            Assert.IsNotNull(getCust.Id);
            Assert.AreEqual(dbCust.AltCustomerId, getCust.AltCustomerId);
            Assert.AreEqual(dbCust.EmailAddress, getCust.EmailAddress);
            Assert.AreEqual(dbCust.LastName, getCust.LastName);
            Assert.AreEqual(dbCust.FirstName, getCust.FirstName);

            getCust = brickStreetConnect.GetCustomerByEmail(email, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNull(statusMessage);
            Assert.IsNotNull(getCust);
            Assert.IsNotNull(getCust.Id);
            Assert.AreEqual(dbCust.AltCustomerId, getCust.AltCustomerId);
            Assert.AreEqual(dbCust.EmailAddress, getCust.EmailAddress);
            Assert.AreEqual(dbCust.LastName, getCust.LastName);
            Assert.AreEqual(dbCust.FirstName, getCust.FirstName);

            //
            // update the customer
            //
            getCust.FirstName = "Testy";
            getCust.LastName = "Testyman";
            getCust.AddressLine1 = "123 Oak St";
            getCust.City = "Oakland";
            getCust.State = "CA";
            dbCust = brickStreetConnect.UpdateCustomer(getCust, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNull(statusMessage);
            Assert.IsNotNull(dbCust);
            Assert.IsNotNull(dbCust.Id);
            Assert.AreEqual((long)getCust.Id, (long)dbCust.Id);
            Assert.AreEqual(getCust.AltCustomerId, dbCust.AltCustomerId);
            Assert.AreEqual(getCust.EmailAddress, dbCust.EmailAddress);
            Assert.AreEqual(getCust.LastName, dbCust.LastName);
            Assert.AreEqual(getCust.FirstName, dbCust.FirstName);
            Assert.AreEqual(getCust.AddressLine1, dbCust.AddressLine1);
            Assert.AreEqual(getCust.City, dbCust.City);
            Assert.AreEqual(getCust.State, dbCust.State);

            //
            // dbCust is valid; recheck GET methods
            //

            getCust = brickStreetConnect.GetCustomer((long)dbCust.Id, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNull(statusMessage);
            Assert.IsNotNull(getCust);
            Assert.IsNotNull(getCust.Id);
            Assert.AreEqual(dbCust.AltCustomerId, getCust.AltCustomerId);
            Assert.AreEqual(dbCust.EmailAddress, getCust.EmailAddress);
            Assert.AreEqual(dbCust.LastName, getCust.LastName);
            Assert.AreEqual(dbCust.FirstName, getCust.FirstName);

            getCust = brickStreetConnect.GetCustomerByAltId(altCustId, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNull(statusMessage);
            Assert.IsNotNull(getCust);
            Assert.IsNotNull(getCust.Id);
            Assert.AreEqual(dbCust.AltCustomerId, getCust.AltCustomerId);
            Assert.AreEqual(dbCust.EmailAddress, getCust.EmailAddress);
            Assert.AreEqual(dbCust.LastName, getCust.LastName);
            Assert.AreEqual(dbCust.FirstName, getCust.FirstName);

            getCust = brickStreetConnect.GetCustomerByEmail(email, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsNull(statusMessage);
            Assert.IsNotNull(getCust);
            Assert.IsNotNull(getCust.Id);
            Assert.AreEqual(dbCust.AltCustomerId, getCust.AltCustomerId);
            Assert.AreEqual(dbCust.EmailAddress, getCust.EmailAddress);
            Assert.AreEqual(dbCust.LastName, getCust.LastName);
            Assert.AreEqual(dbCust.FirstName, getCust.FirstName);
        }    
    }
}
