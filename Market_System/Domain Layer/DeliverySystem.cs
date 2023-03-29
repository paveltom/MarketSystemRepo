using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer
{
    public class DeliverySystem
    {
        private static int amount_Of_Deliveris_Remaining;

        //This variable is going to store the Singleton Instance
        private static DeliverySystem Instance = null;

        //To use the lock, we need to create one variable
        private static readonly object Instancelock = new object();

        //The following Static Method is going to return the Singleton Instance
        public static DeliverySystem GetInstance()
        {
            //This is thread-Safe - Performing a double-lock check.
            if (Instance == null)
            {
                //As long as one thread locks the resource, no other thread can access the resource
                //As long as one thread enters into the Critical Section, 
                //no other threads are allowed to enter the critical section
                lock (Instancelock)
                { //Critical Section Start
                    if (Instance == null)
                    {
                        amount_Of_Deliveris_Remaining = 5;
                        Instance = new DeliverySystem();
                    }
                } //Critical Section End
                //Once the thread releases the lock, the other thread allows entering into the critical section
                //But only one thread is allowed to enter the critical section
            }

            //Return the Singleton Instance
            return Instance;
        }

        //Currently there can be only 5 deliveries - just in order to check the system via TESTS(!)
        public bool Delivery()
        {
            if (amount_Of_Deliveris_Remaining <= 0) return false;
            return true;
        }
    }
}