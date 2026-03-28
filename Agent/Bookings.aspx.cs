using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace WebApplication1.Agent
{
    public partial class Bookings : System.Web.UI.Page
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
                LoadBookings();
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

        void LoadBookings()
        {
            int agentId = GetAgentId();
            SqlConnection con = new SqlConnection(strcon);

            // Gets all bookings for properties belonging to this agent
            string query = @"
                SELECT 
                    b.BookingId,
                    p.Title,
                    u.Email AS UserEmail,
                    b.BookingDate,
                    b.Status
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
            con.Close();

            gvBookings.DataSource = dt;
            gvBookings.DataBind();
        }

        protected void gvBookings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int bookingId = Convert.ToInt32(e.CommandArgument);
            string newStatus = "";

            if (e.CommandName == "ApproveBooking")
                newStatus = "Approved";
            else if (e.CommandName == "RejectBooking")
                newStatus = "Rejected";

            if (newStatus != "")
            {
                SqlConnection con = new SqlConnection(strcon);
                string query = "UPDATE Bookings SET Status = @status WHERE BookingId = @bid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@status", newStatus);
                cmd.Parameters.AddWithValue("@bid", bookingId);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                lblMsg.Text = "Booking " + newStatus + " successfully.";
                LoadBookings();
            }
        }
    }
}