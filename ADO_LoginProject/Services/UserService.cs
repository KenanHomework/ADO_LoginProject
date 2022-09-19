using ADO_LoginProject.Enums;
using ADO_LoginProject.MVVM.Models.BaseClasses;
using ADO_LoginProject.MVVM.Models.DerivedClasses;
using ADO_LoginProject.MVVM.Models.GeneralClasses;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ADO_LoginProject.Services
{
    public abstract class UserService
    {

        public static ProcessResult Search(User user)
        {
            if (!UserExist(user.Username))
                return ProcessResult.NotFound;

            User data;
            try { data = Read(user); }
            catch (Exception) { return ProcessResult.Empty; }

            if (data == null)
                return ProcessResult.NotFound;

            if (data.Password.Compare(user.Password))
                return ProcessResult.Success;

            return ProcessResult.IncorrectPassword;
        }

        public static ProcessResult Search(int? id)
        {
            if (id == null)
                return ProcessResult.NotFound;

            if (!UserExist(id))
                return ProcessResult.NotFound;

            User data;
            try { data = Read(id); }
            catch (Exception) { return ProcessResult.Empty; }

            if (data == null)
                return ProcessResult.NotFound;

            return ProcessResult.Success;
        }


        public static ProcessResult Search(string username, string password)
        {

            if (!UserService.UserExist(username))
                return ProcessResult.NotFound;

            User data;
            try
            {
                using (SqlConnection conn = new(App.ConnectionString))
                {
                    conn.Open();
                    string q = "USE [WolfTaxiDB] SELECT * FROM Users WHERE Username LIKE @Username";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.Add(new() { ParameterName = "@Username", Value = username, SqlDbType = System.Data.SqlDbType.NVarChar });
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    data = User.ReadFromDataReader(reader);


                    try { data = Read(username); }
                    catch (Exception) { return ProcessResult.Empty; }

                    if (data == null)
                        return ProcessResult.NotFound;

                    Hash check = new(reader);
                    check.UpdateValue(password);
                    if (data.Password.Compare(password))
                        return ProcessResult.Success;

                }
            }
            catch (Exception) { }

            return ProcessResult.IncorrectPassword;
        }

        public static bool UserExist(string username)
        {
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new(App.ConnectionString))
                {
                    conn.Open();
                    string q = "USE [WolfTaxiDB] SELECT Id FROM Users WHERE Username LIKE @Username";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.Add(new() { ParameterName = "@Username", Value = username, SqlDbType = System.Data.SqlDbType.NVarChar });
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    int id = (int)reader[0];
                }
            }
            catch (Exception) { return false; }
            return true;
        }

        public static bool UserExist(int? id)
        {
            SqlDataReader reader;
            try
            {
                using (SqlConnection conn = new(App.ConnectionString))
                {
                    conn.Open();
                    string q = "USE [WolfTaxiDB] SELECT Id FROM Users WHERE Id = @ID";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.Add(new() { ParameterName = "@ID", Value = id, SqlDbType = System.Data.SqlDbType.NVarChar });
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    int test = (int)reader[0];
                }
            }
            catch (Exception) { return false; }
            return true;
        }

        public static User Read(User user) => Read(user.Username);

        public static User Read(string username)
        {
            SqlDataReader reader;
            User user = new User();
            try
            {
                using (SqlConnection conn = new(App.ConnectionString))
                {
                    conn.Open();
                    string q = "USE [WolfTaxiDB] SELECT * FROM Users WHERE Username LIKE @Username";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.Add(new() { ParameterName = "@Username", Value = username, SqlDbType = System.Data.SqlDbType.NVarChar });
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    user = User.ReadFromDataReader(reader);
                }
            }
            catch (Exception) { }
            return user;
        }

        public static User Read(int? id)
        {
            if (id == null) return null;
            SqlDataReader reader;
            User user = new User();
            try
            {
                using (SqlConnection conn = new(App.ConnectionString))
                {
                    conn.Open();
                    string q = "USE [WolfTaxiDB] SELECT * FROM Users WHERE ID = @ID";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.Add(new() { ParameterName = "@ID", Value = id, SqlDbType = System.Data.SqlDbType.NVarChar });
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    user = User.ReadFromDataReader(reader);
                }
            }
            catch (Exception) { }
            return user;
        }


        public static void Update(User user)
        {
            try
            {
                using (SqlConnection conn = new("Data Source=ASUS-TUF-KENAN\\KENANSQL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    conn.Open();
                    string q = "USE [WolfTaxiDB] UPDATE  Users SET [Username] = @Username ,[Password] = @Password,[Phone] = @Phone,[Email] = @Email,[SaltStr1] = @SaltStr1,[SaltStr2] = @SaltStr2 , [Key] = @Key where Id = @Id";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.Add(new() { ParameterName = "@Id", Value = user.ID, SqlDbType = System.Data.SqlDbType.Int });
                    cmd.Parameters.Add(new() { ParameterName = "@Username", Value = user.Username, SqlDbType = System.Data.SqlDbType.NVarChar });
                    cmd.Parameters.Add(new() { ParameterName = "@Password", Value = user.Password.Value, SqlDbType = System.Data.SqlDbType.NVarChar });
                    cmd.Parameters.Add(new() { ParameterName = "@Email", Value = user.Email, SqlDbType = System.Data.SqlDbType.NVarChar });
                    cmd.Parameters.Add(new() { ParameterName = "@Phone", Value = user.Phone, SqlDbType = System.Data.SqlDbType.NVarChar });
                    cmd.Parameters.Add(new() { ParameterName = "@SaltStr1", Value = user.Password.SaltStrings[0], SqlDbType = System.Data.SqlDbType.NVarChar });
                    cmd.Parameters.Add(new() { ParameterName = "@SaltStr2", Value = user.Password.SaltStrings[1], SqlDbType = System.Data.SqlDbType.NVarChar });
                    cmd.Parameters.Add(new() { ParameterName = "@Key", Value = user.Password.HashKey, SqlDbType = System.Data.SqlDbType.NChar });
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception) { }

        }

        public static void Write(User user)
        {
            try
            {
                using (SqlConnection conn = new("Data Source=ASUS-TUF-KENAN\\KENANSQL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    conn.Open();
                    string q = "USE [WolfTaxiDB] INSERT INTO  Users VALUES(@Username ,@Password,@Phone,@Email, @SaltStr1, @SaltStr2 , @Key)";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.Add(new() { ParameterName = "@Username", Value = user.Username, SqlDbType = System.Data.SqlDbType.NVarChar });
                    cmd.Parameters.Add(new() { ParameterName = "@Password", Value = user.Password.Value, SqlDbType = System.Data.SqlDbType.NVarChar });
                    cmd.Parameters.Add(new() { ParameterName = "@Email", Value = user.Email, SqlDbType = System.Data.SqlDbType.NVarChar });
                    cmd.Parameters.Add(new() { ParameterName = "@Phone", Value = user.Phone, SqlDbType = System.Data.SqlDbType.NVarChar });
                    cmd.Parameters.Add(new() { ParameterName = "@SaltStr1", Value = user.Password.SaltStrings[0], SqlDbType = System.Data.SqlDbType.NVarChar });
                    cmd.Parameters.Add(new() { ParameterName = "@SaltStr2", Value = user.Password.SaltStrings[1], SqlDbType = System.Data.SqlDbType.NVarChar });
                    cmd.Parameters.Add(new() { ParameterName = "@Key", Value = user.Password.HashKey, SqlDbType = System.Data.SqlDbType.NChar });
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception) { }
        }

    }
}
