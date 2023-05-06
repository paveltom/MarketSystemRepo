<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Market_System.Presentaion_Layer.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="display: flex; flex-direction: column; justify-content: center; align-items: center; height: 100vh;">
        <div style="margin-bottom: 20px; text-align: center;">
            <asp:Image ID="Image1" runat="server" src="images/Login1.jpg" Height="259px" Width="342px"/>
            &nbsp;<br />
            <br />
            <label for="txt_username">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Username:<br />
            <asp:RequiredFieldValidator runat="server" ID="user_name_validator" ControlToValidate="txt_username" ErrorMessage="Please enter a username" CssClass="text-danger"></asp:RequiredFieldValidator>
            <br />
            </label>
            <br />
            <asp:TextBox ID="txt_username" runat="server"  Width="140px" />
            <br />
            <br />
            <label for="txt_password">Password:</label></div>

        <div style="margin-bottom: 20px; text-align: center;">
            &nbsp;<asp:TextBox ID="txt_password" TextMode="Password" runat="server"  Width="140px" />
            <br />
            <asp:RequiredFieldValidator runat="server" ID="password_validator" ControlToValidate="txt_password" ErrorMessage="Please enter a password" CssClass="text-danger"></asp:RequiredFieldValidator>
        </div>

        <asp:Button ID="Button1" runat="server" OnClick="Login_click" Text="Login" CssClass="btn btn-primary" />
         <asp:Label ID="error_message" runat="server" Font-Size="Large" ForeColor="Red"></asp:Label>
    </div>
</asp:Content>
