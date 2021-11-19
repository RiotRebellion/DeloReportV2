using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;

namespace Infrastructure
{
	public class Connection
	{
		public static (SqlConnection, string) Connect(string connectionString)
		{
			SqlConnection connection = new SqlConnection(connectionString);
			try
            {
                connection.OpenAsync();
				if (connection.State == ConnectionState.Open) return (connection, "Подключено");
				return (connection, "Нет подключения");

			}
            catch (Exception ex)
            {
				MessageBox.Show(ex.Message, "Некорректные настройки подключения");
				return(connection, "Нет подключения");
			}
		}
	}
}