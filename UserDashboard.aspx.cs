using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class UserDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblWelcome.Text = "Welcome, " + Session["email"].ToString();
                ddlCity.Items.Add("Select City");
                ddlCity.Items.Add("Surat");
                ddlCity.Items.Add("Ahmedabad");
                ddlCity.Items.Add("Mumbai");

                ddlType.Items.Add("Select Property Type");
                ddlType.Items.Add("Residential");
                ddlType.Items.Add("Commercial");
                ddlType.Items.Add("Land");

                ddlStatus.Items.Add("Select Status");
                ddlStatus.Items.Add("Available");
                ddlStatus.Items.Add("Sold");
            }
        }

        protected void btnProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("Profile.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Home.aspx");
        }
    }
}