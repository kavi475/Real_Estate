<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDashboard.aspx.cs" Inherits="WebApplication1.UserDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="CSS/HomeStyleSheet.css" />
    <title>User Dashboard</title>
</head>
<body>
    <form id="form1" runat="server">

        <!-- ================= NAVBAR ================= -->
        <nav class="navbar fixed-top" style="background: #f2f2f2; padding: 15px 40px; display: flex; justify-content: space-between; align-items: center; height: 70px;">

            <div class="title">
                <h3>Real Estate Management System</h3>
            </div>

            <ul class="menu">
                <li><a href="UserDashboard.aspx" class="nav-link">Home</a></li>
                <li><a href="About.aspx" class="nav-link">About</a></li>
                <li><a href="Properties.aspx" class="nav-link">Properties</a></li>
                <li><a href="Contact.aspx" class="nav-link">Contact</a></li>
            </ul>

            <!-- 🔴 ONLY THIS PART IS DIFFERENT FROM Home.aspx -->
            <div class="auth">

                <asp:Label ID="lblWelcome" runat="server"
                    Style="font-weight: bold; margin-right: 10px;"></asp:Label>

                <asp:Button ID="btnProfile" runat="server"
                    Text="Profile" CssClass="loginBtn"
                    OnClick="btnProfile_Click" />

                <asp:Button ID="btnLogout" runat="server"
                    Text="Logout" CssClass="loginBtn"
                    OnClick="btnLogout_Click" />

            </div>

        </nav>

        <!-- ================= HERO ================= -->
        <section class="hero">

            <h2>Find Your Favorite Property</h2>

            <div class="main-container">

                <asp:DropDownList ID="ddlCity" runat="server"></asp:DropDownList>
                <asp:DropDownList ID="ddlType" runat="server"></asp:DropDownList>
                <asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList>

                <asp:Button ID="btnSearch" runat="server" Text="Search" />

            </div>

        </section>

        <!-- ================= PROPERTIES ================= -->
        <section class="properties">

            <h1 class="section-title">Latest Properties</h1>

            <div class="all-properties">

                <div class="property-card">
                    <div class="image-box">
                        <span class="badge">Rent</span>
                        <img src="https://images.unsplash.com/photo-1580587771525-78b9dba3b914" />
                    </div>
                    <div class="property-info">
                        <h3>5 BHK Residential House</h3>
                        <p class="size">4830 Sqft</p>
                        <p class="price">₹45,000</p>
                    </div>
                </div>

                <div class="property-card">
                    <div class="image-box">
                        <span class="badge">Sale</span>
                        <img src="https://images.unsplash.com/photo-1570129477492-45c003edd2be" />
                    </div>
                    <div class="property-info">
                        <h3>3 BHK Apartment</h3>
                        <p class="size">2200 Sqft</p>
                        <p class="price">₹65,00,000</p>
                    </div>
                </div>

                <div class="property-card">
                    <div class="image-box">
                        <span class="badge">Rent</span>
                        <img src="https://images.unsplash.com/photo-1568605114967-8130f3a36994" />
                    </div>
                    <div class="property-info">
                        <h3>Villa House</h3>
                        <p class="size">3500 Sqft</p>
                        <p class="price">₹80,000</p>
                    </div>
                </div>

            </div>
        </section>

        <!-- ================= FOOTER ================= -->
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
