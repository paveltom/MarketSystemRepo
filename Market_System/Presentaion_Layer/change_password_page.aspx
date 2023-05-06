<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="change_password_page.aspx.cs" Inherits="Market_System.Presentaion_Layer.change_password_page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: left; font-size: xx-large;">Change your password.</div>
    <div style="height: 58px"></div>
    <div style="text-align: left; height: 43px;">
        <asp:Label ID="Label2" runat="server" Text="new password:"></asp:Label>
        <asp:TextBox ID="new_password" TextMode="Password" runat="server" OnTextChanged="new_password_TextChanged"></asp:TextBox>
    </div>
    <div style="text-align: left"> <asp:Label ID="Label3" runat="server" Text="retype new password:"></asp:Label>
        <asp:TextBox ID="confirm_new_password" textmode="Password" runat="server"></asp:TextBox></div>
    <div style="height: 28px">
    </div>
    <div style="text-align: left">
        <asp:Label ID="message" runat="server" Font-Size="Medium" style="text-align: left"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="change_password_button" OnClick="change_password_click" runat="server" Text="change password" />
&nbsp;&nbsp; </div>
   
</asp:Content>
