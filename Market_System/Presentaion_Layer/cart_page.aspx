<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="cart_page.aspx.cs" Inherits="Market_System.Presentaion_Layer.cart_page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: left; margin-bottom: 0px">
        <label>please select a Store</label>
        <asp:DropDownList ID="ddl_store_id" runat="server" >
        </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="error_message" runat="server" Font-Size="Medium" ForeColor="Red"></asp:Label>
    </div>
</asp:Content>

