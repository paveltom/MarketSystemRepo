﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="lottery_page.aspx.cs" Inherits="Market_System.Presentaion_Layer.lottery_page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div></div>
    <div>    <div></div>
    <div></div>
    <div></div>
    <div style="text-align: left">
        <asp:Label ID="label2" runat="server" Text="product_id:"></asp:Label>
        <asp:Label ID="product_id_label" runat="server" Text=""></asp:Label>
    </div>
    <div></div>
    <div style="text-align: left">
    </div>

     <asp:Timer ID="Timer1" runat="server" Interval="500" ontick="Timer1_Tick">
            </asp:Timer>
            <asp:UpdatePanel ID="UpdatePanel1"
                runat="server">
                <ContentTemplate>
                  <div style="text-align: left">
                    <asp:Label ID="time_left" runat="server" Text="time left:"></asp:Label>
                      <asp:Label ID="put_me_time" runat="server" Text=""></asp:Label>
                  </div>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick">
                    </asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
    <div style="text-align: left"></div>
    <div style="text-align: left">
    </div>



    <div></div>
    

    <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label3" runat="server" Text="buy your ticket:"></asp:Label>
    &nbsp;</div>
    <div style="text-align: left">
        <asp:Label ID="Label4" runat="server" Text="desired percentage:" Font-Size="Medium" ForeColor="Green"></asp:Label>
        <asp:TextBox ID="percentage_text" runat="server"></asp:TextBox>
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
        <asp:Button ID="Button1" runat="server" Text="buy your ticket" CssClass="btn btn-primary" OnClick="lottery_Click" />
        <asp:Label ID="lottery_message" runat="server"  ></asp:Label>
    </div>
    </div>
</asp:Content>