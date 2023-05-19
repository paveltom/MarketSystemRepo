<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseStrategyManagePage.aspx.cs" Inherits="Market_System.Presentaion_Layer.PurchaseStrategyManagePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br>
    <div style="text-align: left;">
        <asp:DropDownList ID="SelectStrategiesToViewList" runat="server" Height="37px" AutoPostBack="True" OnSelectedIndexChanged="SelectStrategiesToViewList_SelectedIndexChanged" Width="213px">
            <asp:ListItem>Store Strategies</asp:ListItem>
            <asp:ListItem>Product Strategies</asp:ListItem>
        </asp:DropDownList>
        <input id="ProductIDToViewStrategies" type="text" name="productID" placeholder="Enter wanted product ID..." runat="server" visible="false" >
        <asp:Button ID="ShowProductStrategiesButton" runat="server" Text="Show" OnClick="ShowProductStrategies" Height="29px" Width="195px" Visible="false" />
        <asp:Label ID="NoProductIdErrorMessage" runat="server" Font-Size="Large" ForeColor="Red"></asp:Label>
    </div>
    <br>
    <br>
    <div style="text-align: left;">    
        <asp:Button ID="AddNewStrategyButton" runat="server" Text="Add new Strategy" OnClick="AddNewStrategyClick" Height="29px" Width="195px"/>
        <br />
        <h1>Existing strategies</h1>
        <br />
        <asp:DataList ID="StrategiesList" runat="server">
            <ItemTemplate>
                <asp:Label Text="<%#Container.DataItem %>" runat="server" />
         </ItemTemplate>
        </asp:DataList>
        <br />
        <asp:Button ID="ReturnToStoreButtonID" runat="server" Text="Return to store" OnClick="ReturnToStoreButtonClick" Height="29px" Width="195px"/>
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
