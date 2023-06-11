using Market_System.Domain_Layer.Communication_Component;
using Market_System.DomainLayer.StoreComponent;
using Market_System.DomainLayer.UserComponent;
using Market_System.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DAL
{
    public class DALController // call it from MarketSystem instance Builder to verify database exxistance
    {
        private static DALController Instance;
        private static readonly object Instancelock = new object();

        public static DALController GetInstance()
        {
            if (Instance == null)
            {
                lock (Instancelock)
                { 
                    if (Instance == null)
                    {
                        // verify if database exists -> create if not
                    }
                } 

            }
            return Instance;
        }










        public void DestroyMe()
        {
            Instance = null;

        }
    }
}