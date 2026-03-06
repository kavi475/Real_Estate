<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Properties.aspx.cs" Inherits="WebApplication1.Properties" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<title>Properties</title>

<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
<link rel="stylesheet" href="/css/HomeStyleSheet.css" />

<style>
    .property-card {
        border-radius: 10px;
        overflow: hidden;
        box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        transition: 0.3s;
    }

        .property-card:hover {
            transform: translateY(-5px);
        }

    .property-img {
        height: 200px;
        object-fit: cover;
    }

    .search-box {
        background: #f8f9fa;
        padding: 30px;
        border-radius: 10px;
        margin-bottom: 40px;
    }
</style>

</head>


<body>

    <form id="form1" runat="server">
        <!-- NAVBAR START  -->
        <div class="navbar">

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
                <asp:Button ID="btnSignup" runat="server" Text="Sign Up" CssClass="loginBtn" OnClick="btnSignup_Click"/>
                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="loginBtn" OnClick="btnLogin_Click"/>
            </div>

        </div>
        <!-- NAVBAR END -->
        <div class="container" style="margin-top: 100px;">

            <h2 class="text-center mb-4">Find Your Dream Property</h2>

            <!-- SEARCH BOX -->

            <div class="search-box">

                <div class="row g-3">

                    <div class="col-md-4">
                        <asp:TextBox ID="txtKeyword" runat="server" CssClass="form-control" Placeholder="Search property..."></asp:TextBox>
                    </div>

                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>

                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>

                    <div class="col-md-2">
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

                                <img src='<%# Eval("ImagePath") %>' class="card-img-top property-img" />

                                <div class="card-body">

                                    <h5 class="card-title">
                                        <%# Eval("Title") %>
                                    </h5>

                                    <p class="text-muted">
                                        📍 <%# Eval("Location") %>
                                    </p>

                                    <h6 class="text-primary">₹ <%# Eval("Price") %>
                                    </h6>

                                    <a href='PropertyDetails.aspx?id=<%# Eval("PropertyId") %>'
                                        class="btn btn-outline-primary btn-sm">View Details

                                    </a>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <!-- PAGE CONTENT ABOVE -->

        <!-- FOOTER START (PUT AT BOTTOM) -->
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
        <!-- FOOTER END -->
    </form>
</body>
</form>
</body>
</html>
