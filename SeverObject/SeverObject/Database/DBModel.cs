using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Database
{
    public sealed class DBModel
    {



        public DBModel()
        {
            //string server = "DESKTOP-FIJV45R";
            string server = "localhost";
            //string server = "127.0.0.1";
            string database = "remotetest";
            string uid = "root";
            string password = "windowsActivation0110";

            string connectionString = "SERVER=" + server + "; DATABASE=" + database +

                "; UID=" + uid + "; PASSWORD=" + password + ";";
            

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string query = "show databases";

                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();


                using(SqlDataReader reader = command.ExecuteReader())
                {
                    
                    while(reader.Read())
                    {
                        object test = reader[0];
                    }
                }

            }

        }

    }
}
