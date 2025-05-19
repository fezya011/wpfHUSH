using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace wpfHUSH.Model
{
    public class SwipeDB
    {
        ConnectionDB connection;

        private SwipeDB(ConnectionDB db)
        {
            this.connection = db;
        }

        public bool InsertSwipe(Swipes swipe)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Swipes` Values (@SwiperId, @SwipedId, @Action, @IsNotificated);");

                cmd.Parameters.Add(new MySqlParameter("SwiperId", swipe.SwiperId));
                cmd.Parameters.Add(new MySqlParameter("SwipedId", swipe.SwipedId));
                cmd.Parameters.Add(new MySqlParameter("Action", swipe.Action));
                cmd.Parameters.Add(new MySqlParameter("IsNotificated", false));

                try
                {
                    cmd.ExecuteScalar();                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        internal List<Swipes> SelectAll()
        {
            List<Swipes> swipes = new List<Swipes>();
            if (connection == null)
                return swipes;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT u.Id AS uId, u1.Id AS u1Id, u.Name AS uName, u.Age AS uAge, u.City AS uCity, u.Photo AS uPhoto, u.Gender AS uGender, u.About AS uAbout, u.ContactId AS uContactId ,SwiperId, SwipedId, IsNotificated, Action FROM Swipes s LEFT JOIN User u ON s.SwiperId = u.Id LEFT JOIN User u1 ON s.SwipedId = u1.Id");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        int uId = 0;
                        if (dr.IsDBNull("uId"))
                            uId = dr.GetInt32("uId");

                        int u1Id = 0;
                        if (dr.IsDBNull("u1Id"))
                            u1Id = dr.GetInt32("u1Id");

                        string uName = string.Empty;
                        if (!dr.IsDBNull("uName"))
                            uName = dr.GetString("uName");

                        int uAge = 0;
                        if (!dr.IsDBNull("uAge"))
                            uAge = dr.GetInt32("uAge");

                        string uCity = string.Empty;
                        if (!dr.IsDBNull("uCity"))
                            uCity = dr.GetString("uCity");

                        byte[] uPhoto = null;
                        if (!dr.IsDBNull(6))
                        {
                            long size = dr.GetBytes(5, 0, null, 0, 0);
                            uPhoto = new byte[size];
                            dr.GetBytes(5, 0, uPhoto, 0, (int)size);
                        }

                        bool uGender = false;
                        if (!dr.IsDBNull("uGender"))
                            uGender = dr.GetBoolean("uGender");
                        
                        string uAbout = string.Empty;
                        if (!dr.IsDBNull("uAbout"))
                            uAbout = dr.GetString("uAbout");

                        int uContactId = 0;
                        if (!dr.IsDBNull("uContactId"))
                            uContactId = dr.GetInt32("uContactId");

                        int swiperId = 0;
                        if (!dr.IsDBNull("SwiperId"))
                            swiperId = dr.GetInt32("SwiperId");

                        int swipedId = 0;
                        if (!dr.IsDBNull("SwipedId"))
                            swipedId = dr.GetInt32("SwipedId");

                        bool isNotificated = false;
                        if (!dr.IsDBNull("IsNotificated"))
                            isNotificated = dr.GetBoolean("IsNotificated");

                        bool action = false;
                        if (!dr.IsDBNull("Action"))
                            action = dr.GetBoolean("Action");
                      
                        User swiped = new User();
                        swiped.Id = u1Id;

                        User swiper = new User();
                        swiper.Id = uId;
                        swiper.Name = uName;
                        swiper.Age = uAge;
                        swiper.Gender = uGender;
                        swiper.City = uCity;
                        swiper.Photo = uPhoto;
                        swiper.About = uAbout;
                        swiper.ContactId = uContactId;

                        swipes.Add(new Swipes
                        {
                            Action = action,
                            SwiperId = swiperId,
                            SwipedId = swipedId,
                            IsNotificated = isNotificated,
                            Swiped = swiped,
                            Swiper = swiper,
                        });


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return swipes;
        }

        internal bool Update(Swipes swipes)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Swipes` set `SwiperId`=@SwiperId, `SwipedId`=@SwipedId, `Action`=@Action, `IsNotificated`=@IsNotificated where `SwiperId` = {swipes.SwiperId} and `SwipedId` = {swipes.SwipedId}");
                mc.Parameters.Add(new MySqlParameter("SwiperId", swipes.SwiperId));
                mc.Parameters.Add(new MySqlParameter("SwipedId", swipes.SwipedId));
                mc.Parameters.Add(new MySqlParameter("Action", swipes.Action));
                mc.Parameters.Add(new MySqlParameter("IsNotificated", swipes.IsNotificated));
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

        internal bool Remove(Swipes remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Swipes` where `SwiperId` = {remove.SwiperId} and `SwipedId` = {remove.SwipedId}");
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

        static SwipeDB db;
        public static SwipeDB GetDb()
        {
            if (db == null)
                db = new SwipeDB(ConnectionDB.GetDbConnection());
            return db;
        }
    }
}
