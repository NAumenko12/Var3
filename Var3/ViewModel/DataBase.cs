using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Var3.Model;

namespace Var3.ViewModel
{
    public class DataBase : DbContext
    {
        public DbSet<Товар> Товары { get; set; }
        public DbSet<Заказ> Заказ { get; set; }
       // public DbSet<Пункт_выдачи> Пункт_выдачи { get; set; }

        public string connectionString = @"Data Source=EUGENE; DataBase=Var3; Integrated Security=True; Trusted_Connection=true; MultipleActiveResultSets=true; TrustServerCertificate=true; encrypt=false;";
        private SqlConnection connection;

        public DataTable SqlSelect(string cmd)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(cmd, conn);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    reader.Close();
                    return dt;
                }
            }
        }

        public DataTable SqlInsert(string cmd)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand command = new SqlCommand(cmd, conn);
            command.ExecuteNonQuery();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        // Изменить поля
        public bool AddProduct(int ID, string Name, string Opisanie, int? Price)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Товары (id_Товара, Название, Описание, Цена) VALUES (@ID, @Name, @Opisanie, @Price)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@Name", string.IsNullOrEmpty(Name) ? DBNull.Value : (object)Name);
                        cmd.Parameters.AddWithValue("@Opisanie", string.IsNullOrEmpty(Opisanie) ? DBNull.Value : (object)Opisanie);
                        cmd.Parameters.AddWithValue("@Price", Price ?? 0);

                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                // В случае ошибки возвращаем false
                return false;
            }
        }

        public void DeleteProduct(int productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Товары WHERE id_Товара = @ID";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", productId);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Метод для открытия соединения с БД
        /// </summary>
        public void OpenConnection()
        {
            // Если состояние строки закрыто, открываем
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        /// <summary>
        /// Метод для закрытия соединения с БД
        /// </summary>
        public void CloseConnection()
        {
            // Если состояние строки открыто, закрываем
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Метод для возвращения строки подключения
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetConnection()
        {
            return connection;
        }
    }
}
