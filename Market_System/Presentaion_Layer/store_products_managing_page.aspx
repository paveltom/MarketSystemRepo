<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="store_products_managing_page.aspx.cs" Inherits="Market_System.Presentaion_Layer.store_products_managing_page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center">
        <asp:Label ID="title_label" Size="Large" runat="server" Text=""></asp:Label>
    </div>
    <div style="text-align: left">
        <asp:Label ID="Label2" runat="server" Text="products in store:"></asp:Label>
    </div>
    <div</div>
    <div><asp:DataList ID="products_list" runat="server">
                  <ItemTemplate>
        <asp:Label Text="<%#Container.DataItem %>" runat="server" />
    </ItemTemplate>
        </asp:DataList></div>
    <div></div>
    <div></div>
    <div style="text-align: left">
    <div style="text-align: center">
        <asp:Label ID="Label9" runat="server" Text="add new product:"></asp:Label>
        &nbsp;<asp:Label ID="Label11" runat="server" Text="name:"></asp:Label>
        <asp:TextBox ID="name_txt" runat="server" Width="70px"></asp:TextBox>
        &nbsp;<asp:Label ID="Label12" runat="server" Text="description:"></asp:Label>
        <asp:TextBox ID="desc_txt" runat="server" Width="55px"></asp:TextBox>
        &nbsp;<asp:Label ID="Label13" runat="server" Text="price:"></asp:Label>
        <asp:TextBox ID="price_txt" Text="5" runat="server" Width="55px"></asp:TextBox>
        &nbsp;<asp:Label ID="Label14" runat="server" Text="quantity:"></asp:Label>
        <asp:TextBox ID="quantity_txt" Text="100" runat="server" Width="55px"></asp:TextBox>
&nbsp; <asp:Label ID="Label15" runat="server" Text="sale:"></asp:Label>
        <asp:TextBox ID="sale_txt" Text="0" runat="server" Width="55px"></asp:TextBox>
&nbsp;&nbsp; <asp:Label ID="Label16" runat="server" Text="weight:"></asp:Label>
        <asp:TextBox ID="weight_txt" Text="0" runat="server" Width="55px"></asp:TextBox>
&nbsp;&nbsp; <asp:Label ID="Label17" runat="server" Text="dimenstions:"></asp:Label>
        <asp:TextBox ID="dimenstios_txt" runat="server" Text="0.5_20.0_7.0" Width="55px"></asp:TextBox>
&nbsp;&nbsp; <asp:Label ID="Label18" runat="server" Text="attributes:"></asp:Label>
        <asp:TextBox ID="attribute_txt" Text="attr" runat="server" Width="50px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;<asp:Label ID="Label19" runat="server" Text="category:"></asp:Label>
        <asp:DropDownList ID="ddl_categories"  runat="server" AutoPostBack="True" Height="16px" Width="129px"  >
         <asp:ListItem Text="please select a category" Value="nothing_to_show" />
            </asp:DropDownList>
&nbsp;<asp:Button ID="add_product_button" runat="server" OnClick="add_product_click" Text="!Add" Width="285px" />
&nbsp;<asp:Label ID="add_message" runat="server" ForeColor="Red"></asp:Label>
        </div>
       <div></div>
        <div></div>
    <div style="text-align: center">
        <asp:Label ID="Label10" runat="server" Text="remove product"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="remove_product_button" runat="server" Text="!Remove" />
&nbsp;</div>
       </div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
</asp:Content>
