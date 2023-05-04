<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="open_new_store_page.aspx.cs" Inherits="Market_System.Presentaion_Layer.open_new_store_page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: left">
        <asp:Label ID="store_name" runat="server" Text="store name:"></asp:Label>
    &nbsp;
        <asp:TextBox ID="store_name_txt" runat="server"></asp:TextBox>
&nbsp;&nbsp;
        &nbsp;<asp:Button ID="open_button" runat="server" Text="Open!" OnClick="Open_store_click" />
    </div>
    <div style="height: 21px"></div>
    <div style="text-align: center">
        <asp:Label ID="message" runat="server" Font-Size="Medium" style="text-align: center"></asp:Label>
    </div>

</asp:Content>
