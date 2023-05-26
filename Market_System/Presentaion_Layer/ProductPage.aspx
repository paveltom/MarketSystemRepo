<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductPage.aspx.cs" Inherits="Market_System.Presentaion_Layer.ProductPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <div style="text-align: left; margin-bottom: 20px;">
        <asp:Label ID="quantityToAddLabel" runat="server" Text="amount: "></asp:Label>
        <asp:TextBox ID="quantityToAdd" runat="server"></asp:TextBox>
        <asp:Button ID="addToCart" runat="server" Text="add To Cart" OnClick="addToCart_Click" CssClass="btn btn-primary" />
        <asp:Label ID="clickmsg" runat="server" Text=""></asp:Label>
    </div>
    <div>&nbsp;</div>

    <div style="text-align: left">
        <asp:Label ID="id" runat="server" Text=""></asp:Label>
    </div>
    <div>&nbsp;</div>
     <div style="text-align: left">
        <asp:Label ID="price" runat="server" style="text-align: left" Text=""></asp:Label>
    </div>
    <div>&nbsp;</div>
    <div style="text-align: left">
        <asp:Label ID="quantity" runat="server" Text=""></asp:Label>
    </div>
    <div>&nbsp;</div>
    <div></div>
    <div style="text-align: left">
        <asp:Label ID="description" runat="server" style="text-align: left" Text=""></asp:Label>
    </div>
    <div></div>
    <div></div>
    <div style="text-align: left">
        <asp:Label ID="add_commnet_label" runat="server" Text="add your comment:"></asp:Label>
        <asp:TextBox ID="commnet_txtbox" runat="server"></asp:TextBox>
        <asp:Label ID="rating_label" runat="server" Text="rating:"></asp:Label>
        <asp:DropDownList ID="ddl_rating" runat="server" AutoPostBack="True" >
             <asp:ListItem Text="Please choose" Value="nothing_to_show" />
            <asp:ListItem Text="1" Value="1" />
            <asp:ListItem Text="2" Value="2" />
            <asp:ListItem Text="3" Value="3" />
            <asp:ListItem Text="4" Value="4" />
            <asp:ListItem Text="5" Value="5" />
       
    </asp:DropDownList>
        <asp:Button ID="comment_button" runat="server" Text="Comment" OnClick="add_commnet_click" CssClass="btn btn-primary" />
        <asp:Label ID="comment_message" runat="server" Text=""></asp:Label>
    </div>
    <div></div>
    <div style="text-align: left">
        <asp:Label ID="Label2" runat="server" Text="comments:"></asp:Label>
    </div>
    <div><asp:DataList ID="comments_list" runat="server">
            <ItemTemplate>
                <asp:Label Text="<%# Container.DataItem %>" runat="server" />
            </ItemTemplate>
        </asp:DataList></div>

    
</asp:Content>

