<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="product_in_basket_page.aspx.cs" Inherits="Market_System.Presentaion_Layer.product_in_basket_page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: left"> </div>
      <div style="text-align: left"> </div>
      <div style="text-align: left"> &nbsp;<asp:Label ID="product_name_label_label" runat="server" Text="product name:"></asp:Label>
          <asp:Label ID="product_name_label" runat="server"></asp:Label>
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="quantity_in_basket_label" runat="server" Text="quantity:"></asp:Label>
          &nbsp;&nbsp;<asp:Label ID="quantity_label" runat="server"></asp:Label>
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="price_label_label" runat="server" Text="price:"></asp:Label>
          &nbsp; <asp:Label ID="price_label" runat="server"></asp:Label>
    </div>
    <div style="text-align: left"> </div>
    <div style="text-align: center"> 
        <asp:Label ID="invalid_product_id" runat="server" Font-Size="XX-Large" ForeColor="Red"></asp:Label>
    </div>
    <div style="text-align: left"> </div>
    <div style="text-align: left"> 
        <asp:TextBox ID="add_quantity" runat="server"></asp:TextBox>
        <asp:Button ID="add_button" runat="server" Text="add" OnClick="add_button_click" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="remove_quantity" runat="server"></asp:TextBox>
        <asp:Button ID="remove_button" runat="server" Text="remove" OnClick="remove_button_click" />
    </div>
    <div style="text-align: left">&nbsp;&nbsp;&nbsp;
        <asp:Label ID="add_error" runat="server" Font-Size="Medium" ForeColor="Red"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="remove_error" runat="server" Font-Size="Medium" ForeColor="Red"></asp:Label>
&nbsp;</div>
</asp:Content>
