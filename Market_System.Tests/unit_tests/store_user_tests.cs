using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NUnit.Framework;
using Market_System.Domain_Layer.Store_Component;
using Market_System.Domain_Layer.User_Component;

namespace Market_System.Tests.unit_tests
{
    [TestClass]
    public class store_user_tests
    {
        private Market_System.Domain_Layer.MarketSystem m;
        private Market_System.Domain_Layer.User_Component.UserFacade user_facade;
        private Market_System.Domain_Layer.Store_Component.StoreFacade store_facade;


        //i dont know how to do after each and not sure if this is before each so do setup and detroy after each

        //[TestInitialize] // this means before each
        public void Setup() 
        {
            user_facade= Market_System.Domain_Layer.User_Component.UserFacade.GetInstance();
            store_facade = Market_System.Domain_Layer.Store_Component.StoreFacade.GetInstance();
        }

        public void Destory_all_singletons()
        {
            store_facade.Destroy_me();
            user_facade.Destroy_me();
        }

        [Test]
        public void register_user_test()
        {
            Setup();
            user_facade.register("test1", "pass");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(true, user_facade.check_if_user_exists("test1", "pass"));
            Destory_all_singletons();
        }
    }


    
}
