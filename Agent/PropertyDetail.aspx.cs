using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Agent
{
    public partial class PropertyDetail : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null || Session["role"]?.ToString() != "Agent")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (Request.QueryString["id"] == null)
            {
                Response.Redirect("/Agent/MyProperties.aspx");
                return;
            }

            if (!IsPostBack)
            {
                int propertyId = Convert.ToInt32(Request.QueryString["id"]);
                LoadPropertyDetail(propertyId);
                LoadImages(propertyId);
            }
        }

        void LoadPropertyDetail(int propertyId)
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = @"SELECT Title, Location, Price, Status, IsApproved 
                            FROM Properties WHERE PropertyId = @pid";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@pid", propertyId);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                lblTitle.Text = dr["Title"].ToString();
                lblLocation.Text = dr["Location"].ToString();
                lblPrice.Text = dr["Price"].ToString();
                lblStatus.Text = dr["Status"].ToString();

                string approval = dr["IsApproved"].ToString();
                lblApproval.Text = approval;

                if (approval == "Approved")
                    lblApproval.CssClass = "badge badge-approved";
                else if (approval == "Rejected")
                    lblApproval.CssClass = "badge badge-rejected";
                else
                    lblApproval.CssClass = "badge badge-pending";
            }

            con.Close();
        }

        void LoadImages(int propertyId)
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = "SELECT ImagePath FROM PropertyImages WHERE PropertyId = @pid";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@pid", propertyId);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Close();

            rptImages.DataSource = dt;
            rptImages.DataBind();
        }
    }
}