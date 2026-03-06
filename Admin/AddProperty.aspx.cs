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
            // STEP 1 - Validate all fields
            if (txtTitle.Text.Trim() == "" || txtLocation.Text.Trim() == "" ||
                txtPrice.Text.Trim() == "" || ddlStatus.SelectedValue == "" ||
                ddlState.SelectedValue == "" || ddlCity.SelectedValue == "")
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "All fields are required.";
                return;
            }

            if (!fileImage.HasFile)
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Please select an image.";
                return;
            }

            // STEP 2 - Validate image extension
            string extension = Path.GetExtension(fileImage.FileName).ToLower();
            if (extension != ".jpg" && extension != ".jpeg" &&
                extension != ".png" && extension != ".webp")
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Only jpg, jpeg, png, webp images allowed.";
                return;
            }

            // STEP 3 - Create unique file name to avoid overwriting
            string fileName = Guid.NewGuid().ToString() + extension;

            // STEP 4 - Save image physically to PropertyImages folder
            string savePath = Server.MapPath("~/PropertyImages/") + fileName;
            fileImage.SaveAs(savePath);

            // STEP 5 - Store only relative path in database
            string imagePath = "PropertyImages/" + fileName;

            // STEP 6 - Save to database
            SqlConnection con = new SqlConnection(strcon);
            con.Open();

            // Insert into Properties and get new PropertyId
            string propQuery = @"INSERT INTO Properties 
                                (Title, Location, Price, Status) 
                                VALUES (@title, @location, @price, @status);
                                SELECT SCOPE_IDENTITY();";

            SqlCommand propCmd = new SqlCommand(propQuery, con);
            propCmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
            propCmd.Parameters.AddWithValue("@location", txtLocation.Text.Trim());
            propCmd.Parameters.AddWithValue("@price", txtPrice.Text.Trim());
            propCmd.Parameters.AddWithValue("@status", ddlStatus.SelectedValue);

            // Get the new PropertyId
            int propertyId = Convert.ToInt32(propCmd.ExecuteScalar());

            // Insert image path into PropertyImages table
            string imgQuery = "INSERT INTO PropertyImages (PropertyId, ImagePath) VALUES (@propertyId, @imagePath)";
            SqlCommand imgCmd = new SqlCommand(imgQuery, con);
            imgCmd.Parameters.AddWithValue("@propertyId", propertyId);
            imgCmd.Parameters.AddWithValue("@imagePath", imagePath);
            imgCmd.ExecuteNonQuery();

            con.Close();

            // STEP 7 - Redirect back to manage properties
            Response.Redirect("/Admin/ManageProperties.aspx");
        }
    }
}