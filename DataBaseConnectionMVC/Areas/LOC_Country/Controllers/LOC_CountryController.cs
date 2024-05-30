using DataBaseConnectionMVC.Areas.LOC_Country.Models;
using DataBaseConnectionMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataBaseConnectionMVC.Areas.LOC_Country.Controllers
{
    [Area("LOC_Country")]
    [Route("LOC_Country/{Controller}/{Action}")]
    public class LOC_CountryController : Controller
    {
        public IConfiguration Configuration { get; }
        public LOC_CountryController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        //-----------------------------Country-------------------------------
        // Country Table List
        #region CountrySelectAll
        public IActionResult CountryList()
        {
            string conn = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com = new(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Country_SelectAll";
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new();
            dt.Load(reader);
            return View("CountryList", dt);
        }
        #endregion

        public IActionResult CountryFilter(string? CountryName, string? CountryCode, bool filter = false)
        {
            string conn = Configuration.GetConnectionString("myConnectionString");
            DataTable dtable = new DataTable();
            SqlConnection com = new SqlConnection(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (Convert.ToBoolean(filter))
            {
                cmd.CommandText = "PR_CountryFilter"; 
                cmd.Parameters.AddWithValue("@CountryName", CountryName);
                cmd.Parameters.AddWithValue("@CountryCode", CountryCode);
            }
            else
            {
                cmd.CommandText = "PR_Country_SelectAll";
            }
            SqlDataReader objStr = cmd.ExecuteReader();
            dtable.Load(objStr);
            return View("CountryList", dtable);
        }
        public IActionResult CancleCountry()
        {
            return RedirectToAction("CountryList");
        }

        // Country Table Delete By Id
        #region CountryDeletById
        public IActionResult Country_Delete(int CountryId)
        {
            string conn = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com = new(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Country_DeleteByPK";
            cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = CountryId;
            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                TempData["CountryDelete"] = "Country Deleted Sucessfully..!";
            }
            com.Close();
            return RedirectToAction("CountryList");
        }
        #endregion

        //Counrty Add Edit
        #region CountryAdd/Edit
        [HttpPost]
        public IActionResult SaveCountry(LOC_CountryModel modelLOC_Country)
        {
            string conn = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com = new(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelLOC_Country.CountryId == 0)
            {
                cmd.CommandText = "PR_Country_Insert";
            }
            else
            {
                cmd.CommandText = "PR_Country_UpdateByPK";
                cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = modelLOC_Country.CountryId;
            }
            cmd.Parameters.Add("@CountryName", SqlDbType.VarChar).Value = modelLOC_Country.CountryName;
            cmd.Parameters.Add("@CountryCode", SqlDbType.VarChar).Value = modelLOC_Country.CountryCode;
            cmd.Parameters.Add("@Created", SqlDbType.Date).Value = modelLOC_Country.Created;
            cmd.Parameters.Add("@Modified", SqlDbType.Date).Value = modelLOC_Country.Modified;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLOC_Country.CountryId == 0)
                    TempData["CountryInsertMessage"] = "Record Inserted Successfully..!";
                else
                    TempData["CountryInsertMessage"] = "Record Updated Successfully..!";
            }
            com.Close();

            return RedirectToAction("CountryList");
        }
        #endregion
        public IActionResult AddCountry(int? CountryId)
        {
            if (CountryId != null)
            {
                string conn = Configuration.GetConnectionString("myConnectionString");
                SqlConnection com = new(conn);
                com.Open();
                SqlCommand cmd = com.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Country_SelectByPK";
                cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = CountryId;
                DataTable dt = new();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                LOC_CountryModel modelLOC_Country = new();
                foreach (DataRow dr in dt.Rows)
                {

                    //modelLOC_Country.CountryId = Convert.ToInt32(dr["CountryId"]);
                    modelLOC_Country.CountryName = (string?)dr["CountryName"];
                    modelLOC_Country.CountryCode = (string?)dr["CountryCode"];
                    modelLOC_Country.Created = Convert.ToDateTime(dr["Created"]);
                    modelLOC_Country.Modified = Convert.ToDateTime(dr["Modified"]);
                }
                return View("LOC_CountryAddEdit", modelLOC_Country);
            }
            return View("LOC_CountryAddEdit");
        }
    }
}