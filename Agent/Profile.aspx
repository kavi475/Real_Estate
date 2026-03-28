<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="WebApplication1.Agent.Profile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Profile</title>
    <link rel="stylesheet" href="/CSS/agent-dashboard.css">
    <link rel="stylesheet" href="/CSS/AgentDashboardCommon.css">
    <link rel="stylesheet" href="/CSS/AgentComponent.css">
</head>
<body>
    <form id="form1" runat="server">
        <div class="sidebar">
            <h2 class="logo">REMS</h2>
            <a href="/Agent/Dashboard.aspx">Dashboard</a>
            <a href="/Agent/Add-property.aspx">Add Property</a>
            <a href="/Agent/MyProperties.aspx">My Properties</a>
            <a href="/Agent/Bookings.aspx">Bookings</a>
            <a href="/Agent/Profile.aspx" class="active">Profile</a>
            <a href="../Logout.aspx">Logout</a>
        </div>

        <div class="main">
            <div class="content">
                <h1>My Profile</h1>

                <!-- Update Email Section -->
                <div class="form-box">
                    <h3>Update Email</h3>

                    <asp:TextBox ID="txtEmail" runat="server"
                        CssClass="input-box"
                        Placeholder="Enter new email">
                    </asp:TextBox>



                    <asp:TextBox ID="txtPhone" runat="server"
                        CssClass="input-box"
                        Placeholder="Phone Number">
                    </asp:TextBox>

                    <asp:Button ID="btnUpdateEmail" runat="server"
                        CssClass="btn-submit"
                        Text="Update Email"
                        OnClick="btnUpdateEmail_Click" />

                    <asp:Label ID="lblEmailMsg" runat="server"
                        Style="display: block; margin-top: 8px;">
                    </asp:Label>
                </div>

                <!-- Change Password Section -->
                <div class="form-box" style="margin-top: 30px;">
                    <h3>Change Password</h3>

                    <asp:TextBox ID="txtCurrentPassword" runat="server"
                        CssClass="input-box"
                        TextMode="Password"
                        Placeholder="Current Password">
                    </asp:TextBox>

                    <asp:TextBox ID="txtNewPassword" runat="server"
                        CssClass="input-box"
                        TextMode="Password"
                        Placeholder="New Password">
                    </asp:TextBox>

                    <asp:TextBox ID="txtConfirmPassword" runat="server"
                        CssClass="input-box"
                        TextMode="Password"
                        Placeholder="Confirm New Password">
                    </asp:TextBox>

                    <asp:Button ID="btnChangePassword" runat="server"
                        CssClass="btn-submit"
                        Text="Change Password"
                        OnClick="btnChangePassword_Click" />

                    <asp:Label ID="lblPasswordMsg" runat="server"
                        Style="display: block; margin-top: 8px;">
                    </asp:Label>
                </div>

            </div>
        </div>
    </form>
</body>
</html>
