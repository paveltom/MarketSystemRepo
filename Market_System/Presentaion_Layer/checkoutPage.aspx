<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="checkoutPage.aspx.cs" Inherits="Market_System.Presentaion_Layer.checkoutPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <div>&nbsp;</div>
     <div style="text-align: left">
        <asp:Label ID="adressLabel" runat="server" Text="adress:"></asp:Label>
         <asp:TextBox ID="adressTextBox" Text="" runat="server" 
             OnTextChanged="adressTextBox_TextChanged" AutoPostBack="true"></asp:TextBox>
         <br />
         <asp:Label ID="Label2" runat="server" ForeColor="Green" Font-Size="Medium" Font-Bold="true"  Text="Payment"></asp:Label>
         <br />
         <asp:Label ID="Label3" runat="server" ForeColor="Green" Font-Size="Medium" Text="Id: "></asp:Label>
         <asp:TextBox ID="TextOfId" runat="server"></asp:TextBox>
         <br />
         <asp:Label ID="Label1" runat="server" ForeColor="Green" Font-Size="Medium" Text="credit card number:"></asp:Label>
         <asp:TextBox ID = "creditTextBox" runat="server"></asp:TextBox>
         <br />
         <asp:Label ID="Label4" runat="server" ForeColor="Green" Font-Size="Medium" Text="Month:"></asp:Label>
    </div>

    <div>&nbsp;</div>
    <div style="text-align: left">
        <asp:Label ID="priceLabel" runat="server" Text="total price: "></asp:Label>
        <asp:Label ID="priceLabel2" runat="server" Text=""></asp:Label>
    </div>

    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <div>
        <asp:Button ID="payButton" runat="server" Text="Pay" OnClick="payClick"></asp:Button>&nbsp;</div>
        <asp:Label ID="payButtonMsg" runat="server" Text=""></asp:Label>
</asp:Content>
