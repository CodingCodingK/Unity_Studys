using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DBHelper
{
	/// <summary>
	/// Mapper、Reflection相关共通方法抽出
	/// </summary>
	public static class DBMapperHelper
	{
		/// <summary>
		/// Query操作共通抽出
		/// </summary>
		public static List<T> Query<T>(this MySqlCommand cmd) where T : new()
		{
			MySqlDataReader reader = cmd.ExecuteReader();

			var backupList = new List<T>();
			// 检测是否有数据
			while (reader.Read())
			{
				var dto = new T();

				foreach (var propertyInfo in dto.GetType().GetProperties())
				{
					// 数据Mapper
					if (propertyInfo.PropertyType == typeof(int))
					{
						propertyInfo.SetValue(dto, reader.GetInt32(propertyInfo.Name.ToLower()));
					}
					else if (propertyInfo.PropertyType == typeof(string))
					{
						propertyInfo.SetValue(dto, reader.GetString(propertyInfo.Name.ToLower()));

					}
					else if (propertyInfo.PropertyType == typeof(float))
					{
						propertyInfo.SetValue(dto, reader.GetFloat(propertyInfo.Name.ToLower()));
					}
					else if (propertyInfo.PropertyType == typeof(decimal))
					{
						propertyInfo.SetValue(dto, reader.GetDecimal(propertyInfo.Name.ToLower()));
					}

				}

				backupList.Add(dto);
			}

			// 最后要关闭！！！
			reader.Close();
			return backupList;
		}

		/// <summary>
		/// 将数据全部Mapper到cmd的Parameter中
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="cmd"></param>
		/// <param name=""></param>
		/// <returns></returns>
		public static void SetAllParameters<T>(this MySqlCommand cmd,T dto)
		{
			foreach (var propertyInfo in dto.GetType().GetProperties())
			{
				// 数据Mapper
				cmd.Parameters.AddWithValue(propertyInfo.Name.ToLower(), propertyInfo.GetValue(dto));
			}
		}

	}
}
