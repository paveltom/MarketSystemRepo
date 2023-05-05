<%@ Page Title="Notfications Center." Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="notifications.aspx.cs" Inherits="Market_System.Presentaion_Layer.notifications" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: left">
        <asp:Label ID="messages_label" runat="server" size="Large" Text="Messages:"></asp:Label>
    </div>
    <div></div>
    <div style="text-align: center">
        <asp:Label ID="no_messages_label" runat="server" Font-Size="X-Large" ForeColor="Red"></asp:Label>
</div>
    <div></div>
    <div><asp:DataList ID="messages_list" runat="server">
            <ItemTemplate>
        <asp:Label Text="<%#Container.DataItem %>" runat="server" />
    </ItemTemplate>
        </asp:DataList></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
</asp:Content>
