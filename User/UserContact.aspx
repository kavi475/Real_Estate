<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserContact.aspx.cs" Inherits="WebApplication1.User.UserContact" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Contact - REMS</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="/CSS/HomeStyleSheet.css" />
    <style>
        .contact-hero { background: linear-gradient(rgba(0,0,0,0.6), rgba(0,0,0,0.6)), url("https://images.unsplash.com/photo-1560185127-6ed189bf02f4"); background-size: cover; background-position: center; height: 300px; display: flex; align-items: center; justify-content: center; margin-top: 70px; }
        .contact-hero h1 { color: white; font-size: 42px; font-weight: bold; }
        .contact-section { padding: 70px 0; background: #f8f9fa; }
        .contact-card { background: white; border-radius: 12px; padding: 40px; box-shadow: 0 5px 20px rgba(0,0,0,0.08); }
        .contact-info-card { background: black; color: white; border-radius: 12px; padding: 40px; height: 100%; }
        .contact-info-card h4 { color: #2ecc71; margin-bottom: 30px; font-size: 22px; }
        .info-item { display: flex; align-items: flex-start; gap: 15px; margin-bottom: 25px; }
        .info-icon { font-size: 22px; }
        .info-text h6 { color: #2ecc71; margin-bottom: 3px; font-size: 14px; }
        .info-text p { color: #ccc; margin: 0; font-size: 15px; }
        .form-label { font-weight: 500; color: #333; }
        .form-control { border-radius: 8px; padding: 12px; border: 1px solid #ddd; }
        .form-control:focus { border-color: #2ecc71; box-shadow: 0 0 0 0.2rem rgba(46,204,113,0.2); }
        .btn-submit { background: #2ecc71; color: white; border: none; padding: 12px 40px; border-radius: 8px; font-size: 16px; width: 100%; cursor: pointer; transition: 0.3s; }
        .btn-submit:hover { background: #27ae60; }
        .social-links { display: flex; gap: 12px; margin-top: 30px; }
        .social-btn { background: rgba(255,255,255,0.1); color: white; border: 1px solid rgba(255,255,255,0.2); padding: 8px 18px; border-radius: 6px; text-decoration: none; font-size: 14px; transition: 0.3s; }
        .social-btn:hover { background: #2ecc71; color: white; border-color: #2ecc71; }
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

    <!-- CONTACT HERO -->
    <div class="contact-hero">
        <h1>Contact Us</h1>
    </div>

    <!-- CONTACT SECTION -->
    <div class="contact-section">
        <div class="container">
            <div class="row g-4">
                <div class="col-lg-4">
                    <div class="contact-info-card">
                        <h4>Get In Touch</h4>
                        <div class="info-item">
                            <span class="info-icon">📍</span>
                            <div class="info-text"><h6>ADDRESS</h6><p>Prime Arcade, Adajan<br />Surat - 395009</p></div>
                        </div>
                        <div class="info-item">
                            <span class="info-icon">📞</span>
                            <div class="info-text"><h6>PHONE</h6><p>+91 8529631235</p></div>
                        </div>
                        <div class="info-item">
                            <span class="info-icon">✉️</span>
                            <div class="info-text"><h6>EMAIL</h6><p>info@gmail.com</p></div>
                        </div>
                        <div class="info-item">
                            <span class="info-icon">🕐</span>
                            <div class="info-text"><h6>WORKING HOURS</h6><p>Mon - Sat: 9:00 AM - 6:00 PM</p></div>
                        </div>
                        <div class="social-links">
                            <a href="#" class="social-btn">Facebook</a>
                            <a href="#" class="social-btn">Twitter</a>
                            <a href="#" class="social-btn">Instagram</a>
                        </div>
                    </div>
                </div>
                <div class="col-lg-8">
                    <div class="contact-card">
                        <h3 class="mb-4" style="font-size:26px;">Send Us a Message</h3>
                        <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-bold mb-3" Visible="false"></asp:Label>
                        <div class="row g-3">
                            <div class="col-md-6">
                                <label class="form-label">Full Name</label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Placeholder="Enter your name"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Email Address</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="Enter your email"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Phone Number</label>
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" Placeholder="Enter your phone"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Subject</label>
                                <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" Placeholder="Enter subject"></asp:TextBox>
                            </div>
                            <div class="col-12">
                                <label class="form-label">Message</label>
                                <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" Placeholder="Write your message here..."></asp:TextBox>
                            </div>
                            <div class="col-12 mt-2">
                                <asp:Button ID="btnSubmit" runat="server" Text="Send Message" CssClass="btn-submit" OnClick="btnSubmit_Click" />
                            </div>
                        </div>
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

</form>
</body>
</html>