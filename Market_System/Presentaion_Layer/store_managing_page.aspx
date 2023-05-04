<%@ Page Title="Store Managing Center." Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="store_managing_page.aspx.cs" Inherits="Market_System.Presentaion_Layer.store_managing_page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: left">
        <asp:Label ID="Label2" runat="server" Text="your stores:"></asp:Label>
    </div>
    <div>
        <asp:DataList ID="stores_user_works_in" runat="server">
            <ItemTemplate>
        <asp:Label Text="<%#Container.DataItem %>" runat="server" />
    </ItemTemplate>
        </asp:DataList>
    </div> 
    <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label3" runat="server" Text="enter store ID to manage:"></asp:Label>
        <asp:TextBox ID="entered_store_id" runat="server"></asp:TextBox>
        <asp:Label ID="error_message" runat="server" Font-Size="Large" ForeColor="Red"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="manage_store_products" runat="server" OnClick="manage_store_products_Click" Text="manage store's products" />
&nbsp;&nbsp;
        <div></div>
    </div>
    <div style="text-align: left">
        </div>
       <div></div>
    <div style="text-align: left">
        </div>
       <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label6" runat="server" Text="assign new manager"></asp:Label>
    &nbsp;&nbsp;&nbsp; &nbsp;<asp:Label ID="Label15" runat="server" Text="type new manager name"></asp:Label>
        &nbsp;&nbsp;<asp:TextBox ID="new_manager_username" runat="server"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="add_product_button1" onclick="add_product_button1_Click" runat="server" Text="GO!" Height="26px" />
&nbsp;<asp:Label ID="new_manager_message" runat="server" Font-Size="Medium" ForeColor="Red"></asp:Label>
    </div>
       <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label7" runat="server" Text="assign new owner"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="add_product_button2" runat="server" Text="GO!" />
&nbsp;</div>
       <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label8" runat="server" Text="remove owner"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="add_product_button3" runat="server" Text="GO!" />
&nbsp;</div>
       <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label9" runat="server" Text="add employe permission"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="add_product_button4" runat="server" Text="GO!" />
&nbsp;</div>
       <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label10" runat="server" Text="remove employe permission"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="add_product_button5" runat="server" Text="GO!" />
&nbsp;</div>
       <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label11" runat="server" Text="close store temporary"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="add_product_button6" runat="server" Text="GO!" />
&nbsp;</div>
       <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label12" runat="server" Text="get managers of store"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="add_product_button7" runat="server" Text="Show!" />
&nbsp;</div>
       <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label13" runat="server" Text="get owners of store"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="add_product_button8" runat="server" Text="Show!" />
&nbsp;</div>
       <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label14" runat="server" Text="get purchase history of the store"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="add_product_button9" runat="server" Text="Show!" />
&nbsp;</div>
    <div></div>
</asp:Content>
