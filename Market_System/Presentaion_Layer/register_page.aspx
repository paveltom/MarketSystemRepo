<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="register_page.aspx.cs" Inherits="Market_System.Presentaion_Layer.Register_page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <head>
    <style>
          body {
            font-family:  sans-serif;
            margin: 0;
            padding: 0;
            background-size: cover;
            background-repeat:no-repeat;
            background-color: #f2f2f2;
            background-image:url(https://media.istockphoto.com/id/1136526877/photo/handwriting-text-register-now-concept.jpg?s=612x612&w=0&k=20&c=6QMNkPztpOE_G1ywxd8jjp1UUlaaqWkjjl_CFkk66wg=);
        }
          .RegisterBox{
            position: absolute;
            top:50%;
            left:80%;
            transform:translate(-50%,-50%);
            width: 320px;
            height:420px;
            padding:80px 40px;
            box-sizing:border-box;
            background-position-x:center;
            background-color:rgba(0,0,0,0.5);
          }
          .buttonBox:hover{
               background-color: rgb(255, 0, 0);
          }
          .user1{
            width:150px;
            height:100px;
            overflow:hidden;
            top:-50px;
            position:absolute;
            left:30%
          }
          .text-danger2{
             font-weight:bold;
            color:aliceblue;
            text-align:center;
          }



                 </style>
        </head>
        
        <div style="text-align: center; margin-bottom: 20px;">
             <h2><%: Title %></h2>
    <div class="RegisterBox">
        <img src="images/signUp.jpg" alt="Alternate Text"  class="user1"/>
            <div style="text-align: center;">
       
        
            &nbsp;<br />
            <br />
            <br />
            <br />
                 <div class="text-danger2">
            <label >Username:</label>
                </div>
                <asp:TextBox ID="txt_username" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="user_name_validator" ControlToValidate="txt_username" ErrorMessage="Please enter a username" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
        </div>
                <div style="text-align: center; margin-bottom: 20px;">
                     <div class="text-danger2">
            <label>Password:</label>
                         </div>
            <asp:TextBox ID="txt_password" TextMode="Password" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="password_validator" ControlToValidate="txt_password" ErrorMessage="Please enter a password" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
        </div>
              <div style="text-align: center; margin-bottom: 20px;">
                  <div class="text-danger2">
            <label>Address:</label>
                      </div>
            <asp:TextBox ID="txt_address" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="address_validator" ControlToValidate="txt_address" ErrorMessage="Please enter an address" Font-Bold="true" ForeColor="Red"> </asp:RequiredFieldValidator>
        </div>
                <div style="text-align: center; margin-bottom: 20px;">
            <label>Your favorite product:</label>
            <asp:DropDownList ID="ddlproduct" runat="server" >
                <asp:ListItem Text="Beer" Value="Blue" />
                <asp:ListItem Text="Meat" Value="Red" />
            </asp:DropDownList>
        </div>
                 <div class="buttonBox">
                <div style="text-align: center; margin-bottom: 20px;">
            <asp:Button ID="btnSubmit" runat="server" Text="Register" OnClick="register_button" Font-Size="Medium" CssClass="btn btn-primary"></asp:Button>
        </div>
                     </div>
                <div style="text-align: center;">
            <asp:Label ID="messageLabel" runat="server" Font-Bold="true" ></asp:Label>
        </div>
          </div>

    </div>


 
</asp:Content>
