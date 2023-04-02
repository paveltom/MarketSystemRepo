﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer.Store_Component
{
    public class Store : Property
    {
        //Implement all of the Property Methods here
        private int store_ID;
        private string name;


        private Dictionary<int, int> products; //<product_id, quantity>
        private List<string> owners; //<Owner's_username>
        private List<string> managers;
        private String founder; //founder's username
       

        // =======Fields ToDo===========:
        // Attributes -  Dictionary<String>: {atb1: opt1->opt2->op3...., atb2: opt1->opt2....,...}
        // *Comments




        public Store(string founder, int store_ID)
        {
            //TODO:: change it later to load the info from the database.
            products = new Dictionary<int, int>();
            owners = new List<string>();
            managers = new List<string>();
            this.founder = founder;
            this.store_ID = store_ID;
        }

        public int GetStore_ID()
        {
            return this.store_ID;
        }

        public string GetFounder()
        {
            return this.founder;
        }

        public void Add_Product(string userID, List<string> productProperties)
        {
            // validate userID first
            products.Add(product_id, quantity);
        }

        public void Remove_Product(int product_id)
        {
            products.Remove(product_id);
        }

        public void Add_New_Owner(string username)
        {
            owners.Add(username);
        }

        public bool Already_Has_Owner(string username)
        {
            if (owners.Contains(username)) return true;
            return false;
        }

        public void Add_New_Manager(string username)
        {
            managers.Add(username);
        }

        public bool Already_Has_Manager(string username)
        {
            if (managers.Contains(username)) return true;
            return false;
        }

        // ========Methods ToDo==========:
        // passing a data for store representation (including what details?)
        // passing a data for store content representation- what products availble for a buyer
        // *maby should an additional store content view, a seperate view for a manager of the store
        // (search)-get products by name: getting all products with same name *or similar name
        // (search)-get products by catagory: getting all products from that catagory

        // ======more ideas=====
        /*
         *  .חיפוש מוצרים ללא התמקדות בחנות ספציפית, לפי שם המוצר, קטגוריה או מילות מפתח.
         *  ניתן לסנן את התוצאות בהתאם למאפיינים כגון: טווח מחירים, דירוג המוצר, קטגוריה
         * דירוג החנות וכד'.
         */
    }
}