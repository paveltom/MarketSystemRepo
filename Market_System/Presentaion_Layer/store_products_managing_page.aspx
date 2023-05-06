<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="store_products_managing_page.aspx.cs" Inherits="Market_System.Presentaion_Layer.store_products_managing_page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center">
        <asp:Label ID="title_label" Size="Large" runat="server" Text=""></asp:Label>
    </div>
    <div style="text-align: left">
        <asp:Label ID="Label2" runat="server" Text="products in store:"></asp:Label>
    </div>
    <div</div>
    <div><asp:DataList ID="products_list" runat="server">
                  <ItemTemplate>
        <asp:Label Text="<%#Container.DataItem %>" runat="server" />
    </ItemTemplate>
        </asp:DataList></div>
    <div></div>
    <div></div>
    <div style="text-align: center">
    <div style="text-align: center">
        <asp:Label ID="Label9" runat="server" Text="add new product:"></asp:Label>
        &nbsp;<asp:Label ID="Label11" runat="server" Text="name:"></asp:Label>
        <asp:TextBox ID="name_txt" runat="server" Width="49px" ></asp:TextBox>
        &nbsp;<asp:Label ID="Label12" runat="server" Text="description:"></asp:Label>
        <asp:TextBox ID="desc_txt" runat="server" Width="44px"></asp:TextBox>
        &nbsp;<asp:Label ID="Label13" runat="server" Text="price:"></asp:Label>
        <asp:TextBox ID="price_txt" Text="5" runat="server" Width="55px"></asp:TextBox>
        &nbsp;<asp:Label ID="Label14" runat="server" Text="quantity:"></asp:Label>
        <asp:TextBox ID="quantity_txt" Text="100" runat="server" Width="55px"></asp:TextBox>
&nbsp; <asp:Label ID="Label15" runat="server" Text="sale:"></asp:Label>
        <asp:TextBox ID="sale_txt" Text="0" runat="server" Width="20px"></asp:TextBox>
&nbsp;&nbsp; <asp:Label ID="Label16" runat="server" Text="weight:"></asp:Label>
        <asp:TextBox ID="weight_txt" Text="0" runat="server" Width="33px"></asp:TextBox>
&nbsp;&nbsp; <asp:Label ID="Label17" runat="server" Text="dimenstions:"></asp:Label>
        <asp:TextBox ID="dimenstios_txt" runat="server" Text="0.5_20.0_7.0" Width="55px"></asp:TextBox>
&nbsp;&nbsp; <asp:Label ID="Label18" runat="server" Text="attributes:"></asp:Label>
        <asp:TextBox ID="attribute_txt" Text="attr" runat="server" Width="50px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;<asp:Label ID="Label19" runat="server" Text="category:"></asp:Label>
        <asp:DropDownList ID="ddl_categories"  runat="server" AutoPostBack="True" Height="16px" Width="129px" OnSelectedIndexChanged="ddl_categories_SelectedIndexChanged"  >
         <asp:ListItem Text="please select a category" Value="nothing_to_show" />
            </asp:DropDownList></div>
&nbsp;<div style="text-align: center"><asp:Button ID="add_product_button" runat="server" OnClick="add_product_click" Text="!Add" Width="285px" CssClass="btn btn-primary" />
     <asp:Label ID="add_message" runat="server" ForeColor="Red" style="text-align: center"></asp:Label>
      </div>
&nbsp;
       
        
       <div></div>
        <div></div>
    <div style="text-align: center">
        <asp:Label ID="Label10" runat="server" Text="remove product"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="product_to_remove_id" runat="server" Text="please type-in product to remove ID" Width="227px" ></asp:TextBox>
        &nbsp;
        <asp:Button ID="remove_product_button" OnClick="remove_product_button_Click" runat="server" Text="!Remove" CssClass="btn btn-primary" />
&nbsp;<asp:Label ID="remove_message" runat="server" ForeColor="Red"></asp:Label>
        </div>
       </div>
    <div></div>
    <div></div>
    <div style="text-align: center">
        <asp:Label ID="Label29" runat="server" Text="please enter product ID in order to user the functions below."></asp:Label>
        <asp:TextBox ID="product_ID_txt" runat="server"></asp:TextBox>
    </div>
    <div></div>
    <div></div>
    <div style="text-align: center">
        <asp:Label ID="Label21" runat="server" Text="edit product name:"></asp:Label>
        <asp:TextBox ID="edit_name_txt" runat="server" Text="new name" CssClass="glyphicon"></asp:TextBox>
        <asp:Button ID="edit_name_button" OnClick="edit_name_click" runat="server" Text="Edit" CssClass="btn btn-primary" />
        <asp:Label ID="edit_name_message" runat="server" ></asp:Label>
    </div>
    <div></div>
    <div style="text-align: center">
        <asp:Label ID="Label22" runat="server" Text="edit product description:"></asp:Label>
        <asp:TextBox ID="edit_description_txt" runat="server" Text="new description" CssClass="glyphicon"></asp:TextBox>
        <asp:Button ID="edit_name_button0" OnClick="edit_description_click" runat="server" Text="Edit" CssClass="btn btn-primary" />
        <asp:Label ID="edit_description_message" runat="server" ></asp:Label>
    </div>
     <div></div>
    <div class="text-center">
        <asp:Label ID="Label23" runat="server" Text="edit product price:"></asp:Label>
        <asp:TextBox ID="edit_price_txt" runat="server" Text="new price" CssClass="glyphicon"></asp:TextBox>
        <asp:Button ID="edit_price_button" OnClick="edit_price_click" runat="server" Text="Edit" CssClass="btn btn-primary" />
        <asp:Label ID="edit_price_message" runat="server" ></asp:Label>
    </div>
    <div></div>
    <div class="text-center">
        <asp:Label ID="Label24" runat="server" Text="edit product quantity:"></asp:Label>
        <asp:TextBox ID="edit_quantity_txt"   runat="server" Text="new quantity" CssClass="glyphicon" ></asp:TextBox>
        <asp:Button ID="edit_quantity_button" OnClick="edit_quantity_click"  runat="server" Text="Edit" CssClass="btn btn-primary" />
        <asp:Label ID="edit_quantity_message" runat="server" ></asp:Label>
    </div>
     <div></div>
    <div class="text-center">
        <asp:Label ID="Label25" runat="server" Text="edit product wieght:"></asp:Label>
        <asp:TextBox ID="edit_wieght_txt" runat="server" Text="new wieght" CssClass="glyphicon"></asp:TextBox>
        <asp:Button ID="edit_wieght_button" OnClick="edit_wieght_click" runat="server" Text="Edit" CssClass="btn btn-primary" />
        <asp:Label ID="edit_wieght_message" runat="server" ></asp:Label>
    </div>
    <div></div>
     <div class="text-center">
        <asp:Label ID="Label26" runat="server" Text="edit product sale:"></asp:Label>
        <asp:TextBox ID="edit_sale_txt" runat="server" Text="new sale" CssClass="glyphicon"></asp:TextBox>
        <asp:Button ID="edit_sale_button" OnClick="edit_sale_click" runat="server" Text="Edit" CssClass="btn btn-primary" />
        <asp:Label ID="edit_sale_message" runat="server" ></asp:Label>
    </div>
    <div></div>
    <div class="text-center">
        <asp:Label ID="Label27" runat="server" Text="edit product category:"></asp:Label>
        <asp:TextBox ID="edit_category_txt" runat="server" Text="new category" CssClass="glyphicon"></asp:TextBox>
        <asp:Button ID="edit_category_button" OnClick="edit_category_click" runat="server" Text="Edit" CssClass="btn btn-primary" />
        <asp:Label ID="edit_category_message" runat="server" ></asp:Label>
    </div>
     <div></div>
    <div class="text-center">
        <asp:Label ID="Label28" runat="server" Text="edit product dimensions:"></asp:Label>
        <asp:TextBox ID="edit_dimensions_txt" runat="server" Text="new dimensions" CssClass="glyphicon"></asp:TextBox>
        <asp:Button ID="edit_dimensions_button" OnClick="edit_dimensions_click" runat="server" Text="Edit" CssClass="btn btn-primary" />
        <asp:Label ID="edit_dimensions_message" runat="server" ></asp:Label>
    </div>
    <div></div>
     <div></div>
    <div></div>
    <div></div>
     <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
</asp:Content>
