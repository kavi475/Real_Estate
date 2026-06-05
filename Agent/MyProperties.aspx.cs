using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace WebApplication1.Agent
{
    public partial class MyProperties : System.Web.UI.Page
    {
        private readonly string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null || Session["role"]?.ToString() != "Agent")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                RefreshAllGrids();
            }
        }

        private int GetAgentId()
        {
            if (Session["AgentId"] != null)
            {
                return Convert.ToInt32(Session["AgentId"]);
            }

            string email = Session["email"].ToString();
            using (SqlConnection con = new SqlConnection(strcon))
            {
                string query = "SELECT UserId FROM Users WHERE Email = @email";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        int agentId = Convert.ToInt32(result);
                        Session["AgentId"] = agentId;
                        return agentId;
                    }
                }
            }
            Response.Redirect("~/Login.aspx");
            return 0;
        }

        private void RefreshAllGrids()
        {
            int agentId = GetAgentId();
            LoadApprovedProperties(agentId);
            LoadPendingProperties(agentId);
            LoadRejectedProperties(agentId);
        }

        private void LoadApprovedProperties(int agentId)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                string query = @"
                    SELECT 
                        p.PropertyId, p.Title, p.Location, p.Price, p.Status,
                        (SELECT TOP 1 ImagePath FROM PropertyImages 
                         WHERE PropertyId = p.PropertyId) AS ImagePath
                    FROM Properties p
                    WHERE p.AgentId = @agentId AND p.Status = 'Approved'
                    ORDER BY p.PropertyId DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@agentId", agentId);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvApprovedProperties.DataSource = dt;
                        gvApprovedProperties.DataBind();
                    }
                }
            }
        }

        private void LoadPendingProperties(int agentId)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                string query = @"
                    SELECT 
                        p.PropertyId, p.Title, p.Location, p.Price, p.Status,
                        (SELECT TOP 1 ImagePath FROM PropertyImages 
                         WHERE PropertyId = p.PropertyId) AS ImagePath
                    FROM Properties p
                    WHERE p.AgentId = @agentId AND p.Status = 'Pending'
                    ORDER BY p.PropertyId DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@agentId", agentId);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvPendingProperties.DataSource = dt;
                        gvPendingProperties.DataBind();
                    }
                }
            }
        }

        private void LoadRejectedProperties(int agentId)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                string query = @"
                    SELECT 
                        p.PropertyId, p.Title, p.Location, p.Price, p.Status, p.RejectionReason,
                        (SELECT TOP 1 ImagePath FROM PropertyImages 
                         WHERE PropertyId = p.PropertyId) AS ImagePath
                    FROM Properties p
                    WHERE p.AgentId = @agentId AND p.Status = 'Rejected'
                    ORDER BY p.PropertyId DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@agentId", agentId);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvRejectedProperties.DataSource = dt;
                        gvRejectedProperties.DataBind();
                    }
                }
            }
        }

        protected void gvProperties_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteProperty")
            {
                int propertyId = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection con = new SqlConnection(strcon))
                {
                    con.Open();
                    using (SqlTransaction trans = con.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand("DELETE FROM PropertyImages WHERE PropertyId = @pid", con, trans))
                            {
                                cmd.Parameters.AddWithValue("@pid", propertyId);
                                cmd.ExecuteNonQuery();
                            }

                            using (SqlCommand cmd = new SqlCommand(@"
                                IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Bookings]') AND type in (N'U'))
                                    DELETE FROM Bookings WHERE PropertyId = @pid;", con, trans))
                            {
                                cmd.Parameters.AddWithValue("@pid", propertyId);
                                cmd.ExecuteNonQuery();
                            }

                            using (SqlCommand cmd = new SqlCommand(@"
                                IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reviews]') AND type in (N'U'))
                                    DELETE FROM Reviews WHERE PropertyId = @pid;", con, trans))
                            {
                                cmd.Parameters.AddWithValue("@pid", propertyId);
                                cmd.ExecuteNonQuery();
                            }

                            using (SqlCommand cmd = new SqlCommand(@"
                                IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Wishlist]') AND type in (N'U'))
                                    DELETE FROM Wishlist WHERE PropertyId = @pid;", con, trans))
                            {
                                cmd.Parameters.AddWithValue("@pid", propertyId);
                                cmd.ExecuteNonQuery();
                            }

                            using (SqlCommand cmd = new SqlCommand(@"
                                IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Inquiries]') AND type in (N'U'))
                                    DELETE FROM Inquiries WHERE PropertyId = @pid;", con, trans))
                            {
                                cmd.Parameters.AddWithValue("@pid", propertyId);
                                cmd.ExecuteNonQuery();
                            }

                            using (SqlCommand cmd = new SqlCommand("DELETE FROM Properties WHERE PropertyId = @pid", con, trans))
                            {
                                cmd.Parameters.AddWithValue("@pid", propertyId);
                                cmd.ExecuteNonQuery();
                            }

                            trans.Commit();
                            lblMsg.Text = "Property deleted successfully.";
                            lblMsg.ForeColor = System.Drawing.Color.Green;
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            lblMsg.Text = "Error deleting property: " + ex.Message;
                            lblMsg.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }

                RefreshAllGrids();
            }
        }
    }
}