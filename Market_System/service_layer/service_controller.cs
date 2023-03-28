﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.service_layer
{
    public class service_controller : service_layer_interface
    {
        private user_service_controller usc;
        private store_service_controller ssc;
        public service_controller()
        {
            this.usc = new user_service_controller();
            this.ssc = new store_service_controller();
        }
        public void add_product_to_basket()
        {
            throw new NotImplementedException();
        }

        public void add_product_to_store()
        {
            throw new NotImplementedException();
        }

        public void apply_purchase_policy()
        {
            throw new NotImplementedException();
        }

        public void assign_new_manager()
        {
            throw new NotImplementedException();
        }

        public void assign_new_owner()
        {
            throw new NotImplementedException();
        }

        public void check_delivery()
        {
            throw new NotImplementedException();
        }

        public void check_out()
        {
            throw new NotImplementedException();
        }

        public void close_store()
        {
            throw new NotImplementedException();
        }

        public void comment_on_product()
        {
            throw new NotImplementedException();
        }

        public void edit_manger_permissions()
        {
            throw new NotImplementedException();
        }

        public void edit_product_details()
        {
            throw new NotImplementedException();
        }

        public void get_managers_of_store()
        {
            throw new NotImplementedException();
        }

        public void get_products_from_shop()
        {
            throw new NotImplementedException();
        }

        public void get_purchase_history_from_store()
        {
            throw new NotImplementedException();
        }

        public void get_purchase_history_of_a_memebr()
        {
            throw new NotImplementedException();
        }

        public void get_shop()
        {
            throw new NotImplementedException();
        }

        public void login_guest()
        {
            throw new NotImplementedException();
        }

        public void login_member()
        {
            throw new NotImplementedException();
        }

        public void log_out()
        {
            throw new NotImplementedException();
        }

        public void open_new_store()
        {
            throw new NotImplementedException();
        }

        public void register()
        {
            throw new NotImplementedException();
        }

        public void remove_product_from_basket()
        {
            throw new NotImplementedException();
        }

        public void remove_product_from_store()
        {
            throw new NotImplementedException();
        }

        public void search_product_by_category()
        {
            throw new NotImplementedException();
        }

        public void search_product_by_keyword()
        {
            throw new NotImplementedException();
        }

        public void search_product_by_name()
        {
            throw new NotImplementedException();
        }
    }
}