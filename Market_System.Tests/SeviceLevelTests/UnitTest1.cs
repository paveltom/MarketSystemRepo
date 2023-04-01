
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Market_System.Service_Layer;
using Market_System.ServiceLayer;

namespace Market_System.Tests.ServiceLevelTests
{
    /// <summary>
    /// Summary description for UserTests
    /// </summary>
    [TestClass]
    public class UserTests
    {
        public UserTests()
        {
            // TODO: Add constructor logic here  
        }

        private Service_Controller service_Controller;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public Service_Controller Service_Controller
        {
            get
            {
                return this.service_Controller;
            }
            set
            {
                this.Service_Controller = value;
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

        //ClassInitialize runs before running the first test in the class
        [ClassInitialize()]
        public void ClassInitialize()
        {

        }

        [TestMethod]
        public void UserRegistersAsMember()
        {
            //Setup: none

            //Action:
            //Response response =Service_Controller.register();

            //Result:
            //1.Response->not null meaning the registration completed without error
            //2:chacking if member is now in the system by doing login.
            //Response response2 = Service_Controller.login_member();

            //tearDown
            //Service_Controller.
        }
    }
    /*points coming from writing tests:
     * 
     * 1.implementing a response, or other ansewring mechanizem that will pass service caller success/fail signal
     * 2.implement register
     * 3.we talked about mabye the defult state of a user entering
     *   to the system should be automatycly guest.
     *   in order to let him see as fast as posibble products to keep him entreeged.
     *   also there is not much functionality being a user without pressing continue as guest.
     * 4.add unregister(memberID);
     * 
     */
}