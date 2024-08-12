using Microsoft.Data.SqlClient;
using System.Data;
using WebApplicationAPI_2.Connections;
using WebApplicationAPI_2.Modelos;

namespace WebApplicationAPI_2.Repos
{
    public class UserSQL: IUserSQL
    {
        private string CadConexion;
        private readonly ILogger<UserSQL> log;
        public UserSQL(AccessSQL cadConexion, ILogger<UserSQL> log)
        {
            CadConexion = cadConexion.CadConnection;
            this.log = log;
        }
        private SqlConnection conexion()
        {
            return new SqlConnection(CadConexion);
        }

        public async Task<User> LoginUser(UserSession login)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand comm = null;
            User person = null;

            try
            {
                sqlConexion.Open();
                comm = sqlConexion.CreateCommand();
                comm.CommandText = "dbo.LogInUser";
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = login.EmailUser;
                comm.Parameters.Add("@Password", SqlDbType.VarChar, 500).Value = login.passUser;
                comm.ExecuteNonQuery();
                SqlDataReader reader = await comm.ExecuteReaderAsync();

                if (reader.Read())
                {
                    person = new User
                    {
                        userId = reader["id"].ToString(),
                        nameUser = reader["userName"].ToString(),
                        emailUser = reader["userEmail"].ToString(),
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error to get by user from DB" + ex.ToString());
            }
            finally
            {
                comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return person;
        }
    }
}
