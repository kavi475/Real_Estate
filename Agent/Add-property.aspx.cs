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

namespace WebApplication1.Agent
{
    public partial class Add_property : System.Web.UI.Page
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
                LoadStates();
            }
        }

        int GetAgentId()
        {
            string email = Session["email"].ToString();
            SqlConnection con = new SqlConnection(strcon);
            string query = "SELECT UserId FROM Users WHERE Email = @email AND Role = 'Agent'";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@email", email);
            con.Open();
            object result = cmd.ExecuteScalar();
            con.Close();

            if (result == null)
            {
                Response.Redirect("~/Login.aspx");
                return 0;
            }

            return Convert.ToInt32(result);
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
                LoadCities(ddlState.SelectedValue);
            else
            {
                ddlCity.Items.Clear();
                ddlCity.Items.Insert(0, new ListItem("-- Select City --", ""));
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate all fields
            if (txtTitle.Text.Trim() == "" || txtLocation.Text.Trim() == "" ||
                txtPrice.Text.Trim() == "" || txtDescription.Text.Trim() == "" ||
                ddlStatus.SelectedValue == "" || ddlState.SelectedValue == "" ||
                ddlCity.SelectedValue == "")
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "All fields are required.";
                return;
            }

            // Get agent ID — only declared ONCE
            int agentId = GetAgentId();

            if (agentId == 0)
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Session expired. Please login again.";
                return;
            }

            // Validate price
            decimal price;
            if (!decimal.TryParse(txtPrice.Text.Trim(), out price))
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Please enter a valid numeric price.";
                return;
            }

            // Validate image
            HttpFileCollection uploadedFiles = Request.Files;
            if (uploadedFiles.Count == 0 || uploadedFiles[0].ContentLength == 0)
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Please select at least one image.";
                return;
            }

            SqlConnection con = new SqlConnection(strcon);
            con.Open();

            string propQuery = @"INSERT INTO Properties 
                                (Title, Location, Price, Status, IsApproved, AgentId) 
                                VALUES (@title, @location, @price, @status, 'Pending', @agentId);
                                SELECT SCOPE_IDENTITY();";

            SqlCommand propCmd = new SqlCommand(propQuery, con);
            propCmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
            propCmd.Parameters.AddWithValue("@location", txtLocation.Text.Trim());

            SqlParameter priceParam = new SqlParameter("@price", SqlDbType.Decimal);
            priceParam.Precision = 10;
            priceParam.Scale = 2;
            priceParam.Value = price;
            propCmd.Parameters.Add(priceParam);

            propCmd.Parameters.AddWithValue("@status", ddlStatus.SelectedValue);
            propCmd.Parameters.AddWithValue("@agentId", agentId);

            int propertyId = Convert.ToInt32(propCmd.ExecuteScalar());

            // Save images
            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                HttpPostedFile file = uploadedFiles[i];
                if (file.ContentLength == 0) continue;

                string extension = Path.GetExtension(file.FileName).ToLower();
                if (extension != ".jpg" && extension != ".jpeg" &&
                    extension != ".png" && extension != ".webp") continue;

                string fileName = Guid.NewGuid().ToString() + extension;
                string savePath = Server.MapPath("~/PropertyImages/") + fileName;
                file.SaveAs(savePath);

                string imgQuery = "INSERT INTO PropertyImages (PropertyId, ImagePath) VALUES (@propertyId, @imagePath)";
                SqlCommand imgCmd = new SqlCommand(imgQuery, con);
                imgCmd.Parameters.AddWithValue("@propertyId", propertyId);
                imgCmd.Parameters.AddWithValue("@imagePath", "PropertyImages/" + fileName);
                imgCmd.ExecuteNonQuery();
            }

            con.Close();
            Response.Redirect("/Agent/MyProperties.aspx");
        }
    }
}