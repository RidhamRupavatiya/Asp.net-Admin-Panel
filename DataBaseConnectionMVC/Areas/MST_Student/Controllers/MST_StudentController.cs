using DataBaseConnectionMVC.Areas.MST_Student.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using DataBaseConnectionMVC.Areas.LOC_Country.Models;
using DataBaseConnectionMVC.Areas.MST_Branch.Models;
using DataBaseConnectionMVC.Areas.LOC_State.Models;
using DataBaseConnectionMVC.Areas.LOC_City.Models;
using System.Collections.Generic;

namespace DataBaseConnectionMVC.Areas.MST_Student.Controllers
{
    [Area("MST_Student")]
    [Route("MST_Student/{Controller}/{Action}")]
    public class MST_StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IConfiguration Configuration { get; }
        public MST_StudentController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //-----------------------------Student-------------------------------
        // Student Table List
        #region StudentSelectAll
        public IActionResult StudentList(string? StudentName, string? CityId, string? Email, string? BranchName, bool filter = false)
        {
            string conn = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com = new(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (Convert.ToBoolean(filter))
            {
                cmd.CommandText = "PR_StudentFilter";
                cmd.Parameters.AddWithValue("StudentName", StudentName);
                cmd.Parameters.AddWithValue("Email", Email);
                cmd.Parameters.AddWithValue("BranchName", BranchName);
                cmd.Parameters.AddWithValue("CityId", CityId);
            }
            else
            {
                cmd.CommandText = "PR_MST_Students_SelectAll";
            }
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new();
            dt.Load(reader);
            return View("StudentList", dt);
        }
        #endregion
        public IActionResult CancleStudent()
        {
            return RedirectToAction("StudentList");
        }
        // Student Table Delete By Id
        #region StudentDeletById
        public IActionResult Student_Delete(int StudentId)
        {
            string conn = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com = new(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_MST_Students_DeleteByPK";
            cmd.Parameters.Add("@StudentId", SqlDbType.Int).Value = StudentId;
            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                TempData["StudentDelete"] = "Student Deleted Successfully..!";
            }
            com.Close();
            return RedirectToAction("StudentList");
        }
        #endregion

        //Counrty Add Edit
        #region StudentAdd/Edit
        [HttpPost]
        public IActionResult SaveStudent(MST_StudentModel modelMST_Student)
        {
            string conn = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com = new(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelMST_Student.StudentId == 0)
            {
                cmd.CommandText = "PR_MST_Students_Insert";
            }
            else
            {
                cmd.CommandText = "PR_MST_Students_UpdateByPK";
                cmd.Parameters.Add("@StudentId", SqlDbType.Int).Value = modelMST_Student.StudentId;
            }
            cmd.Parameters.Add("@StateId", SqlDbType.Int).Value = modelMST_Student.StateId;
            cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = modelMST_Student.CountryId;
            cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = modelMST_Student.CityId;
            cmd.Parameters.Add("@BranchId", SqlDbType.Int).Value = modelMST_Student.BranchId;
            cmd.Parameters.Add("@StudentName", SqlDbType.VarChar).Value = modelMST_Student.StudentName;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = modelMST_Student.MobileNo;
            cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = modelMST_Student.Email;
            cmd.Parameters.Add("@FatherMobile", SqlDbType.VarChar).Value = modelMST_Student.FatherMobile;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = modelMST_Student.Address;
            cmd.Parameters.Add("@BirthDate", SqlDbType.Date).Value = modelMST_Student.BirthDate;
            cmd.Parameters.Add("@Modified", SqlDbType.Date).Value = modelMST_Student.Modified;
            cmd.Parameters.Add("@Created", SqlDbType.Date).Value = modelMST_Student.Created;



            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelMST_Student.StudentId == 0)
                    TempData["StudentInsertMessage"] = "Record Inserted Successfully..!";
                else
                    TempData["StudentInsertMessage"] = "Record Updated Successfully..!";
            }
            com.Close();

            return RedirectToAction("StudentList");
        }
        #endregion
        public IActionResult AddStudent(int? StudentId)
        {
            //----City DropDown----
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

            string conn2 = Configuration.GetConnectionString("myConnectionString");
            SqlConnection com2 = new(conn1);
            com2.Open();
            SqlCommand cmd2 = com2.CreateCommand();
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.CommandText = "PR_MST_Branch_SelectByComboBox";
            DataTable dt2 = new();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            dt2.Load(reader2);
            List<MST_BranchDropDownModel> list2 = new();
            foreach (DataRow dr in dt2.Rows)
            {
                MST_BranchDropDownModel vlst = new();
                vlst.BranchId = Convert.ToInt32(dr["BranchId"]);
                vlst.BranchName = dr["BranchName"].ToString();
                list2.Add(vlst);
            }
            ViewBag.BranchList = list2;

            List<LOC_StateDropDownModel> loc_State = new List<LOC_StateDropDownModel>();
            ViewBag.StateList = loc_State;

            List<LOC_CityDropDownModel> loc_City = new List<LOC_CityDropDownModel>();
            ViewBag.CityList = loc_City;

            if (StudentId != null)
            {
                string conn = Configuration.GetConnectionString("myConnectionString");
                SqlConnection com = new(conn);
                com.Open();
                SqlCommand cmd = com.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_MST_Students_SelectByPK";
                cmd.Parameters.Add("@StudentId", SqlDbType.Int).Value = StudentId;
                DataTable dt = new();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                MST_StudentModel modelMST_Student = new();
                foreach (DataRow dr in dt.Rows)
                {
                    modelMST_Student.CountryId = Convert.ToInt32(dr["CountryId"]);
                    modelMST_Student.StateId = Convert.ToInt32(dr["StateId"]);
                    modelMST_Student.BranchId = Convert.ToInt32(dr["BranchId"]);
                    modelMST_Student.StudentName = (string?)dr["StudentName"];
                    modelMST_Student.CityId = Convert.ToInt32(dr["CityId"]);
                    modelMST_Student.BranchId = Convert.ToInt32(dr["BranchId"]);
                    modelMST_Student.MobileNo = (string?)dr["MobileNo"];
                    modelMST_Student.Email = (string?)dr["Email"];
                    modelMST_Student.FatherMobile = (string?)dr["FatherMobile"];
                    modelMST_Student.Address = (string?)dr["Address"];
                    modelMST_Student.BirthDate = Convert.ToDateTime(dr["BirthDate"]);
                    modelMST_Student.Created = Convert.ToDateTime(dr["Created"]);
                    modelMST_Student.Modified = Convert.ToDateTime(dr["Modified"]);
                }
                return View("MST_StudentAddEdit", modelMST_Student);
            }
            return View("MST_StudentAddEdit");
        }
        [HttpPost]
        public IActionResult DropDownStateByCountry(int CountryId)
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
        [HttpPost]
        public IActionResult DropDownCityByState(int StateId)
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn1 = new SqlConnection(str);
            conn1.Open();
            SqlCommand objCmd = conn1.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_City_SelectComboBoxByStateId";
            objCmd.Parameters.AddWithValue("@StateId", StateId);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            DataTable dt = new();
            dt.Load(objSDR);
            conn1.Close();
            List<LOC_CityDropDownModel> loc_City = new List<LOC_CityDropDownModel>();
            foreach (DataRow dr in dt.Rows)
            {
                LOC_CityDropDownModel vlst = new LOC_CityDropDownModel();
                vlst.CityId = Convert.ToInt32(dr["CityId"]);
                vlst.CityName = dr["CityName"].ToString();
                loc_City.Add(vlst);
            }
            var vModel = loc_City;
            return Json(vModel);
        }
    }
}
