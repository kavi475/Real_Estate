<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="WebApplication1.User.Profile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Profile</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

</head>

<body class="bg-light">

    <form id="form1" runat="server">

        <div class="container mt-5">

            <div class="card shadow p-4">

                <h3 class="text-center mb-4">User Profile</h3>

                <div class="mb-3">
                    <label>Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>

                <div class="mb-3">
                    <label>Role</label>
                    <asp:TextBox ID="txtRole" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>

                <asp:Button ID="btnBack" runat="server" Text="Back to Dashboard"
                    CssClass="btn btn-success" OnClick="btnBack_Click" />

            </div>

        </div>

    </form>

</body>
</html>
