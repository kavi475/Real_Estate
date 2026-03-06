<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PropertyDetails.aspx.cs" Inherits="WebApplication1.PropertyDetails" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Property Details - REMS</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="/css/HomeStyleSheet.css" />
    <style>
        .detail-section {
            margin-top: 100px;
            padding: 40px 0;
            background: #f8f9fa;
            min-height: 100vh;
        }
        .detail-card {
            background: white;
            border-radius: 12px;
            box-shadow: 0 5px 20px rgba(0,0,0,0.08);
            overflow: hidden;
        }
        .detail-img {
            width: 100%;
            height: 400px;
            object-fit: cover;
        }
        .detail-body {
            padding: 30px;
        }
        .detail-title {
            font-size: 28px;
            font-weight: bold;
            color: #2f3640;
            margin-bottom: 10px;
        }
        .detail-price {
            font-size: 24px;
            color: #2ecc71;
            font-weight: bold;
            margin-bottom: 15px;
        }
        .detail-location {
            font-size: 16px;
            color: #666;
            margin-bottom: 10px;
        }
        .detail-status {
            display: inline-block;
            padding: 5px 15px;
            border-radius: 20px;
            font-size: 14px;
            font-weight: bold;
            margin-bottom: 20px;
        }
        .status-available {
            background: #d4edda;
            color: #155724;
        }
        .status-sold {
            background: #f8d7da;
            color: #721c24;
        }
        .status-rent {
            background: #fff3cd;
            color: #856404;
        }
        .info-box {
            background: #f8f9fa;
            border-radius: 10px;
            padding: 20px;
            margin-top: 20px;
        }
        .info-box h5 {
            font-size: 16px;
            color: #888;
            margin-bottom: 5px;
        }
        .info-box p {
            font-size: 18px;
            color: #2f3640;
            font-weight: bold;
            margin: 0;
        }
        .btn-book {
            background: #667eea;
            color: white;
            border: none;
            padding: 12px 40px;
            border-radius: 8px;
            font-size: 16px;
            cursor: pointer;
            margin-top: 20px;
            width: 100%;
        }
        .btn-book:hover {
            background: #5a67d8;
            color: white;
        }
        .btn-back {
            background: #f8f9fa;
            color: #333;
            border: 1px solid #ddd;
            padding: 10px 25px;
            border-radius: 8px;
            font-size: 14px;
            cursor: pointer;
            margin-top: 10px;
            width: 100%;
            text-align: center;
            display: block;
            text-decoration: none;
        }
        .btn-back:hover {
            background: #e9ecef;
            color: #333;
        }
        asp\:Label.success-msg {
            color: green;
        }
    </style>
</head>
<body>
<form id="form1" runat="server">

    <!-- NAVBAR -->
    <nav class="navbar fixed-top" style="background:#f2f2f2; padding: 15px 40px; display:flex; justify-content:space-between; align-items:center; height:70px;">
        <div class="title">
            <h3>Real Estate Management System</h3>
        </div>
        <ul class="menu">
            <li><a href="Home.aspx">Home</a></li>
            <li><a href="About.aspx">About</a></li>
            <li><a href="Properties.aspx">Properties</a></li>
            <li><a href="Contact.aspx">Contact</a></li>
        </ul>
        <div class="auth">
            <asp:Button ID="btnSignup" runat="server" Text="Sign Up" CssClass="loginBtn" OnClick="btnSignup_Click" />
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="loginBtn" OnClick="btnLogin_Click" />
        </div>
    </nav>

    <!-- DETAIL SECTION -->
    <div class="detail-section">
        <div class="container">
            <div class="row">

                <!-- LEFT - IMAGE AND DETAILS -->
                <div class="col-lg-8">
                    <div class="detail-card">
                        <asp:Image ID="imgProperty" runat="server" CssClass="detail-img" />
                        <div class="detail-body">
                            <div class="detail-title">
                                <asp:Label ID="lblTitle" runat="server"></asp:Label>
                            </div>
                            <div class="detail-price">
                                ₹ <asp:Label ID="lblPrice" runat="server"></asp:Label>
                            </div>
                            <div class="detail-location">
                                📍 <asp:Label ID="lblLocation" runat="server"></asp:Label>
                            </div>
                            <asp:Label ID="lblStatus" runat="server" CssClass="detail-status"></asp:Label>

                            <div class="row mt-3">
                                <div class="col-md-6">
                                    <div class="info-box">
                                        <h5>Property Type</h5>
                                        <p><asp:Label ID="lblType" runat="server" Text="Residential"></asp:Label></p>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="info-box">
                                        <h5>Listed Status</h5>
                                        <p><asp:Label ID="lblApproved" runat="server"></asp:Label></p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- RIGHT - BOOKING FORM -->
                <div class="col-lg-4">
                    <div class="detail-card" style="padding: 25px;">
                        <h4 style="margin-bottom:20px; color:#2f3640;">Book This Property</h4>

                        <asp:Label ID="lblMsg" runat="server"
                            Style="display:block; margin-bottom:10px; font-weight:bold;">
                        </asp:Label>

                        <div style="margin-bottom:15px;">
                            <label style="font-size:13px; color:#555; font-weight:bold;">Your Name</label>
                            <asp:TextBox ID="txtName" runat="server"
                                CssClass="form-control"
                                Placeholder="Enter your name"
                                Style="margin-top:5px;"></asp:TextBox>
                        </div>

                        <div style="margin-bottom:15px;">
                            <label style="font-size:13px; color:#555; font-weight:bold;">Phone Number</label>
                            <asp:TextBox ID="txtPhone" runat="server"
                                CssClass="form-control"
                                Placeholder="Enter phone number"
                                Style="margin-top:5px;"></asp:TextBox>
                        </div>

                        <div style="margin-bottom:15px;">
                            <label style="font-size:13px; color:#555; font-weight:bold;">Booking Date</label>
                            <asp:TextBox ID="txtDate" runat="server"
                                CssClass="form-control"
                                TextMode="Date"
                                Style="margin-top:5px;"></asp:TextBox>
                        </div>

                        <asp:Button ID="btnBook" runat="server"
                            Text="Book Now"
                            CssClass="btn-book"
                            OnClick="btnBook_Click" />

                        <a href="Properties.aspx" class="btn-back">← Back to Properties</a>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <!-- FOOTER -->
    <section class="about-footer">
        <div class="about-wrapper">
            <div class="about-col">
                <h2 class="logo">REMS</h2>
                <p>Prime Arcade, Adajan</p>
                <p>Surat - 395009</p>
                <p>+91 8529631235</p>
                <p>info@gmail.com</p>
            </div>
            <div class="about-col">
                <h3>Company</h3>
                <a href="About.aspx">About Us</a>
                <a href="Contact.aspx">Contact Us</a>
                <a href="Properties.aspx">Properties</a>
            </div>
            <div class="about-col">
                <h3>More Links</h3>
                <a href="Login.aspx">Admin</a>
                <a href="Home.aspx">Home</a>
            </div>
            <div class="about-image">
                <img src="https://cdn-icons-png.flaticon.com/512/1946/1946436.png" />
            </div>
        </div>
    </section>

</form>
</body>
</html>