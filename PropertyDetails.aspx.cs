using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1
{
    public partial class PropertyDetails : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = Request.QueryString["id"];
                if (string.IsNullOrEmpty(id))
                {
                    Response.Redirect("Properties.aspx");
                    return;
                }
                LoadPropertyDetails(id);
            }
        }

        void LoadPropertyDetails(string id)
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = @"SELECT p.PropertyId, p.Title, p.Location, p.Price, 
                            p.Status, p.IsApproved,
                            pi.ImagePath
                            FROM Properties p
                            LEFT JOIN PropertyImages pi ON p.PropertyId = pi.PropertyId
                            WHERE p.PropertyId = @id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                lblTitle.Text = dr["Title"].ToString();
                lblPrice.Text = dr["Price"].ToString();
                lblLocation.Text = dr["Location"].ToString();
                lblApproved.Text = dr["IsApproved"].ToString();

                // Set status badge color
                string status = dr["Status"].ToString();
                lblStatus.Text = status;
                if (status == "Available")
                    lblStatus.CssClass = "detail-status status-available";
                else if (status == "Sold")
                    lblStatus.CssClass = "detail-status status-sold";
                else
                    lblStatus.CssClass = "detail-status status-rent";

                // Set image
                string imagePath = dr["ImagePath"].ToString();
                imgProperty.ImageUrl = string.IsNullOrEmpty(imagePath)
                    ? "https://via.placeholder.com/800x400?text=No+Image"
                    : "/" + imagePath;
            }
            con.Close();
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "" || txtPhone.Text.Trim() == "" || txtDate.Text.Trim() == "")
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "All fields are required.";
                return;
            }

            string id = Request.QueryString["id"];

            SqlConnection con = new SqlConnection(strcon);
            string query = "INSERT INTO Bookings (PropertyId, BookingDate, Status) VALUES (@propertyId, @date, @status)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@propertyId", id);
            cmd.Parameters.AddWithValue("@date", txtDate.Text);
            cmd.Parameters.AddWithValue("@status", "Pending");
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "✅ Booking submitted successfully!";
            txtName.Text = "";
            txtPhone.Text = "";
            txtDate.Text = "";
        }

        protected void btnSignup_Click(object sender, EventArgs e)
        {
            Response.Redirect("Signup.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}