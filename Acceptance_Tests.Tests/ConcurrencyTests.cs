using Market_System.ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Collections.Concurrent;
using Market_System.DomainLayer;
using System.Collections.Generic;
using Market_System.DomainLayer.StoreComponent;
using System.Runtime.InteropServices;
using System.Collections;

namespace Market_System.Tests.SeviceLevelTests
{
    [TestClass]
    public class ConcurrencyTests
    {
        private Service_Controller service;
        private BlockingCollection<bool> boolCollection;
        private Response<StoreDTO> store;
        private Response<StoreDTO> store1;
        Response<ItemDTO> respo;
        Response<ItemDTO> respo1;


        public void Setup()
        {
            service = new Service_Controller();
            boolCollection = new BlockingCollection<bool>();
            // store1 = service.open_new_store(new List<string> { "store_1234" });

        }
        public void oneThreadCleanup()
        {
            service.destroy();
        }

        public void ThreadWork()
        {
            //Response response=
            Response<string> response = service.register("michel", "123456m", "add1");
            if (response.ErrorOccured == false)
            {
                boolCollection.TryAdd(true);
            }
            else
                boolCollection.TryAdd(false);
        }


        // 2 threads attempt to register with the same data at the same time, but only one should success!
        [TestMethod]
        public void ConcurrencyRegister()
        {
            //Setup
            Setup();

            //Action
            Thread thread1 = new Thread(() => ThreadWork());
            Thread thread2 = new Thread(() => ThreadWork());

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
            int num = 0;
            foreach (bool res in boolCollection)
            {
                if (res)
                    num++;
            }
            // Act
            Assert.IsTrue(num == 1);
            oneThreadCleanup();
        }


        //login
        // 2 threads attempt to login with the same data at the same time, but only one should success!
        /*[TestMethod]
        public void ConcurrencyLogIn()
        {
            //Setup
            Setup();
            SetupForLogIn();

            //Action
            Thread thread1 = new Thread(() => ThreadWork1());
            Thread thread2 = new Thread(() => ThreadWork1());

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
            int num = 0;
            foreach (bool res in boolCollection)
            {
                if (res)
                    num++;
            }
            // Act
            Assert.IsTrue(num == 1);
            oneThreadCleanup();
        }*/

        public void SetupForLogIn()
        {
            service.register("michel", "123456m", "add1");
        }
        public void ThreadWork1()
        {
            //Response response=
            Response<string> response = service.login_member("michel", "123456m");
            if (response.ErrorOccured == false)
            {
                boolCollection.TryAdd(true);
            }
            else
                boolCollection.TryAdd(false);
        }


        //logout
        // 2 threads attempt to logout with the same data at the same time, but only one should success!

        [TestMethod]
        public void ConcurrencyLogOut()
        {
            //Setup
            Setup();
            service.register("michel", "123456m", "add1");
            service.login_member("michel", "123456m");

            //Action
            Thread thread1 = new Thread(() => ThreadWork2());
            Thread thread2 = new Thread(() => ThreadWork2());

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
            int num = 0;
            foreach (bool res in boolCollection)
            {
                if (res)
                    num++;
            }
            // Act
            Assert.IsTrue(num == 1);
            oneThreadCleanup();
        }
        public void ThreadWork2()
        {
            //Response response=
            Response<string> response = service.log_out();
            if (response.ErrorOccured == false)
            {
                boolCollection.TryAdd(true);
            }
            else
                boolCollection.TryAdd(false);
        }


        // 3 threads attempt to RemoveFromBasket with the same data and have one product in basket ,at the same time, but only one shoul success!
        [TestMethod]
        public void ConcurrencyRemoveFromBasket()
        {
            //Setup
            Setup();
            service.register("user123", "123456m", "add1");
            service.login_member("user123", "123456m");
            store = service.open_new_store(new List<string> { "store_1234" });
            respo = service.add_product_to_store(store.Value.StoreID, "bamba", "desc1", "1", "3", "1", "5.0", "2.0", "77.0", "0.5_20.0", "Attr", "ctgr");
            service.add_product_to_basket(respo.Value.GetID(), "1");


            //Action
            Thread thread1 = new Thread(() => ThreadWorkRemoveFromBasket());
            Thread thread2 = new Thread(() => ThreadWorkRemoveFromBasket());
            Thread thread3 = new Thread(() => ThreadWorkRemoveFromBasket());

            thread1.Start();
            thread2.Start();
            thread3.Start();

            thread1.Join();
            thread2.Join();
            thread3.Join();
            int num = 0;
            foreach (bool res in boolCollection)
            {
                if (res)
                    num++;
            }
            // Act
            Assert.IsTrue(num == 1);
            oneThreadCleanup();
        }
        public void ThreadWorkRemoveFromBasket()
        {
            Response<string> response = service.remove_product_from_basket(respo.Value.GetID(), "1");
            if (response.ErrorOccured == false)
            {
                boolCollection.TryAdd(true);
            }
            else
                boolCollection.TryAdd(false);

        }

        // 2 threads attempt to add product to the store with same data at the same time, the two threads should success!

        [TestMethod]
        public void ConcurrencyAddNewProduct()
        {
            //Setup
            Setup();
            service.register("michellm", "123456m", "add1");
            //  service.register("michellm1", "123456m", "add1");


            service.login_member("michellm", "123456m");
            // service.AddNewAdmin("michell");

            store1 = service.open_new_store(new List<string> { "store_1234" });


            //Action
            Thread thread1 = new Thread(() => ThreadWorkAddNewProduct());
            Thread thread2 = new Thread(() => ThreadWorkAddNewProduct());

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
            int num = 0;
            foreach (bool res in boolCollection)
            {
                if (res)
                    num++;
            }
            // Act
            Assert.IsTrue(num == 2);
            oneThreadCleanup();
        }
        public void ThreadWorkAddNewProduct()
        {
            Response<ItemDTO> response = service.add_product_to_store(store1.Value.StoreID, "bamba", "desc1", "1", "1", "0", "5.0", "2.0", "77.0", "0.5_20.0", "Attr", "ctgr");

            if (response.ErrorOccured == false)
            {
                boolCollection.TryAdd(true);
            }
            else
                boolCollection.TryAdd(false);

        }

        /*
        // 3 threads attempt to RemoveFromBasket with the same data and have one product in basket ,at the same time, but only one shoul success!
        [TestMethod]
        public void ConcurrencyRemoveFromStore()
        {
            //Setup
            Setup();
            service.register("user12345", "123456m", "add1");
            service.login_member("user12345", "123456m");
            store1 = service.open_new_store(new List<string> { "store_12345" });
            respo1 = service.add_product_to_store(store1.Value.StoreID, "product1", "desc1", "1", "1", "1", "5.0", "2.0", "77.0", "0.5_20.0", "Attr", "ctgr");


            //Action
            Thread thread1 = new Thread(() => ThreadWorkRemoveFromStore());
            Thread thread2 = new Thread(() => ThreadWorkRemoveFromStore());
            Thread thread3 = new Thread(() => ThreadWorkRemoveFromStore());

            thread1.Start();
            thread2.Start();
            thread3.Start();

            thread1.Join();
            thread2.Join();
            thread3.Join();
            int num = 0;
            foreach (bool res in boolCollection)
            {
                if (res)
                    num++;
            }
            // Act
            Assert.IsTrue(num == 1);
            oneThreadCleanup();
        }
        public void ThreadWorkRemoveFromStore()
        {
            Response<string> response = service.remove_product_from_store(store1.Value.StoreID,respo1.Value.GetID());
            if (response.ErrorOccured == false)
            {
                boolCollection.TryAdd(true);
            }
            else
                boolCollection.TryAdd(false);

        }
       
               //open new store
               // 2 threads attempt to open new store with the same data at the same time, but only one should success!
               [TestMethod]
               public void ConcurrencyOpenNewStore()
               {
                   //Setup
                   Setup();
                   service.register("michel", "123456m", "add1");
                   service.login_member("michel", "123456m");

                   //Action
                   Thread thread1 = new Thread(() => ThreadWork3());
                   Thread thread2 = new Thread(() => ThreadWork3());

                   thread1.Start();
                   thread2.Start();

                   thread1.Join();
                   thread2.Join();
                   int num = 0;
                   foreach (bool res in boolCollection)
                   {
                       if (res)
                           num++;
                   }
                   // Act
                   Assert.IsTrue(num == 1);
                   oneThreadCleanup();
               }
               public void ThreadWork3()
               {
                   //Response response=
                  // Response<string> response = service.log_out();
                   Response<StoreDTO> response = service.open_new_store(new List<string> { "store_123" });
                   if (response.ErrorOccured == false)
                   {
                       boolCollection.TryAdd(true);
                   }
                   else
                       boolCollection.TryAdd(false);
               }
              */


        /*
        // 2 threads attempt to new admin with the same data at the same time, but only one should success!
        [TestMethod]
        public void ConcurrencyAddNewAdmin()
        {
            //Setup
            Setup();
            service.register("michellm", "123456m", "add1");
            service.register("michellm1", "123456m", "add1");


            service.login_member("michellm", "123456m");
            // service.AddNewAdmin("michell");
            store1 = service.open_new_store(new List<string> { "store_1234" });
            service.assign_new_owner(store1.Value.StoreID, "michellm1");
            service.assign_new_owner(store1.Value.StoreID, "michellm1");

            //  store1 = service.open_new_store(new List<string> { "store_1234" });
            //service.assign_new_owner(store1.Value.StoreID, "michellm");
            //service.assign_new_owner(store1.Value.StoreID, "michellm");
            // service.RemoveEmployeePermission
            //Response<string> response = service.assign_new_manager(store1.Value.StoreID, "michellm");

            // service.assign_new_manager(store1.Value.StoreID,"michellm");
            //respo = service.add_product_to_store(store.Value.StoreID, "bamba", "desc1", "1", "1", "0", "5.0", "2.0", "77.0", "0.5_20.0", "Attr", "ctgr");
            // service.add_product_to_basket("bamba", "1");


            //Action
            Thread thread1 = new Thread(() => ThreadWorkAddNewAdmin());
            Thread thread2 = new Thread(() => ThreadWorkAddNewAdmin());

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
            int num = 0;
            foreach (bool res in boolCollection)
            {
                if (res)
                    num++;
            }
            // Act
            Assert.IsTrue(num == 1);
            oneThreadCleanup();
        }
        public void ThreadWorkAddNewAdmin()
        {
            //Response<string> response = service.AddNewAdmin("michellm");
            // Response<string> response = service.assign_new_manager(store.Value.StoreID,"michellm");
            // Response<string> response=  service.close_store_temporary(store1.Value.StoreID);
            //  Response<string> response =  service.Remove_A_Member("michellm");
            Response<string> response = service.Remove_Store_Owner(store1.Value.StoreID, "michellm1111");
            //   Response<string> response  service.RemoveEmployeePermission(store1.Value.StoreID, "michellm", "STOCK");
            if (response.ErrorOccured == false)
            {
                boolCollection.TryAdd(true);
            }
            else
                boolCollection.TryAdd(false);

        }





        */


        /*
            public void SetupForRemoveProductFromStore()
            {
                service = new Service_Controller();
                boolCollection = new BlockingCollection<bool>();
                service.register("michel", "123456m", "add1");
                service.register("user2", "pass2", "add2");
                service.login_member("michel", "123456m");
                service.login_member("user2", "pass2");
                Response<StoreDTO> store = service.open_new_store(new List<string> { "store_123" });
                service.add_product_to_store(store.Value.StoreID, "bamba", "desc1", "1", "1", "1", "5.0", "2.0", "77.0", "0.5_20.0", "Attr", "ctgr");
                service.add_product_to_basket("bamba", "1");



            }
            public void ThreadWorkRemove()
            {
                Response<string> response = service.remove_product_from_basket("bamba","1");
                if (response.ErrorOccured == false)
                {
                    boolCollection.TryAdd(true);
                }
                else
                    boolCollection.TryAdd(false);

            }
            [TestMethod]
            public void ConcurrencyRemove()
            {
                //Setup
                SetupForRemoveProductFromStore();
                //Action
                Thread thread1 = new Thread(() => ThreadWorkRemove());
                Thread thread2 = new Thread(() => ThreadWorkRemove());

                thread1.Start();
                thread2.Start();

                thread1.Join();
                thread2.Join();
                int num = 0;
                foreach (bool res in boolCollection)
                {
                    if (res)
                        num++;
                }
                // Act
                Assert.IsTrue(num == 1);

            }


            */



    }
}
