using DataBaseConnectionMVC.Areas.MST_Branch.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace DataBaseConnectionMVC.Areas.MST_Branch.Controllers
{
    [Area("MST_Branch")]
    [Route("MST_Branch/{Controller}/{Action}")]
    public class MST_BranchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IConfiguration Configuration { get; }
        public MST_BranchController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        //-----------------------------Branch-------------------------------
        // Branch Table List
        #region BranchSelectAll
        public IActionResult BranchList(string? BranchName, string? BranchCode, bool filter = false)
        {
            string conn = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com = new(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (Convert.ToBoolean(filter))
            {
                cmd.CommandText = "PR_BranchFilter"; 
                cmd.Parameters.AddWithValue("@BranchName", BranchName);
                cmd.Parameters.AddWithValue("@BranchCode", BranchCode);
            }
            else
            {
                cmd.CommandText = "PR_MST_Branch_SelectAll";    
            }
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new();
            dt.Load(reader);
            return View("BranchList", dt);
        }
        #endregion
        public IActionResult CancleBranch()
        {
            return RedirectToAction("BranchList");
        }
        // Branch Table Delete By Id
        #region BranchDeletById
        public IActionResult Branch_Delete(int BranchId)
        {
            string conn = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com = new(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_MST_Branch_DeleteByPK";
            cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                TempData["BranchDelete"] = "Branch Deleted Successfully..!";
            }
            com.Close();
            return RedirectToAction("BranchList");
        }
        #endregion

        //Counrty Add Edit
        #region BranchAdd/Edit
        [HttpPost]
        public IActionResult SaveBranch(MST_BranchModel modelMST_Branch)
        {
            string conn = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com = new(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelMST_Branch.BranchId == 0)
            {
                cmd.CommandText = "PR_MST_Branch_Insert";
            }
            else
            {
                cmd.CommandText = "PR_MST_Branch_UpdateByPK";
                cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = modelMST_Branch.BranchId;
            }
            cmd.Parameters.Add("@BranchName", SqlDbType.VarChar).Value = modelMST_Branch.BranchName;
            cmd.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = modelMST_Branch.BranchCode;
            cmd.Parameters.Add("@Created", SqlDbType.Date).Value = modelMST_Branch.Created;
            cmd.Parameters.Add("@Modified", SqlDbType.Date).Value = modelMST_Branch.Modified;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelMST_Branch.BranchId == 0)
                    TempData["BranchInsertMessage"] = "Record Inserted Successfully..!";
                else
                    TempData["BranchInsertMessage"] = "Record Updated Successfully..!";
            }
            com.Close();

            return RedirectToAction("BranchList");
        }
        #endregion
        public IActionResult AddBranch(int? BranchId)
        {
            if (BranchId != null)
            {
                string conn = Configuration.GetConnectionString("myConnectionString");
                SqlConnection com = new(conn);
                com.Open();
                SqlCommand cmd = com.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_MST_Branch_SelectByPK";
                cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
                DataTable dt = new();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                MST_BranchModel modelMST_Branch = new();
                foreach (DataRow dr in dt.Rows)
                {

                    //modelMST_Branch.BranchId = Convert.ToInt32(dr["BranchId"]);
                    modelMST_Branch.BranchName = (string?)dr["BranchName"];
                    modelMST_Branch.BranchCode = (string?)dr["BranchCode"];
                    modelMST_Branch.Created = Convert.ToDateTime(dr["Created"]);
                    modelMST_Branch.Modified = Convert.ToDateTime(dr["Modified"]);
                }
                return View("MST_BranchAddEdit", modelMST_Branch);
            }
            return View("MST_BranchAddEdit");
        }
    }
}
