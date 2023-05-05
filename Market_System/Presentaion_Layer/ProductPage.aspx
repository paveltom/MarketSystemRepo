<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductPage.aspx.cs" Inherits="Market_System.Presentaion_Layer.ProductPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <div style="text-align: left">
        <asp:Label ID="quantityToAddLabel" runat="server" Text="amount: "></asp:Label>
        <asp:TextBox ID="quantityToAdd" runat="server"></asp:TextBox>
        <asp:Button ID="addToCart" runat="server" Text="add To Cart" OnClick="addToCart_Click" />
        <asp:Label ID="clickmsg" runat="server" Text=""></asp:Label>
    </div>
    <div>&nbsp;</div>

    <div style="text-align: left">
        <asp:Label ID="id" runat="server" Text=""></asp:Label>
    </div>
    <div>&nbsp;</div>
    <div style="text-align: center">
        &nbsp;</div>
    <div>&nbsp;</div>
    <div style="text-align: left">
        <asp:Label ID="quantity" runat="server" Text=""></asp:Label>
    </div>
    <div>&nbsp;</div>
</asp:Content>
