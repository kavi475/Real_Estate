using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace WebApplication1.Admin
{
    public partial class AddProperty : System.Web.UI.Page
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
                LoadStates();
            }
        }

        void LoadStates()
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = "SELECT StateId, StateName FROM States";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            ddlState.DataSource = dr;
            ddlState.DataTextField = "StateName";
            ddlState.DataValueField = "StateId";
            ddlState.DataBind();
            con.Close();
            ddlState.Items.Insert(0, new ListItem("-- Select State --", ""));
        }

        void LoadCities(string stateId)
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = "SELECT CityId, CityName FROM Cities WHERE StateId = @stateId";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@stateId", stateId);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            ddlCity.DataSource = dr;
            ddlCity.DataTextField = "CityName";
            ddlCity.DataValueField = "CityId";
            ddlCity.DataBind();
            con.Close();
            ddlCity.Items.Insert(0, new ListItem("-- Select City --", ""));
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlState.SelectedValue != "")
            {
                LoadCities(ddlState.SelectedValue);
            }
            else
            {
                ddlCity.Items.Clear();
                ddlCity.Items.Insert(0, new ListItem("-- Select City --", ""));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // STEP 1 - Validate fields
            if (txtTitle.Text.Trim() == "" || txtLocation.Text.Trim() == "" ||
                txtPrice.Text.Trim() == "" || ddlStatus.SelectedValue == "" ||
                ddlState.SelectedValue == "" || ddlCity.SelectedValue == "")
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "All fields are required.";
                return;
            }

            // STEP 2 - Check if any files uploaded
            HttpFileCollection uploadedFiles = Request.Files;
            if (uploadedFiles.Count == 0 || uploadedFiles[0].ContentLength == 0)
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Please select at least one image.";
                return;
            }

            // STEP 3 - Save property to database first
            SqlConnection con = new SqlConnection(strcon);
            con.Open();

            string propQuery = @"INSERT INTO Properties 
                        (Title, Location, Price, Status, IsApproved) 
                        VALUES (@title, @location, @price, @status, 'Pending');
                        SELECT SCOPE_IDENTITY();";

            SqlCommand propCmd = new SqlCommand(propQuery, con);
            propCmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
            propCmd.Parameters.AddWithValue("@location", txtLocation.Text.Trim());
            propCmd.Parameters.AddWithValue("@price", txtPrice.Text.Trim());
            propCmd.Parameters.AddWithValue("@status", ddlStatus.SelectedValue);

            int propertyId = Convert.ToInt32(propCmd.ExecuteScalar());

            // STEP 4 - Loop through all uploaded images
            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                HttpPostedFile file = uploadedFiles[i];

                if (file.ContentLength == 0) continue;

                // Validate extension
                string extension = System.IO.Path.GetExtension(file.FileName).ToLower();
                if (extension != ".jpg" && extension != ".jpeg" &&
                    extension != ".png" && extension != ".webp")
                    continue;

                // Create unique file name
                string fileName = Guid.NewGuid().ToString() + extension;

                // Save file physically
                string savePath = Server.MapPath("~/PropertyImages/") + fileName;
                file.SaveAs(savePath);

                // Save path to database
                string imagePath = "PropertyImages/" + fileName;
                string imgQuery = "INSERT INTO PropertyImages (PropertyId, ImagePath) VALUES (@propertyId, @imagePath)";
                SqlCommand imgCmd = new SqlCommand(imgQuery, con);
                imgCmd.Parameters.AddWithValue("@propertyId", propertyId);
                imgCmd.Parameters.AddWithValue("@imagePath", imagePath);
                imgCmd.ExecuteNonQuery();
            }

            con.Close();

            Response.Redirect("/Admin/ManageProperties.aspx");
        }
    }
}