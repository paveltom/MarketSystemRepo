using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.Service_Layer
{
    interface Service_Layer_Interface
    {
        public void check_out(); //I.3 מערכת
        public void check_delivery(); //I.4 מערכת
        public void login_member(); // 1.4
        public void login_guest(); //1.1
        public void log_out(); //3.1
        public void register(); // 1.3
        public void get_shop();
        public void get_products_from_shop();
        public void search_product_by_category(); //2.2
        public void search_product_by_keyword(); //2.2
        public void search_product_by_name(); //2.2
        public void apply_purchase_policy(); //2.5
        public void add_product_to_basket();
        public void remove_product_from_basket();
        public void open_new_store(); // 3.2
        public void comment_on_product(); // 3.3
        public void add_product_to_store(); //4.1
        public void remove_product_from_store(); //4.1
        public void edit_product_details(); //4.1
        public void assign_new_owner(); // 4.4
        public void assign_new_manager(); // 4.6
        public void edit_manger_permissions(); //4.7
        public void close_store(); //4.9
        public void get_managers_of_store(); //4.11
        public void get_purchase_history_from_store(); //4.13
        public void get_purchase_history_of_a_memebr(); //6.4

    }
}
