<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchasePolicyViewPage.aspx.cs" Inherits="Market_System.Presentaion_Layer.PurchasePolicyViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <br>
    <div style="text-align: left;">
        <asp:DropDownList ID="SelectPoliciesToViewList" runat="server" Height="37px" AutoPostBack="True" OnSelectedIndexChanged="SelectPoliciesToViewList_SelectedIndexChanged" Width="213px">
            <asp:ListItem>Store Policies</asp:ListItem>
            <asp:ListItem>Product Policies</asp:ListItem>
        </asp:DropDownList>
        <input id="ProductIDToViewPolicies" type="text" name="productID" placeholder="Enter wanted product ID..." runat="server" visible="false" >
        <asp:Button ID="ShowProductPoliciesButton" runat="server" Text="Show" OnClick="ShowProductPolicies" Height="29px" Width="195px" Visible="false" />
        <asp:Label ID="NoProductIdErrorMessage" runat="server" Font-Size="Large" ForeColor="Red"></asp:Label>
    </div>
    <br>
    <br>
    <div style="text-align: left;">    
        <asp:Button ID="AddNewPolicyButton" runat="server" Text="Add new Policy" OnClick="AddNewPolicyClick" Height="29px" Width="195px"/>
        <br />
        <h1>Existing Policies</h1>
        <br />
        <asp:DataList ID="PoliciesList" runat="server">
            <ItemTemplate>
                <asp:Label ID ="ItemPolicyID" Text="<%# Container.DataItem %>" runat="server" />
                <asp:Button ID="RemoveButton" runat="server" Text="Remove" OnClick="RemovePolicyClick" />
            </ItemTemplate>
        </asp:DataList>
        <br />
        <asp:Button ID="ReturnToStoreButtonID" runat="server" Text="Return to store" OnClick="ReturnToStoreButtonClick" Height="29px" Width="195px"/>
    </div>
    <br />


</asp:Content>
