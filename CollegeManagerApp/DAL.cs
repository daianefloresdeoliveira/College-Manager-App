using Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    internal class Connect
    {
        private static String cliComConnectionString = GetConnectString();

        internal static String ConnectionString { get => cliComConnectionString; }

        private static String GetConnectString()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs.DataSource = "(local)";
            cs.InitialCatalog = "College1en";
            cs.UserID = "sa";
            cs.Password = "sysadm";
            return cs.ConnectionString;
        }
    }
    internal class DataTables
    {
        private static SqlDataAdapter adapterStudents = InitAdapterStudents();
        private static SqlDataAdapter adapterPrograms = InitAdapterPrograms();
        private static SqlDataAdapter adapterCourses = InitAdapterCourses();
        private static SqlDataAdapter adapterEnroll = InitAdapterEnroll();
        private static SqlDataAdapter adapterDisplayEnroll = InitAdapterDisplayEnroll();

        private static DataSet InitDataSet()
        {
            DataSet ds = new DataSet();
            loadPrograms(ds);
            loadStudents(ds);            
            loadCourses(ds);
            loadEnroll(ds);
            loadDisplayEnroll(ds);
            return ds;
        }

        private static DataSet ds = InitDataSet();

        private static SqlDataAdapter InitAdapterStudents()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Students ORDER BY StId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitAdapterPrograms()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Programs ORDER BY ProgId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitAdapterCourses()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Courses ORDER BY CId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitAdapterEnroll()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Enrollments ORDER BY StId, CId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitAdapterDisplayEnroll()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT s.StId, s.StName, c.CId, c.CName, e.FinalGrade " +
                "FROM Enrollments e, Students s, Courses c " +
                "WHERE s.StId = e.StId AND c.CId = e.CId " +
                "ORDER BY StId, CId ",
                Connect.ConnectionString);

            return r;
        }

        private static void loadStudents(DataSet ds)
        {
            
            adapterStudents.Fill(ds, "Students");

            // =========================================================================
              ds.Tables["Students"].Columns["StId"].AllowDBNull = false;
              ds.Tables["Students"].Columns["StName"].AllowDBNull = false;
              //ds.Tables["Students"].Columns["ProgId"].AllowDBNull = false;
            
              ds.Tables["Students"].PrimaryKey = new DataColumn[1]
                   { ds.Tables["Students"].Columns["StId"]};
            // =========================================================================

            ForeignKeyConstraint myFK03 = new ForeignKeyConstraint("MyFK03",    //FK: FK_S_P
                new DataColumn[]{
                    ds.Tables["Programs"].Columns["ProgId"]
                },
                new DataColumn[] {
                    ds.Tables["Students"].Columns["ProgId"]
                }
            );
            myFK03.DeleteRule = Rule.None;
            myFK03.UpdateRule = Rule.Cascade;
            ds.Tables["Students"].Constraints.Add(myFK03);
        }

        private static void loadPrograms(DataSet ds)
        {
            adapterPrograms.Fill(ds, "Programs");

            ds.Tables["Programs"].Columns["ProgId"].AllowDBNull = false;
            ds.Tables["Programs"].Columns["ProgName"].AllowDBNull = false;

            ds.Tables["Programs"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Programs"].Columns["ProgId"]};
            
        }

        private static void loadCourses(DataSet ds)
        {
            adapterCourses.Fill(ds, "Courses");

            // =========================================================================
            ds.Tables["Courses"].Columns["CId"].AllowDBNull = false;
            ds.Tables["Courses"].Columns["CName"].AllowDBNull = false;
            // ds.Tables["Students"].Columns["ProgId"].AllowDBNull = false;

            ds.Tables["Courses"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Courses"].Columns["CId"]};
            // =========================================================================    

            ForeignKeyConstraint myFK04 = new ForeignKeyConstraint("MyFK04",    //FK: FK_C_P
                new DataColumn[]{
                    ds.Tables["Programs"].Columns["ProgId"]
                },
                new DataColumn[] {
                    ds.Tables["Courses"].Columns["ProgId"]
                }
            );
            myFK04.DeleteRule = Rule.Cascade;
            myFK04.UpdateRule = Rule.Cascade;
            ds.Tables["Courses"].Constraints.Add(myFK04);
        }

        private static void loadEnroll(DataSet ds)
        {
            adapterEnroll.Fill(ds, "Enrollments");

            // =========================================================================
            ds.Tables["Enrollments"].Columns["StId"].AllowDBNull = false;
            ds.Tables["Enrollments"].Columns["CId"].AllowDBNull = false;

            ds.Tables["Enrollments"].PrimaryKey = new DataColumn[2]
                    { ds.Tables["Enrollments"].Columns["StId"], ds.Tables["Enrollments"].Columns["CId"] };

            // =========================================================================  
            /* Foreign Key between DataTables */

            ForeignKeyConstraint myFK01 = new ForeignKeyConstraint("MyFK01",       //FK: FK_E_S
                new DataColumn[]{
                    ds.Tables["Students"].Columns["StId"]
                },
                new DataColumn[] {
                    ds.Tables["Enrollments"].Columns["StId"]
                }
            );
            myFK01.DeleteRule = Rule.Cascade;
            myFK01.UpdateRule = Rule.Cascade;
            ds.Tables["Enrollments"].Constraints.Add(myFK01);

            ForeignKeyConstraint myFK02 = new ForeignKeyConstraint("MyFK02",          //FK: FK_E_C
              new DataColumn[]{
                    ds.Tables["Courses"].Columns["CId"]
              },
              new DataColumn[] {
                    ds.Tables["Enrollments"].Columns["CId"]
              }
          );
            myFK02.DeleteRule = Rule.None;
            myFK02.UpdateRule = Rule.None;
            ds.Tables["Enrollments"].Constraints.Add(myFK02);

            // =========================================================================  
        }

        private static void loadDisplayEnroll(DataSet ds)
        {
            adapterDisplayEnroll.Fill(ds, "DisplayEnrollments");

            // =========================================================================  
            /* Foreign Key between DataTables */

            ForeignKeyConstraint myFK01 = new ForeignKeyConstraint("MyFK01",
                new DataColumn[]{
                    ds.Tables["Enrollments"].Columns["StId"],
                    ds.Tables["Enrollments"].Columns["CId"]
                },
                new DataColumn[] {
                    ds.Tables["DisplayEnrollments"].Columns["StId"],
                    ds.Tables["DisplayEnrollments"].Columns["CId"]
                }
            );
            myFK01.DeleteRule = Rule.Cascade;
            myFK01.UpdateRule = Rule.Cascade;
            ds.Tables["DisplayEnrollments"].Constraints.Add(myFK01);

            // ========================================================================= 
        }

        internal static SqlDataAdapter getAdapterStudents()
        {
            return adapterStudents;
        }
        internal static SqlDataAdapter getAdapterCourses()
        {
            return adapterCourses;
        }

        internal static SqlDataAdapter getAdapterPrograms()
        {
            return adapterPrograms;
        }
        internal static SqlDataAdapter getAdapterEnroll()
        {
            return adapterEnroll;
        }
        internal static SqlDataAdapter getAdapterDisplayEnroll()
        {
            return adapterDisplayEnroll;
        }
        internal static DataSet getDataSet()
        {
            return ds;
        }
    }

    internal class Students
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterStudents();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetStudents()
        {
            return ds.Tables["Students"];
        }

        internal static int UpdateStudents()
        {
            if (!ds.Tables["Students"].HasErrors)
            {
                return adapter.Update(ds.Tables["Students"]);
            }
            else
            {
                return -1;
            }
        }
    }
    internal class Courses
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterCourses();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetCourses()
        {
            return ds.Tables["Courses"];
        }

        internal static int UpdateCourses()
        {
            if (!ds.Tables["Courses"].HasErrors)
            {
                return adapter.Update(ds.Tables["Courses"]);
            }
            else
            {
                return -1;
            }
        }
    }

    internal class Programs
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterPrograms();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetPrograms()
        {
            return ds.Tables["Programs"];
        }

        internal static int UpdatePrograms()
        {
            if (!ds.Tables["Programs"].HasErrors)
            {
                return adapter.Update(ds.Tables["Programs"]);
            }
            else
            {
                return -1;
            }
        }
    }


    internal class Enrollments
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterEnroll();
        private static DataSet ds = DataTables.getDataSet();

        private static DataTable displayEnroll = null;

        internal static DataTable GetDisplayEnrollments()
        {
            displayEnroll = ds.Tables["DisplayEnrollments"];
            return displayEnroll;
        }

        internal static int InsertData(string[] a)
        {
            var test = (
                   from enroll in ds.Tables["Enrollments"].AsEnumerable()
                   where enroll.Field<string>("StId") == a[0]
                   where enroll.Field<string>("CId") == a[1]
                   select enroll);
            if (test.Count() > 0)
            {
                College1en.Form1.DALMessage("This enrollment already exists");
                return -1;
            }
            try
            {
                DataRow line = ds.Tables["Enrollments"].NewRow();
                line.SetField("StId", a[0]);
                line.SetField("CId", a[1]);
                ds.Tables["Enrollments"].Rows.Add(line);

                adapter.Update(ds.Tables["Enrollments"]);

                if (displayEnroll != null)
                {
                    var query = (
                           from Students in ds.Tables["Students"].AsEnumerable()
                           from Courses in ds.Tables["Courses"].AsEnumerable()
                           where Students.Field<string>("StId") == a[0]
                           where Courses.Field<string>("CId") == a[1]
                           select new
                           {
                               StudentsId = Students.Field<string>("StId"),
                               StudentsName = Students.Field<string>("StName"),
                               CId = Courses.Field<string>("CId"),
                               CName = Courses.Field<string>("CName"),
                               FinalGrade = line.Field<Nullable<int>>("FinalGrade")
                           });
                    var r = query.Single();
                    displayEnroll.Rows.Add(new object[] { r.StudentsId, r.StudentsName, r.CId, r.CName, r.FinalGrade });
                }
                return 0;
            }
            catch (Exception)
            {
                College1en.Form1.DALMessage("Insertion / Update rejected");
                return -1;
            }
        }
        internal static int UpdateData(string[] a)
        {
            return 0;  
        }
        internal static int DeleteData(List<string[]> lId)
        {
            try
            {
                var lines = ds.Tables["Enrollments"].AsEnumerable()
                                .Where(s =>
                                   lId.Any(x => (x[0] == s.Field<string>("StId") && x[1] == s.Field<string>("CId"))));

                foreach (var line in lines)
                {
                    line.Delete();
                }

                adapter.Update(ds.Tables["Enrollments"]);

                return 0;
            }
            catch (Exception)
            {
                College1en.Form1.DALMessage("Update / Deletion rejected");
                return -1;
            }
        }

        internal static int UpdateFinalGrade(string[] a, Nullable<int> finalgrade)
        {
            try
            {
                var line = ds.Tables["Enrollments"].AsEnumerable()
                                    .Where(s =>
                                      (s.Field<string>("StId") == a[0] && s.Field<string>("CId") == a[1]))
                                    .Single();

                line.SetField("FinalGrade", finalgrade);

                adapter.Update(ds.Tables["Enrollments"]);

                if (displayEnroll != null)
                {
                    var r = displayEnroll.AsEnumerable()
                                    .Where(s =>
                                      (s.Field<string>("StId") == a[0] && s.Field<string>("CId") == a[1]))
                                    .Single();
                    r.SetField("FinalGrade", finalgrade);
                }
                return 0;
            }
            catch (Exception)
            {
                College1en.Form1.DALMessage("Update / Deletion rejected");
                return -1;
            }
        }
    }

}
