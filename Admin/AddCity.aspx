<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCity.aspx.cs" Inherits="WebApplication1.Admin.AddCity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add City</title>
     <link rel="stylesheet" href="/css/admin-form.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <h2>Add City</h2>

            <!-- State Dropdown -->
            <asp:DropDownList ID="ddlState" runat="server"
                CssClass="input-box">
            </asp:DropDownList>

            <!-- City Name -->
            <asp:TextBox ID="txtCityName" runat="server"
                CssClass="input-box"
                Placeholder="Enter city name"></asp:TextBox>

            <asp:Button ID="btnAddCity" runat="server"
                CssClass="btn-submit"
                Text="Add City"
                OnClick="btnAddCity_Click" />

            <asp:Label ID="lblMsg" runat="server"
                CssClass="msg"></asp:Label>
        </div>
    </form>
</body>
</html>
