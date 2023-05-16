<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StrategyBuilderPage.aspx.cs" Inherits="Market_System.Presentaion_Layer.StrategyBuilderPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    

    <asp:Panel ID="MainPanel" runat="server" style="text-align: left;">
        <asp:DropDownList ID="StatementDLL" runat="server" AutoPostBack="true" OnSelectedIndexChanged="StatementDLL_SelectedIndexChanged" Width="213px">
                 <asp:ListItem Text = "--Select Statement or Relation--" Value = ""></asp:ListItem>
            </asp:DropDownList>
    </asp:Panel>

<%--// choose a struct
    // represent it to user:
        // at each end of the struct will be new ListButton depending on Sruct kind
        // something like new LogicORWeb(nested left, nested right) /--%>




</asp:Content>
