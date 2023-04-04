
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Market_System.Service_Layer;
using Market_System.ServiceLayer;

namespace Market_System.Tests.ServiceLevelTests
{
    /// <summary>
    /// tests in this class:
    /// 1.regetretion.
    ///     1.1 
    ///     1.2
    ///     1.3
    ///     1.4
    /// 2.login
    ///     2.1
    ///     2.2
    ///     2.3
    ///     2.4
    /// 3.<######>
    ///     3.1
    ///     3.2
    ///     3.3
    ///     3.4
    /// 4.<######>
    ///     4.1
    ///     4.2
    ///     4.3
    ///     4.4
    /// </summary>
   
    [TestClass]
    public class UserUscasesTests
    {
        public UserUscasesTests()
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
        //1.1
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

        public void FailUserRegistersUsedUserame()
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

        public void FailUserRegistersUsedPassword()
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
     * 2.complete service layer signatures and return values
     * 3.we talked about mabye the defult state of a user entering
     *   to the system should be automatycly guest.
     *   in order to let him see as fast as posibble products to keep him entreeged.
     *   also there is not much functionality being a user without pressing continue as guest.
     * 4.add unregister(memberID) function;
     * 5.
     */
}