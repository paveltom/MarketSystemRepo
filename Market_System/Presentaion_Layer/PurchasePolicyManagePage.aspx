<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchasePolicyManagePage.aspx.cs" Inherits="Market_System.Presentaion_Layer.PurchasePolicyManagePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div ID="MainDiv" runat="server" align="left" enableviewstate="true" >

    </div>
    <br />
    <br />
    <div ID="SaleDiv" runat="server" align="left" enableviewstate="true" >
        <asp:DropDownList ID="SelectPolicyTypeList" runat="server" Height="37px" AutoPostBack="false" Width="213px">
            <asp:ListItem>--SELECT SALE TARGET--</asp:ListItem>
            <asp:ListItem>Product</asp:ListItem>
            <asp:ListItem>Category</asp:ListItem>
            <asp:ListItem>Store</asp:ListItem>
        </asp:DropDownList>
        <input id="PolicyAttributeID" type="text" name="PolicyAttribute" placeholder="Enter target attribute..." runat="server" Height="30px" Width="200px" visible="true" >
        <input id="PolicySaleValueID" type="text" name="PolicySaleValue" placeholder="Enter sale percentage..." runat="server" Height="30px" Width="200px" visible="true" >
    </div>
    <div ID="SaveDiv" runat="server" align="left" enableviewstate="true" >
        <input id="PolicyNameID" type="text" name="PolicyName" placeholder="Enter new policy name..." runat="server" Height="30px" Width="200px" visible="true" >
    </div>
    <div ID="PolicyDescriptionDiv" runat="server" align="left" enableviewstate="true" >
        <asp:TextBox ID="PolicyDescriptionID" TextMode="MultiLine" Rows="5" placeholder="Enter new policy description..." Height="120px" Width="300px" runat="server"/>
    </div>
    <div ID="AddPolicyButtonDiv" runat="server" align="left" enableviewstate="true" >
        <asp:Button ID="AddPolicyButtonID" runat="server" Text="Add Policy" OnClick="AddPolicyButtonCLick" Height="30px" Width="150px" Visible="true" />
        <asp:Button ID="CancelButton" runat="server" Text="Cancel" OnClick="CancelButtonClick" Height="30px" Width="150px" Visible="true" />
    </div>



</asp:Content>
