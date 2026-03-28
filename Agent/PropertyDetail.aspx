<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PropertyDetail.aspx.cs" Inherits="WebApplication1.Agent.PropertyDetail" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Property Detail</title>
    <link rel="stylesheet" href="/CSS/agent-dashboard.css">
    <link rel="stylesheet" href="/CSS/AgentDashboardCommon.css">
    <link rel="stylesheet" href="/CSS/AgentComponent.css">
    <style>
        .detail-box {
            background: #fff;
            border: 1px solid #e0e0e0;
            border-radius: 10px;
            padding: 25px;
            max-width: 800px;
        }
        .detail-box h2 {
            margin-bottom: 5px;
        }
        .detail-row {
            display: flex;
            gap: 20px;
            margin-top: 10px;
            flex-wrap: wrap;
        }
        .detail-item {
            background: #f5f5f5;
            border-radius: 8px;
            padding: 10px 18px;
            font-size: 14px;
        }
        .detail-item span {
            font-weight: bold;
            display: block;
            font-size: 12px;
            color: #888;
            margin-bottom: 3px;
        }
        .description-box {
            margin-top: 20px;
            background: #f9f9f9;
            border-radius: 8px;
            padding: 15px;
            font-size: 14px;
            line-height: 1.6;
        }
        .images-grid {
            display: flex;
            flex-wrap: wrap;
            gap: 12px;
            margin-top: 20px;
        }
        .images-grid img {
            width: 180px;
            height: 130px;
            object-fit: cover;
            border-radius: 8px;
            border: 1px solid #ddd;
        }
        .back-link {
            display: inline-block;
            margin-bottom: 20px;
            color: #4f7ef7;
            text-decoration: none;
            font-size: 14px;
        }
        .badge {
            display: inline-block;
            padding: 4px 12px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: bold;
            margin-left: 10px;
        }
        .badge-pending  { background:#fff3cd; color:#856404; }
        .badge-approved { background:#d4edda; color:#155724; }
        .badge-rejected { background:#f8d7da; color:#721c24; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="sidebar">
            <h2 class="logo">REMS</h2>
            <a href="/Agent/Dashboard.aspx">Dashboard</a>
            <a href="/Agent/Add-property.aspx">Add Property</a>
            <a href="/Agent/MyProperties.aspx" class="active">My Properties</a>
            <a href="/Agent/Bookings.aspx">Bookings</a>
            <a href="/Agent/Profile.aspx">Profile</a>
            <a href="../Logout.aspx">Logout</a>
        </div>
        <div class="main">
            <div class="content">

                <a href="MyProperties.aspx" class="back-link">← Back to My Properties</a>

                <div class="detail-box">

                    <h2>
                        <asp:Label ID="lblTitle" runat="server"></asp:Label>
                        <asp:Label ID="lblApproval" runat="server" CssClass="badge"></asp:Label>
                    </h2>

                    <div class="detail-row">
                        <div class="detail-item">
                            <span>Location</span>
                            <asp:Label ID="lblLocation" runat="server"></asp:Label>
                        </div>
                        <div class="detail-item">
                            <span>Price</span>
                            ₹ <asp:Label ID="lblPrice" runat="server"></asp:Label>
                        </div>
                        <div class="detail-item">
                            <span>Status</span>
                            <asp:Label ID="lblStatus" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="description-box">
                        <asp:Label ID="lblDescription" runat="server"></asp:Label>
                    </div>

                    <h3 style="margin-top:25px;">Images</h3>
                    <div class="images-grid">
                        <asp:Repeater ID="rptImages" runat="server">
                            <ItemTemplate>
                                <img src='/<%# Eval("ImagePath") %>' alt="Property Image" />
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                </div>
            </div>
        </div>
    </form>
</body>
</html>