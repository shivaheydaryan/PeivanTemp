using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

//------
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Peivandyar_Site.API.School
{

    [App_Start.LogApi(false)]
    public class SchoolController : ApiController
    {

        [App_Start.LogApi(true)]
        [HttpGet]
        [Route("api/School/ListSchoolInfo")]
        public IHttpActionResult ListSchoolInfo (){

            List<ViewModel.API.School> SchoolInfoList = new List<ViewModel.API.School>();


            string ConnectionString;
            App_Start.ConnectionString constr = new App_Start.ConnectionString();
            ConnectionString = constr.GetConnectionString();

            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataReader rdr = null;

            //===========================================================================
            //------------------------------- Get Cities -----------
            List<Models.City> Cities = new List<Models.City>();
            #region Get Cities
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                SqlCommand cmd = new SqlCommand(@"select * from Cities where (active =1 or active is null)", conn);


                rdr = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();
                
                dataTable.Load(rdr);

                if (dataTable != null)
                {
                    if ((dataTable.Rows.Count > 0))
                    {
                        Cities = (from DataRow dr in dataTable.Rows
                                      select new Models.City()
                                      {
                                          Code = Convert.ToInt32(dr["Code"].ToString()),
                                          State_Code= Convert.ToInt32(dr["State_Code"].ToString()),
                                          Pname = dr["Pname"].ToString(),
                                          Ename=dr["Ename"].ToString(),
                                          
                                      }
                                  ).ToList();
                    }
                }
            }
            catch(Exception ex)
            {
                if (rdr != null)
                {
                    rdr.Close();
                    rdr = null;
                }
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Internal Server Error!!!" };
                throw new HttpResponseException(msg);
            }
            #endregion
            //------------------------------- Get Cities -----------
            //===========================================================================


            //===========================================================================
            //------------------------------- Get CityZones -----------
            List<Models.CityZone> CityZones = new List<Models.CityZone>();
            #region Get CityZones
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                SqlCommand cmd = new SqlCommand(@"select * from CityZones where (active =1 or active is null)", conn);


                rdr = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();
                

                dataTable.Load(rdr);

                if (dataTable != null)
                {
                    if ((dataTable.Rows.Count > 0))
                    {
                        CityZones = (from DataRow dr in dataTable.Rows
                                  select new Models.CityZone()
                                  {
                                      Code = Convert.ToInt32(dr["Code"].ToString()),
                                      Zone_Code = Convert.ToInt32(dr["Zone_Code"].ToString()),
                                      Pname = dr["Pname"].ToString(),
                                      Ename = dr["Ename"].ToString(),

                                  }
                                  ).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                if (rdr != null)
                {
                    rdr.Close();
                    rdr = null;
                }
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Internal Server Error!!!" };
                throw new HttpResponseException(msg);
            }
            #endregion
            //------------------------------- Get Cities -----------
            //===========================================================================



            //===========================================================================
            //------------------------------- Get School List -----

            #region Get School List



            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                SqlCommand cmd = new SqlCommand(@"select id,name,address,city_code,tel1,En_Name,Description,boyOrGirl,Edit_Date from Institutes
	where (Active =1 or Active is null)", conn);
                
                
                rdr = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();
                

                dataTable.Load(rdr);

                if (dataTable != null)
                {
                    if ((dataTable.Rows.Count > 0))
                    {
                        foreach (DataRow item in dataTable.Rows)
                        {
                            ViewModel.API.School CurrentSchool = new ViewModel.API.School();

                            CurrentSchool.about = item["Description"].ToString();
                            CurrentSchool.address = item["address"].ToString();

                            bool gender= item["boyOrGirl"].ToString() != "" ? bool.Parse(item["boyOrGirl"].ToString()) : true;
                            if (gender==true)
                            {
                                CurrentSchool.gender = "پسرانه";
                            }
                            else
                            {
                                CurrentSchool.gender = "دخترانه";
                            }

                            CurrentSchool.id = int.Parse(item["id"].ToString());
                            CurrentSchool.lastRefreshed = item["Edit_Date"].ToString() != null ? item["Edit_Date"].ToString() : "";
                            //CurrentSchool.liked = "";
                            CurrentSchool.phoneNumber = item["tel1"].ToString();
                            CurrentSchool.schoolName = item["name"].ToString();

                            string url = "";
                            if (item["En_Name"].ToString()!="")
                            {
                                url = "http://egbaliye.forooshgahyas.ir/Content/images/schools/"+CurrentSchool.id+"/"+ item["En_Name"].ToString() + "-main.JPG";
                            }
                            CurrentSchool.imageUrl = url;

                            int Zone_Code=0, Code=0, State_Code=0;

                            #region Analyze CityCode
                            if (item["city_code"].ToString() != "")
                            {
                                string CityCode = item["city_code"].ToString();

                                if (CityCode.Length >= 4)
                                {
                                    string provinceCode = CityCode.Substring(0, 4);

                                    if (provinceCode != "")
                                    {
                                        Code = int.Parse(provinceCode);


                                        if (CityCode.Length >= 8)
                                        {
                                            string townCode = CityCode.Substring(4, 4);

                                            if (townCode != "")
                                            {
                                                State_Code = int.Parse(townCode);


                                                if (CityCode.Length >= 12)
                                                {
                                                    string regionCode = CityCode.Substring(8, 4);
                                                    if (regionCode != "")
                                                    {
                                                        Zone_Code = int.Parse(regionCode);
                                                    }



                                                }


                                            }

                                        }



                                    }

                                }

                            }
                            #endregion


                            Models.City CurrentCity = Cities.Where(p => p.Code == Code && p.State_Code == Code).FirstOrDefault();

                            if (CurrentCity!=null)
                            {
                                CurrentSchool.province = CurrentCity.Pname;
                            }

                            CurrentCity = Cities.Where(p => p.Code == State_Code && p.State_Code == Code).FirstOrDefault();

                            if (CurrentCity != null)
                            {
                                CurrentSchool.town = CurrentCity.Pname;
                            }

                            Models.CityZone myzone = CityZones.Where(p => p.Code == State_Code && p.Zone_Code == Zone_Code).FirstOrDefault();
                            if (myzone!=null)
                            {
                                CurrentSchool.region = myzone.Zone_Code;
                            }


                            SchoolInfoList.Add(CurrentSchool);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                if (rdr != null)
                {
                    rdr.Close();
                    rdr = null;
                }
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Internal Server Error!!!" };
                throw new HttpResponseException(msg);
            }



            #endregion
            //------------------------------- Get School List -----
            //===========================================================================
            if (rdr!=null)
            {
                rdr.Close();
                rdr = null;
            }
            if (conn.State == ConnectionState.Open)
                conn.Close();

            return Ok(SchoolInfoList);

        }

    }
}
