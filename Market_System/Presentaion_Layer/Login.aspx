<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Market_System.Presentaion_Layer.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <head>
    <style>
          body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f2f2f2;
            background-size: contain;
            background-repeat:no-repeat;
            background-image:url(images/LogInCover.jpg);
        }
        .form-group  {
             width: 100%;
            padding: 10px;
            font-size: 16px;
            margin-bottom:20px;

            color: white;
            border: none;
            outline:none;
            height:40px;

            border-radius: 4px;
            cursor: pointer;
             text-align: center;
        }
           .form-group :hover {
            background-color: rgb(255, 0, 0);
        }

        .form-group {
            margin-bottom: 20px;
        }
        .LoginBox{
            position: absolute;
            top:50%;
            left:50%;
            transform:translate(-50%,-50%);
            width: 320px;
            height:420px;
            padding:80px 40px;
            box-sizing:border-box;
            background-position-x:center;
            background-color:rgba(0,0,0,0.5);

        }
        
        .user{
            width:100px;
            height:100px;
            overflow:hidden;
            top:-50px;
            position:absolute;
            left:35%
        }
        .text-danger{
            font-weight:bold;
            color:aliceblue;
            text-align:center;
        }
        .text-danger1{
            font-weight:bold;
            color:aliceblue;
            text-align:center;
        }

        .text_username{
            width:100%;
            margin-bottom:20px;
            text-align:center;
            
        }
        .text_password{
             width:100%;
            margin-bottom:20px;
            text-align:center;
        }


        }
        
        
         </style>
        </head>


    <body>

    <div style="display: flex; flex-direction: column; justify-content: center; align-items: center; height: 100vh;">
        <div class="LoginBox">
            <img src="images/loginPhoto1.png" alt="Alternate Text"  class="user"/>
              
            &nbsp;<br />
            <br />

            <div class="text-danger">
            <label  for="txt_username"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Username:<br />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txt_username" ErrorMessage="Please enter a username" ForeColor="Red" ></asp:RequiredFieldValidator>
            <br />
            </label>
              </div>

            <div class="text_username">
             <br />
            <asp:TextBox ID="txt_username" runat="server"  Width="140px" Class="text_username"/>
            <br />
              </div>
             <br />
            <div class="text-danger1">
            <label for="txt_password">Password:</label>
                 </div>
             <div class="text_password">
               <asp:TextBox ID="txt_password" TextMode="Password" runat="server"  Width="140px" />
            </div>
                 <br />
            <asp:RequiredFieldValidator runat="server" ID="password_validator" ControlToValidate="txt_password" Font-Bold="true" ErrorMessage="Please enter a password" ForeColor="Red"></asp:RequiredFieldValidator>
                   <div class="form-group">
        <asp:Button ID="Button1" runat="server" OnClick="Login_click" Text="Login"   Font-Size="Medium" CssClass="btn btn-primary" />
               </div>
                     <asp:Label ID="error_message" runat="server" Font-Size="Large" ForeColor="Red"></asp:Label>

        </div>

 
 
    </div>
</body>


</asp:Content>
