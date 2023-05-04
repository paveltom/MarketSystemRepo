<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="store_products_managing.aspx.cs" Inherits="Market_System.Presentaion_Layer.store_products_managing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: left">
        <asp:Label ID="Label2" runat="server" Text="store's products:"></asp:Label>
    </div>
    <div><asp:DataList ID="stores_user_works_in" runat="server">
            <ItemTemplate>
        <asp:Label Text="<%#Container.DataItem %>" runat="server" />
    </ItemTemplate>
        </asp:DataList></div>
    <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label4" runat="server" Text="add product to store"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="add_product_button" runat="server" Text="Add" />
&nbsp;</div>
    <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label5" runat="server" Text="remove product form store"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="add_product_button0" runat="server" Text="Remove" />
&nbsp;</div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
</asp:Content>
