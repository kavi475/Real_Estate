using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Agent
{
    public partial class Dashboard : System.Web.UI.Page
    {
        // Connection string sourced from Web.config
        private readonly string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Security check: Ensure agent is logged in
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
                // Retrieve AgentId with complete session isolation
                int agentId = GetAgentId();

                // 1) Dashboard Summary Cards (Data Binding)
                lblTotalProperties.Text = GetTotalProperties(agentId).ToString();
                lblApproved.Text = GetApprovedProperties(agentId).ToString();
                lblPending.Text = GetPendingProperties(agentId).ToString(); // Match the HTML "Pending" card
                lblBookings.Text = GetTotalBookings(agentId).ToString();

                // 2) Financial metrics (10% fixed commission)
                decimal totalRevenue = GetTotalRevenue(agentId);
                decimal commission = totalRevenue * 0.10m;
                lblTotalRevenue.Text = "₹" + totalRevenue.ToString("N2");
                lblCommission.Text = "₹" + commission.ToString("N2");

                // 3) Recent Properties Section
                gvRecentProperties.DataSource = GetRecentProperties(agentId);
                gvRecentProperties.DataBind();

                // 4) Recent Bookings Section
                gvRecentBookings.DataSource = GetRecentBookings(agentId);
                gvRecentBookings.DataBind();
            }
        }

        /// <summary>
        /// Retrieves the Agent ID (UserId) corresponding to the logged-in agent.
        /// First checks the Session["AgentId"], and falls back to looking it up via Session["email"].
        /// </summary>
        public int GetAgentId()
        {
            if (Session["AgentId"] != null)
            {
                return Convert.ToInt32(Session["AgentId"]);
            }

            // Fallback: lookup by email
            using (SqlConnection con = new SqlConnection(strcon))
            {
                string query = "SELECT UserId FROM Users WHERE Email = @email";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar, 100).Value = Session["email"].ToString();
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        int agentId = Convert.ToInt32(result);
                        Session["AgentId"] = agentId; // Cache for downstream isolation
                        return agentId;
                    }
                }
            }

            // Redirect if not found
            Response.Redirect("~/Login.aspx");
            return 0;
        }

        /// <summary>
        /// Returns total properties added by the logged-in agent.
        /// </summary>
        public int GetTotalProperties(int agentId)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                string query = "SELECT COUNT(*) FROM Properties WHERE AgentId = @agentId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@agentId", SqlDbType.Int).Value = agentId;
                    con.Open();
                    return Convert.ToDecimal(cmd.ExecuteScalar()) != null ? Convert.ToInt32(cmd.ExecuteScalar()) : 0;
                }
            }
        }

        /// <summary>
        /// Returns total approved properties (Status = 'Approved') for the agent.
        /// </summary>
        public int GetApprovedProperties(int agentId)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                string query = "SELECT COUNT(*) FROM Properties WHERE AgentId = @agentId AND Status = 'Approved'";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@agentId", SqlDbType.Int).Value = agentId;
                    con.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Returns total rejected properties (Status = 'Rejected') for the agent.
        /// </summary>
        public int GetRejectedProperties(int agentId)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                string query = "SELECT COUNT(*) FROM Properties WHERE AgentId = @agentId AND Status = 'Rejected'";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@agentId", SqlDbType.Int).Value = agentId;
                    con.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Returns total pending properties (Status = 'Pending') for the agent.
        /// </summary>
        public int GetPendingProperties(int agentId)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                string query = "SELECT COUNT(*) FROM Properties WHERE AgentId = @agentId AND Status = 'Pending'";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@agentId", SqlDbType.Int).Value = agentId;
                    con.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Returns total bookings for the agent's properties.
        /// </summary>
        public int GetTotalBookings(int agentId)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                string query = @"SELECT COUNT(*) 
                                 FROM Bookings b
                                 INNER JOIN Properties p ON b.PropertyId = p.PropertyId
                                 WHERE p.AgentId = @agentId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@agentId", SqlDbType.Int).Value = agentId;
                    con.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Returns the total revenue generated from bookings on agent's properties.
        /// </summary>
        public decimal GetTotalRevenue(int agentId)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                string query = @"SELECT ISNULL(SUM(b.TotalAmount), 0)
                                 FROM Bookings b
                                 INNER JOIN Properties p ON b.PropertyId = p.PropertyId
                                 WHERE p.AgentId = @agentId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@agentId", SqlDbType.Int).Value = agentId;
                    con.Open();
                    return Convert.ToDecimal(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Fetches top 5 latest properties added by the agent, ordered by CreatedDate DESC.
        /// </summary>
        public DataTable GetRecentProperties(int agentId)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                string query = @"SELECT TOP 5 
                                     Title, 
                                     Price, 
                                     Status, 
                                     CreatedAt AS [Created Date]
                                 FROM Properties
                                 WHERE AgentId = @agentId
                                 ORDER BY CreatedAt DESC";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@agentId", SqlDbType.Int).Value = agentId;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        /// <summary>
        /// Fetches top 5 latest bookings for agent properties, ordered by BookingDate DESC.
        /// </summary>
        public DataTable GetRecentBookings(int agentId)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                string query = @"SELECT TOP 5 
                                     p.Title AS [Property Title],
                                     u.Email AS [User Name],
                                     b.Status AS [Booking Status],
                                     b.BookingDate AS [Booking Date]
                                 FROM Bookings b
                                 INNER JOIN Properties p ON b.PropertyId = p.PropertyId
                                 INNER JOIN Users u ON b.UserId = u.UserId
                                 WHERE p.AgentId = @agentId
                                 ORDER BY b.BookingDate DESC";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@agentId", SqlDbType.Int).Value = agentId;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }
}