<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Properties.aspx.cs" Inherits="WebApplication1.User.UserProperties" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Properties - REMS</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="/CSS/HomeStyleSheet.css" />
    <style>
        .property-card { border-radius: 10px; overflow: hidden; box-shadow: 0 2px 10px rgba(0,0,0,0.1); transition: 0.3s; }
        .property-card:hover { transform: translateY(-5px); }
        .property-img { height: 200px; object-fit: cover; }
        .search-box { background: #f8f9fa; padding: 30px; border-radius: 10px; margin-bottom: 40px; }
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

        <div class="container" style="margin-top:100px;">

            <h2 class="text-center mb-4">Find Your Dream Property</h2>

            <!-- SEARCH BOX -->
            <div class="search-box">
    <div class="row g-3">
        <div class="col-md-3">
            <asp:TextBox ID="txtKeyword" runat="server" CssClass="form-control" Placeholder="Search by title or location..."></asp:TextBox>
        </div>
        <div class="col-md-2">
            <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="col-md-2">
            <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="col-md-2">
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                <asp:ListItem Text="All Status" Value="" />
                <asp:ListItem Text="Available" Value="Available" />
                <asp:ListItem Text="Rent" Value="Rent" />
                <asp:ListItem Text="Sold" Value="Sold" />
            </asp:DropDownList>
        </div>
        <div class="col-md-1">
            <asp:TextBox ID="txtMinPrice" runat="server" CssClass="form-control" Placeholder="Min ₹"></asp:TextBox>
        </div>
        <div class="col-md-1">
            <asp:TextBox ID="txtMaxPrice" runat="server" CssClass="form-control" Placeholder="Max ₹"></asp:TextBox>
        </div>
        <div class="col-md-1">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary w-100" OnClick="btnSearch_Click" />
        </div>
    </div>
</div>

            <!-- PROPERTY LIST -->
            <div class="row">
                <asp:Repeater ID="rptProperties" runat="server">
                    <ItemTemplate>
                        <div class="col-md-4 mb-4">
                            <div class="card property-card">
                                <img src='/<%# Eval("ImagePath") %>'
                                     class="card-img-top property-img" />
                                <div class="card-body">
                                    <h5 class="card-title"><%# Eval("Title") %></h5>
                                    <p class="text-muted">📍 <%# Eval("Location") %></p>
                                    <h6 class="text-primary">₹ <%# Eval("Price") %></h6>
                                    <a href='/User/PropertyDetail.aspx?id=<%# Eval("PropertyId") %>'
                                       class="btn btn-outline-primary btn-sm">
                                        View Details
                                    </a>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
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