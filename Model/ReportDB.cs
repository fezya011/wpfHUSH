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
    public class ReportDB
    {
        ConnectionDB connection;

        private ReportDB(ConnectionDB db)
        {
            this.connection = db;
        }

        public bool InsertReport(Report report)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Report` Values (0, @Text, @ReasonId, @ReportedId, CURRENT_TIMESTAMP, @ReporterId);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Text", report.Text));
                cmd.Parameters.Add(new MySqlParameter("ReasonId", report.ReasonId));
                cmd.Parameters.Add(new MySqlParameter("ReportedId", report.ReportedId));
                cmd.Parameters.Add(new MySqlParameter("ReporterId", report.ReporterId));


                try
                {

                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        report.Id = id;
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
        internal List<Reason> SelectAll()
        {
            List<Reason> reasons = new List<Reason>();
            if (connection == null)
                return reasons;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT Id, Type FROM Reason");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);

                        string type = string.Empty;
                        if (!dr.IsDBNull("Type"))
                            type = dr.GetString("Type");

                        reasons.Add(new Reason
                        {
                            Id = id,
                            Type = type
                        });

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return reasons;
        }


        static ReportDB db;
        public static ReportDB GetDb()
        {
            if (db == null)
                db = new ReportDB(ConnectionDB.GetDbConnection());
            return db;
        }
    }
}
