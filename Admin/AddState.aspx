<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddState.aspx.cs" Inherits="WebApplication1.Admin.AddState" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add State</title>
     <link rel="stylesheet" href="/css/admin-form.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <h2>Add State</h2>

            <asp:TextBox ID="txtStateName" runat="server"
                CssClass="input-box"
                Placeholder="Enter state name"></asp:TextBox>

            <asp:Button ID="btnAddState" runat="server"
                CssClass="btn-submit"
                Text="Add State"
                OnClick="btnAddState_Click" />

            <asp:Label ID="lblMsg" runat="server"
                CssClass="msg"></asp:Label>
        </div>
    </form>
</body>
</html>
