using Market_System.Domain_Layer.Store_Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.Domain_Layer
{
    public class ItemDTO
    {
        private string _catagory;
        private string _description;
        private double _price;
        private string _name;
        private int _itemId;

    public ItemDTO(Product product)
        {
            _description = product.GetDescription();
            _catagory = product.GetCatagory();
            _itemId = product.GetProductID();
            _name = product.GetName();
            _price = product.GetPrice();
        }



    }
}