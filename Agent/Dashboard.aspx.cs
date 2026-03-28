using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1.Agent
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null || Session["role"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (Session["role"].ToString() != "Agent")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                int agentId = GetAgentId();
                LoadTotalProperties(agentId);
                LoadApprovedProperties(agentId);
                LoadPendingProperties(agentId);
                LoadTotalBookings(agentId);
                LoadRecentProperties(agentId);
                LoadRecentBookings(agentId);
            }
        }

        // Get logged in agent's UserId from DB
        int GetAgentId()
        {
            string email = Session["email"].ToString();
            SqlConnection con = new SqlConnection(strcon);
            string query = "SELECT UserId FROM Users WHERE Email = @email";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@email", email);
            con.Open();
            int agentId = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return agentId;
        }

        // Card 1 - Total Properties by this agent
        void LoadTotalProperties(int agentId)
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = "SELECT COUNT(*) FROM Properties WHERE AgentId = @agentId";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@agentId", agentId);
            con.Open();
            lblTotalProperties.Text = cmd.ExecuteScalar().ToString();
            con.Close();
        }

        // Card 2 - Approved Properties by this agent
        void LoadApprovedProperties(int agentId)
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = "SELECT COUNT(*) FROM Properties WHERE AgentId = @agentId AND IsApproved = 'Approved'";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@agentId", agentId);
            con.Open();
            lblApproved.Text = cmd.ExecuteScalar().ToString();
            con.Close();
        }

        // Card 3 - Pending Properties by this agent
        void LoadPendingProperties(int agentId)
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = "SELECT COUNT(*) FROM Properties WHERE AgentId = @agentId AND IsApproved = 'Pending'";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@agentId", agentId);
            con.Open();
            lblPending.Text = cmd.ExecuteScalar().ToString();
            con.Close();
        }

        // Card 4 - Total Bookings on agent's properties
        void LoadTotalBookings(int agentId)
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = @"SELECT COUNT(*) FROM Bookings b
                            INNER JOIN Properties p ON b.PropertyId = p.PropertyId
                            WHERE p.AgentId = @agentId";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@agentId", agentId);
            con.Open();
            lblBookings.Text = cmd.ExecuteScalar().ToString();
            con.Close();
        }

        // Grid 1 - Recent 5 Properties by this agent
        void LoadRecentProperties(int agentId)
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = @"SELECT TOP 5 PropertyId, Title, Location, Price, Status, IsApproved 
                            FROM Properties 
                            WHERE AgentId = @agentId 
                            ORDER BY PropertyId DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@agentId", agentId);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            gvRecentProperties.DataSource = dt;
            gvRecentProperties.DataBind();
            con.Close();
        }

        // Grid 2 - Recent 5 Bookings on agent's properties
        void LoadRecentBookings(int agentId)
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = @"SELECT TOP 5 b.BookingId, p.Title AS Property, 
                            u.Email AS UserEmail, b.BookingDate, b.Status
                            FROM Bookings b
                            INNER JOIN Properties p ON b.PropertyId = p.PropertyId
                            INNER JOIN Users u ON b.UserId = u.UserId
                            WHERE p.AgentId = @agentId
                            ORDER BY b.BookingId DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@agentId", agentId);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            gvRecentBookings.DataSource = dt;
            gvRecentBookings.DataBind();
            con.Close();
        }
    }
}