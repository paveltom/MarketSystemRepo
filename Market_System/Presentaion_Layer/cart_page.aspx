<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="cart_page.aspx.cs" Inherits="Market_System.Presentaion_Layer.cart_page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: left; margin-bottom: 0px">
        <label>please select a Store</label>
        <asp:DropDownList ID="ddl_store_id"  OnSelectedIndexChanged="show_basket_of_selected_store_id" runat="server" AutoPostBack="True"  >
        </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="error_message" runat="server" Font-Size="Medium" ForeColor="Red"></asp:Label>
    &nbsp;&nbsp; product ID:<asp:TextBox ID="product_id_txt" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="GO!" OnClick="GO_button_click" />
        <asp:Label ID="error_message_GO_button" runat="server" Font-Size="Medium" ForeColor="Red"></asp:Label>
    </div>
    



    <div>
  
        <asp:DataList ID="products_list" runat="server">
                  <ItemTemplate>
        <asp:Label Text="<%#Container.DataItem %>" runat="server" />
    </ItemTemplate>
        </asp:DataList>
        
      
        
    </div>
</asp:Content>

