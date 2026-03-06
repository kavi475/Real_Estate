<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add-property.aspx.cs" Inherits="WebApplication1.Agent.Add_property" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Property</title>
    <link rel="stylesheet" href="/CSS/agent-dashboard.css">
    <link rel="stylesheet" href="/CSS/AgentDashboardCommon.css">
<link rel="stylesheet" href="/CSS/AgentComponent.css">

</head>
<body>
    <form id="form1" runat="server">
        <div class="sidebar">
            <h2 class="logo">REMS</h2>
            <a href="/Agent/Dashboard.aspx" class="active">Dashboard</a>
            <a href="/Agent/Add-property.aspx">Add Property</a>
            <a href="/Agent/MyProperties.aspx">My Properties</a>
            <a href="/Agent/Bookings.aspx">Bookings</a>
            <a href="/Agent/Profile.aspx">Profile</a>
            <a href="../Logout.aspx">Logout</a>
        </div>

        <div class="main">

            <div class="content">
                <h1>Add Property</h1>

                <div class="form-box">
                    <input type="text" placeholder="Property Title" />
                    <input type="text" placeholder="City" />
                    <input type="number" placeholder="Price" />
                    <textarea placeholder="Description"></textarea>
                    <button>Add Property</button>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
