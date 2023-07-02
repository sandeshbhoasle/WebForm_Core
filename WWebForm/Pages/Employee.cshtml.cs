using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WWebForm.Model;



namespace WWebForm
{
    public class EmployeeModel : PageModel
    {
        public string ConnectionString { get; set; }
        public EmployeeModel(IConfiguration configuration)
        {

            Configuration = configuration;
            ConnectionString = configuration.GetConnectionString("DataBaseContext");
        }
        public IConfiguration Configuration { get; }


        public string txtCustName { get; set; }

        [BindProperty]
        public string txtAccNo { get; set; }


        [BindProperty]
        public string txtBillCycle { get; set; }
       

        [BindProperty]
        public string txtMinAmt { get; set; }


        public List<SelectListItem> DispositionList { get; set; }
        [BindProperty]
        public string SelectedDisposition { get; set; }



        [BindProperty]
        public DateTime txtNRDDate { get; set; }

        [BindProperty]
        public string Remarks { get; set; }
        [BindProperty]
        public string lblReason { get; set; }





        public static DateTime sysdate;
        private static int dispose_code = 0;
        public static DateTime MasterNRDDate;
        private static int NRD = 0;
        private static bool NRD_Status;
        public static string Mycode;
        public static string AgentID;
        public static string ConnID;
        public static string AgentName;
        public static string BatchId;



        public IActionResult OnGet()
        {

            //if (Request.Query["status"] == "1" && Request.Query["mobile"].Count > 0)
            //{
            //    ViewData["Counter"] = "";
            //    ViewData["Status"] = "";
            //    Response.Cookies.Append("pushback", "Yes", new CookieOptions { Expires = DateTimeOffset.Now.AddMinutes(1) });
            //    string script = "updateURL();";
            //    ViewData["Script"] = script;
            //    return Page();
            //}

            //if (Request.Query["Mycode"].Count > 0 && Request.Query["AgentID"] != "" && Request.Query["ConnID"] != "")
            //{
            //    Response.Cookies.Append("pushback", "No", new CookieOptions { Expires = DateTimeOffset.Now.AddMinutes(1) });

            //    try
            //    {
            //        ViewData["Status"] = Request.Query["status"].ToString();
            //    }
            //    catch (Exception ex)
            //    {
            //        ViewData["Status"] = "";
            //    }

            //    try
            //    {
            //        ViewData["Counter"] = Request.Query["Counter"].ToString();
            //    }
            //    catch (Exception ex)
            //    {
            //        ViewData["Counter"] = "";
            //    }

            //    Response.Headers.Add("Cache-Control", "no-cache");
            //    ViewData["NRDDateReadOnly"] = "readonly";

            //    lblReason = "Event Reached: " + Request.Query["Mycode"].ToString();
            //    DataSet ds = new DataSet();
            //    SqlConnection connection = new SqlConnection(ConnectionString);
            //    SqlCommand cmd10 = new SqlCommand("USP_FetchMycode", connection);
            //    cmd10.Parameters.AddWithValue("@Mycode", Request.Query["Mycode"].ToString());
            //    cmd10.Parameters.AddWithValue("@ConnId", Request.Query["ConnID"].ToString());
            //    cmd10.Parameters.AddWithValue("@AgentID", Request.Query["AgentID"].ToString());

            //    cmd10.CommandType = CommandType.StoredProcedure;

            //    SqlDataAdapter da = new SqlDataAdapter(cmd10);
            //    da.Fill(ds);

            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        if (ds.Tables[0].Rows[0][1].ToString() == "")
            //        {
            //            ViewData["Mycode"] = ds.Tables[0].Rows[0][0].ToString();
            //            ViewData["BatchId"] = ds.Tables[0].Rows[0]["BatchId"].ToString();
            //            ViewData["DN"] = ds.Tables[0].Rows[0]["DN"].ToString();
            //           // ShowData(ds.Tables[0].Rows[0][0].ToString());
            //            filldisposition();
            //        }
            //        else
            //        {
            //            lblReason = "Form Submitted";
            //        }
            //    }

            //    ViewData["AgentID"] = Request.Query["AgentID"].ToString();

            //    ViewData["ConnID"] = Request.Query["ConnID"].ToString();
            //}
            //else
            //{
            //  //  ShowData("-1");
            //}
            filldisposition();
            return Page();
        }

        private void filldisposition()
        {

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("USP_GETDISPOSITION", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DispositionList = new List<SelectListItem>();

                    foreach (DataRow row in dt.Rows)
                    {
                        SelectListItem item = new SelectListItem
                        {
                            Text = row["DISP_NAME"].ToString(),
                            Value = row["DispositionCode"].ToString()
                        };

                        DispositionList.Add(item);
                    }

                }
            }
        }


        private void clearfields()
        {
            //gvHistoryData.DataSource = null;
            //   gvHistoryData.DataBind();

            lblReason = "";
            txtCustName = "";
            txtAccNo = "";
            txtMinAmt = "";
            txtBillCycle = "";
            // txtTotalDue.Text = "";
            //txtNRDDate = "";

        }

        private void ShowData(string Mycode)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection connection = new SqlConnection(ConnectionString);
                SqlCommand cmd10 = new SqlCommand("sp_show_masterData", connection);
                cmd10.Parameters.AddWithValue("@MYCODE", Mycode);
                cmd10.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd10);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //  gvHistoryData.DataSource = ds;
                    //   gvHistoryData.DataBind();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtCustName = ds.Tables[0].Rows[0]["Cust_Name"].ToString();
                        txtAccNo = ds.Tables[0].Rows[0]["Account_No"].ToString();
                        txtBillCycle = ds.Tables[0].Rows[0]["Billing_Cycle"].ToString();
                        txtMinAmt = ds.Tables[0].Rows[0]["Total_Amount_Due"].ToString();

                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                //}

            }
        }


        public IActionResult OnPostSelectedDisposition(string selectedValue)
        {

            SelectedDisposition = selectedValue;

            var data = SelectedDisposition.IndexOf(selectedValue);
            if (SelectedDisposition.IndexOf(selectedValue) > 0)
            {
                //fillSubdisposition(ddlDisposition.SelectedItem.Text);

                if (SelectedDisposition == "Call Back Later")
                {
                  //  divCallBack.Attributes.Add("style", "display:block");
                }
                else
                {
                   // divCallBack.Attributes.Add("style", "display:none");
                }
                SqlConnection connection = new SqlConnection(ConnectionString);

                SqlCommand cmd = new SqlCommand("SP_Disposition", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@disp_name", SelectedDisposition);
                cmd.Parameters.AddWithValue("@Operation", "ShowDisptype");
                SqlDataReader dr = cmd.ExecuteReader();
                //if (dr.HasRows == true)
                //{
                //    while (dr.Read())
                //    {

                //        //NRD = Int32.Parse(dr["NRD"].ToString());
                //        //NRD_Status = bool.Parse(dr["NRD_Status"].ToString());

                //        ViewState["NRD"] = Int32.Parse(dr["NRD"].ToString());
                //        ViewState["NRD_Status"] = bool.Parse(dr["NRD_Status"].ToString());ddlDisposition_SelectedIndexChanged
                //    }
                //}
                dr.Close();
                //lblReason.Text = "";
                //GetNRDDate(DateTime.Now.AddDays(Convert.ToInt32(ViewState["NRD"])));

                //if (strcon.State != ConnectionState.Closed)
                //{
                //    strcon.Close();
                //}
            }

            return Page();
        }
        public IActionResult OnPosttxtNRDDate(string txtNRDDate)
        {

            lblReason = "";
        //    GetNRDDate(Convert.ToDateTime(txtNRDDate));

            return Page();
        }



        //private void GetNRDDate(DateTime nrdDate)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(Request.Form["ddlDisposition"]))
        //        {
        //            int dispose_code = Convert.ToInt32(Request.Form["ddlDisposition"]);

        //            TimeSpan k = (nrdDate.Subtract(Convert.ToDateTime(Convert.ToDateTime(ViewData["sysdate"]).ToString("yyyy-MM-dd"))));
        //            if (ViewData["BatchId"].ToString().Contains("SBI_PTP") || ViewData["BatchId"].ToString().Contains("_PTP_"))
        //            {
        //                if (dispose_code == 36 || dispose_code == 25 || dispose_code == 7 || dispose_code == 45 || dispose_code == 31 || dispose_code == 42)
        //                {
        //                    k = (Convert.ToDateTime(ViewData["MasterNRDDate"]).Subtract(Convert.ToDateTime(Convert.ToDateTime(ViewData["sysdate"]).ToString("yyyy-MM-dd"))));
        //                }
        //            }
        //            else
        //            {
        //                k = (nrdDate.Subtract(Convert.ToDateTime(Convert.ToDateTime(ViewData["sysdate"]).ToString("yyyy-MM-dd"))));
        //            }
        //            if (ViewData["BatchId"].ToString().Contains("SBI_PTP") || ViewData["BatchId"].ToString().Contains("_PTP_"))
        //            {
        //                if (dispose_code == 36 || dispose_code == 25 || dispose_code == 7 || dispose_code == 45 || dispose_code == 31 || dispose_code == 42)
        //                {
        //                    txtNRDDate = Convert.ToDateTime(ViewData["MasterNRDDate"]).AddDays(0);
        //                    return;
        //                }
        //                else
        //                {
        //                    txtNRDDate = Convert.ToDateTime(ViewData["sysdate"]).AddDays(Convert.ToInt32(ViewData["NRD"]));
        //                    return;
        //                }
        //            }

        //            if (Convert.ToBoolean(ViewData["NRD_Status"]) == false)
        //            {
        //                k = (nrdDate.Subtract(Convert.ToDateTime(Convert.ToDateTime(ViewData["sysdate"]).ToString("yyyy-MM-dd"))));
        //                if (k.TotalSeconds < 0)
        //                {
        //                    lblReason = "Please Select Proper NRD Date";
        //                    txtNRDDate = Convert.ToDateTime(ViewData["sysdate"]);
        //                    return;
        //                }
        //                else
        //                {
        //                    if (Convert.ToBoolean(ViewData["NRD_Status"]) == false)
        //                    {
        //                        if (Convert.ToInt32(ViewData["NRD"]) < (nrdDate.Day - ViewData["sysdate"].Day))
        //                        {
        //                            lblReason = "NRD Date should not be greater than " + Convert.ToInt32(ViewData["NRD"]) + " Days";
        //                            txtNRDDate = Convert.ToDateTime(ViewData["sysdate"]);
        //                            return;
        //                        }
        //                        else
        //                        {

        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (k.TotalSeconds < 0)
        //                {
        //                    lblReason = "Please Select Proper NRD Date";
        //                    txtNRDDate = Convert.ToDateTime(ViewData["sysdate"]);
        //                    return;
        //                }
        //                else if (Convert.ToInt32(ViewData["NRD"]) < (nrdDate.Day - ViewData["sysdate"].Day))
        //                {
        //                    lblReason = "NRD Date should not be greater than " + Convert.ToInt32(ViewData["NRD"]) + " Days";
        //                    txtNRDDate = Convert.ToDateTime(ViewData["sysdate"]);
        //                    return;
        //                }

        //                if (dispose_code == 25)
        //                {
        //                    txtNRDDate = Convert.ToDateTime(ViewData["sysdate"]).AddDays(0);
        //                    return;
        //                }
        //                else
        //                {
        //                    txtNRDDate = Convert.ToDateTime(ViewData["sysdate"]).AddDays(Convert.ToInt32(ViewData["NRD"]));
        //                    return;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            lblReason = "Select Disposition First";
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblReason = "Error occurred: " + ex.Message;
        //        return;
        //    }
        //}

        public void OnPost()
        {
          
        }


//    }


//    AS
//BEGIN
//    -- Returning three rows with hardcoded values
//    SELECT 'Sandesh' AS DISP_NAME, 'Code 1' AS DispositionCode
//    UNION ALL
//    SELECT 'Bhosale' AS DISP_NAME, 'Code 2' AS DispositionCode
//    UNION ALL
//    SELECT 'C_Dac' AS DISP_NAME, 'Code 3' AS DispositionCode
//END

}
