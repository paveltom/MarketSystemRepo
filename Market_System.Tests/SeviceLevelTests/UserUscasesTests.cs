
using Market_System.ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

 

namespace Market_System.Tests.ServiceLevelTests
{
    /// <summary>
    /// tests in this class:
    /// 1.regetretion.
    ///     1.1 Successful registration
    ///     1.2 Failed registration - used username
    ///     1.3 Failed registration - used password
    ///     1.4 *email.....
    ///     1.5 
    /// 2.login
    ///     2.1
    ///     2.2
    ///     2.3
    ///     2.4
    /// 3.add product to bucket member
    ///     3.1
    ///     3.2
    ///     3.3
    ///     3.4
    /// 4.add product to bucket guest
    ///     4.1
    ///     4.2
    ///     4.3
    ///     4.4
    /// </summary>

    [TestClass]
    public class UserUscasesTests
    {
        private Service_Controller service_Controller;

        public void setup()
        {
            service_Controller = new Service_Controller();
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
            setup();
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            service_Controller.destroy();
        }

        [TestMethod]
        //1.1
        public void UserRegistersAsMemberAndLogin()
        {
            //Setup: none

            //Action:
            Response<string> response =service_Controller.register("user1", "pass1", "add1");

            //Result:
            Assert.IsNotNull(response.Value);
            Assert.Equals(false, response.ErrorOccured);

            Response<string> responseLogin = service_Controller.login_member("user1", "pass1");
            Assert.Equals(false, responseLogin.ErrorOccured);

            //tearDown: (TestCleanup())
        }

        public void FailUserRegistersUsedUserame()
        {
            //Setup: none

            //Action:
            Response<string> response1 = service_Controller.register("user1", "pass1", "add1");
            Response<string> response2 = service_Controller.register("user1", "pass2", "add2");

            //Result:
            Assert.Equals(true, response2.ErrorOccured);

            //tearDown: (TestCleanup())
        }

    }
}