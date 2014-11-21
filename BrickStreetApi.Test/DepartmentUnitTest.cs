using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using BrickStreetAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using BrickStreetAPI.Connect;

namespace BrickStreetApi.Test
{
    [TestClass]
    public class DepartmentUnitTest
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
        public void DepartmentGets()
        {
            BrickStreetConnect brickStreetConnect = makeClient();
            HttpStatusCode status;
            string statusMessage;

            //
            // fetch all depts
            //
            List<Department> allDepts = brickStreetConnect.GetDepartments(out status, out statusMessage);
            Assert.IsTrue(status == HttpStatusCode.OK);
            Assert.IsNotNull(allDepts);
            Assert.IsTrue(allDepts.Count >= 1);
            
            // find default dept
            Department defaultDept = null;
            foreach (Department dept in allDepts)
            {
                if (dept.Name == "Default")
                {
                    defaultDept = dept;
                    break;
                }
            }
            Assert.IsNotNull(defaultDept);

            //
            // fetch individual depts and verify that they are the same
            //
            foreach (Department dept in allDepts)
            {
                Department fetchedDept = brickStreetConnect.GetDepartment(dept.Id.Value, out status, out statusMessage);
                Assert.IsTrue(status == HttpStatusCode.OK);
                Assert.IsNotNull(fetchedDept);
                Assert.IsTrue(fetchedDept.Id.Value == dept.Id.Value);
                Assert.IsTrue(fetchedDept.Name == dept.Name);
                // NOTE: GetDepartments only returns ID and Name fields
                // Assert.IsTrue(fetchedDept.ProfileId.Value == dept.ProfileId.Value);
            }

        }
    }
}
