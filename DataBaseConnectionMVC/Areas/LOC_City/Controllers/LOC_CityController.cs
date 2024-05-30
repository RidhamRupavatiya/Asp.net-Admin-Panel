using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using DataBaseConnectionMVC.Areas.LOC_City.Models;
using DataBaseConnectionMVC.Areas.LOC_Country.Models;
using DataBaseConnectionMVC.Areas.LOC_State.Models;

namespace DataBaseConnectionMVC.Areas.LOC_City.Controllers
{
    [Area("LOC_City")]
    [Route("LOC_City/{Controller}/{Action}")]
    public class LOC_CityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IConfiguration Configuration { get; }
        public LOC_CityController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //-----------------------------City-------------------------------
        // City Table List
        #region CitySelectAll
        public IActionResult CityList(string? CityName, string?CountryId, string? StateId, string? CityCode, bool filter = false)
        {
            string conn = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com = new(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (Convert.ToBoolean(filter))
            {
                cmd.CommandText = "PR_CityFilter"; 
                cmd.Parameters.AddWithValue("CityName", CityName);
                cmd.Parameters.AddWithValue("CityCode", CityCode);
                cmd.Parameters.AddWithValue("CountryId", CountryId);
                cmd.Parameters.AddWithValue("StateId", StateId);
            }
            else
            {
                cmd.CommandText = "PR_City_SelectAll";
            }
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new();
            dt.Load(reader);
            return View("CityList", dt);
        }
        #endregion
        public IActionResult CancleCity()
        {
            return RedirectToAction("CityList");
        }

        // City Table Delete By Id
        #region CityDeletById
        public IActionResult City_Delete(int CityId)
        {
            string conn = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com = new(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_City_DeleteByPK";
            cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = CityId;
            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                TempData["CityDelete"] = "City Deleted Successfully..!";
            }
            com.Close();
            return RedirectToAction("CityList");
        }
        #endregion

        //Counrty Add Edit
        #region CityAdd/Edit
        [HttpPost]
        public IActionResult SaveCity(LOC_CityModel modelLOC_City)
        {
            string conn = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com = new(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelLOC_City.CityId == 0)
            {
                cmd.CommandText = "PR_City_Insert";
            }
            else
            {
                cmd.CommandText = "PR_City_UpdateByPK";
                cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = modelLOC_City.CityId;
            }
            cmd.Parameters.Add("@StateId", SqlDbType.Int).Value = modelLOC_City.StateId;
            cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = modelLOC_City.CountryId;
            cmd.Parameters.Add("@CityName", SqlDbType.VarChar).Value = modelLOC_City.CityName;
            cmd.Parameters.Add("@CityCode", SqlDbType.VarChar).Value = modelLOC_City.CityCode;
            cmd.Parameters.Add("@Modified", SqlDbType.Date).Value = modelLOC_City.Modified;
            cmd.Parameters.Add("@Created", SqlDbType.Date).Value = modelLOC_City.Created;



            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLOC_City.CityId == 0)
                    TempData["CityInsertMessage"] = "Record Inserted Successfully..!";
                else
                    TempData["CityInsertMessage"] = "Record Updated Successfully..!";
            }
            com.Close();

            return RedirectToAction("CityList");
        }
        #endregion
        public IActionResult AddCity(int? CityId)
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

            List<LOC_StateDropDownModel> list1 = new List<LOC_StateDropDownModel>();
            ViewBag.StateList = list1;

            if (CityId != null)
            {
                string conn = Configuration.GetConnectionString("myConnectionString");
                SqlConnection com = new(conn);
                com.Open();
                SqlCommand cmd = com.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_City_SelectByPK";
                cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = CityId;
                DataTable dt = new();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                LOC_CityModel modelLOC_City = new();
                foreach (DataRow dr in dt.Rows)
                {
                    modelLOC_City.StateId = Convert.ToInt32(dr["StateId"]);
                    modelLOC_City.CityId = Convert.ToInt32(dr["CityId"]);
                    modelLOC_City.CountryId = Convert.ToInt32(dr["CountryId"]);
                    modelLOC_City.CityName = (string?)dr["CityName"];
                    modelLOC_City.CityCode = (string?)dr["CityCode"];
                    modelLOC_City.Created = Convert.ToDateTime(dr["Created"]);
                    modelLOC_City.Modified = Convert.ToDateTime(dr["Modified"]);
                }
                return View("LOC_CityAddEdit", modelLOC_City);
            }
            return View("LOC_CityAddEdit");
        }
        [HttpPost]
        public IActionResult DropDownByCountry(int CountryId)
        {
            string conn = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com = new(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_State_SelectComboBoxByCountryId";
            cmd.Parameters.AddWithValue("@CountryId", CountryId);
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new();
            dt.Load(reader);
            com.Close();

            List<LOC_StateDropDownModel> list = new List<LOC_StateDropDownModel>();
            foreach (DataRow dr in dt.Rows)
            {
                LOC_StateDropDownModel vlst = new LOC_StateDropDownModel();
                vlst.StateId = Convert.ToInt32(dr["StateId"]);
                vlst.StateName = dr["StateName"].ToString();
                list.Add(vlst);
            }
            var vModel = list;
            return Json(vModel);
        }
    }
}
