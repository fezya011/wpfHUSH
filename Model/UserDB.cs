using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
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

        public bool InsertLoginPassword(LoginPassword loginPassword)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `LoginPassword` Values (0, @Login, @Password);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Login", loginPassword.Login));
                cmd.Parameters.Add(new MySqlParameter("Password", loginPassword.Password));
                
                try
                {

                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        //MessageBox.Show(id.ToString());
                        loginPassword.Id = id;
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка базы данных при добавлении LoginPassword в User");
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

        public bool InsertContact(Contact contact)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Contact` Values (0, @Telegram, @VK);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Telegram", contact.Telegram));
                cmd.Parameters.Add(new MySqlParameter("VK", contact.VK));

                try
                {

                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        //MessageBox.Show(id.ToString());
                        contact.Id = id;
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка базы данных при добавлении Contact в User");
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

        public bool Insert(User user)
        {
            //InsertContact(user.Contact);
            InsertLoginPassword(user.LoginPassword);
            user.LoginPasswordId = user.LoginPassword.Id;
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `User` Values (0, @Name, @About, @Age, @Gender, @Photo, @LoginPasswordId, 1, @City, NULL, CURRENT_TIMESTAMP);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Name", user.Name));
                cmd.Parameters.Add(new MySqlParameter("About", user.About));
                cmd.Parameters.Add(new MySqlParameter("Age", user.Age));
                cmd.Parameters.Add(new MySqlParameter("Gender", user.Gender));
                cmd.Parameters.Add(new MySqlParameter("Photo", user.Photo));
                cmd.Parameters.Add(new MySqlParameter("LoginPasswordId", user.LoginPasswordId));
                cmd.Parameters.Add(new MySqlParameter("City", user.City));
                try
                {

                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        user.Id = id;
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка базы данных при добавлении User");
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
                var command = connection.CreateCommand("SELECT User.Id, Name, About, Age, Gender, Photo, LoginPasswordId, RoleId, ContactId, City, Login, Password, RoleName, Telegram, VK FROM User LEFT JOIN LoginPassword ON LoginPasswordId = LoginPassword.Id LEFT JOIN Role ON RoleId = Role.Id LEFT JOIN Contact ON ContactId = Contact.Id");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);

                        string name = string.Empty;
                        if (!dr.IsDBNull("Name"))
                            name = dr.GetString("Name");

                        string about = string.Empty;
                        if (!dr.IsDBNull("About"))
                            about = dr.GetString("About");

                        int age = 0;
                        if (!dr.IsDBNull("Age"))
                            age = dr.GetInt32("Age");

                        bool gender = false;
                        if (!dr.IsDBNull("Gender"))
                            gender = dr.GetBoolean("Gender");

                        byte[] photo = null;
                        if (!dr.IsDBNull(5))
                        {
                            long size = dr.GetBytes(5, 0, null, 0, 0);
                            photo = new byte[size];
                            dr.GetBytes(5, 0, photo, 0, (int)size);
                        }

                        int loginPasswordId = 0;
                        if (!dr.IsDBNull("LoginPasswordId"))
                            loginPasswordId = dr.GetInt32("LoginPasswordId");

                        int roleId = 0;
                        if (!dr.IsDBNull("RoleId"))
                            roleId = dr.GetInt32("RoleId");

                        int contactId = 0;
                        if (!dr.IsDBNull("ContactId"))
                            contactId = dr.GetInt32("ContactId");

                        string city = string.Empty;
                        if (!dr.IsDBNull("City"))
                            city = dr.GetString("City");

                        string login = string.Empty;
                        if (!dr.IsDBNull("Login"))
                            login = dr.GetString("Login");

                        string password = string.Empty;
                        if (!dr.IsDBNull("Password"))
                            password = dr.GetString("Password");

                        string roleName = string.Empty;
                        if (!dr.IsDBNull("RoleName"))
                            roleName = dr.GetString("RoleName");
                        
                        string telegram = string.Empty;
                        if (!dr.IsDBNull("Telegram"))
                            telegram = dr.GetString("Telegram");

                        string vk = string.Empty;
                        if (!dr.IsDBNull("VK"))
                            vk = dr.GetString("VK");

                        LoginPassword loginPassword = new LoginPassword();
                        loginPassword.Login = login;
                        loginPassword.Password = password;

                        Role role = new Role();
                        role.RoleName = roleName;                        

                        Contact contact = new Contact();
                        contact.Telegram = telegram;
                        contact.VK = vk;


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
                            City = city,
                            LoginPassword = loginPassword,
                            Role = role,
                            Contact = contact
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

        internal User SelectById(int userId)
        {
            User user = null;
            if (connection == null)
                return user;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT User.Id, Name, About, Age, Gender, Photo, LoginPasswordId, RoleId, ContactId, City, Login, Password, RoleName, Telegram, VK FROM User LEFT JOIN LoginPassword ON LoginPasswordId = LoginPassword.Id LEFT JOIN Role ON RoleId = Role.Id LEFT JOIN Contact ON ContactId = Contact.Id");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);

                        string name = string.Empty;
                        if (!dr.IsDBNull("Name"))
                            name = dr.GetString("Name");

                        string about = string.Empty;
                        if (!dr.IsDBNull("About"))
                            about = dr.GetString("About");

                        int age = 0;
                        if (!dr.IsDBNull("Age"))
                            age = dr.GetInt32("Age");

                        bool gender = false;
                        if (!dr.IsDBNull("Gender"))
                            gender = dr.GetBoolean("Gender");

                        byte[] photo = null;
                        if (!dr.IsDBNull(5))
                        {
                            long size = dr.GetBytes(5, 0, null, 0, 0);
                            photo = new byte[size];
                            dr.GetBytes(5, 0, photo, 0, (int)size);
                        }

                        int loginPasswordId = 0;
                        if (!dr.IsDBNull("LoginPasswordId"))
                            loginPasswordId = dr.GetInt32("LoginPasswordId");

                        int roleId = 0;
                        if (!dr.IsDBNull("RoleId"))
                            roleId = dr.GetInt32("RoleId");

                        int contactId = 0;
                        if (!dr.IsDBNull("ContactId"))
                            contactId = dr.GetInt32("ContactId");

                        string city = string.Empty;
                        if (!dr.IsDBNull("City"))
                            city = dr.GetString("City");

                        string login = string.Empty;
                        if (!dr.IsDBNull("Login"))
                            login = dr.GetString("Login");

                        string password = string.Empty;
                        if (!dr.IsDBNull("Password"))
                            password = dr.GetString("Password");

                        string roleName = string.Empty;
                        if (!dr.IsDBNull("RoleName"))
                            roleName = dr.GetString("RoleName");

                        string telegram = string.Empty;
                        if (!dr.IsDBNull("Telegram"))
                            telegram = dr.GetString("Telegram");

                        string vk = string.Empty;
                        if (!dr.IsDBNull("VK"))
                            vk = dr.GetString("VK");

                        LoginPassword loginPassword = new LoginPassword();
                        loginPassword.Login = login;
                        loginPassword.Password = password;

                        Role role = new Role();
                        role.RoleName = roleName;

                        Contact contact = new Contact();
                        contact.Telegram = telegram;
                        contact.VK = vk;


                        user = new User
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
                            City = city,
                            LoginPassword = loginPassword,
                            Role = role,
                            Contact = contact
                        };


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return user;
        }

        internal List<User> SearchSelectAll()
        {
            List<User> users = new List<User>();
            if (connection == null)
                return users;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand($"SELECT User.Id, Name, About, Age, Gender, Photo, LoginPasswordId, RoleId, ContactId, City, Login, Password, RoleName, Telegram, VK FROM User LEFT JOIN LoginPassword ON LoginPasswordId = LoginPassword.Id LEFT JOIN Role ON RoleId = Role.Id LEFT JOIN Contact ON ContactId = Contact.Id where User.Id != {UserStatic.CurrentUser.Id} and RoleId = 1");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);

                        string name = string.Empty;
                        if (!dr.IsDBNull("Name"))
                            name = dr.GetString("Name");

                        string about = string.Empty;
                        if (!dr.IsDBNull("About"))
                            about = dr.GetString("About");

                        int age = 0;
                        if (!dr.IsDBNull("Age"))
                            age = dr.GetInt32("Age");

                        bool gender = false;
                        if (!dr.IsDBNull("Gender"))
                            gender = dr.GetBoolean("Gender");

                        byte[] photo = null;
                        if (!dr.IsDBNull(5))
                        {
                            long size = dr.GetBytes(5, 0, null, 0, 0);
                            photo = new byte[size];
                            dr.GetBytes(5, 0, photo, 0, (int)size);
                        }

                        int loginPasswordId = 0;
                        if (!dr.IsDBNull("LoginPasswordId"))
                            loginPasswordId = dr.GetInt32("LoginPasswordId");

                        int roleId = 0;
                        if (!dr.IsDBNull("RoleId"))
                            roleId = dr.GetInt32("RoleId");

                        int contactId = 0;
                        if (!dr.IsDBNull("ContactId"))
                            contactId = dr.GetInt32("ContactId");

                        string city = string.Empty;
                        if (!dr.IsDBNull("City"))
                            city = dr.GetString("City");

                        string login = string.Empty;
                        if (!dr.IsDBNull("Login"))
                            login = dr.GetString("Login");

                        string password = string.Empty;
                        if (!dr.IsDBNull("Password"))
                            password = dr.GetString("Password");

                        string roleName = string.Empty;
                        if (!dr.IsDBNull("RoleName"))
                            roleName = dr.GetString("RoleName");

                        string telegram = string.Empty;
                        if (!dr.IsDBNull("Telegram"))
                            telegram = dr.GetString("Telegram");

                        string vk = string.Empty;
                        if (!dr.IsDBNull("VK"))
                            vk = dr.GetString("VK");

                        LoginPassword loginPassword = new LoginPassword();
                        loginPassword.Login = login;
                        loginPassword.Password = password;

                        Role role = new Role();
                        role.RoleName = roleName;

                        Contact contact = new Contact();
                        contact.Telegram = telegram;
                        contact.VK = vk;


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
                            City = city,
                            LoginPassword = loginPassword,
                            Role = role,
                            Contact = contact
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
            InsertContact(edit.Contact);
            edit.ContactId = edit.Contact.Id;
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `User` set `Name`=@Name, `About`=@About, `Age`=@Age, `Gender`=@Gender, `Photo`=@Photo, `RoleId`=@RoleId, `ContactId`=@ContactId, `City`=@City where `Id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("Name", edit.Name));
                mc.Parameters.Add(new MySqlParameter("About", edit.About));
                mc.Parameters.Add(new MySqlParameter("Age", edit.Age));
                mc.Parameters.Add(new MySqlParameter("Gender", edit.Gender));
                mc.Parameters.Add(new MySqlParameter("Photo", edit.Photo));
                mc.Parameters.Add(new MySqlParameter("RoleId", edit.RoleId));
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
