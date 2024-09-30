using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeHRM.Utility
{
    public class DBConnection
    {
        public static string connectingstring = "Data Source=Evol ;";

        public static DataTable GetQueryResult(string vConnectingString, string vQuery) {

            SqlConnection connection; //It is SQl Connection;
            DataSet ds = new DataSet(); //it is for store qurey result
            try
            {
                connection = new SqlConnection(vConnectingString);//declare sql connection with connection string
                connection.Open(); // connect to database
                Console.WriteLine("Connection with database is done."); 

                SqlDataAdapter adapter = new SqlDataAdapter(vQuery,connection); //Execute the query
                adapter.Fill(ds); //store query result into DataSet object
                connection.Close(); //close connection
                connection.Dispose();
            }
            catch(Exception e) {
                Console.WriteLine("Error in getting result of query");
                Console.WriteLine(e.Message);
                return new DataTable();
            }
            return ds.Tables[0];
        }

        public static DataTable getDetails() {
            string query = "select top 50 * from orange.dbo.Customer where customerId is not null ";
            return GetQueryResult(connectingstring, query);
        }

    }
}
