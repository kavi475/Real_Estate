using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace WebApplication1
{
    public partial class adminDashboard : System.Web.UI.Page
    {
        // Safe connection string configuration lookup
        private string GetConnectionString()
        {
            var connSetting = ConfigurationManager.ConnectionStrings["RealEstateDB"];
            return connSetting != null ? connSetting.ConnectionString : string.Empty;
        }

        public string BookingLabels = "";
        public string BookingData = "";

        public string RevenueLabels = "";
        public string RevenueData = "";

        public string CityLabels = "";
        public string CityData = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Prevent browser caching so the page always displays fresh database data
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            if (Session["email"] == null || Session["role"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            // Supports both "Admin" string or "1" (Admin RoleId)
            string role = Session["role"].ToString();
            if (role != "Admin" && role != "1")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                Total_Property();
                Total_State();
                Total_city();
                Total_Users();
                Total_Agents();
                Total_ListedProperty();

                LoadRevenue();

                // Get current logged-in User ID
                int currentUserId = 0;
                if (Session["UserId"] != null)
                {
                    int.TryParse(Session["UserId"].ToString(), out currentUserId);
                }
                if (currentUserId <= 0 && Session["email"] != null)
                {
                    currentUserId = GetUserIdByEmail(Session["email"].ToString());
                }

                // Log automatic user visit
                LogActivity("Admin viewed dashboard", currentUserId);

                // Load logs after registering the activity
                LoadActivity();

                LoadBookingChart();
                LoadRevenueChart();
                LoadCityChart();
            }
        }

        // ─── SAFE DB HELPER ───────────────────────────────────────────
        private decimal SafeScalar(string sql)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                object result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value) return 0;
                decimal val;
                return decimal.TryParse(result.ToString(), out val) ? val : 0;
            }
        }

        // ─── USER LOOKUP HELPER ───────────────────────────────────────
        private int GetUserIdByEmail(string email)
        {
            try
            {
                string connectionString = GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return 0;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT TOP 1 UserId FROM Users WHERE Email = @Email", con);
                    cmd.Parameters.AddWithValue("@Email", email);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        int id;
                        if (int.TryParse(result.ToString(), out id))
                        {
                            return id;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in GetUserIdByEmail: " + ex.Message);
            }
            return 0;
        }

        // ─── LOG ACTIVITY HELPER ──────────────────────────────────────
        public static void LogActivity(string action, int userId = 0)
        {
            try
            {
                var connSetting = ConfigurationManager.ConnectionStrings["RealEstateDB"];
                string connectionString = connSetting != null ? connSetting.ConnectionString : string.Empty;
                if (string.IsNullOrEmpty(connectionString)) return;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // This dynamic T-SQL block detects the columns, check foreign key constraints, 
                    // handles missing/non-identity primary keys, and inserts log safely.
                    string sql = @"
                        DECLARE @ActionText NVARCHAR(500) = @Action;
                        DECLARE @InputUserId INT = @UserId;
                        DECLARE @FinalUserId INT = NULL;

                        -- 1. Verify and get valid UserId to prevent Foreign Key validation failure
                        IF @InputUserId > 0 AND EXISTS (SELECT 1 FROM Users WHERE UserId = @InputUserId)
                        BEGIN
                            SET @FinalUserId = @InputUserId;
                        END
                        ELSE
                        BEGIN
                            -- Find a valid fallback user ID (preferring Admin)
                            SELECT TOP 1 @FinalUserId = UserId 
                            FROM Users 
                            ORDER BY CASE WHEN Email = 'admin@site.com' OR Role = 'Admin' OR RoleId = 1 THEN 0 ELSE 1 END, UserId ASC;
                        END

                        -- 2. Build and execute dynamic INSERT to match whatever schema exists
                        DECLARE @InsertSql NVARCHAR(MAX);
                        DECLARE @Columns NVARCHAR(MAX) = '';
                        DECLARE @Values NVARCHAR(MAX) = '';

                        -- Handle Primary Key if it's not identity (LogId or Id)
                        IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'LogId' AND Object_ID = Object_ID('ActivityLogs') AND is_identity = 0)
                        BEGIN
                            DECLARE @NextLogId INT;
                            SELECT @NextLogId = ISNULL(MAX(LogId), 0) + 1 FROM ActivityLogs;
                            SET @Columns = @Columns + 'LogId, ';
                            SET @Values = @Values + CAST(@NextLogId AS VARCHAR) + ', ';
                        END
                        ELSE IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'Id' AND Object_ID = Object_ID('ActivityLogs') AND is_identity = 0)
                        BEGIN
                            DECLARE @NextId INT;
                            SELECT @NextId = ISNULL(MAX(Id), 0) + 1 FROM ActivityLogs;
                            SET @Columns = @Columns + 'Id, ';
                            SET @Values = @Values + CAST(@NextId AS VARCHAR) + ', ';
                        END

                        -- Handle UserId column if it exists in ActivityLogs
                        IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'UserId' AND Object_ID = Object_ID('ActivityLogs'))
                        BEGIN
                            IF @FinalUserId IS NOT NULL OR NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'UserId' AND Object_ID = Object_ID('ActivityLogs') AND is_nullable = 0)
                            BEGIN
                                SET @Columns = @Columns + 'UserId, ';
                                SET @Values = @Values + '@UserIdParam, ';
                            END
                        END

                        -- Handle Action column
                        SET @Columns = @Columns + 'Action, ';
                        SET @Values = @Values + '@ActionParam, ';

                        -- Handle CreatedAt column
                        SET @Columns = @Columns + 'CreatedAt';
                        SET @Values = @Values + 'GETDATE()';

                        SET @InsertSql = 'INSERT INTO ActivityLogs (' + @Columns + ') VALUES (' + @Values + ')';

                        -- Execute dynamic script safely
                        EXEC sp_executesql @InsertSql, 
                                           N'@UserIdParam INT, @ActionParam NVARCHAR(500)', 
                                           @UserIdParam = @FinalUserId, 
                                           @ActionParam = @ActionText;";

                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@Action", string.IsNullOrEmpty(action) ? "Unknown Action" : action);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LogActivity completely failed: " + ex.Message);
            }
        }

        // ─── COUNTER CARDS ────────────────────────────────────────────

        public void Total_Property()
        {
            try { lblProperty.Text = SafeScalar("SELECT ISNULL(COUNT(*), 0) FROM Properties").ToString("N0"); }
            catch { lblProperty.Text = "0"; }
        }

        public void Total_State()
        {
            try { lblState.Text = SafeScalar("SELECT ISNULL(COUNT(*), 0) FROM States").ToString("N0"); }
            catch { lblState.Text = "0"; }
        }

        public void Total_city()
        {
            try { lblCity.Text = SafeScalar("SELECT ISNULL(COUNT(*), 0) FROM Cities").ToString("N0"); }
            catch { lblCity.Text = "0"; }
        }

        public void Total_Users()
        {
            try { lblUsers.Text = SafeScalar("SELECT ISNULL(COUNT(*), 0) FROM Users").ToString("N0"); }
            catch { lblUsers.Text = "0"; }
        }

        public void Total_Agents()
        {
            // Fallback strategy: Supports string column "Role" or numeric column "RoleId"
            try
            {
                lblAgents.Text = SafeScalar("SELECT ISNULL(COUNT(*), 0) FROM Users WHERE Role = 'Agent'").ToString("N0");
            }
            catch
            {
                try { lblAgents.Text = SafeScalar("SELECT ISNULL(COUNT(*), 0) FROM Users WHERE RoleId = 2").ToString("N0"); }
                catch { lblAgents.Text = "0"; }
            }
        }

        public void Total_ListedProperty()
        {
            try { lblListed.Text = SafeScalar("SELECT ISNULL(COUNT(*), 0) FROM Properties WHERE Status = 'Approved'").ToString("N0"); }
            catch { lblListed.Text = "0"; }
        }

        // ─── REVENUE CARD ─────────────────────────────────────────────
        public void LoadRevenue()
        {
            decimal revenue = 0;
            try
            {
                // Try 1: Joined Payments (Standard schema)
                revenue = SafeScalar(@"
                    SELECT ISNULL(SUM(p.Price), 0)
                    FROM Bookings b
                    INNER JOIN Properties p ON b.PropertyId = p.PropertyId
                    INNER JOIN Payments pay ON b.BookingId = pay.BookingId
                    WHERE pay.PaymentStatus = 'Paid'");
            }
            catch
            {
                try
                {
                    // Try 2: Basic payments sum
                    revenue = SafeScalar("SELECT ISNULL(SUM(Amount), 0) FROM Payments WHERE PaymentStatus = 'Paid'");
                }
                catch
                {
                    try
                    {
                        // Try 3: Original design sum (PaymentStatus directly on Bookings)
                        revenue = SafeScalar(@"
                            SELECT ISNULL(SUM(p.Price), 0)
                            FROM Bookings b
                            INNER JOIN Properties p ON b.PropertyId = p.PropertyId
                            WHERE b.PaymentStatus = 'Paid'");
                    }
                    catch
                    {
                        revenue = 0;
                    }
                }
            }

            lblRevenue.Text = "₹ " + revenue.ToString("N0");
        }

        // ─── SAFE CHART BUILDER ───────────────────────────────────────
        private void BuildChartData(string sql,
                                    string labelCol,
                                    string dataCol,
                                    out string labelsJs,
                                    out string dataJs)
        {
            StringBuilder lblBuf = new StringBuilder();
            StringBuilder dataBuf = new StringBuilder();
            bool first = true;

            string connectionString = GetConnectionString();
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sql, con);
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            object lblObj = dr[labelCol];
                            object valObj = dr[dataCol];

                            string lbl = (lblObj == null || lblObj == DBNull.Value || string.IsNullOrEmpty(lblObj.ToString().Trim())) ? "Unknown" : lblObj.ToString();
                            string val = (valObj == null || valObj == DBNull.Value) ? "0" : valObj.ToString();

                            lbl = lbl.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "").Replace("\n", " ");

                            decimal num;
                            if (!decimal.TryParse(val, out num)) num = 0;

                            if (!first)
                            {
                                lblBuf.Append(",");
                                dataBuf.Append(",");
                            }
                            first = false;

                            lblBuf.Append("'" + lbl + "'");
                            dataBuf.Append(num.ToString(System.Globalization.CultureInfo.InvariantCulture));
                        }
                    }
                }
            }

            labelsJs = lblBuf.Length > 0 ? lblBuf.ToString() : "'No Data'";
            dataJs = dataBuf.Length > 0 ? dataBuf.ToString() : "0";
        }

        // ─── BOOKING CHART ────────────────────────────────────────────
        public void LoadBookingChart()
        {
            string labels = "";
            string data = "";

            try
            {
                // Try 1: Use CreatedDate
                BuildChartData(@"
                    SELECT ISNULL(CONVERT(varchar(7), CreatedDate, 120), 'Unknown') AS Month,
                           COUNT(*) AS Total
                    FROM   Bookings
                    GROUP  BY CONVERT(varchar(7), CreatedDate, 120)
                    ORDER  BY Month",
                    "Month", "Total",
                    out labels, out data);
            }
            catch
            {
                try
                {
                    // Try 2: Fallback to BookingDate
                    BuildChartData(@"
                        SELECT ISNULL(CONVERT(varchar(7), BookingDate, 120), 'Unknown') AS Month,
                               COUNT(*) AS Total
                        FROM   Bookings
                        GROUP  BY CONVERT(varchar(7), BookingDate, 120)
                        ORDER  BY Month",
                        "Month", "Total",
                        out labels, out data);
                }
                catch
                {
                    labels = "'No Data'";
                    data = "0";
                }
            }

            BookingLabels = labels;
            BookingData = data;
        }

        // ─── REVENUE CHART ────────────────────────────────────────────
        public void LoadRevenueChart()
        {
            string labels = "";
            string data = "";

            try
            {
                // Try 1: Join Payments on CreatedDate
                BuildChartData(@"
                    SELECT ISNULL(CONVERT(varchar(7), b.CreatedDate, 120), 'Unknown') AS Month,
                           ISNULL(SUM(p.Price), 0) AS Revenue
                    FROM   Bookings b
                    INNER  JOIN Properties p ON b.PropertyId = p.PropertyId
                    INNER  JOIN Payments pay ON b.BookingId = pay.BookingId
                    WHERE  pay.PaymentStatus = 'Paid'
                    GROUP  BY CONVERT(varchar(7), b.CreatedDate, 120)
                    ORDER  BY Month",
                    "Month", "Revenue",
                    out labels, out data);
            }
            catch
            {
                try
                {
                    // Try 2: Join Payments on BookingDate
                    BuildChartData(@"
                        SELECT ISNULL(CONVERT(varchar(7), b.BookingDate, 120), 'Unknown') AS Month,
                               ISNULL(SUM(p.Price), 0) AS Revenue
                        FROM   Bookings b
                        INNER  JOIN Properties p ON b.PropertyId = p.PropertyId
                        INNER  JOIN Payments pay ON b.BookingId = pay.BookingId
                        WHERE  pay.PaymentStatus = 'Paid'
                        GROUP  BY CONVERT(varchar(7), b.BookingDate, 120)
                        ORDER  BY Month",
                        "Month", "Revenue",
                        out labels, out data);
                }
                catch
                {
                    try
                    {
                        // Try 3: Legacy Bookings schema with BookingDate
                        BuildChartData(@"
                            SELECT ISNULL(CONVERT(varchar(7), BookingDate, 120), 'Unknown') AS Month,
                                   ISNULL(SUM(p.Price), 0) AS Revenue
                            FROM   Bookings b
                            INNER  JOIN Properties p ON b.PropertyId = p.PropertyId
                            WHERE  b.PaymentStatus = 'Paid'
                            GROUP  BY CONVERT(varchar(7), BookingDate, 120)
                            ORDER  BY Month",
                            "Month", "Revenue",
                            out labels, out data);
                    }
                    catch
                    {
                        labels = "'No Data'";
                        data = "0";
                    }
                }
            }

            RevenueLabels = labels;
            RevenueData = data;
        }

        // ─── CITY CHART ───────────────────────────────────────────────
        public void LoadCityChart()
        {
            try
            {
                BuildChartData(@"
                    SELECT TOP 5
                           ISNULL(c.CityName, 'Unknown') AS CityName,
                           COUNT(b.BookingId) AS Total
                    FROM   Bookings b
                    INNER  JOIN Properties p ON b.PropertyId  = p.PropertyId
                    INNER  JOIN Cities     c ON p.CityId      = c.CityId
                    GROUP  BY c.CityName
                    ORDER  BY Total DESC",
                    "CityName", "Total",
                    out CityLabels, out CityData);
            }
            catch
            {
                CityLabels = "'No Data'";
                CityData = "0";
            }
        }

        // ─── ACTIVITY FEED ────────────────────────────────────────────
        public void LoadActivity()
        {
            try
            {
                string connectionString = GetConnectionString();
                if (string.IsNullOrEmpty(connectionString))
                {
                    rptActivity.Visible = false;
                    lblNoActivity.Visible = true;
                    return;
                }

                string query = "";

                // TRY 1: Try sorting by LogId first (ensures timezone-independent order)
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand("SELECT TOP 1 LogId FROM ActivityLogs", con);
                        con.Open();
                        cmd.ExecuteScalar();
                        query = @"
                            SELECT TOP 10
                                   ISNULL(Action, '—')                            AS Action,
                                   ISNULL(CONVERT(varchar, CreatedAt, 120), '—') AS CreatedAt
                            FROM   ActivityLogs
                            ORDER  BY LogId DESC";
                    }
                }
                catch
                {
                    // TRY 2: Try sorting by Id
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            SqlCommand cmd = new SqlCommand("SELECT TOP 1 Id FROM ActivityLogs", con);
                            con.Open();
                            cmd.ExecuteScalar();
                            query = @"
                                SELECT TOP 10
                                       ISNULL(Action, '—')                            AS Action,
                                       ISNULL(CONVERT(varchar, CreatedAt, 120), '—') AS CreatedAt
                                FROM   ActivityLogs
                                ORDER  BY Id DESC";
                        }
                    }
                    catch
                    {
                        // FALLBACK: Sort by CreatedAt
                        query = @"
                            SELECT TOP 10
                                   ISNULL(Action, '—')                            AS Action,
                                   ISNULL(CONVERT(varchar, CreatedAt, 120), '—') AS CreatedAt
                            FROM   ActivityLogs
                            ORDER  BY CreatedAt DESC";
                    }
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        rptActivity.DataSource = dt;
                        rptActivity.DataBind();
                        rptActivity.Visible = true;
                        lblNoActivity.Visible = false;
                    }
                    else
                    {
                        rptActivity.Visible = false;
                        lblNoActivity.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in LoadActivity: " + ex.Message);
                rptActivity.Visible = false;
                lblNoActivity.Visible = true;
            }
        }
    }
}