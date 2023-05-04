<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="myShops.aspx.cs" Inherits="Market_System.Presentaion_Layer.myShops" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br /><br /><br />
            <asp:Label runat="server" Text="my shops"></asp:Label>
            <br /><br /><br />
            <asp:DropDownList runat="server" OnSelectedIndexChanged="shops">
            </asp:DropDownList><br /><br />
        </div>
    </form>
</body>
</html>
