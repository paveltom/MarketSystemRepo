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
        <asp:Label ID="Label6" runat="server" Text="assign new manager:"></asp:Label>
    &nbsp;&nbsp;&nbsp; &nbsp;<asp:Label ID="Label15" runat="server" Text="type new manager name"></asp:Label>
        &nbsp;&nbsp;<asp:TextBox ID="new_manager_username" runat="server"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="assign_new_manager_button" onclick="assign_new_manager_click" runat="server" Text="Assign" Height="26px" />
&nbsp;<asp:Label ID="new_manager_message" runat="server" Font-Size="Medium" ForeColor="Red"></asp:Label>
    </div>
       <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label7" runat="server" Text="assign new owner:"></asp:Label>
    &nbsp;&nbsp;&nbsp;<asp:Label ID="Label16" runat="server" Text="type new owner name"></asp:Label>
        &nbsp;<asp:TextBox ID="new_owner_username" runat="server"></asp:TextBox>
        &nbsp;&nbsp;
        <asp:Button ID="assign_new_owner_button" onclick="assign_new_owner_click" runat="server" Text="Assign" />
&nbsp;<asp:Label ID="new_owner_message" runat="server" Font-Size="Medium" ForeColor="Red"></asp:Label>
    </div>
       <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label8" runat="server" Text="remove owner"></asp:Label>
    &nbsp;&nbsp;<asp:Label ID="Label17" runat="server" Text="type owner name"></asp:Label>
        &nbsp;&nbsp;<asp:TextBox ID="owner_to_remove" runat="server"></asp:TextBox>
        &nbsp;&nbsp;
        <asp:Button ID="owner_remove_button" runat="server" OnClick="remove_owner_click" Text="Remove" />
&nbsp;<asp:Label ID="owner_remove_message" runat="server" Font-Size="Medium" ForeColor="Red"></asp:Label>
    </div>
       <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label9" runat="server" Text="add employe permission"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;type userName of employee&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
        <asp:TextBox ID="add_employee_permissionT" runat="server"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;add permission: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="add_permission" runat="server"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; <asp:Button ID="add_employee_permission" runat="server" Text="Add" OnClick="add_employee_permission_Click" Width="42px" />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Add_Permission_Message" runat="server"  Font-Size="Medium" ForeColor="Red"></asp:Label>
    </div>
       <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label10" runat="server" Text="remove employe permission"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;type userName of employee&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="remove_username" runat="server"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;remove permission:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="remove_employee_permission" runat="server"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
        <asp:Button ID="remove_permission_button5" runat="server" Text="remove" OnClick="remove_permission_Click" />
&nbsp;&nbsp;&nbsp;&nbsp;<br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Remove_Permission_Message" runat="server" Font-Size="Medium" ForeColor="Red" ></asp:Label>
        </div>
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
        <asp:Button ID="add_product_button7" runat="server" OnClick="show_managers_click" Text="Show!" />
&nbsp;<asp:Label ID="show_managers_message" runat="server" Font-Size="Medium" ForeColor="Red"></asp:Label>
    </div>
       <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label13" runat="server" Text="get owners of store"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="add_product_button8" runat="server"  onclick="show_owners_click" Text="Show!" />
&nbsp;<asp:Label ID="show_owners_message" runat="server" Font-Size="Medium" ForeColor="Red"></asp:Label>
    </div>
       <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label14" runat="server" Text="get purchase history of the store"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="add_product_button9" runat="server" OnClick="show_purchase_history_click" Text="Show!" />
&nbsp;<asp:Label ID="show_purchase_history_message" runat="server" Font-Size="Medium" ForeColor="Red"></asp:Label>
    </div>
    <div></div>
    <div style="text-align: left">
        <asp:Label ID="show_info_label" runat="server" Font-Size="Large"></asp:Label>
    </div>
    <div></div>
    <div> <asp:DataList ID="show_info_list" runat="server">
            <ItemTemplate>
        <asp:Label Text="<%#Container.DataItem %>" runat="server" />
    </ItemTemplate>
        </asp:DataList></div>
</asp:Content>
