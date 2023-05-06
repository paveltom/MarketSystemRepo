<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="cart_page.aspx.cs" Inherits="Market_System.Presentaion_Layer.cart_page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center; margin-top: 50px;">
        <div style="margin-bottom: 10px;">
            <label style="font-size: large; font-weight: bold;">Please select a store:</label>
            <asp:DropDownList ID="ddl_store_id" OnSelectedIndexChanged="show_basket_of_selected_store_id" runat="server" AutoPostBack="True">
                <asp:ListItem Text="Please select a store" Value="nothing_to_show" />
            </asp:DropDownList>

            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="error_message" runat="server" Font-Size="Medium" ForeColor="Red"></asp:Label>
        </div>
        <div style="margin-bottom: 10px;">
            <asp:Label ID="product_id_label" runat="server" Text="Product ID:" style="font-size: large; font-weight: bold;"></asp:Label>
            <asp:TextBox ID="product_id_txt" runat="server" placeholder="Enter product ID"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="GO" OnClick="GO_button_click" CssClass="btn btn-primary" />
            <asp:Label ID="error_message_GO_button" runat="server" Font-Size="Medium" ForeColor="Red"></asp:Label>
        </div>
        <div>
            <asp:Label ID="cart_empty_label" runat="server" Text="Cart is empty" Font-Size="Medium" ForeColor="Red" Visible="false"></asp:Label>
        </div>

    </div>


    <div style="margin-top: 50px; text-align: center;">
        <asp:DataList ID="products_list" runat="server" RepeatColumns="3" ItemStyle-CssClass="product-item">
            <ItemTemplate>
                <asp:Label Text="<%#Container.DataItem %>" runat="server" />
            </ItemTemplate>
        </asp:DataList>

    </div>
</asp:Content>
