using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Agent
{
    public partial class Add_property : System.Web.UI.Page
    {
        private readonly string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadStates();

                // Check for edit/resubmission mode
                if (Request.QueryString["propertyId"] != null)
                {
                    if (int.TryParse(Request.QueryString["propertyId"], out int propertyId))
                    {
                        int agentId = GetAgentId();
                        LoadPropertyForEdit(propertyId, agentId);
                    }
                }
            }
        }

        private int GetAgentId()
        {
            if (Session["AgentId"] != null)
            {
                return Convert.ToInt32(Session["AgentId"]);
            }

            using (SqlConnection con = new SqlConnection(strcon))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT UserId FROM Users WHERE Email=@email", con);
                cmd.Parameters.AddWithValue("@email", Session["email"].ToString());
                con.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    int id = Convert.ToInt32(result);
                    Session["AgentId"] = id;
                    return id;
                }
            }
            Response.Redirect("~/Login.aspx");
            return 0;
        }

        private void LoadStates()
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT StateId, StateName FROM States ORDER BY StateName", con);
                con.Open();
                ddlState.DataSource = cmd.ExecuteReader();
                ddlState.DataTextField = "StateName";
                ddlState.DataValueField = "StateId";
                ddlState.DataBind();
            }
            ddlState.Items.Insert(0, new ListItem("-- Select State --", ""));

            ddlCity.Items.Clear();
            ddlCity.Items.Insert(0, new ListItem("-- Select City --", ""));

            ddlLocality.Items.Clear();
            ddlLocality.Items.Insert(0, new ListItem("-- Select Locality --", ""));
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCity.Items.Clear();
            ddlLocality.Items.Clear();
            ddlLocality.Items.Insert(0, new ListItem("-- Select Locality --", ""));

            if (string.IsNullOrEmpty(ddlState.SelectedValue))
            {
                ddlCity.Items.Insert(0, new ListItem("-- Select City --", ""));
                return;
            }

            LoadCities(Convert.ToInt32(ddlState.SelectedValue));
        }

        private void LoadCities(int stateId)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT CityId, CityName FROM Cities WHERE StateId=@id ORDER BY CityName", con);
                cmd.Parameters.AddWithValue("@id", stateId);
                con.Open();
                ddlCity.DataSource = cmd.ExecuteReader();
                ddlCity.DataTextField = "CityName";
                ddlCity.DataValueField = "CityId";
                ddlCity.DataBind();
            }
            ddlCity.Items.Insert(0, new ListItem("-- Select City --", ""));
        }

        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlLocality.Items.Clear();

            if (string.IsNullOrEmpty(ddlCity.SelectedValue))
            {
                ddlLocality.Items.Insert(0, new ListItem("-- Select Locality --", ""));
                return;
            }

            LoadLocalities(Convert.ToInt32(ddlCity.SelectedValue));
        }

        private void LoadLocalities(int cityId)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(
                        "SELECT LocalityId, LocalityName FROM Localities WHERE CityId=@id ORDER BY LocalityName", con);
                    cmd.Parameters.AddWithValue("@id", cityId);
                    con.Open();
                    ddlLocality.DataSource = cmd.ExecuteReader();
                    ddlLocality.DataTextField = "LocalityName";
                    ddlLocality.DataValueField = "LocalityId";
                    ddlLocality.DataBind();
                }
                catch (SqlException)
                {
                    // Fallback in case Localities table is missing from schema
                }
            }
            ddlLocality.Items.Insert(0, new ListItem("-- Select Locality --", ""));
        }

        private void LoadPropertyForEdit(int propertyId, int agentId)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                // Verify owner to ensure proper Data Isolation
                string query = @"
                    SELECT p.*, c.StateId 
                    FROM Properties p
                    LEFT JOIN Cities c ON p.CityId = c.CityId
                    WHERE p.PropertyId = @propertyId AND p.AgentId = @agentId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@propertyId", propertyId);
                    cmd.Parameters.AddWithValue("@agentId", agentId);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            litPageHeader.Text = "Edit & Resubmit Property";
                            btnAdd.Text = "Resubmit Property";

                            txtTitle.Text = reader["Title"].ToString();
                            txtLocation.Text = reader["Location"] != DBNull.Value ? reader["Location"].ToString() : "";
                            txtPrice.Text = reader["Price"].ToString();
                            txtDescription.Text = reader["Description"].ToString();
                            txtBHK.Text = reader["BHK"].ToString();
                            txtArea.Text = reader["Area"].ToString();
                            chkFeatured.Checked = Convert.ToBoolean(reader["IsFeatured"]);

                            // Dropdowns
                            SelectItem(ddlFurnishing, reader["Furnishing"].ToString());
                            SelectItem(ddlPropertyType, reader["PropertyType"].ToString());
                            SelectItem(ddlStatus, reader["ListingStatus"] != DBNull.Value ? reader["ListingStatus"].ToString() : "Available");

                            // Cascade values
                            if (reader["StateId"] != DBNull.Value)
                            {
                                int stateId = Convert.ToInt32(reader["StateId"]);
                                SelectItem(ddlState, stateId.ToString());
                                LoadCities(stateId);
                            }

                            if (reader["CityId"] != DBNull.Value)
                            {
                                int cityId = Convert.ToInt32(reader["CityId"]);
                                SelectItem(ddlCity, cityId.ToString());
                                LoadLocalities(cityId);
                            }

                            // Phase 2 Inputs
                            txtVideoUrl.Text = reader["VideoUrl"] != DBNull.Value ? reader["VideoUrl"].ToString() : "";
                            txtLatitude.Text = reader["Latitude"] != DBNull.Value ? reader["Latitude"].ToString() : "";
                            txtLongitude.Text = reader["Longitude"] != DBNull.Value ? reader["Longitude"].ToString() : "";
                            txtMapLink.Text = reader["MapLink"] != DBNull.Value ? reader["MapLink"].ToString() : "";

                            if (reader["Status"].ToString() == "Rejected")
                            {
                                lblMsg.Text = "Property was rejected. Reason: " + reader["RejectionReason"].ToString();
                                lblMsg.ForeColor = System.Drawing.Color.OrangeRed;
                            }
                        }
                    }
                }
            }
        }

        private void SelectItem(DropDownList ddl, string value)
        {
            ListItem item = ddl.Items.FindByValue(value);
            if (item == null) item = ddl.Items.FindByText(value);
            if (item != null)
            {
                ddl.ClearSelection();
                item.Selected = true;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (Session["email"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            int agentId = GetAgentId();

            // Safe parsing & validation of inputs to avoid format exceptions
            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                lblMsg.Text = "Validation Error: Price is required.";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }
            decimal price = 0;
            if (!decimal.TryParse(txtPrice.Text.Trim(), out price) || price < 0)
            {
                lblMsg.Text = "Validation Error: Please enter a valid numeric value for Price.";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (string.IsNullOrWhiteSpace(txtBHK.Text))
            {
                lblMsg.Text = "Validation Error: BHK is required.";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }
            int bhk = 0;
            if (!int.TryParse(txtBHK.Text.Trim(), out bhk) || bhk < 0)
            {
                lblMsg.Text = "Validation Error: Please enter a valid integer for BHK.";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (string.IsNullOrWhiteSpace(txtArea.Text))
            {
                lblMsg.Text = "Validation Error: Area is required.";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }
            decimal area = 0;
            if (!decimal.TryParse(txtArea.Text.Trim(), out area) || area < 0)
            {
                lblMsg.Text = "Validation Error: Please enter a valid numeric value for Area.";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            object cityId = DBNull.Value;
            if (!string.IsNullOrEmpty(ddlCity.SelectedValue))
            {
                if (int.TryParse(ddlCity.SelectedValue, out int parsedCityId))
                {
                    cityId = parsedCityId;
                }
            }

            // Multi-image upload validations (Max 10 images)
            if (fuImages.HasFiles)
            {
                if (fuImages.PostedFiles.Count > 10)
                {
                    lblMsg.Text = "Error: You can upload a maximum of 10 images.";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                foreach (HttpPostedFile file in fuImages.PostedFiles)
                {
                    if (file.ContentLength > 0)
                    {
                        string ext = Path.GetExtension(file.FileName).ToLower();
                        if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                        {
                            lblMsg.Text = "Error: Only JPG, JPEG, and PNG image files are allowed.";
                            lblMsg.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }
                }
            }

            try
            {
                // Upload folders creation
                string folder = Server.MapPath("~/PropertyImages/");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                // Process files
                string uploadedFiles = "";
                if (fuImages.HasFiles)
                {
                    foreach (HttpPostedFile file in fuImages.PostedFiles)
                    {
                        if (file.ContentLength > 0)
                        {
                            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                            file.SaveAs(Path.Combine(folder, fileName));
                            uploadedFiles += fileName + ",";
                        }
                    }
                }
                uploadedFiles = uploadedFiles.TrimEnd(',');

                int propertyId = 0;
                bool isEdit = Request.QueryString["propertyId"] != null;

                if (isEdit)
                {
                    int.TryParse(Request.QueryString["propertyId"], out propertyId);
                }

                using (SqlConnection con = new SqlConnection(strcon))
                {
                    con.Open();
                    using (SqlTransaction trans = con.BeginTransaction())
                    {
                        try
                        {
                            object lat = DBNull.Value;
                            if (decimal.TryParse(txtLatitude.Text.Trim(), out decimal latitudeVal)) lat = latitudeVal;

                            object lng = DBNull.Value;
                            if (decimal.TryParse(txtLongitude.Text.Trim(), out decimal longitudeVal)) lng = longitudeVal;

                            string videoUrl = txtVideoUrl.Text.Trim();
                            string mapLink = txtMapLink.Text.Trim();

                            if (isEdit)
                            {
                                // Resubmission: update the property details, set Status = 'Pending', and clear RejectionReason
                                string query = @"
                                    UPDATE Properties
                                    SET Title = @Title, Location = @Location, Price = @Price, Description = @Desc,
                                        BHK = @BHK, Area = @Area, Furnishing = @Furnishing, PropertyType = @Type,
                                        CityId = @City, Status = 'Pending', RejectionReason = NULL, IsFeatured = @Featured,
                                        VideoUrl = @VideoUrl, Latitude = @Lat, Longitude = @Lng, MapLink = @MapLink,
                                        ListingStatus = @ListingStatus
                                    WHERE PropertyId = @PropertyId AND AgentId = @AgentId";

                                using (SqlCommand cmd = new SqlCommand(query, con, trans))
                                {
                                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                                    cmd.Parameters.AddWithValue("@Location", txtLocation.Text.Trim());
                                    cmd.Parameters.AddWithValue("@Price", price);
                                    cmd.Parameters.AddWithValue("@Desc", txtDescription.Text.Trim());
                                    cmd.Parameters.AddWithValue("@BHK", bhk);
                                    cmd.Parameters.AddWithValue("@Area", area);
                                    cmd.Parameters.AddWithValue("@Furnishing", ddlFurnishing.SelectedValue);
                                    cmd.Parameters.AddWithValue("@Type", ddlPropertyType.SelectedValue);
                                    cmd.Parameters.AddWithValue("@City", cityId);
                                    cmd.Parameters.AddWithValue("@Featured", chkFeatured.Checked);
                                    cmd.Parameters.AddWithValue("@VideoUrl", string.IsNullOrEmpty(videoUrl) ? (object)DBNull.Value : videoUrl);
                                    cmd.Parameters.AddWithValue("@Lat", lat);
                                    cmd.Parameters.AddWithValue("@Lng", lng);
                                    cmd.Parameters.AddWithValue("@MapLink", string.IsNullOrEmpty(mapLink) ? (object)DBNull.Value : mapLink);
                                    cmd.Parameters.AddWithValue("@ListingStatus", ddlStatus.SelectedValue);
                                    cmd.Parameters.AddWithValue("@PropertyId", propertyId);
                                    cmd.Parameters.AddWithValue("@AgentId", agentId);

                                    int rows = cmd.ExecuteNonQuery();
                                    if (rows == 0)
                                    {
                                        throw new Exception("Property update failed or you do not have permission.");
                                    }
                                }
                            }
                            else
                            {
                                // New Property Creation: sets Status = 'Pending'
                                string query = @"
                                    INSERT INTO Properties
                                    (AgentId, Title, Location, Price, Description, BHK, Area, Furnishing,
                                     PropertyType, CityId, Status, RejectionReason, IsFeatured, CreatedAt,
                                     VideoUrl, Latitude, Longitude, MapLink, ListingStatus)
                                    VALUES
                                    (@AgentId, @Title, @Location, @Price, @Desc, @BHK, @Area, @Furnishing,
                                     @Type, @City, 'Pending', NULL, @Featured, GETDATE(),
                                     @VideoUrl, @Lat, @Lng, @MapLink, @ListingStatus);
                                    SELECT SCOPE_IDENTITY();";

                                using (SqlCommand cmd = new SqlCommand(query, con, trans))
                                {
                                    cmd.Parameters.AddWithValue("@AgentId", agentId);
                                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                                    cmd.Parameters.AddWithValue("@Location", txtLocation.Text.Trim());
                                    cmd.Parameters.AddWithValue("@Price", price);
                                    cmd.Parameters.AddWithValue("@Desc", txtDescription.Text.Trim());
                                    cmd.Parameters.AddWithValue("@BHK", bhk);
                                    cmd.Parameters.AddWithValue("@Area", area);
                                    cmd.Parameters.AddWithValue("@Furnishing", ddlFurnishing.SelectedValue);
                                    cmd.Parameters.AddWithValue("@Type", ddlPropertyType.SelectedValue);
                                    cmd.Parameters.AddWithValue("@City", cityId);
                                    cmd.Parameters.AddWithValue("@Featured", chkFeatured.Checked);
                                    cmd.Parameters.AddWithValue("@VideoUrl", string.IsNullOrEmpty(videoUrl) ? (object)DBNull.Value : videoUrl);
                                    cmd.Parameters.AddWithValue("@Lat", lat);
                                    cmd.Parameters.AddWithValue("@Lng", lng);
                                    cmd.Parameters.AddWithValue("@MapLink", string.IsNullOrEmpty(mapLink) ? (object)DBNull.Value : mapLink);
                                    cmd.Parameters.AddWithValue("@ListingStatus", ddlStatus.SelectedValue);

                                    propertyId = Convert.ToInt32(cmd.ExecuteScalar());
                                }
                            }

                            // Save new images
                            if (!string.IsNullOrEmpty(uploadedFiles))
                            {
                                foreach (string img in uploadedFiles.Split(','))
                                {
                                    if (string.IsNullOrWhiteSpace(img)) continue;

                                    SqlCommand imgCmd = new SqlCommand(
                                        "INSERT INTO PropertyImages (PropertyId, ImagePath) VALUES (@id, @img)", con, trans);
                                    imgCmd.Parameters.AddWithValue("@id", propertyId);
                                    imgCmd.Parameters.AddWithValue("@img", "PropertyImages/" + img);
                                    imgCmd.ExecuteNonQuery();
                                }
                            }

                            trans.Commit();
                            lblMsg.Text = isEdit ? "Property Resubmitted Successfully!" : "Property Added Successfully and Pending Review!";
                            lblMsg.ForeColor = System.Drawing.Color.Green;

                            if (isEdit)
                            {
                                Response.Redirect("MyProperties.aspx");
                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Database Error: " + ex.Message;
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}