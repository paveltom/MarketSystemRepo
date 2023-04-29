<%@ Page Title="register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="register_page.aspx.cs" Inherits="Market_System.Presentaion_Layer.Register_page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    
    <div>
        <label>username:</label>
        <asp:textbox ID="txt_username" runat="server" />
        <asp:RequiredFieldValidator runat="server" ID="user_name_validator" ControlToValidate="txt_username" ErrorMessage="please enter a username"></asp:RequiredFieldValidator>
    </div>
        <div>
        <label>password:</label>
        <asp:textbox ID="txt_password" TextMode="Password" runat="server" />
            <asp:RequiredFieldValidator runat="server" ID="password_validator" ControlToValidate="txt_password" ErrorMessage="please enter a password"></asp:RequiredFieldValidator>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </div>
         <div>
        <label>address:</label>
        <asp:textbox ID="txt_address" runat="server" />
               <asp:RequiredFieldValidator runat="server" ID="address_validator" ControlToValidate="txt_address" ErrorMessage="please enter an address"></asp:RequiredFieldValidator>
    </div>
    <div>
        <label>your favorite product</label>
        <asp:DropDownList ID="ddlproduct" runat="server" >
            <asp:ListItem Text="beer" Value="Blue" />
             <asp:ListItem Text="meat" Value="Red" />
        </asp:DropDownList>
    </div>
    <div>
        <asp:Button ID="btnSubmit" runat="server" Text="register" Onclick="register_button"/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="message" runat="server" Font-Size="Medium" ForeColor="Black"></asp:Label>
    &nbsp;</div>


    <address>
        <strong>Support:</strong>   <a href="mailto:Support@example.com">Support@example.com</a><br />
        <strong>Marketing:</strong> <a href="mailto:Marketing@example.com">Marketing@example.com</a>
    </address>
</asp:Content>

