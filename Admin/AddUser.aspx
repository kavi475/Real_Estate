<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="WebApplication1.Admin.AddUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add User</title>
    <link rel="stylesheet" href="/css/admin-form.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <h2>Add User</h2>

            <asp:TextBox ID="txtEmail" runat="server"
                CssClass="input-box"
                Placeholder="Enter email"></asp:TextBox>

            <asp:TextBox ID="txtPassword" runat="server"
                CssClass="input-box"
                TextMode="Password"
                Placeholder="Enter password"></asp:TextBox>

            <asp:Button ID="btnAddUser" runat="server"
                CssClass="btn-submit"
                Text="Add User"
                OnClick="btnAddUser_Click" />

            <asp:Label ID="lblMsg" runat="server"
                CssClass="msg"></asp:Label>
        </div>
    </form>
</body>
</html>
