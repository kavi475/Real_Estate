<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserAbout.aspx.cs" Inherits="WebApplication1.User.UserAbout" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>About Us - REMS</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="/CSS/HomeStyleSheet.css" />
    <style>
        .hero { background: #0d6efd; color: white; padding: 60px 0; text-align: center; }
        .section { padding: 60px 0; }
        .icon-box { background: #f8f9fa; padding: 25px; border-radius: 10px; text-align: center; transition: 0.3s; }
        .icon-box:hover { transform: translateY(-5px); box-shadow: 0 4px 10px rgba(0,0,0,0.1); }
        .team img { border-radius: 10px; }
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
                <li><a href="/User/UserProfile.aspx" class="nav-link">Profile</a></li>
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

        <!-- Hero Section -->
        <section class="hero">
            <div class="container">
                <h1 class="display-4">About REMS</h1>
                <p class="lead">Your trusted platform for buying, selling, and renting properties.</p>
            </div>
        </section>

        <!-- About Section -->
        <section class="section">
            <div class="container">
                <div class="row align-items-center">
                    <div class="col-md-6">
                        <img src="https://images.unsplash.com/photo-1560518883-ce09059eeffa" class="img-fluid rounded" />
                    </div>
                    <div class="col-md-6">
                        <h2>Who We Are</h2>
                        <p>REMS (Real Estate Management System) is a modern online platform designed to simplify the process of buying, selling, and renting properties. Our goal is to connect property owners, agents, and customers in one easy-to-use system.</p>
                        <p>With REMS, users can explore property listings, connect with agents, manage bookings, and find their dream home quickly and securely.</p>
                    </div>
                </div>
            </div>
        </section>

        <!-- Features Section -->
        <section class="section bg-light">
            <div class="container">
                <div class="text-center mb-5">
                    <h2>Why Choose Us</h2>
                    <p>We make real estate simple and efficient.</p>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="icon-box">
                            <h4>Verified Properties</h4>
                            <p>All listed properties are verified to ensure authenticity and trust.</p>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="icon-box">
                            <h4>Professional Agents</h4>
                            <p>Connect with experienced and trusted real estate agents.</p>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="icon-box">
                            <h4>Secure Booking</h4>
                            <p>Book property visits easily and safely through our system.</p>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <!-- Team Section -->
        <section class="section">
            <div class="container">
                <div class="text-center mb-5">
                    <h2>Our Team</h2>
                    <p>The people behind REMS</p>
                </div>
                <div class="row text-center team">
                    <div class="col-md-4">
                        <img src="https://randomuser.me/api/portraits/men/32.jpg" class="img-fluid mb-3" />
                        <h5>Rahul Sharma</h5>
                        <p>Founder & CEO</p>
                    </div>
                    <div class="col-md-4">
                        <img src="https://randomuser.me/api/portraits/women/44.jpg" class="img-fluid mb-3" />
                        <h5>Priya Mehta</h5>
                        <p>Marketing Head</p>
                    </div>
                    <div class="col-md-4">
                        <img src="https://randomuser.me/api/portraits/men/50.jpg" class="img-fluid mb-3" />
                        <h5>Arjun Patel</h5>
                        <p>Lead Developer</p>
                    </div>
                </div>
            </div>
        </section>

    </form>

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
                <a href="/User/UserAbout.aspx">About Us</a>
                <a href="/User/UserContact.aspx">Contact Us</a>
                <a href="/User/UserProperties.aspx">Properties</a>
            </div>
            <div class="about-col">
                <h3>More Links</h3>
                <a href="/User/UserDashboard.aspx">Home</a>
            </div>
            <div class="about-image">
                <img src="https://cdn-icons-png.flaticon.com/512/1946/1946436.png" />
            </div>
        </div>
    </section>
    <footer class="bg-dark text-white text-center p-3">
        <p>© 2026 REMS - Real Estate Management System | All Rights Reserved</p>
    </footer>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>