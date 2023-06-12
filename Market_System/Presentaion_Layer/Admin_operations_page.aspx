﻿<%@ Page Title="Behold Admin's Special Arsenal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admin_operations_page.aspx.cs" Inherits="Market_System.Presentaion_Layer.Admin_operations_page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center; font-size: medium">
        <asp:Label ID="Label4" runat="server" Text="Behold Admin's Arsenal"></asp:Label>
    </div>
<div></div>

    <div></div>
    <div style="text-align: center">
        <asp:Label ID="Label2" runat="server" Text="Add New Admin:"></asp:Label>
        <asp:TextBox ID="new_admin_name" text="type-in new admin's name" runat="server" Width="173px"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" OnClick="add_new_admin_click" Text="!Add" CssClass="btn btn-primary"  />
        <asp:Label ID="add_new_admin_message" runat="server" Text=""></asp:Label>
    </div>
    <div></div>
    <div>
    </div>
    <div style="text-align: center">
        <asp:Label ID="Label5" runat="server" Text="show member info"></asp:Label>
        &nbsp;
        <asp:TextBox ID="member_to_show_txt"  text="type-in member's username" runat="server" Width="208px"></asp:TextBox>
&nbsp;
        <asp:Button ID="Button2" runat="server" OnClick="show_user_click" Text="!Show" CssClass="btn btn-primary"  />
        <asp:Label ID="show_user_message" runat="server" Text=""></asp:Label>
    </div>
    <div style="text-align: center">
        <asp:Label ID="member_info" runat="server" Text=""></asp:Label>
    </div>
    <div>
    </div>
    <div style="text-align: center">
        <asp:Label ID="Label6" runat="server" Text="remove member"></asp:Label>
        &nbsp;
        <asp:TextBox ID="member_to_delete_txt"  text="type-in member's username" runat="server" Width="208px"></asp:TextBox>
&nbsp;
        <asp:Button ID="Button3" runat="server" OnClick="remove_user_click" Text="!Remove" CssClass="btn btn-primary"  />
        <asp:Label ID="remove_user_message" runat="server" Text=""></asp:Label>
    </div>
    <div></div>
    <div style="text-align: center">
        <asp:Label ID="sdasd" runat="server" Text="enter date to show the profit of the market:"></asp:Label>
        
        <asp:TextBox ID="market_profit_txt" runat="server"></asp:TextBox>
        <asp:Button ID="market_profit" runat="server" Text="Show"  CssClass="btn btn-primary" OnClick="show_market_sale"  />
        <asp:Label ID="daily_profit_of_market_message" runat="server" ForeColor="#FF3300"></asp:Label>
        
    </div>
    <div style="text-align: center">
        <asp:Label ID="Label8" runat="server" Text="please enter in this format, dd/mm/yyyy"></asp:Label>
        </div>
    <div style="text-align: center">
        <asp:Chart ID="market_sale_chart" runat="server">
            <Series>
                <asp:Series Name="Series1" IsValueShownAsLabel="true"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
    </div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div style="text-align: center">
    </div>
    <div style="text-align: center">
    </div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
</asp:Content>
