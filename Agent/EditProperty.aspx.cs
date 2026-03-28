using System;
using System.Configuration;
using System.Data.SqlClient;

namespace WebApplication1.Agent
{
    public partial class EditProperty : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;
        int propertyId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null || Session["role"]?.ToString() != "Agent")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            // Get PropertyId from URL
            if (Request.QueryString["id"] == null)
            {
                Response.Redirect("/Agent/MyProperties.aspx");
                return;
            }

            propertyId = Convert.ToInt32(Request.QueryString["id"]);

            if (!IsPostBack)
            {
                LoadProperty();
            }
        }

        void LoadProperty()
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = "SELECT Title, Location, Price, Status FROM Properties WHERE PropertyId = @pid";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@pid", propertyId);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                txtTitle.Text = dr["Title"].ToString();
                txtLocation.Text = dr["Location"].ToString();
                txtPrice.Text = dr["Price"].ToString();
                ddlStatus.SelectedValue = dr["Status"].ToString();
            }

            con.Close();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text.Trim() == "" || txtLocation.Text.Trim() == "" ||
                txtPrice.Text.Trim() == "")
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "All fields are required.";
                return;
            }

            decimal price;
            if (!decimal.TryParse(txtPrice.Text.Trim(), out price))
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Please enter a valid numeric price.";
                return;
            }

            SqlConnection con = new SqlConnection(strcon);
            string query = @"UPDATE Properties 
                             SET Title=@title, Location=@location, 
                                 Price=@price, Status=@status,
                                 IsApproved='Pending'
                             WHERE PropertyId=@pid";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
            cmd.Parameters.AddWithValue("@location", txtLocation.Text.Trim());

            SqlParameter priceParam = new SqlParameter("@price", System.Data.SqlDbType.Decimal);
            priceParam.Precision = 10;
            priceParam.Scale = 2;
            priceParam.Value = price;
            cmd.Parameters.Add(priceParam);

            cmd.Parameters.AddWithValue("@status", ddlStatus.SelectedValue);
            cmd.Parameters.AddWithValue("@pid", propertyId);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Property updated successfully. It will be re-reviewed by Admin.";
        }
    }
}