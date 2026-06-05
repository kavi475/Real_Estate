<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Add-property.aspx.cs"
    Inherits="WebApplication1.Agent.Add_property" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Add Property</title>
    <link rel="stylesheet" href="/css/agent-dashboard.css" />
    <style>
        .main-content {
            margin-left: 240px;
            padding: 20px;
        }
        .form-box {
            background: #fff;
            padding: 20px;
            max-width: 750px;
            border-radius: 10px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }
        .input-box {
            width: 100%;
            padding: 10px;
            margin: 6px 0;
            border: 1px solid #ddd;
            border-radius: 6px;
        }
        .btn-submit {
            background: #4f7ef7;
            color: white;
            padding: 10px 18px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
        }
    </style>
</head>

<body>
<form id="form1" runat="server" enctype="multipart/form-data">

    
        <!-- ================= SIDEBAR ================= -->
        <div class="sidebar">
            <h2 class="logo">REMS</h2>

            <a href="Dashboard.aspx" class="active">Dashboard</a>
            <a href="/Agent/Add-property.aspx">Add Property</a>
            <a href="/Agent/MyProperties.aspx">My Properties</a>
            <a href="/Agent/Bookings.aspx">Bookings</a>
            <a href="/Agent/Profile.aspx">Profile</a>
            <a href="../Logout.aspx">Logout</a>
        </div>

    <!-- MAIN -->
    <div class="main-content">
        <h2><asp:Literal ID="litPageHeader" runat="server" Text="Add Property" /></h2>

        <div class="form-box">

            <asp:TextBox ID="txtTitle"       runat="server" CssClass="input-box" placeholder="Title" />
            <asp:TextBox ID="txtLocation"    runat="server" CssClass="input-box" placeholder="Location" />
            <asp:TextBox ID="txtPrice"       runat="server" CssClass="input-box" placeholder="Price" />

            <asp:TextBox ID="txtDescription" runat="server"
                TextMode="MultiLine" Rows="3"
                CssClass="input-box" placeholder="Description" />

            <asp:TextBox ID="txtBHK"  runat="server" CssClass="input-box" placeholder="BHK" />
            <asp:TextBox ID="txtArea" runat="server" CssClass="input-box" placeholder="Area (sq ft)" />

            <asp:DropDownList ID="ddlFurnishing" runat="server" CssClass="input-box">
                <asp:ListItem Text="Furnished"      Value="Furnished" />
                <asp:ListItem Text="Semi-Furnished" Value="Semi-Furnished" />
                <asp:ListItem Text="Unfurnished"    Value="Unfurnished" />
            </asp:DropDownList>

            <asp:DropDownList ID="ddlPropertyType" runat="server" CssClass="input-box">
                <asp:ListItem Text="Flat"  Value="Flat" />
                <asp:ListItem Text="House" Value="House" />
                <asp:ListItem Text="Villa" Value="Villa" />
            </asp:DropDownList>

            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input-box">
                <asp:ListItem Text="Available" Value="Available" />
                <asp:ListItem Text="Rent"      Value="Rent" />
                <asp:ListItem Text="Sold"      Value="Sold" />
            </asp:DropDownList>

            <%-- State → City → Locality cascade --%>
            <asp:DropDownList ID="ddlState" runat="server"
                CssClass="input-box"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlState_SelectedIndexChanged" />

            <asp:DropDownList ID="ddlCity" runat="server"
                CssClass="input-box"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlCity_SelectedIndexChanged" />

            <asp:DropDownList ID="ddlLocality" runat="server" CssClass="input-box" />

            <%-- Phase 2 Additional Fields --%>
            <asp:TextBox ID="txtVideoUrl" runat="server" CssClass="input-box" placeholder="YouTube Video / Embed Link" />
            <asp:TextBox ID="txtLatitude" runat="server" CssClass="input-box" placeholder="Latitude (e.g., 12.9716)" />
            <asp:TextBox ID="txtLongitude" runat="server" CssClass="input-box" placeholder="Longitude (e.g., 77.5946)" />
            <asp:TextBox ID="txtMapLink" runat="server" CssClass="input-box" placeholder="Google Maps Location Link" />

            <asp:CheckBox ID="chkFeatured" runat="server" Text=" Featured Property" />

            <br /><br />

            <asp:FileUpload ID="fuImages" runat="server" AllowMultiple="true" CssClass="input-box" />
            <small style="color: #666; display: block; margin-top: -4px; margin-bottom: 10px;">Max 10 images (JPEG, JPG, PNG only)</small>

            <br /><br />

            <asp:Button ID="btnAdd" runat="server"
                Text="Add Property"
                CssClass="btn-submit"
                OnClick="btnAdd_Click" />

            <br /><br />

            <asp:Label ID="lblMsg" runat="server" />

        </div>
    </div>

</form>
</body>
</html>