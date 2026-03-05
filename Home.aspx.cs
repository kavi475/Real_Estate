using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void btnSignup_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }
    }
}