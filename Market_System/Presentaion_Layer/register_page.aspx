<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="register_page.aspx.cs" Inherits="Market_System.Presentaion_Layer.Register_page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;">
        <h2><%: Title %></h2>

        <div style="text-align: center; margin-bottom: 20px;">
            <label>Username:</label>
            <asp:TextBox ID="txt_username" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="user_name_validator" ControlToValidate="txt_username" ErrorMessage="Please enter a username"></asp:RequiredFieldValidator>
        </div>

        <div style="text-align: center; margin-bottom: 20px;">
            <label>Password:</label>
            <asp:TextBox ID="txt_password" TextMode="Password" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="password_validator" ControlToValidate="txt_password" ErrorMessage="Please enter a password"></asp:RequiredFieldValidator>
        </div>

        <div style="text-align: center; margin-bottom: 20px;">
            <label>Address:</label>
            <asp:TextBox ID="txt_address" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="address_validator" ControlToValidate="txt_address" ErrorMessage="Please enter an address"></asp:RequiredFieldValidator>
        </div>

        <div style="text-align: center; margin-bottom: 20px;">
            <label>Your favorite product:</label>
            <asp:DropDownList ID="ddlproduct" runat="server" >
                <asp:ListItem Text="Beer" Value="Blue" />
                <asp:ListItem Text="Meat" Value="Red" />
            </asp:DropDownList>
        </div>
        <asp:Label ID="message" runat="server" Font-Size="Medium" ForeColor="Black"></asp:Label>
        <div style="text-align: center; margin-bottom: 20px;">
            <asp:Button ID="btnSubmit" runat="server" Text="Register" OnClick="register_button" CssClass="btn btn-primary"></asp:Button>
        </div>

        <div style="text-align: center;">
            <asp:Label ID="messageLabel" runat="server" Visible="False"></asp:Label>
        </div>
    </div>
</asp:Content>
