<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Market_System.Presentaion_Layer.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<asp:ListView ID="ListView1" runat="server" >
    <ItemTemplate>
        <div><%# Container.DataItem %></div>
    </ItemTemplate>
</asp:ListView>

</asp:Content>
