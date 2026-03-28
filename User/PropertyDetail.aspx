<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PropertyDetail.aspx.cs" Inherits="WebApplication1.User.PropertyDetail" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Property Detail</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="/CSS/HomeStyleSheet.css" />
    <style>
        body { margin-top: 70px; background: #f8f9fa; }
        .detail-wrapper { max-width: 1000px; margin: 40px auto; padding: 0 20px; }
        .main-image img { width: 100%; height: 420px; object-fit: cover; border-radius: 12px; }
        .thumb-row { display: flex; gap: 10px; margin-top: 12px; flex-wrap: wrap; }
        .thumb-row img { width: 100px; height: 70px; object-fit: cover; border-radius: 8px; border: 2px solid #ddd; cursor: pointer; }
        .thumb-row img:hover { border-color: #4f7ef7; }
        .info-card { background: white; border-radius: 12px; padding: 30px; margin-top: 25px; box-shadow: 0 2px 10px rgba(0,0,0,0.06); }
        .info-card h2 { font-size: 26px; font-weight: 700; margin-bottom: 5px; }
        .badge-status { display: inline-block; padding: 4px 14px; border-radius: 20px; font-size: 13px; font-weight: 600; margin-left: 10px; }
        .badge-available { background: #d4edda; color: #155724; }
        .badge-rent { background: #cce5ff; color: #004085; }
        .badge-sold { background: #f8d7da; color: #721c24; }
        .price-tag { font-size: 28px; font-weight: 700; color: #2ecc71; margin: 10px 0; }
        .detail-grid { display: flex; gap: 15px; flex-wrap: wrap; margin: 15px 0; }
        .detail-chip { background: #f0f0f0; border-radius: 8px; padding: 8px 16px; font-size: 14px; }
        .detail-chip span { font-weight: 600; display: block; font-size: 11px; color: #888; }
        .desc-box { margin-top: 15px; font-size: 15px; line-height: 1.8; color: #444; }
        .book-btn { background: #4f7ef7; color: white; border: none; padding: 14px 40px; border-radius: 10px; font-size: 16px; font-weight: 600; cursor: pointer; margin-top: 20px; width: 100%; transition: 0.3s; }
        .book-btn:hover { background: #2d5be3; }
        .book-btn:disabled { background: #aaa; cursor: not-allowed; }
        .msg-box { margin-top: 15px; padding: 12px 18px; border-radius: 8px; font-size: 14px; }
        .msg-success { background: #d4edda; color: #155724; }
        .msg-error { background: #f8d7da; color: #721c24; }
        .msg-info { background: #cce5ff; color: #004085; }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <!-- NAVBAR -->
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
                <asp:Label ID="lblWelcome" runat="server"
                    Style="font-weight:bold; margin-right:10px;">
                </asp:Label>
                <asp:Button ID="btnLogout" runat="server"
                    Text="Logout" CssClass="loginBtn"
                    OnClick="btnLogout_Click" />
            </div>
        </nav>

        <div class="detail-wrapper">

            <!-- Back Link -->
            <a href="/User/UserDashboard.aspx"
               style="color:#4f7ef7; text-decoration:none; font-size:14px;">
                ← Back to Properties
            </a>

            <!-- Images -->
            <div class="main-image" style="margin-top:15px;">
                <asp:Image ID="imgMain" runat="server"
                    style="width:100%; height:420px; object-fit:cover; border-radius:12px;" />
            </div>
            <div class="thumb-row">
                <asp:Repeater ID="rptThumbs" runat="server">
                    <ItemTemplate>
                        <img src='/<%# Eval("ImagePath") %>'
                             onclick="document.getElementById('<%=imgMain.ClientID%>').src=this.src" />
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!-- Info Card -->
            <div class="info-card">
                <h2>
                    <asp:Label ID="lblTitle" runat="server"></asp:Label>
                    <asp:Label ID="lblStatusBadge" runat="server" CssClass="badge-status"></asp:Label>
                </h2>

                <div class="price-tag">
                    ₹ <asp:Label ID="lblPrice" runat="server"></asp:Label>
                </div>

                <div class="detail-grid">
                    <div class="detail-chip">
                        <span>Location</span>
                        <asp:Label ID="lblLocation" runat="server"></asp:Label>
                    </div>
                    <div class="detail-chip">
                        <span>Status</span>
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                    </div>
                    <div class="detail-chip">
                        <span>Approval</span>
                        <asp:Label ID="lblApproval" runat="server"></asp:Label>
                    </div>
                </div>

                <div class="desc-box">
                    <asp:Label ID="lblDescription" runat="server"></asp:Label>
                </div>

                <!-- Book Now Button -->
                <asp:Button ID="btnBook" runat="server"
                    Text="Book Now"
                    CssClass="book-btn"
                    OnClick="btnBook_Click" />

                <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
            </div>

        </div>

    </form>
</body>
</html>