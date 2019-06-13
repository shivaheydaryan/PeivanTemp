using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//--
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Owin;
using System.Web;

namespace API.Login
{
    [App_Start.LogApi(false)]
    public class LoginController : ApiController
    {
        //[App_Start.Token]
        [App_Start.LogApi(true)]
        [HttpPost]
        [Route("api/userlogin")]
        public IHttpActionResult UserLogin(string Username ,string Password )
        {
            

            if (Username != null && Password!=null)
            {
                //=========================================================
                //-------------------- Get User Institute ------
                ViewModel.API.Login.LoginAPIResult CurrentLoginAPIResult = new ViewModel.API.Login.LoginAPIResult();
                #region Get User Institute

                string ConnectionString;
                App_Start.ConnectionString constr = new App_Start.ConnectionString();
                ConnectionString = constr.GetConnectionString();

                SqlConnection conn = new SqlConnection(ConnectionString);
                SqlDataReader rdr = null;
                SqlDataReader rdrClass = null;
                SqlDataReader rdrAccess = null;



                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    SqlCommand cmd = new SqlCommand(@"SP_API_USER_Institutes", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar));
                    cmd.Parameters["@Username"].Value = Username;

                    cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar));
                    cmd.Parameters["@Password"].Value = Password;

                    rdr = cmd.ExecuteReader();

                    DataTable dataTable = new DataTable();
                    DataTable dataTableClass = new DataTable();
                    DataTable dataTableAccess = new DataTable();

                    dataTable.Load(rdr);

                    if (dataTable != null)
                    {
                        if ((dataTable.Rows.Count > 0))
                        {
                            DataRow dr = dataTable.Rows[0];

                            #region Get User Information

                            CurrentLoginAPIResult.Username = Username;
                            CurrentLoginAPIResult.Password = Password;
                            CurrentLoginAPIResult.Firstname = dr["firstname"].ToString();
                            CurrentLoginAPIResult.Lastname = dr["lastname"].ToString();
                            CurrentLoginAPIResult.token = NewTokenCode(Username, Password);


                            #endregion

                            //============================================================
                            //----------------- Get User Class Course -----
                            List<ViewModel.API.Login.classInfo> CurrentClassInfo = new List<ViewModel.API.Login.classInfo>();
                            #region Get User Class Course


                            try
                            {
                                cmd = new SqlCommand(@"SP_API_CLASS_COURSE_USER", conn);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar));
                                cmd.Parameters["@Username"].Value = Username;
                                rdrClass = cmd.ExecuteReader();
                                dataTableClass.Load(rdrClass);

                                if (dataTableClass != null)
                                {
                                    if (dataTableClass.Rows.Count > 0)
                                    {
                                        foreach (DataRow itemclass in dataTableClass.Rows)
                                        {
                                            ViewModel.API.Login.classInfo myClassInfo = new ViewModel.API.Login.classInfo();

                                            myClassInfo.id = int.Parse(itemclass["Calssid"].ToString());
                                            myClassInfo.Instituteid = int.Parse(itemclass["Instituteid"].ToString());
                                            myClassInfo.name = itemclass["Classname"].ToString();
                                            myClassInfo.courseTitle = itemclass["Coursename"].ToString();
                                            myClassInfo.studentsNumber = itemclass["StudentNumber"].ToString() != "" ? int.Parse(itemclass["StudentNumber"].ToString()) : 0;


                                            CurrentClassInfo.Add(myClassInfo);
                                        }

                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                                var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Internal Server Error!!!" };
                                throw new HttpResponseException(msg);
                            }



                            #endregion
                            //----------------- Get User Class Course -----
                            //============================================================

                            //============================================================
                            //----------------- Get User Access -----
                            List<ViewModel.API.Login.AccessLevels> CurrentAccessLevels = new List<ViewModel.API.Login.AccessLevels>();
                            #region Get User Access

                            try
                            {
                                cmd = new SqlCommand(@"SP_API_USER_ACCESS", conn);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar));
                                cmd.Parameters["@Username"].Value = Username;
                                rdrAccess = cmd.ExecuteReader();
                                dataTableAccess.Load(rdrAccess);

                                if (dataTableAccess != null)
                                {
                                    if (dataTableAccess.Rows.Count > 0)
                                    {
                                        foreach (DataRow itemaccess in dataTableAccess.Rows)
                                        {
                                            ViewModel.API.Login.AccessLevels myAccess = new ViewModel.API.Login.AccessLevels();

                                            myAccess.Accessid = int.Parse(itemaccess["Accessid"].ToString());
                                            myAccess.caption = itemaccess["caption"].ToString();
                                            myAccess.Instituteid = int.Parse(itemaccess["Instituteid"].ToString());
                                            
                                            CurrentAccessLevels.Add(myAccess);
                                        }

                                    }

                                }
                            }
                            catch(Exception ex)
                            {
                                var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Internal Server Error!!!" };
                                throw new HttpResponseException(msg);
                            }

                            #endregion
                            //----------------- Get User Access -----
                            //============================================================

                            CurrentLoginAPIResult.jobs = new List<ViewModel.API.Login.job>();
                            #region Get User Jobs
                            foreach (DataRow item in dataTable.Rows)
                            {
                                ViewModel.API.Login.job Currentjob = new ViewModel.API.Login.job();
                                #region Get Job Name
                                byte Manager_Teacher_Student_Parent = byte.Parse(item["Manager_Teacher_Student_Parent"].ToString());
                                if (Manager_Teacher_Student_Parent == 0)
                                {
                                    Currentjob.name = "Manager";
                                }
                                else if (Manager_Teacher_Student_Parent == 1)
                                {
                                    Currentjob.name = "Teacher";
                                }
                                else if (Manager_Teacher_Student_Parent == 2)
                                {
                                    Currentjob.name = "Student";
                                }
                                else if (Manager_Teacher_Student_Parent == 3)
                                {
                                    Currentjob.name = "Parent";
                                }
                                else if (Manager_Teacher_Student_Parent == 4)
                                {
                                    Currentjob.name = "Employee";
                                }
                                #endregion


                                Currentjob.schools = new List<ViewModel.API.Login.School>();


                                ViewModel.API.Login.School CurrentSchool = new ViewModel.API.Login.School();
                                #region Get School Info
                                CurrentSchool.id = int.Parse(item["Instituteid"].ToString());

                                CurrentSchool.name = item["name"].ToString();
                                #endregion

                                CurrentSchool.classes = new List<ViewModel.API.Login.classInfo>();



                                #region Set ClassInfo School
                                List<ViewModel.API.Login.classInfo> CurrentClassInfo_CurrentInstitute = CurrentClassInfo.Where(p => p.Instituteid == CurrentSchool.id).ToList();

                                foreach (var itemcalssinfo in CurrentClassInfo_CurrentInstitute)
                                {
                                    CurrentSchool.classes.Add(itemcalssinfo);
                                }
                                #endregion

                                Currentjob.schools.Add(CurrentSchool);


                                Currentjob.accesseLevels = new List<ViewModel.API.Login.AccessLevels>();

                                List<ViewModel.API.Login.AccessLevels> CurrentAccess = CurrentAccessLevels.Where(p => p.Instituteid == CurrentSchool.id).ToList();
                                #region Set AccessLevel Job
                                foreach (var itemaccess in CurrentAccess)
                                {
                                    Currentjob.accesseLevels.Add(itemaccess);
                                }
                                #endregion

                                


                                CurrentLoginAPIResult.jobs.Add(Currentjob);
                            }

                            #endregion

                        }
                        else
                        {
                            var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Username Or Password Is Not Valid!!!" };
                            throw new HttpResponseException(msg);
                        }
                    }
                    else
                    {
                        var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Username Or Password Is Not Valid!!!" };
                        throw new HttpResponseException(msg);
                    }


                }
                catch (Exception ex)
                {
                    var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Internal Server Error!!!" };
                    throw new HttpResponseException(msg);
                }

                #endregion
                //-------------------- Get User Institute ------
                //=========================================================

                return Ok(CurrentLoginAPIResult);
            }

            else
            {
                var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Username Or Password Is Not Valid!!!" };
                throw new HttpResponseException(msg);
            }





            
        }


        [Route("api/test2")]
        public IHttpActionResult test (){
            return Ok("HI");
            }


        [NonAction]
        public string NewTokenCode(string username,string password)
        {
            
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputbyte = System.Text.Encoding.ASCII.GetBytes(username+password + "305051");
                byte[] hashBytes = md5.ComputeHash(inputbyte);

                //--- Convert the byte array to hexadecimal
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }

        }

    }
}
