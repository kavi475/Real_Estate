using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1.Admin
{
    public partial class ManageBookings : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null || Session["role"]?.ToString() != "Admin")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                LoadBookings();
            }
        }

        void LoadBookings()
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = @"SELECT b.BookingId, p.Title AS PropertyTitle, 
                            u.Email AS UserEmail, b.BookingDate, b.Status 
                            FROM Bookings b
                            INNER JOIN Properties p ON b.PropertyId = p.PropertyId
                            INNER JOIN Users u ON b.UserId = u.UserId";
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            gvBookings.DataKeyNames = new string[] { "BookingId" };
            gvBookings.DataSource = dt;
            gvBookings.DataBind();
            con.Close();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            SqlConnection con = new SqlConnection(strcon);
            string query = @"SELECT b.BookingId, p.Title AS PropertyTitle, 
                            u.Email AS UserEmail, b.BookingDate, b.Status 
                            FROM Bookings b
                            INNER JOIN Properties p ON b.PropertyId = p.PropertyId
                            INNER JOIN Users u ON b.UserId = u.UserId
                            WHERE p.Title LIKE @search OR u.Email LIKE @search";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            sda.SelectCommand.Parameters.AddWithValue("@search", "%" + keyword + "%");
            DataTable dt = new DataTable();
            sda.Fill(dt);
            gvBookings.DataSource = dt;
            gvBookings.DataBind();
        }

        protected void gvBookings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int bookingId = Convert.ToInt32(e.CommandArgument);
            string status = "";

            if (e.CommandName == "Approve")
                status = "Approved";
            else if (e.CommandName == "Reject")
                status = "Rejected";
            else
                return;

            SqlConnection con = new SqlConnection(strcon);
            string query = "UPDATE Bookings SET Status=@status WHERE BookingId=@id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@id", bookingId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            LoadBookings();
        }

        protected void gvBookings_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvBookings.DataKeys[e.RowIndex].Value);

            SqlConnection con = new SqlConnection(strcon);
            string query = "DELETE FROM Bookings WHERE BookingId=@id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            LoadBookings();
        }
    }
}