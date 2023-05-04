<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="Market_System.Presentaion_Layer.Products" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <div>&nbsp;</div>


    <asp:ListView ID="ListView1" runat="server">
        <ItemTemplate>
            <div><%# Container.DataItem %></div>
        </ItemTemplate>
    </asp:ListView>


    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <div>&nbsp;</div>
</asp:Content>

