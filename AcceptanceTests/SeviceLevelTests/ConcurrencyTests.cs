using Market_System.ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Collections.Concurrent;
using Market_System.DomainLayer;
using System.Collections.Generic;

namespace Market_System.Tests.SeviceLevelTests
{
    [TestClass]
    public class ConcurrencyTests
    {
        private Service_Controller service;
        private BlockingCollection<bool> boolCollection;


        public void Setup()
        {
            service = new Service_Controller();
            boolCollection = new BlockingCollection<bool>();
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
   /*
    
        public void SetupForRemoveProductFromStore()
        {
            service = new Service_Controller();
            boolCollection = new BlockingCollection<bool>();
            service.register("michel", "123456m", "add1");
            service.register("user2", "pass2", "add2");
            service.login_member("michel", "123456m");
            service.login_member("user2", "pass2");
             service.add_product_to_basket("bamba", "1");



        }
        public void ThreadWorkRemove()
        {
            Response<string> response = service.remove_product_from_basket("bamba");
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
