<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductsPage.aspx.cs" Inherits="Market_System.Presentaion_Layer.ProductsPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


        <div>&nbsp;</div>
    <div>&nbsp;<asp:Label runat="server" Text="Label"></asp:Label></div>
    <div>&nbsp;</div>
    <div>
        <asp:Button ID="Button1" runat="server" Text="go to product" OnClick="Button1_Click" />
        <asp:TextBox ID="product_id_text" runat="server"></asp:TextBox>
        &nbsp;</div>
    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <div style ="text-align:left">   
        <asp:ListView ID="ListView1" runat="server" OnSelectedIndexChanged="ListView1_SelectedIndexChanged">
            <ItemTemplate>
                <div><%# Container.DataItem  %></div>
            </ItemTemplate>
        </asp:ListView>
    </div>
       <div>&nbsp;</div>
    <div>&nbsp;</div>
    <div>&nbsp;</div>
</asp:Content>
