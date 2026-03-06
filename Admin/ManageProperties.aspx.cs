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
    public partial class ManageProperties : System.Web.UI.Page
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
                LoadProperties();
            }
        }

        void LoadProperties()
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = "SELECT PropertyId, Title, Location, Price, Status, IsApproved FROM Properties";
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            gvProperties.DataKeyNames = new string[] { "PropertyId" };
            gvProperties.DataSource = dt;
            gvProperties.DataBind();
            con.Close();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            SqlConnection con = new SqlConnection(strcon);
            string query = "SELECT PropertyId, Title, Location, Price, Status, IsApproved FROM Properties WHERE Title LIKE @search OR Location LIKE @search";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            sda.SelectCommand.Parameters.AddWithValue("@search", "%" + keyword + "%");
            DataTable dt = new DataTable();
            sda.Fill(dt);
            gvProperties.DataKeyNames = new string[] { "PropertyId" };
            gvProperties.DataSource = dt;
            gvProperties.DataBind();
        }
        protected void gvProperties_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int propertyId = Convert.ToInt32(e.CommandArgument);
            string status = "";

            if (e.CommandName == "Approve")
                status = "Approved";
            else if (e.CommandName == "Reject")
                status = "Rejected";
            else if (e.CommandName == "Delete")
            {
                DeleteProperty(propertyId);
                return;
            }
            else
                return;

            // Update IsApproved status
            SqlConnection con = new SqlConnection(strcon);
            string query = "UPDATE Properties SET IsApproved=@status WHERE PropertyId=@id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@id", propertyId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            LoadProperties();
        }

        protected void gvProperties_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvProperties.DataKeys[e.RowIndex].Value);
            DeleteProperty(id);
        }

        void DeleteProperty(int id)
        {
            SqlConnection con = new SqlConnection(strcon);
            con.Open();

            // Delete images first due to foreign key
            string deleteImages = "DELETE FROM PropertyImages WHERE PropertyId=@id";
            SqlCommand cmd1 = new SqlCommand(deleteImages, con);
            cmd1.Parameters.AddWithValue("@id", id);
            cmd1.ExecuteNonQuery();

            // Then delete property
            string deleteProperty = "DELETE FROM Properties WHERE PropertyId=@id";
            SqlCommand cmd2 = new SqlCommand(deleteProperty, con);
            cmd2.Parameters.AddWithValue("@id", id);
            cmd2.ExecuteNonQuery();

            con.Close();
            LoadProperties();
        }
    }
}