<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="cart_page.aspx.cs" Inherits="Market_System.Presentaion_Layer.cart_page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center; margin-top: 50px;">
        <label style="font-size: 18px; font-weight: bold;">Please select a store:</label>
        <asp:DropDownList ID="ddl_store_id" runat="server" AutoPostBack="True" OnSelectedIndexChanged="show_basket_of_selected_store_id">
            <asp:ListItem Text="Please select a store" Value="nothing_to_show" />
        </asp:DropDownList>
        <br />
        <br />
        <asp:Label ID="error_message" runat="server" Font-Size="Medium" ForeColor="Red"></asp:Label>
    </div>
    <br />
    <div style="text-align: center; margin-top: 30px;">
        <label style="font-size: 18px; font-weight: bold;">Product ID:</label>
        <asp:TextBox ID="product_id_txt" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Edit Amount" OnClick="GO_button_click" />
        <br />
        <asp:Label ID="error_message_GO_button" runat="server" Font-Size="Medium" ForeColor="Red"></asp:Label>
    </div>
    <br />
    <div style="text-align: center;">
        <asp:DataList ID="products_list" runat="server">
            <ItemTemplate>
                <asp:Label Text="<%# Container.DataItem %>" runat="server" />
            </ItemTemplate>
        </asp:DataList>
    </div>
    <br />
    <div style="text-align: center;">
        <asp:Button ID="checkoutButton" runat="server" Text="Checkout" OnClick="checkoutButton_Click" CssClass="btn btn-primary"/>
    </div>
</asp:Content>
