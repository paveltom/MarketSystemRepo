<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="allProducts.aspx.cs" Inherits="Market_System.Presentaion_Layer.allProducts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <section>
            <div>
                <hgroup>
                    <h2><%: Page.Title %></h2>
                </hgroup>

                <asp:ListView ID="productList" runat="server" 
                    DataKeyNames="ProductID" GroupItemCount="4"
                    ItemType="Market_System.DomainLayer.ItemDTO" SelectMethod="productList_GetData">


                    <EmptyDataTemplate>
                        <table >
                            <tr>
                                <td>No data was returned.</td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        <td/>
                    </EmptyItemTemplate>
                    <GroupTemplate>
                        <tr id="itemPlaceholderContainer" runat="server">
                            <td id="itemPlaceholder" runat="server"></td>
                        </tr>
                    </GroupTemplate>
                    <ItemTemplate>
                        <td runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <a href="ProductDetails.aspx?productID=<%#:Item.GetID()%>">
                                            <%--     %><img src="/Catalog/Images/Thumbs/<%#:Item.ImagePath%>" --%>
                                                width="100" height="75" style="border: solid" /></a>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="ProductDetails.aspx?productID=<%#:Item.GetID()%>">
                                            <span>
                                                <%#:Item.GetID()%>
                                            </span>
                                        </a>
                                        <br />
                                        <span>
                                            <b>Price: </b><%#:String.Format("{0:c}", Item.GetID())%>
                                        </span>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                            </p>
                        </td>
                    </ItemTemplate>
                    <LayoutTemplate>
                        <table style="width:100%;">
                            <tbody>
                                <tr>
                                    <td>
                                        <table id="groupPlaceholderContainer" runat="server" style="width:100%">
                                            <tr id="groupPlaceholder"></tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                                <tr></tr>
                            </tbody>
                        </table>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
        </section>
</asp:Content>