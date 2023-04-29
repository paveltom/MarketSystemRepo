<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Market_System.Presentaion_Layer.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <label>
    username:</label><asp:textbox ID="txt_username" runat="server" Text="artanis" />
    <asp:RequiredFieldValidator runat="server" ID="user_name_validator" ControlToValidate="txt_username" ErrorMessage="please enter a username"></asp:RequiredFieldValidator>
    <br />
    <label>
    password:</label><asp:textbox ID="txt_password" TextMode="Password" runat="server" />
    <asp:RequiredFieldValidator runat="server" ID="password_validator" ControlToValidate="txt_password" ErrorMessage="please enter a password"></asp:RequiredFieldValidator>
    <br />
    <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="Button1" runat="server" OnClick="Login_click"  Text="Login" Width="145px" />
    <br />
    <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="error_message" runat="server" Font-Size="Large" ForeColor="Red"></asp:Label>
    <br />
    <br />
</asp:Content>
