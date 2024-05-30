using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using DataBaseConnectionMVC.Models;
using DataBaseConnectionMVC.Areas.LOC_State.Models;
using DataBaseConnectionMVC.Areas.LOC_Country.Models;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace DataBaseConnectionMVC.Areas.LOC_State.Controllers
{
    [Area("LOC_State")]
    [Route("LOC_State/{Controller}/{Action}")]
    public class LOC_StateController : Controller
    {
        
        public LOC_StateController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IActionResult Index()
        {
            return View();
        }
        // ------------------------------State--------------------------
        // State List
        #region StateSelectAll
        public IActionResult StateList(string? StateName, string? StateCode, string? CountryId,bool filter = false)
        {
            string conn = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com = new(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (Convert.ToBoolean(filter))
            {
                cmd.CommandText = "PR_StateFilter";
                cmd.Parameters.AddWithValue("@StateName", StateName);
                cmd.Parameters.AddWithValue("@StateCode", StateCode);
                cmd.Parameters.AddWithValue("@CountryId", CountryId);
            }
            else
            {
                cmd.CommandText = "PR_State_SelectAll";
            }
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt2 = new();
            dt2.Load(reader);
            com.Close();
            return View("StateList", dt2);
        }
        public IActionResult CancleState()
        {
            return RedirectToAction("StateList");
        }
        #endregion
        // State Table Delete By Id
        #region State_Delete
        public IActionResult State_Delete(int StateId)
        {
            string conn = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com = new(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_State_DeleteByPK";
            cmd.Parameters.Add("@StateId", SqlDbType.Int).Value = StateId;
            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                TempData["StateDelete"] = "State Deleted Successfully..!";
            }
            com.Close();
            return RedirectToAction("StateList");
        }
        #endregion
        //Counrty Add Edit
        #region StateAdd/Edit
        [HttpPost]
        public IActionResult SaveState(LOC_StateModel modelLOC_State)
        {
            string conn = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com = new(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelLOC_State.StateId == 0)
            {
                cmd.CommandText = "PR_State_Insert";
            }
            else
            {
                cmd.CommandText = "PR_State_UpdateByPK";
                cmd.Parameters.Add("@StateId", SqlDbType.Int).Value = modelLOC_State.StateId;
            }
            cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = modelLOC_State.CountryId;
            cmd.Parameters.Add("@StateName", SqlDbType.VarChar).Value = modelLOC_State.StateName;
            cmd.Parameters.Add("@StateCode", SqlDbType.VarChar).Value = modelLOC_State.StateCode;
            cmd.Parameters.Add("@Created", SqlDbType.Date).Value = modelLOC_State.Created;
            cmd.Parameters.Add("@Modified", SqlDbType.Date).Value = modelLOC_State.Modified;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLOC_State.StateId == 0)
                    TempData["StateInsertMessage"] = "Record Inserted Successfully..!";
                else
                    TempData["StateInsertMessage"] = "Record Updated Successfully..!";
            }
            com.Close();

            return RedirectToAction("StateList");
        }
        #endregion
        public IActionResult AddState(int? StateId)
        {

            string conn1 = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com1 = new(conn1);
            com1.Open();
            SqlCommand cmd1 = com1.CreateCommand();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "PR_LOC_Country_SelectByComboBox";
            DataTable dt1 = new();
            SqlDataReader reader1 = cmd1.ExecuteReader();
            dt1.Load(reader1);
            List<LOC_CountryDropDownModel> list = new();
            foreach (DataRow dr in dt1.Rows)
            {
                LOC_CountryDropDownModel vlst = new();
                vlst.CountryId = Convert.ToInt32(dr["CountryId"]);
                vlst.CountryName = dr["CountryName"].ToString();
                list.Add(vlst);
            }
            ViewBag.CountryList = list;

            if (StateId != null)
            {
                string conn = Configuration.GetConnectionString("myConnectionString");
                SqlConnection com = new(conn);
                com.Open();
                SqlCommand cmd = com.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_State_SelectByPK";
                cmd.Parameters.Add("@StateId", SqlDbType.Int).Value = StateId;
                DataTable dt = new();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                LOC_StateModel modelLOC_State = new();
                foreach (DataRow dr in dt.Rows)
                {

                    modelLOC_State.CountryId = Convert.ToInt32(dr["CountryId"]);
                    modelLOC_State.StateName = (string?)dr["StateName"];
                    modelLOC_State.StateCode = (string?)dr["StateCode"];
                    modelLOC_State.Created = Convert.ToDateTime(dr["Created"]);
                    modelLOC_State.Modified = Convert.ToDateTime(dr["Modified"]);
                }
                return View("LOC_StateAddEdit", modelLOC_State);
            }
            return View("LOC_StateAddEdit");
        }
    }
}
