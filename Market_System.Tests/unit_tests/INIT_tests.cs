using Market_System.DomainLayer;
using Market_System.ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Market_System.Tests.unit_tests
{
    [TestClass]
    public class INIT_tests
    {


        private MarketSystem ms;
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void Setup()
        {
            Logger.get_instance().change_logger_path_to_tests();
            ms = MarketSystem.GetInstance();
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TearDown()
        {
            Logger.get_instance().change_logger_path_to_regular();
            ms.destroy_me();

        }

        [TestMethod]
        public void read_from_init_success()
        {
           
            Service_Controller sv = new Service_Controller();
            sv.read_from_config_file("INIT_test_success.txt");
            Assert.IsTrue(sv.login_member("bayan","pass").Value.Equals("Logged-In succesfully"));
            Assert.IsTrue(sv.get_stores_that_user_works_in().Value[0].Contains("sword_store"));
            Assert.IsTrue(sv.get_not_zero_quantity_products_from_all_shop().Value[2].Name.Equals("rune_scimitar")); // here the list that returns has 3 items , first 2 are the defualt items , and the third one is rune scimitar



        }

        [TestMethod]
        public void read_from_init_fail()
        {
            Service_Controller sv = new Service_Controller();
            sv.read_from_config_file("‏‏INIT_test_fail.txt");
            Assert.IsTrue(sv.login_member("bayan", "pass").ErrorMessage.Equals("Incorrect login information has been provided")); // this checks that all action are undone
            Assert.IsTrue(Logger.get_instance().Read_Errors_Record().Contains(DateTime.Now.ToLongDateString() + " : " + "reading from configartion file failed due to: Incorrect login information has been provided starting system without any configration")); // this checks that we record this in eror log

        }

    }
    }
