using ADO_LoginProject.MVVM.Models.BaseClasses;
using ADO_LoginProject.MVVM.Models.GeneralClasses;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace ADO_LoginProject.MVVM.Models.DerivedClasses
{
    public class User : Human
    {

        public static User ReadFromDataReader(SqlDataReader reader)
        {
            return new()
            {
                Username = (string)reader.GetValue("Username"),
                Password = new(reader),
                Email = (string)reader.GetValue("Email"),
                ID = (int)reader.GetValue("Id"),
                Phone = (string)reader.GetValue("Phone")
            };
        }

        public User() : base() { }

        public User(string username, string password, string email, string phone) : base(username, password, email, phone) { }

        public User(string username, string password) : base(username, password) { }

        public override string ToString() => $"Username: {Username} Password: {Password} Phone: {Phone} Email: {Email}";
    }
}
