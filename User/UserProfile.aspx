<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserProfile.aspx.cs" Inherits="WebApplication1.User.UserProfile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Profile - REMS</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="/CSS/HomeStyleSheet.css" />
    <style>
        body {
            margin-top: 70px;
            background: #f8f9fa;
        }

        .profile-wrapper {
            max-width: 550px;
            margin: 40px auto;
            padding: 0 20px;
        }

        .form-box {
            background: white;
            border-radius: 12px;
            padding: 30px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.06);
            margin-bottom: 25px;
        }

            .form-box h4 {
                margin-bottom: 20px;
                font-weight: 600;
            }

        .input-box {
            width: 100%;
            padding: 10px 14px;
            border: 1px solid #ddd;
            border-radius: 8px;
            font-size: 15px;
            margin-bottom: 14px;
            box-sizing: border-box;
        }

            .input-box:focus {
                outline: none;
                border-color: #4f7ef7;
                box-shadow: 0 0 0 3px rgba(79,126,247,0.15);
            }

        .btn-submit {
            width: 100%;
            padding: 11px;
            background: #4f7ef7;
            color: white;
            border: none;
            border-radius: 8px;
            font-size: 15px;
            font-weight: 600;
            cursor: pointer;
            transition: 0.2s;
        }

            .btn-submit:hover {
                background: #2d5be3;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar fixed-top" style="background: #f2f2f2; padding: 15px 40px; display: flex; justify-content: space-between; align-items: center; height: 70px;">
            <div class="title">
                <h3>Real Estate Management System</h3>
            </div>
            <ul class="menu">
                <li><a href="/User/UserDashboard.aspx" class="nav-link">Home</a></li>
                <li><a href="/User/UserAbout.aspx" class="nav-link">About</a></li>
                <li><a href="/User/UserProperties.aspx" class="nav-link">Properties</a></li>
                <li><a href="/User/MyBookings.aspx" class="nav-link">My Bookings</a></li>
                <li><a href="/User/UserContact.aspx" class="nav-link">Contact</a></li>
            </ul>
            <div class="auth">
                <asp:Label ID="lblWelcome" runat="server" Style="font-weight: bold; margin-right: 10px;"></asp:Label>
                <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="loginBtn" OnClick="btnLogout_Click" />
            </div>
        </nav>

        <div class="profile-wrapper">
            <h2 style="margin-bottom: 25px;">My Profile</h2>

            <!-- Update Email -->
            <!-- Update Email & Phone -->
            <div class="form-box">
                <h4>Update Profile</h4>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="input-box" Placeholder="Email"></asp:TextBox>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="input-box" Placeholder="Phone Number"></asp:TextBox>
                <asp:Button ID="btnUpdateEmail" runat="server" CssClass="btn-submit" Text="Update Profile" OnClick="btnUpdateEmail_Click" />
                <asp:Label ID="lblEmailMsg" runat="server" Style="display: block; margin-top: 10px;"></asp:Label>
            </div>

            <!-- Change Password -->
            <div class="form-box">
                <h4>Change Password</h4>
                <asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="input-box" TextMode="Password" Placeholder="Current Password"></asp:TextBox>
                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="input-box" TextMode="Password" Placeholder="New Password"></asp:TextBox>
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="input-box" TextMode="Password" Placeholder="Confirm New Password"></asp:TextBox>
                <asp:Button ID="btnChangePassword" runat="server" CssClass="btn-submit" Text="Change Password" OnClick="btnChangePassword_Click" />
                <asp:Label ID="lblPasswordMsg" runat="server" Style="display: block; margin-top: 10px;"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
