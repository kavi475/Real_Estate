<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyBookings.aspx.cs" Inherits="WebApplication1.User.MyBookings" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Bookings - REMS</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="/CSS/HomeStyleSheet.css" />
    <style>
        body { margin-top: 70px; background: #f8f9fa; }
        .bookings-wrapper { max-width: 1000px; margin: 40px auto; padding: 0 20px; }
        .booking-card { background: white; border-radius: 12px; padding: 20px 25px; margin-bottom: 18px; box-shadow: 0 2px 10px rgba(0,0,0,0.06); }
        .booking-top { display: flex; justify-content: space-between; align-items: center; flex-wrap: wrap; gap: 15px; }
        .booking-info h5 { margin: 0 0 5px 0; font-size: 18px; font-weight: 600; }
        .booking-info p { margin: 0; color: #666; font-size: 14px; }
        .booking-badge { padding: 6px 16px; border-radius: 20px; font-size: 13px; font-weight: 600; }
        .badge-pending   { background: #fff3cd; color: #856404; }
        .badge-approved  { background: #d4edda; color: #155724; }
        .badge-rejected  { background: #f8d7da; color: #721c24; }
        .badge-cancelled { background: #e2e3e5; color: #383d41; }
        .cancel-btn { background: none; border: 1px solid #e74c3c; color: #e74c3c; padding: 6px 16px; border-radius: 8px; font-size: 13px; cursor: pointer; transition: 0.2s; }
        .cancel-btn:hover { background: #e74c3c; color: white; }
        .agent-box { margin-top: 15px; padding: 15px 20px; background: #f0f7ff; border-radius: 10px; border-left: 4px solid #4f7ef7; display: flex; align-items: center; gap: 20px; flex-wrap: wrap; }
        .agent-avatar { width: 46px; height: 46px; border-radius: 50%; background: #4f7ef7; color: white; display: flex; align-items: center; justify-content: center; font-size: 20px; font-weight: 700; flex-shrink: 0; }
        .agent-details h6 { margin: 0 0 4px 0; font-size: 15px; font-weight: 600; color: #333; }
        .agent-details p { margin: 0; font-size: 13px; color: #555; }
        .agent-details a { color: #4f7ef7; text-decoration: none; font-weight: 500; }
        .agent-details a:hover { text-decoration: underline; }
        .agent-label { font-size: 12px; font-weight: 600; color: #4f7ef7; text-transform: uppercase; letter-spacing: 0.5px; margin-bottom: 6px; }
        .no-bookings { text-align: center; padding: 60px 20px; color: #999; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar fixed-top" style="background: #f2f2f2; padding: 15px 40px; display: flex; justify-content: space-between; align-items: center; height: 70px;">
            <div class="title"><h3>Real Estate Management System</h3></div>
            <ul class="menu">
                <li><a href="/User/UserDashboard.aspx" class="nav-link">Home</a></li>
                <li><a href="/User/UserAbout.aspx" class="nav-link">About</a></li>
                <li><a href="/User/UserProperties.aspx" class="nav-link">Properties</a></li>
                <li><a href="/User/MyBookings.aspx" class="nav-link">My Bookings</a></li>
                <li><a href="/User/UserContact.aspx" class="nav-link">Contact</a></li>
                 <li><a href="/User/UserProfile.aspx" class="nav-link">Profile</a></li>
            </ul>
            <div class="auth">
                <asp:Label ID="lblWelcome" runat="server" Style="font-weight:bold; margin-right:10px;"></asp:Label>
                <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="loginBtn" OnClick="btnLogout_Click" />
            </div>
        </nav>

        <div class="bookings-wrapper">
            <h2 style="margin-bottom:25px;">My Bookings</h2>

            <asp:Label ID="lblMsg" runat="server" Visible="false"
                style="display:block; margin-bottom:15px; padding:10px 16px; border-radius:8px; background:#d4edda; color:#155724;">
            </asp:Label>

            <asp:Panel ID="pnlNoBookings" runat="server" Visible="false">
                <div class="no-bookings">
                    <h4>No bookings yet</h4>
                    <p>You have not booked any property yet.</p>
                    <a href="/User/UserProperties.aspx" class="btn btn-primary mt-3">Browse Properties</a>
                </div>
            </asp:Panel>

            <asp:Repeater ID="rptBookings" runat="server" OnItemCommand="rptBookings_ItemCommand">
                <ItemTemplate>
                    <div class="booking-card">

                        <!-- Top Row: property info + status + cancel -->
                        <div class="booking-top">
                            <div class="booking-info">
                                <h5><%# Eval("Title") %></h5>
                                <p>📍 <%# Eval("Location") %> &nbsp;|&nbsp; ₹ <%# Eval("Price") %></p>
                                <p style="margin-top:5px; font-size:13px; color:#999;">
                                    Booked on: <%# Convert.ToDateTime(Eval("BookingDate")).ToString("dd MMM yyyy") %>
                                </p>
                            </div>
                            <div style="display:flex; align-items:center; gap:12px;">
                                <span class='booking-badge badge-<%# Eval("Status").ToString().ToLower() %>'>
                                    <%# Eval("Status") %>
                                </span>
                                <asp:LinkButton runat="server"
                                    CommandName="CancelBooking"
                                    CommandArgument='<%# Eval("BookingId") %>'
                                    CssClass="cancel-btn"
                                    Visible='<%# Eval("Status").ToString() == "Pending" %>'
                                    OnClientClick="return confirm('Cancel this booking?');">
                                    Cancel
                                </asp:LinkButton>
                            </div>
                        </div>

                        <!-- Agent Info Box — only shown for Approved bookings -->
                        <%# Eval("Status").ToString() == "Approved" ? "<div class='agent-box'>" : "" %>
                        <%# Eval("Status").ToString() == "Approved" ? "<div class='agent-avatar'>" + Eval("AgentEmail").ToString().Substring(0,1).ToUpper() + "</div>" : "" %>
                        <%# Eval("Status").ToString() == "Approved" ? "<div class='agent-details'><div class='agent-label'>Agent Contact</div><h6>" + Eval("AgentEmail") + "</h6><p>📧 <a href='mailto:" + Eval("AgentEmail") + "'>" + Eval("AgentEmail") + "</a></p>" + (Eval("AgentPhone") != DBNull.Value && Eval("AgentPhone").ToString() != "" ? "<p>📞 " + Eval("AgentPhone") + "</p>" : "<p style='color:#aaa;'>No phone number added by agent</p>") + "</div></div>" : "" %>

                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <section class="about-footer">
            <div class="about-wrapper">
                <div class="about-col">
                    <h2 class="logo">REMS</h2>
                    <p>Prime Arcade, Adajan</p><p>Surat - 395009</p>
                    <p>+91 8529631235</p><p>info@gmail.com</p>
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