<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="checkoutPage.aspx.cs" Inherits="Market_System.Presentaion_Layer.checkoutPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <div>&nbsp;</div>
     <div style="text-align: left">
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
         <asp:TextBox ID="TextOfMonth" runat="server"></asp:TextBox>
         <br />
         <asp:Label ID="Label5" runat="server" ForeColor="Green" Font-Size="Medium" Text="Year:"></asp:Label>
         <asp:TextBox ID="TextOfYear" runat="server"></asp:TextBox>
         <br />
         <asp:Label ID="Label6" runat="server" ForeColor="Green" Font-Size="Medium" Text="Holder:"></asp:Label>
         <asp:TextBox ID="TextOfHolder" runat="server"></asp:TextBox>
         <br />
         <asp:Label ID="Label13" runat="server" ForeColor="Green" Font-Size="Medium" Text="CCV:"></asp:Label>

         <asp:TextBox ID="CVVTextBox" runat="server"></asp:TextBox>

    </div>

    <div>&nbsp;</div>
    <div style="text-align: left">
        <asp:Label ID="priceLabel" runat="server" ForeColor="#000066" Font-Size="Medium" Text="total price: "></asp:Label>
        <asp:Label ID="priceLabel2" runat="server" ForeColor="#0000cc" Font-Size="Medium" Text=""></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label7" runat="server" ForeColor="#0000ff" Font-Size="Large" Text="Deliver"></asp:Label>
        <br />
        <asp:Label ID="Label8" runat="server" ForeColor="#0000ff" Font-Size="Medium" Text="Name:"></asp:Label>
        <asp:TextBox ID="TextOfName" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label9" runat="server" ForeColor="#0000ff" Font-Size="Medium" Text="Address:"></asp:Label>
         <asp:TextBox ID="adressTextBox" Text="" runat="server" 
             OnTextChanged="adressTextBox_TextChanged" AutoPostBack="true"></asp:TextBox>
        <br />
        <asp:Label ID="Label10" runat="server" ForeColor="#0000ff" Font-Size="Medium" Text="City:"></asp:Label>
        <asp:TextBox ID="CityTextBox" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label11" runat="server" ForeColor="#0000ff" Font-Size="Medium" Text="Country:"></asp:Label>
        <asp:TextBox ID="CountryTextBox" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label12" runat="server" ForeColor="#0000ff" Font-Size="Medium"  Text="ZIP:"></asp:Label>
        <asp:TextBox ID="ZipTextBox" runat="server"></asp:TextBox>
        <br />
        <br />
    </div>

    <div>&nbsp;<br />
        <br />
    </div>
    <div>&nbsp;</div>
    <div>
        <asp:Button ID="payButton" runat="server" Text="Pay" OnClick="payClick"></asp:Button>&nbsp;</div>
        <asp:Label ID="payButtonMsg" runat="server" Text=""></asp:Label>
</asp:Content>
