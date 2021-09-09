using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Mysql_Test
{
    class Program
    {
        static MySqlConnection conn = null;
        static void Main(string[] args)
        {
            // init
            conn = new MySqlConnection("server=localhost;User Id = root;password=;Database=test;Charset = utf8");
            conn.Open();

            // do
            Add();
            Delete();
            Update();
            Query();

            Console.ReadKey();

            // destroy
            conn.Close();
        }

        static void Add()
        {
            // 准备sql并执行
            MySqlCommand cmd = new MySqlCommand("insert into userinfo set name = 'haha'", conn);
            cmd.ExecuteNonQuery();
            // 获取返回值
            int id = (int) cmd.LastInsertedId;
            Console.WriteLine("Sql Insert Key:{0}.", id);
        }

        static void Delete()
        {
            MySqlCommand cmd = new MySqlCommand("delete from userinfo where id >= @id", conn);
            cmd.Parameters.AddWithValue("id", 4);
            cmd.ExecuteNonQuery();
        }

        static void Update()
        {
            // 防止sql注入的sql参数写法
            MySqlCommand cmd = new MySqlCommand("update userinfo set name = @name where id = @id", conn);
            cmd.Parameters.AddWithValue("name", "Json");
            cmd.Parameters.AddWithValue("id", 1);
            cmd.ExecuteNonQuery();

        }


        static void Query()
        {
            // 准备sql并获取返回值
            MySqlCommand cmd = new MySqlCommand("select * from userinfo where name = 'haha'", conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            // 检测是否有数据
            var backupList = new List<UserInfoDto>();
            while (reader.Read())
            {
                int id = reader.GetInt32("id");
                string name = reader.GetString("name");
                backupList.Add(new UserInfoDto {Id = id, Name = name});
            }

            foreach (var item in backupList)
            {
                Console.WriteLine($"UserInfo -> Id:{item.Id},Name:{item.Name}");
            }  // 最后要关闭！！！
            reader.Close();

        }

        public class UserInfoDto
        {
            public int Id;
            public string Name;
        }
    }
}
