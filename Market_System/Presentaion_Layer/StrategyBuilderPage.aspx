<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StrategyBuilderPage.aspx.cs" Inherits="Market_System.Presentaion_Layer.StrategyBuilderPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div ID="MainDiv" runat="server" align="left" enableviewstate="true" >

    </div>
    <br />
    <br />
    <div ID="SaveDiv" runat="server" align="left" enableviewstate="true" >
        <input id="StrategyNameID" type="text" name="StrategyName" placeholder="Enter new strategy name..." runat="server" Height="30px" Width="200px" visible="true" >
    </div>
    <div ID="StrategyDescriptionDiv" runat="server" align="left" enableviewstate="true" >
        <asp:TextBox ID="StrategyDescriptionID" TextMode="MultiLine" Rows="5" placeholder="Enter new strategy description..." Height="120px" Width="300px" runat="server"/>
    </div>
    <div ID="AddStrategyButtonDiv" runat="server" align="left" enableviewstate="true" >
        <asp:Button ID="AddStrategyButtonID" runat="server" Text="Add Strategy" OnClick="AddStrategyButtonClick" Height="30px" Width="150px" Visible="true" />
        <asp:Button ID="CancelButton" runat="server" Text="Cancel" OnClick="CancelButtonClick" Height="30px" Width="150px" Visible="true" />
    </div>

<%--// choose a struct
    // represent it to user:
        // at each end of the struct will be new ListButton depending on Sruct kind
        // something like new LogicORWeb(nested left, nested right) 
        <asp:PlaceHolder ID="MainPlaceHolder" runat="server" EnableViewState="true">

    </asp:PlaceHolder>
    
    
    
    /--%>




</asp:Content>
