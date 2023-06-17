         config_file instructions:
         ** always do space after ':'
         add your databse connection string after "database connection string:", example:
         "database connection string: <your_connection_string>".
	  if you have \\ in your connection string please add space before and after // for example:
          Data Source=(localdb)\\MSSQLLocalDB;   ==make it like this==>   Data Source=(localdb) \\ MSSQLLocalDB;
         
         add your URL for external payment system, after "external payment system URL:", example:
         "external payment system URL: <your_URL>", if you dont want to add any URL, just leave it like this: "external payment system URL:" ,
         and the dummy payment system will be used.
         
         
         add your URL for external delivery system, after "external delivery system URL:", example:
         "external delivery system URL: <your_URL>", if you dont want to add any URL, just leave it like this: "external delivery system URL:" ,
         and the dummy delivery system will be used.         
         
         ---------------------------------------------------------------------------------------------------------------------------------------
         init_file instructions:
         
         
         check_out(string credit_card_details);
         login_member(string username,string pass);
         log_out();
         register(string username, string pass,string address); 
         add_product_to_basket(string product_id,string quantity);
         remove_product_from_basket(string product_id, string quantity);
         open_new_store(List<string> newStoreDetails); 
         comment_on_product(string productID, string comment, double rating); 
      add_product_to_store( string product_name,string description, string price,string quantity,string reserved_quantity,string rating,string sale,string wieght,string dimenstions,string attributes,string product_category);
       remove_product_from_store( string productID);
        assign_new_owner(string newOwnerUsername); 
       Remove_Store_Owner(string storeID, string other_Owner_Username); 
        assign_new_manager(string storeID, string new_manager_ID);
        AddEmployeePermission(string storeID, string employee_username, string newPerm);
        RemoveEmployeePermission(string storeID, string employee_username, string permToRemove);
         close_store_temporary(string storeID); 
       Remove_A_Member(string member_Username); 
