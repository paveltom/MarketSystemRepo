<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseStrategyManagePage.aspx.cs" Inherits="Market_System.Presentaion_Layer.PurchaseStrategyManagePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <asp:DropDownList ID="SelectStrategiesToViewList" runat="server" Height="37px" OnSelectedIndexChanged="SelectStrategiesToViewList_SelectedIndexChanged" Width="213px">
        <asp:ListItem>Store Strategies</asp:ListItem>
        <asp:ListItem>Product Strategies</asp:ListItem>
    </asp:DropDownList>
    <input id="ProductIDToViewStrategies" type="text" name="productID" placeholder="Enter wanted product ID..." runat="server"/>
    <br />
    
    <br />
    <div style="text-align: center;">
        <asp:DataList ID="products_list" runat="server">
            <ItemTemplate>
                <asp:Label Text="<%# Container.DataItem %>" runat="server" />
            </ItemTemplate>
        </asp:DataList>
    </div>
    <br />





<%--
    <h1>Login Here</h1>
            <p>Username</p>   
            
    <p>Password</p>
    <input type="password" name="password" placeholder="Enter Password"/>
    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click"/>
    <asp:TextBox ID="txtError" CssClass="text-hide" runat="server">Incorrect username or password!</asp:TextBox>
    <a href="#">Forgot Password</a>   
--%>



            

</asp:Content>
