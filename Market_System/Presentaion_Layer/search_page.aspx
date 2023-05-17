<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="search_page.aspx.cs" Inherits="Market_System.Presentaion_Layer.search_page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: left">
        <asp:TextBox ID="search_text_box" runat="server" ></asp:TextBox>
        <asp:Label ID="Label2" runat="server" Text="search by:" Font-Size="Medium"></asp:Label>
        <asp:DropDownList ID="search_by_ddl" runat="server" AutoPostBack="True" >
        <asp:ListItem Text="Please select search function" Value="nothing_to_show" />
            <asp:ListItem Text="search by category" Value="category" />
            <asp:ListItem Text="search by name" Value="name" />
            <asp:ListItem Text="search by keyword" Value="keyword" />
    </asp:DropDownList>
        <asp:Button ID="Button2" runat="server" Text="search" CssClass="btn btn-primary" OnClick="search_button_click" />
        <asp:Label ID="search_error" runat="server" Font-Size="X-Large" ForeColor="Red"></asp:Label>
    </div>
    <div></div>
    <div style="text-align: left">   <asp:TextBox ID="product_id_text" runat="server"></asp:TextBox>
        &nbsp;<asp:Button ID="Button1" runat="server" Text="go to product" OnClick="go_to_product_page" CssClass="btn btn-primary" />
        &nbsp;&nbsp;
        <asp:Label ID="error_message" runat="server" Font-Size="X-Large" ForeColor="Red"></asp:Label></div>
    <div></div>
    <div>       <asp:DataList ID="search_result" runat="server">
            <ItemTemplate>
                <asp:Label Text="<%# Container.DataItem %>" runat="server" />
            </ItemTemplate>
        </asp:DataList></div>
    <div></div>
    <div></div>
    <div></div>
</asp:Content>
