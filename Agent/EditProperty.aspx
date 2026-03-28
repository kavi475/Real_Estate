<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProperty.aspx.cs" Inherits="WebApplication1.Agent.EditProperty" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Property</title>
    <link rel="stylesheet" href="/CSS/agent-dashboard.css">
    <link rel="stylesheet" href="/CSS/AgentDashboardCommon.css">
    <link rel="stylesheet" href="/CSS/AgentComponent.css">
    In your EditProperty.aspx, add this inside the
    <head>
        tag after your existing CSS links:
aspx<style>
        .styled-select {
            width: 100%;
            padding: 10px 14px;
            border: 1px solid #ccc;
            border-radius: 8px;
            font-size: 15px;
            background-color: #fff;
            color: #333;
            appearance: none;
            -webkit-appearance: none;
            background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='12' height='12' viewBox='0 0 12 12'%3E%3Cpath fill='%23666' d='M6 8L1 3h10z'/%3E%3C/svg%3E");
            background-repeat: no-repeat;
            background-position: right 12px center;
            cursor: pointer;
            margin-bottom: 12px;
            transition: border-color 0.2s;
        }

            .styled-select:focus {
                outline: none;
                border-color: #4f7ef7;
                box-shadow: 0 0 0 3px rgba(79,126,247,0.15);
            }

            .styled-select:hover {
                border-color: #999;
            }
    </style>
    </head>
    <body>
        <form id="form1" runat="server">
            <div class="sidebar">
                <h2 class="logo">REMS</h2>
                <a href="/Agent/Dashboard.aspx">Dashboard</a>
                <a href="/Agent/Add-property.aspx">Add Property</a>
                <a href="/Agent/MyProperties.aspx" class="active">My Properties</a>
                <a href="/Agent/Bookings.aspx">Bookings</a>
                <a href="/Agent/Profile.aspx">Profile</a>
                <a href="../Logout.aspx">Logout</a>
            </div>
            <div class="main">
                <div class="content">
                    <h1>Edit Property</h1>
                    <div class="form-box">

                        <asp:TextBox ID="txtTitle" runat="server"
                            CssClass="input-box"
                            Placeholder="Property Title">
                        </asp:TextBox>

                        <asp:TextBox ID="txtLocation" runat="server"
                            CssClass="input-box"
                            Placeholder="Location">
                        </asp:TextBox>

                        <asp:TextBox ID="txtPrice" runat="server"
                            CssClass="input-box"
                            Placeholder="Price">
                        </asp:TextBox>

                        <asp:TextBox ID="txtDescription" runat="server"
                            CssClass="input-box"
                            TextMode="MultiLine"
                            Rows="4"
                            Placeholder="Description">
                        </asp:TextBox>

                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="styled-select">
                            <asp:ListItem Text="Available" Value="Available" />
                            <asp:ListItem Text="Rent" Value="Rent" />
                            <asp:ListItem Text="Sold" Value="Sold" />
                        </asp:DropDownList>

                        <asp:Button ID="btnUpdate" runat="server"
                            CssClass="btn-submit"
                            Text="Update Property"
                            OnClick="btnUpdate_Click" />

                        <asp:Label ID="lblMsg" runat="server"
                            Style="display: block; margin-top: 8px;">
                        </asp:Label>

                    </div>
                </div>
            </div>
        </form>
    </body>
</html>
