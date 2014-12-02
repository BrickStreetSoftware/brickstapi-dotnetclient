using System;
using System.Collections;
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

        [TestMethod]
        public void AddCustomerWithAttribute()
        {
            String attributeName = ConfigurationManager.AppSettings["AttrName"];//this will be probably fetched from app.config
            String attributeValue = ConfigurationManager.AppSettings["AttrValue"];
            HttpStatusCode status;
            string statusMessage;
            BrickStreetConnect brickStreetConnect = makeClient();
            var attrDef = brickStreetConnect.GetCustomerAttribute(attributeName, out status, out statusMessage);
            Assert.AreNotEqual(attrDef, "Attribute Not Found");

            //create new customer
            Random r = new Random();
            long val = r.Next();
            if (val < 0)
            {
                val *= -1;
            }

            String custVal = "cmaeda" + val + "@cmaeda.com";

            Customer c = new Customer();
            c.EmailAddress = custVal;
            c.AltCustomerId = custVal;
            c.AddressLine1 = "215 S Broadway 241";
            c.City = "Salem";
            c.State = "NH";
            c.Country = "USA";

            Customer c2 = brickStreetConnect.AddCustomer(c, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }

            Assert.IsNotNull(c2);
            Assert.IsNotNull(c2.Id);
            Assert.AreEqual(c2.EmailAddress, custVal);
            Assert.AreEqual(c2.AltCustomerId, custVal);

            CustomerAttribute attr = c2.GetAttribute(attributeName);
            if (attr == null)
            {
                attr = new CustomerAttribute();
                attr.Name = attrDef.Name;
                attr.Type = attrDef.Type;
                attr.DataType = attrDef.DataType;
                attr.Value = attributeValue;

                c2.Attributes.Add(attr);
            }
            else
            {
                attr.Value = attributeValue;
            }

            Customer c3 = brickStreetConnect.UpdateCustomer(c2, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }

            Assert.IsNotNull(c3, "Customer 3 is null");

            CustomerAttribute c3attr = c3.GetAttribute(attributeName);
            Assert.IsNotNull(c3attr);
            Assert.AreEqual(c3attr.Value, attributeValue);
        }

        [TestMethod]
        public void AddCustomerWithPreference()
        {
            string preferenceName = ConfigurationManager.AppSettings["PreferenceName"];
            HttpStatusCode status;
            string statusMessage;
            BrickStreetConnect brickStreetConnect = makeClient();

            BrickStAPI.Connect.Attribute attrDef = brickStreetConnect.GetCustomerAttribute(preferenceName, out status,
                out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }

            Assert.IsNotNull(attrDef);
            string attrType = attrDef.Type;
            //create new customer
            Random r = new Random();
            long val = r.Next();
            if (val < 0)
            {
                val *= -1;
            }

            String custVal = "cmaeda+" + val + "@cmaeda.com";

            Customer c = new Customer();
            c.EmailAddress = custVal;
            c.AltCustomerId = custVal;
            c.AddressLine1 = "215 S Broadway 241";
            c.City = "Salem";
            c.State = "NH";
            c.Country = "USA";

            Customer c2 = brickStreetConnect.AddCustomer(c, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }

            Assert.IsNotNull(c2, "Customer is null");
            Assert.IsNotNull(c2.Id, "Customer ID is null");
            Assert.AreEqual(c2.EmailAddress, custVal);
            Assert.AreEqual(c2.AltCustomerId, custVal);

            c = c2;

            //
            //add preference
            //

            CustomerAttribute attr = null;
            if (string.Compare("preference", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                attr = c.GetAttribute(preferenceName);
                if (attr == null)
                {
                    attr = new CustomerAttribute();
                    attr.Name = attrDef.Name;
                    attr.Type = attrDef.Type;
                    attr.DataType = attrDef.DataType;

                    //start with 1 value
                    attr.PreferenceValues = new string[1];
                    attr.PreferenceValues[0] = "pval" + val;
                    c.Attributes.Add(attr);
                }
                else
                {
                    string[] vals = attr.PreferenceValues;
                    string[] newVals = new string[vals.Length + 1];
                    Array.Copy(vals, 0, newVals, 0, vals.Length);
                    newVals[newVals.Length - 1] = "pval" + val;
                    attr.PreferenceValues = newVals;
                }
            }
            else if (string.Compare("multiaddress", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                //add a channel address
                attr = c.GetChannelAddress(preferenceName);
                if (attr == null)
                {
                    attr = new CustomerAttribute();
                    attr.Name = attrDef.Name;
                    attr.Type = attrDef.Type;
                    attr.DataType = attrDef.DataType;

                    //start with 1 value
                    attr.PreferenceValues = new string[1];
                    attr.PreferenceValues[0] = "pval" + val;
                    c.ChannelAddresses.Add(attr);
                }
                else
                {
                    string[] vals = attr.PreferenceValues;
                    string[] newVals = new string[vals.Length + 1];
                    Array.Copy(vals, 0, newVals, 0, vals.Length);
                    newVals[newVals.Length - 1] = "pval" + val;
                    attr.PreferenceValues = newVals;
                }

            }
            else
            {
                Assert.IsTrue(false, "Unknown pref type " + attrType);
            }

            //save orig value
            string[] prefVals = attr.PreferenceValues;

            c2 = brickStreetConnect.UpdateCustomer(c, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }

            Assert.IsNotNull(c2, "Customer 2 is null");

            //check saved data
            CustomerAttribute c2attr = null;
            if (string.Compare("preference", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                c2attr = c2.GetAttribute(preferenceName);
            }
            else if (string.Compare("multiaddress", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                c2attr = c2.GetChannelAddress(preferenceName);
            }

            Assert.IsNotNull(c2attr);
            // unordered set compare
            CollectionAssert.AreEquivalent(prefVals,c2attr.PreferenceValues);
           
            c = c2;

            //
            //add a second pref value
            //

            //new random value
            val = r.Next();
            if (val < 0)
            {
                val *= -1;
            }

            if (string.Compare("preference", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                attr = c.GetAttribute(preferenceName);
                Assert.IsNotNull(attr);

                //add a value
                string[] vals = attr.PreferenceValues;
                string[] newVals = new string[vals.Length + 1];
                Array.Copy(vals, 0, newVals, 0, vals.Length);
                newVals[newVals.Length - 1] = "pval" + val;
                attr.PreferenceValues = newVals;

            }
            else if (string.Compare("multiaddress", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                //add a channel
                attr = c.GetChannelAddress(preferenceName);
                Assert.IsNotNull(attr);

                string[] vals = attr.PreferenceValues;
                string[] newVals = new string[vals.Length + 1];
                Array.Copy(vals, 0, newVals, 0, vals.Length);
                newVals[newVals.Length - 1] = "pval" + val;
                attr.PreferenceValues = newVals;
            }
            else
            {
                Assert.IsTrue(false, "Unknown pref type " + attrType);
            }

            prefVals = attr.PreferenceValues;
            c2 = brickStreetConnect.UpdateCustomer(c, out status, out statusMessage);
            Assert.IsNotNull(c2, "Customer 2 is null");

            //check saved data
            c2attr = null;
            if (string.Compare("preference", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                c2attr = c2.GetAttribute(preferenceName);
            }
            else if (string.Compare("multiaddress", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                c2attr = c2.GetChannelAddress(preferenceName);
            }

            Assert.IsNotNull(c2attr);
            CollectionAssert.AreEquivalent(prefVals,c2attr.PreferenceValues);

            c = c2;

            //
            //remove a preference
            //
            if (string.Compare("preference", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                attr = c2.GetAttribute(preferenceName);
            }
            else if (string.Compare("multiaddress", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                attr = c2.GetChannelAddress(preferenceName);
            }

            Assert.IsNotNull(attr);

            //remove first value
            string[] oldVals = attr.PreferenceValues;
            string[] newVals1 = new string[oldVals.Length - 1];
            Array.Copy(oldVals, 1, newVals1, 0, newVals1.Length);
            attr.PreferenceValues = newVals1;

            prefVals = attr.PreferenceValues;
            c2 = brickStreetConnect.UpdateCustomer(c, out status, out statusMessage);
            Assert.AreNotEqual(c2, "Customer 2 is null");

            //check saved data
            c2attr = null;
            if (string.Compare("preference", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                c2attr = c2.GetAttribute(preferenceName);
            }
            else if (string.Compare("multiaddress", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                c2attr = c2.GetChannelAddress(preferenceName);
            }

            Assert.IsNotNull(c2attr);
            CollectionAssert.AreEquivalent(prefVals, c2attr.PreferenceValues);

        }

        [TestMethod]
        public void AddCustomerWithMultiaddress()
        {
            string preferenceName = ConfigurationManager.AppSettings["MultiaddressName"];
            HttpStatusCode status;
            string statusMessage;
            BrickStreetConnect brickStreetConnect = makeClient();

            BrickStAPI.Connect.Attribute attrDef = brickStreetConnect.GetCustomerAttribute(preferenceName, out status,
                out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }

            Assert.IsNotNull(attrDef);
            string attrType = attrDef.Type;
            //create new customer
            Random r = new Random();
            long val = r.Next();
            if (val < 0)
            {
                val *= -1;
            }

            String custVal = "cmaeda+" + val + "@cmaeda.com";

            Customer c = new Customer();
            c.EmailAddress = custVal;
            c.AltCustomerId = custVal;
            c.AddressLine1 = "215 S Broadway 241";
            c.City = "Salem";
            c.State = "NH";
            c.Country = "USA";

            Customer c2 = brickStreetConnect.AddCustomer(c, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }

            Assert.IsNotNull(c2, "Customer is null");
            Assert.IsNotNull(c2.Id, "Customer ID is null");
            Assert.AreEqual(c2.EmailAddress, custVal);
            Assert.AreEqual(c2.AltCustomerId, custVal);

            c = c2;

            //
            //add preference
            //

            CustomerAttribute attr = null;
            if (string.Compare("preference", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                attr = c.GetAttribute(preferenceName);
                if (attr == null)
                {
                    attr = new CustomerAttribute();
                    attr.Name = attrDef.Name;
                    attr.Type = attrDef.Type;
                    attr.DataType = attrDef.DataType;

                    //start with 1 value
                    attr.PreferenceValues = new string[1];
                    attr.PreferenceValues[0] = "pval" + val;
                    c.Attributes.Add(attr);
                }
                else
                {
                    string[] vals = attr.PreferenceValues;
                    string[] newVals = new string[vals.Length + 1];
                    Array.Copy(vals, 0, newVals, 0, vals.Length);
                    newVals[newVals.Length - 1] = "pval" + val;
                    attr.PreferenceValues = newVals;
                }
            }
            else if (string.Compare("multiaddress", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                //add a channel address
                attr = c.GetChannelAddress(preferenceName);
                if (attr == null)
                {
                    attr = new CustomerAttribute();
                    attr.Name = attrDef.Name;
                    attr.Type = attrDef.Type;
                    attr.DataType = attrDef.DataType;

                    //start with 1 value
                    attr.PreferenceValues = new string[1];
                    attr.PreferenceValues[0] = "pval" + val;
                    c.ChannelAddresses.Add(attr);
                }
                else
                {
                    string[] vals = attr.PreferenceValues;
                    string[] newVals = new string[vals.Length + 1];
                    Array.Copy(vals, 0, newVals, 0, vals.Length);
                    newVals[newVals.Length - 1] = "pval" + val;
                    attr.PreferenceValues = newVals;
                }

            }
            else
            {
                Assert.IsTrue(false, "Unknown pref type " + attrType);
            }

            //save orig value
            string[] prefVals = attr.PreferenceValues;

            c2 = brickStreetConnect.UpdateCustomer(c, out status, out statusMessage);
            if (status != HttpStatusCode.OK)
            {
                Console.WriteLine("ERROR: STATUS:" + status.ToString() + " " + statusMessage);
            }

            Assert.IsNotNull(c2, "Customer 2 is null");

            //check saved data
            CustomerAttribute c2attr = null;
            if (string.Compare("preference", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                c2attr = c2.GetAttribute(preferenceName);
            }
            else if (string.Compare("multiaddress", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                c2attr = c2.GetChannelAddress(preferenceName);
            }

            Assert.IsNotNull(c2attr);
            CollectionAssert.AreEquivalent(prefVals, c2attr.PreferenceValues);

            c = c2;

            //
            //add a second pref value
            //

            //new random value
            val = r.Next();
            if (val < 0)
            {
                val *= -1;
            }

            if (string.Compare("preference", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                attr = c.GetAttribute(preferenceName);
                Assert.IsNotNull(attr);

                //add a value
                string[] vals = attr.PreferenceValues;
                string[] newVals = new string[vals.Length + 1];
                Array.Copy(vals, 0, newVals, 0, vals.Length);
                newVals[newVals.Length - 1] = "pval" + val;
                attr.PreferenceValues = newVals;

            }
            else if (string.Compare("multiaddress", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                //add a channel
                attr = c.GetChannelAddress(preferenceName);
                Assert.IsNotNull(attr);

                string[] vals = attr.PreferenceValues;
                string[] newVals = new string[vals.Length + 1];
                Array.Copy(vals, 0, newVals, 0, vals.Length);
                newVals[newVals.Length - 1] = "pval" + val;
                attr.PreferenceValues = newVals;
            }
            else
            {
                Assert.IsTrue(false, "Unknown pref type " + attrType);
            }

            prefVals = attr.PreferenceValues;
            c2 = brickStreetConnect.UpdateCustomer(c, out status, out statusMessage);
            Assert.IsNotNull(c2, "Customer 2 is null");

            //check saved data
            c2attr = null;
            if (string.Compare("preference", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                c2attr = c2.GetAttribute(preferenceName);
            }
            else if (string.Compare("multiaddress", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                c2attr = c2.GetChannelAddress(preferenceName);
            }

            Assert.IsNotNull(c2attr);
            CollectionAssert.AreEquivalent(prefVals, c2attr.PreferenceValues);

            c = c2;

            //
            //remove a preference
            //
            if (string.Compare("preference", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                attr = c2.GetAttribute(preferenceName);
            }
            else if (string.Compare("multiaddress", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                attr = c2.GetChannelAddress(preferenceName);
            }

            Assert.IsNotNull(attr);

            //remove first value
            string[] oldVals = attr.PreferenceValues;
            string[] newVals1 = new string[oldVals.Length - 1];
            Array.Copy(oldVals, 1, newVals1, 0, newVals1.Length);
            attr.PreferenceValues = newVals1;

            prefVals = attr.PreferenceValues;
            c2 = brickStreetConnect.UpdateCustomer(c, out status, out statusMessage);
            Assert.AreNotEqual(c2, "Customer 2 is null");

            //check saved data
            c2attr = null;
            if (string.Compare("preference", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                c2attr = c2.GetAttribute(preferenceName);
            }
            else if (string.Compare("multiaddress", attrType, StringComparison.OrdinalIgnoreCase) == 0)
            {
                c2attr = c2.GetChannelAddress(preferenceName);
            }

            Assert.IsNotNull(c2attr);
            CollectionAssert.AreEquivalent(prefVals, c2attr.PreferenceValues);

        }
    
    
    }
}
