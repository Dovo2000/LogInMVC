using LogInMVC.Helpers;
using LogInMVC.Models;
using Microsoft.Data.SqlClient;

namespace LogInMVC.DAL
{
    public class UsuarioDAL
    {
        string connectionString = "Data Source=85.208.21.117,54321;" +
            "Initial Catalog=DavidMartinLogIn;" +
            "User ID=sa;" +
            "Password=Sql#123456789;" +
            "TrustServerCertificate=True;";

        public UsuarioDAL()
        { 

        }

        public int GetCountByUserName(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand(@"SELECT COUNT(*) FROM Usuario
                                            WHERE UserName = @UserName", conn);

                cmd.Parameters.AddWithValue("@UserName", username);
                conn.Open();

                return (int)cmd.ExecuteScalar();
            }
        }

        public Usuario GetByLogIn(string username,  string pwd)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand(@"SELECT * FROM Usuario
                                            WHERE UserName = @UserName", connection);

                cmd.Parameters.AddWithValue("@UserName", username);

                connection.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        byte[] passwordHash = (byte[])reader["PasswordHash"];
                        byte[] passwordSalt = (byte[])reader["PasswordSalt"];

                        if(PasswordHelper.VerifyPasswordHash(pwd, passwordHash, passwordSalt))
                        {
                            return new Usuario
                            {
                                IdUsuario = (int) reader["IdUsuario"],
                                UserName = (string) reader["UserName"],
                                PasswordHash = (byte[]) reader["PasswordHash"],
                                PasswordSalt = (byte[]) reader["PasswordSalt"],
                                Apellido = reader["Apellido"] as string,
                                Email = reader["Email"] as string,
                                FechaNacimiento = reader["FechaNacimiento"] as DateTime?,
                                Telefono = reader["Telefono"] as string,
                                Direccion = reader["Direccion"] as string,
                                Ciudad = reader["Ciudad"] as string,
                                Estado = reader["Estado"] as string,
                                CodigoPostal = reader["CodigoPostal"] as string,
                                FechaRegistro = reader["FechaRegistro"] as DateTime?,
                                Activo = reader["Activo"] as bool?
                            };
                        }
                    }
                    return null;
                }
            }
        }

        internal void InsertBySignUp(Usuario user, string password)
        {
            PasswordHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            using(var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand(@"INSERT INTO Usuario(UserName, PasswordHash, PasswordSalt)
                                            VALUES(@UserName, @PasswordHash, @PasswordSalt);", conn);

                cmd.Parameters.AddWithValue("@UserName", user.UserName);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                cmd.Parameters.AddWithValue("@PasswordSalt", passwordSalt);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
