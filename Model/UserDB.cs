using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wpfHUSH.Model
{
    public class UserDB
    {
        ConnectionDB connection;

        private UserDB(ConnectionDB db)
        {
            this.connection = db;
        }

        public bool Insert(User user)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `User` Values (0, @Name, @About, @Age, @Gender, @Photo, @LoginPasswordId, @RoleId, @ReportId, @ContactId);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Name", user.Name));
                cmd.Parameters.Add(new MySqlParameter("About", user.About));
                cmd.Parameters.Add(new MySqlParameter("Age", user.Age));
                cmd.Parameters.Add(new MySqlParameter("Gender", user.Gender));
                cmd.Parameters.Add(new MySqlParameter("Photo", user.Photo));
                cmd.Parameters.Add(new MySqlParameter("LoginPasswordId", user.LoginPasswordId));
                cmd.Parameters.Add(new MySqlParameter("RoleId", user.RoleId));
                cmd.Parameters.Add(new MySqlParameter("ReportId", user.ReportId));
                cmd.Parameters.Add(new MySqlParameter("ContactId", user.ContactId));
                try
                {

                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());
                        user.Id = id;
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Запись не добавлена");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;

        }

        internal List<User> SelectAll()
        {
            List<User> users = new List<User>();
            if (connection == null)
                return users;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT User.Id, Name, About, Age, Gender, Photo, LoginPasswordId, RoleId, ContactId, City, Login, Password, RoleName, Text, ReasonId, Link FROM User LEFT JOIN LoginPassword ON LoginPasswordId = LoginPassword.Id LEFT JOIN Role ON RoleId = Role.Id LEFT JOIN Report ON ReportId = Report.Id LEFT JOIN Contact ON ContactId = Contact.Id");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);

                        string name = string.Empty;
                        if (!dr.IsDBNull(1))
                            name = dr.GetString("Name");

                        string about = string.Empty;
                        if (!dr.IsDBNull(2))
                            about = dr.GetString("About");

                        int age = 0;
                        if (!dr.IsDBNull(3))
                            age = dr.GetInt32("Age");

                        bool gender = false;
                        if (!dr.IsDBNull(4))
                            gender = dr.GetBoolean("Gender");

                        byte[] photo = null;
                        if (!dr.IsDBNull(5))
                        {
                            long size = dr.GetBytes(5, 0, null, 0, 0);
                            photo = new byte[size];
                            dr.GetBytes(5, 0, photo, 0, (int)size);
                        }

                        int loginPasswordId = 0;
                        if (!dr.IsDBNull(6))
                            loginPasswordId = dr.GetInt32("LoginPasswordId");

                        int roleId = 0;
                        if (!dr.IsDBNull(7))
                            roleId = dr.GetInt32("RoleId");

                        int contactId = 0;
                        if (!dr.IsDBNull(8))
                            contactId = dr.GetInt32("ContactId");

                        string city = string.Empty;
                        if (!dr.IsDBNull(9))
                            city = dr.GetString("City");

                        string login = string.Empty;
                        if (!dr.IsDBNull(10))
                            login = dr.GetString("Login");

                        string password = string.Empty;
                        if (!dr.IsDBNull(11))
                            password = dr.GetString("Password");

                        string roleName = string.Empty;
                        if (!dr.IsDBNull(12))
                            roleName = dr.GetString("RoleName");
                        
                        string text = string.Empty;
                        if (!dr.IsDBNull(13))
                            text = dr.GetString("Text");

                        int reasonId = 0;
                        if (!dr.IsDBNull(14))
                            reasonId = dr.GetInt32("ReasonId");

                        string link = string.Empty;
                        if (!dr.IsDBNull(15))
                            link = dr.GetString("Link");

                        LoginPassword loginPassword = new LoginPassword();
                        loginPassword.Login = login;
                        loginPassword.Password = password;

                        Role role = new Role();
                        role.RoleName = roleName;
                        
                        Report report = new Report();
                        report.Text = text;
                        report.ReasonId = reasonId;

                        Contact contact = new Contact();
                        contact.Link = link;


                        users.Add(new User
                        {
                            Id = id,
                            Name = name,
                            About = about,
                            Age = age,
                            Gender = gender,
                            Photo = photo,
                            LoginPasswordId = loginPasswordId,
                            RoleId = roleId,
                            ContactId = contactId,
                            City = city                           
                        });


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return users;
        }

        internal bool Update(User edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `User` set `Name`=@Name, `About`=@About, `Age`=@Age, `Gender`=@Gender, `Photo`=@Photo, `RoleId`=@RoleId, `ReportId`=@ReportId, `ContactId`=@ContactId, `City`=@City, `Id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("Name", edit.Name));
                mc.Parameters.Add(new MySqlParameter("About", edit.About));
                mc.Parameters.Add(new MySqlParameter("Age", edit.Age));
                mc.Parameters.Add(new MySqlParameter("Gender", edit.Gender));
                mc.Parameters.Add(new MySqlParameter("Photo", edit.Photo));
                mc.Parameters.Add(new MySqlParameter("RoleId", edit.RoleId));
                mc.Parameters.Add(new MySqlParameter("ReportId", edit.ReportId));
                mc.Parameters.Add(new MySqlParameter("ContactId", edit.ContactId));
                mc.Parameters.Add(new MySqlParameter("City", edit.City));

                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }


        internal bool Remove(User remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `User` where `Id` = {remove.Id}");
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        static UserDB db;
        public static UserDB GetDb()
        {
            if (db == null)
                db = new UserDB(ConnectionDB.GetDbConnection());
            return db;
        }
    }
}
