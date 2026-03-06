<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddProperty.aspx.cs" Inherits="WebApplication1.Admin.AddProperty" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Property - REMS</title>
    <link rel="stylesheet" href="/css/admin-form.css" />
    <style>
        .form-container {
            width: 550px;
        }

        .input-box {
            width: 100%;
            padding: 10px 12px;
            margin-bottom: 15px;
            border: 1px solid #ccc;
            border-radius: 6px;
            font-size: 14px;
            box-sizing: border-box;
        }

            .input-box:focus {
                outline: none;
                border-color: #667eea;
                box-shadow: 0 0 0 2px rgba(102,126,234,0.2);
            }

        .form-label {
            font-size: 13px;
            color: #555;
            margin-bottom: 5px;
            display: block;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div class="form-container">
            <h2>Add Property</h2>

            <label class="form-label">Title</label>
            <asp:TextBox ID="txtTitle" runat="server"
                CssClass="input-box"
                Placeholder="Enter property title"></asp:TextBox>

            <label class="form-label">Location</label>
            <asp:TextBox ID="txtLocation" runat="server"
                CssClass="input-box"
                Placeholder="Enter location"></asp:TextBox>

            <label class="form-label">Price (₹)</label>
            <asp:TextBox ID="txtPrice" runat="server"
                CssClass="input-box"
                Placeholder="Enter price"></asp:TextBox>

            <label class="form-label">Status</label>
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input-box">
                <asp:ListItem Text="-- Select Status --" Value="" />
                <asp:ListItem Text="Available" Value="Available" />
                <asp:ListItem Text="Sold" Value="Sold" />
                <asp:ListItem Text="Rent" Value="Rent" />
            </asp:DropDownList>

            <label class="form-label">State</label>
            <asp:DropDownList ID="ddlState" runat="server"
                CssClass="input-box"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
            </asp:DropDownList>

            <label class="form-label">City</label>
            <asp:DropDownList ID="ddlCity" runat="server"
                CssClass="input-box">
            </asp:DropDownList>

            <label class="form-label">Property Images (Select multiple)</label>
            <input type="file" name="fileImages"
                multiple="multiple"
                accept=".jpg,.jpeg,.png,.webp"
                class="input-box" />

            <asp:Button ID="btnSave" runat="server"
                Text="Save Property"
                CssClass="btn-submit"
                OnClick="btnSave_Click" />

            <asp:Label ID="lblMsg" runat="server" CssClass="msg"></asp:Label>
        </div>
    </form>
</body>
</html>
