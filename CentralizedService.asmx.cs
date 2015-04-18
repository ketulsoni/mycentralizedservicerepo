using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace CentralizedWebAPI
{
    /// <summary>
    /// Summary description for CentralizedService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CentralizedService : System.Web.Services.WebService
    {

        SqlConnection con = new SqlConnection(@"Server=fcdef64e-d839-458f-8778-a47e0051d4da.sqlserver.sequelizer.com;Database=dbfcdef64ed839458f8778a47e0051d4da;User ID=kopimrocppmzbfop;Password=UqUkzLeKcJGAgtkouY7wWpifLDBhXr3safhkeLUUCas2ig7rzoJknmWDhPF5j5UT;");
        SqlCommand cmd;
        SqlDataAdapter da;
        DataSet ds;

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string RegisterUser(string FirstName,string LastName,string Email,string Password)
        {
            string Status = "";
            cmd = new SqlCommand("insert into useri(Fname,Lname,Email,Pwd) values(@Fname,@Lname,@Email,@Pass)", con);
            cmd.Parameters.AddWithValue("@Fname", FirstName);
            cmd.Parameters.AddWithValue("@Lname", LastName);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Pass", Password);
            con.Open();
            Status = cmd.ExecuteNonQuery().ToString();
            con.Close();
            if (Status != "")
            {
                return "Registered Successfully";
            }
            else
            {
                return "There should be some error";
            }
            
        }

        [WebMethod]
        public string UpdateProfile(string UID,string Country,string City,string ProfilePic,string Mobile, string FirstName, string LastName, string DOB, string Password)
        {
            string Status = "";
            cmd = new SqlCommand("update useri set Fname=@Fname,Lname=@Lname,Dob=@DOB,Pwd=@Pass,Propic=@PIC,Mob=@Mob,City=@City,Country=@Country where Id=@UID", con);
            cmd.Parameters.AddWithValue("@Fname", FirstName);
            cmd.Parameters.AddWithValue("@Lname", LastName);
            cmd.Parameters.AddWithValue("@DOB", DOB);
            cmd.Parameters.AddWithValue("@Pass", Password);
            cmd.Parameters.AddWithValue("@PIC", ProfilePic);
            cmd.Parameters.AddWithValue("@Mob", Mobile);
            cmd.Parameters.AddWithValue("@City", City);
            cmd.Parameters.AddWithValue("@Country", Country);
            cmd.Parameters.AddWithValue("@UID", UID);
            con.Open();
            Status = cmd.ExecuteNonQuery().ToString();
            con.Close();
            if (Status != "")
            {
                return "Updated Successfully";
            }
            else
            {
                return "There should be some error";
            }
        }

        [WebMethod]
        public DataSet LoginUser(string Email, string Password)
        {
            cmd = new SqlCommand("select * from useri where Email=@Email and Pwd=@Pass", con);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Pass", Password);
            da = new SqlDataAdapter(cmd);
            ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        [WebMethod]
        public DataSet SearchUser(string Exp)
        {
            da=new SqlDataAdapter("select * from useri where Fname like '%"+Exp+"%'", con);
            ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        [WebMethod]
        public DataSet ForgetPassword(string Email)
        {
            da = new SqlDataAdapter("select Pwd from useri where Email=@Email", con);
            da.SelectCommand.Parameters.AddWithValue("@Email", Email);
            ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        [WebMethod]
        public DataSet GetAllCategories()
        {
            da = new SqlDataAdapter("select * from categoryi", con);
            ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        [WebMethod]
        public string PostQuestion(string QuestionTitle,string UserID,string CategoryID)
        {
            string ss = "";
            cmd = new SqlCommand("insert into Post_Questioni values(@PTitle,@UID,@CATID)", con);
            cmd.Parameters.AddWithValue("@PTitle", QuestionTitle);
            cmd.Parameters.AddWithValue("@UID", UserID);
            cmd.Parameters.AddWithValue("@CATID", CategoryID);
            con.Open();
            ss=cmd.ExecuteNonQuery().ToString();
            con.Close();
            if (ss != "")
            {
                return "Question Inserted";
            }
            else {
                return "There should be error";
            }
        }

        [WebMethod]
        public DataSet GetQuestionsWithAnswer()
        {
            da = new SqlDataAdapter("select useri.Fname,useri.Lname,Post_Questioni.PQ_Title,Post_Answeri.PA_Title from Post_Answeri inner join useri on useri.Id=Post_Answeri.Id inner join Post_Questioni on Post_Questioni.PQ_Id=Post_Answeri.PQ_Id", con);
            ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        [WebMethod]
        public DataSet GetQuestionsByUser(string UserID)
        {
            da = new SqlDataAdapter("select a.fname, a.lname, b.pq_title from useri a, post_questioni b where a.id = b.id and a.Id=@UID", con);
            da.SelectCommand.Parameters.AddWithValue("@UID", UserID);
            ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        [WebMethod]
        public DataSet GetQuestionsByCategory(string CategoryID)
        {
            da = new SqlDataAdapter("select * from Post_Questioni where catid=@CID", con);
            da.SelectCommand.Parameters.AddWithValue("@CID", CategoryID);
            ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        [WebMethod]
        public DataSet GetRecentQuestion()
        {
            da = new SqlDataAdapter("select * from Post_Questioni order by PQ_Id desc",con);
            ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        [WebMethod]
        public DataSet GetAnswerByQuestion(string QuestionID)
        {
            da = new SqlDataAdapter("select * from Post_Answeri where PQ_Id=@QID order by PA_Id desc", con);
            da.SelectCommand.Parameters.AddWithValue("@QID", QuestionID);
            ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        [WebMethod]
        public DataSet GetAllVideos()
        {
            da = new SqlDataAdapter("Select * from videoi", con);
            ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public string DeleteVideoByUser(string VideoID,string UserID)
        {
            string ss = "";
            cmd = new SqlCommand("delete from  videoi where Id=@UID and V_Id=@VID", con);
            cmd.Parameters.AddWithValue("@UID", UserID);
            cmd.Parameters.AddWithValue("@VID", VideoID);
            con.Open();
            ss = cmd.ExecuteNonQuery().ToString();
            con.Close();
            if (ss != "")
            {
                return "Deleted";
            }
            else
            {
                return "There should be some error";
            }
        }

        [WebMethod]
        public string PostFeedBack(string FeedBackTitle,string UserID)
        {
            string ss = "";
            cmd = new SqlCommand("insert into Feedbacki values(@FTitle,@UID)", con);
            cmd.Parameters.AddWithValue("@FTitle", FeedBackTitle);
            cmd.Parameters.AddWithValue("@UID", UserID);
            con.Open();
            ss = cmd.ExecuteNonQuery().ToString();
            con.Close();
            if (ss != "")
            {
                return "Submitted";
            }
            else
            {
                return "There should be error";
            }
        }
    }
}
