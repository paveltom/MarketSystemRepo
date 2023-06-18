﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Market_System.DomainLayer.StoreComponent.PolicyStrategy;
using Market_System.DomainLayer.UserComponent;

namespace Market_System.DomainLayer.StoreComponent
{
    public abstract class Purchase_Policy
    {
        public string PolicyID { get; private set; }// ???
        public string PolicyName { get; private set; }
        public double SalePercentage { get; private set; }
        public string Description { get; private set; }
        public Statement SalePolicyFormula { get; private set; }
        public string strFormula { get; private set; }


        /*
        public Purchase_Policy(string polID, string polName, double salePercentage, string description, Statement formula)
        {
            this.PolicyID = polID;
            this.PolicyName = polName;
            this.SalePercentage = salePercentage;
            this.Description = description;
            this.SalePolicyFormula = formula;
        }
        */

        public Purchase_Policy(string polID, string polName, double salePercentage, string description, String formula)
        {
            this.PolicyID = polID;
            this.PolicyName = polName;
            this.SalePercentage = salePercentage;
            this.Description = description;
            this.strFormula = formula;  
            this.SalePolicyFormula = StatementBuilder.GenerateFormula(formula);
        }

        // returns items with saled price
        public abstract List<ItemDTO> ApplyPolicy(List<ItemDTO> chosenProductsWithAttributes, string userID);

        public Boolean Validate(List<ItemDTO> chosenProductsWithAttributes, string userID)
        {           

            User currUser = UserFacade.GetInstance().getUser(UserFacade.GetInstance().get_username_from_user_id(userID));
            Dictionary<string, string> rellevantUserPolicyData = new Dictionary<string, string>();
            if (currUser != null)
                rellevantUserPolicyData = new Dictionary<string, string>
                {
                    { "Name", currUser.GetUsername() }, // actually username
                    { "Address", currUser.get_Address() },
                    { "Age" , "17" }
                };
            else
                rellevantUserPolicyData = new Dictionary<string, string>
                {
                    { "Name", userID }, // actually username
                    { "Address", "" },
                    { "Age" , "19" }
                };
            return this.SalePolicyFormula.Satisfies(chosenProductsWithAttributes, rellevantUserPolicyData);
        }


        public void SetName(string name)
        {
            this.PolicyName = name;
        }

        public void SetDescription(string description)
        {
            this.Description = description;
        }

        public void SetSale(double sale)
        {
            if (sale > 0 && sale < 100)
                this.SalePercentage = sale;
        }

        public void SetFormula(Statement formula)
        {
            this.SalePolicyFormula = formula;
        }
    }
}