<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="auction_page.aspx.cs" Inherits="Market_System.Presentaion_Layer.auction_page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div></div>
    <div></div>
    <div></div>
    <div style="text-align: left">
        <asp:Label ID="label2" runat="server" Text="product_id:"></asp:Label>
        <asp:Label ID="product_id_label" runat="server" Text=""></asp:Label>
    </div>
    <div></div>
    <div style="text-align: left"></div>
    <div></div>
    <div></div>
    

    <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label3" runat="server" Text="add your auction:"></asp:Label>
    &nbsp;</div>
    <div style="text-align: left">
        <asp:Label ID="Label4" runat="server" Text="new price:" Font-Size="Medium" ForeColor="Green"></asp:Label>
        <asp:TextBox ID="new_price_text" runat="server"></asp:TextBox>
    </div>
    <div style="text-align: left">
         <asp:Label ID="Label14" runat="server" ForeColor="Green" Font-Size="Medium" Text="Id: "></asp:Label>
         <asp:TextBox ID="TextOfId" runat="server"></asp:TextBox>
         <br />
         <asp:Label ID="Label1" runat="server" ForeColor="Green" Font-Size="Medium" Text="credit card number:"></asp:Label>
         <asp:TextBox ID = "creditTextBox" runat="server"></asp:TextBox>
         <br />
         <asp:Label ID="Label15" runat="server" ForeColor="Green" Font-Size="Medium" Text="Month:"></asp:Label>
         <asp:TextBox ID="TextOfMonth" runat="server"></asp:TextBox>
         <br />
         <asp:Label ID="Label5" runat="server" ForeColor="Green" Font-Size="Medium" Text="Year:"></asp:Label>
         <asp:TextBox ID="TextOfYear" runat="server"></asp:TextBox>
         <br />
         <asp:Label ID="Label6" runat="server" ForeColor="Green" Font-Size="Medium" Text="Holder:"></asp:Label>
         <asp:TextBox ID="TextOfHolder" runat="server"></asp:TextBox>
         <br />
         <asp:Label ID="Label13" runat="server" ForeColor="Green" Font-Size="Medium" Text="CCV:"></asp:Label>

         <asp:TextBox ID="CVVTextBox" runat="server"></asp:TextBox>

    </div>
    <div style="text-align: center">
        <asp:Button ID="Button1" runat="server" Text="set your auction" CssClass="btn btn-primary" OnClick="auction_Click" />
        <asp:Label ID="auction_message" runat="server"  ></asp:Label>
    </div>
    <div></div>
</asp:Content>
