<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDashboard.aspx.cs" Inherits="WebApplication1.User.UserDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="/CSS/HomeStyleSheet.css" />
    <title>User Dashboard</title>
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
                    Style="font-weight: bold; margin-right: 10px;"></asp:Label>
                <asp:Button ID="btnLogout" runat="server"
                    Text="Logout" CssClass="loginBtn"
                    OnClick="btnLogout_Click" />
            </div>
        </nav>

        <!-- HERO -->
        <section class="hero">
            <h2>Find Your Favorite Property</h2>
            <!-- STATS CARDS -->
            <div style="display: flex; gap: 15px; flex-wrap: wrap; margin-bottom: 25px;">
                <div style="background: white; border-radius: 12px; padding: 20px 30px; flex: 1; min-width: 150px; box-shadow: 0 2px 8px rgba(0,0,0,0.07); text-align: center;">
                    <div style="font-size: 28px; font-weight: 700; color: #4f7ef7;">
                        <asp:Label ID="lblTotal" runat="server">0</asp:Label>
                    </div>
                    <div style="font-size: 13px; color: #888; margin-top: 4px;">Total Bookings</div>
                </div>
                <div style="background: white; border-radius: 12px; padding: 20px 30px; flex: 1; min-width: 150px; box-shadow: 0 2px 8px rgba(0,0,0,0.07); text-align: center;">
                    <div style="font-size: 28px; font-weight: 700; color: #2ecc71;">
                        <asp:Label ID="lblApproved" runat="server">0</asp:Label>
                    </div>
                    <div style="font-size: 13px; color: #888; margin-top: 4px;">Approved</div>
                </div>
                <div style="background: white; border-radius: 12px; padding: 20px 30px; flex: 1; min-width: 150px; box-shadow: 0 2px 8px rgba(0,0,0,0.07); text-align: center;">
                    <div style="font-size: 28px; font-weight: 700; color: #f39c12;">
                        <asp:Label ID="lblPending" runat="server">0</asp:Label>
                    </div>
                    <div style="font-size: 13px; color: #888; margin-top: 4px;">Pending</div>
                </div>
                <div style="background: white; border-radius: 12px; padding: 20px 30px; flex: 1; min-width: 150px; box-shadow: 0 2px 8px rgba(0,0,0,0.07); text-align: center;">
                    <div style="font-size: 28px; font-weight: 700; color: #e74c3c;">
                        <asp:Label ID="lblRejected" runat="server">0</asp:Label>
                    </div>
                    <div style="font-size: 13px; color: #888; margin-top: 4px;">Rejected</div>
                </div>
            </div>
            <div class="main-container">

                <asp:DropDownList ID="ddlCity" runat="server"></asp:DropDownList>

                <asp:DropDownList ID="ddlType" runat="server">
                    <asp:ListItem Text="Select Property Type" Value="" />
                    <asp:ListItem Text="Residential" Value="Residential" />
                    <asp:ListItem Text="Commercial" Value="Commercial" />
                    <asp:ListItem Text="Land" Value="Land" />
                </asp:DropDownList>

                <asp:DropDownList ID="ddlStatus" runat="server">
                    <asp:ListItem Text="All Status" Value="" />
                    <asp:ListItem Text="Available" Value="Available" />
                    <asp:ListItem Text="Rent" Value="Rent" />
                    <asp:ListItem Text="Sold" Value="Sold" />
                </asp:DropDownList>

                <asp:Button ID="btnSearch" runat="server"
                    Text="Search"
                    OnClick="btnSearch_Click" />

            </div>
        </section>

        <!-- PROPERTIES -->
        <section class="properties">
            <h1 class="section-title">Latest Properties</h1>
            <div class="all-properties">
                <asp:Repeater ID="rptProperties" runat="server">
                    <ItemTemplate>
                        <div class="property-card">
                            <div class="image-box">
                                <span class="badge"><%# Eval("Status") %></span>
                                <img src='/<%# Eval("ImagePath") %>'
                                    style="width: 100%; height: 200px; object-fit: cover;" />
                            </div>
                            <div class="property-info">
                                <h3><%# Eval("Title") %></h3>
                                <p class="size"><%# Eval("Location") %></p>
                                <p class="price">₹ <%# Eval("Price") %></p>
                                <a href='/User/PropertyDetail.aspx?id=<%# Eval("PropertyId") %>'
                                    style="display: inline-block; margin-top: 8px; color: #4f7ef7;">View Details →
                                </a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </section>

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
