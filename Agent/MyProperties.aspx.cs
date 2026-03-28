using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace WebApplication1.Agent
{
    public partial class MyProperties : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null || Session["role"]?.ToString() != "Agent")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadProperties();
            }
        }

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

        void LoadProperties()
        {
            int agentId = GetAgentId();
            SqlConnection con = new SqlConnection(strcon);

            // Gets property details + first image for each property
            string query = @"
                SELECT 
                    p.PropertyId, p.Title, p.Location, p.Price, p.Status, p.IsApproved,
                    (SELECT TOP 1 ImagePath FROM PropertyImages 
                     WHERE PropertyId = p.PropertyId) AS ImagePath
                FROM Properties p
                WHERE p.AgentId = @agentId
                ORDER BY p.PropertyId DESC";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@agentId", agentId);

            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Close();

            gvProperties.DataSource = dt;
            gvProperties.DataBind();
        }

        protected void gvProperties_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteProperty")
            {
                int propertyId = Convert.ToInt32(e.CommandArgument);

                SqlConnection con = new SqlConnection(strcon);
                con.Open();

                // Delete images first (foreign key)
                SqlCommand delImages = new SqlCommand(
                    "DELETE FROM PropertyImages WHERE PropertyId = @pid", con);
                delImages.Parameters.AddWithValue("@pid", propertyId);
                delImages.ExecuteNonQuery();

                // Delete bookings linked to property
                SqlCommand delBookings = new SqlCommand(
                    "DELETE FROM Bookings WHERE PropertyId = @pid", con);
                delBookings.Parameters.AddWithValue("@pid", propertyId);
                delBookings.ExecuteNonQuery();

                // Delete the property
                SqlCommand delProp = new SqlCommand(
                    "DELETE FROM Properties WHERE PropertyId = @pid", con);
                delProp.Parameters.AddWithValue("@pid", propertyId);
                delProp.ExecuteNonQuery();

                con.Close();

                lblMsg.Text = "Property deleted successfully.";
                LoadProperties();
            }
        }
    }
}