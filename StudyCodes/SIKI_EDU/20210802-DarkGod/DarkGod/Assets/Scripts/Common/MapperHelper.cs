using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace MyHelper
{
    public class MapperHelper
    {
        /*
        /// <summary>
        /// Xml Mapper操作共通抽出
        /// </summary>
        public static List<T> MapperXmlNodeList<T>(XmlNodeList nodList) where T : BaseData<T>,new()
        {
            var backupList = new List<T>();
            
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                var eleID = ele.GetAttributeNode("ID");
                if (eleID == null)
                {
                    continue;
                }

                int id = Convert.ToInt32(eleID.InnerText);
                T dto = new T();
                
                foreach (var propertyInfo in dto.GetType().GetProperties())
                {
                    // 数据Mapper
                    if (propertyInfo.PropertyType == typeof(int))
                    {
                        nodList[i].ChildNodes.
                        propertyInfo.SetValue(dto, );
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
                
                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "surname":
                            surnameList.Add(e.InnerText);
                            break;
                        case "man":
                            manList.Add(e.InnerText);
                            break;
                        case "woman":
                            womanList.Add(e.InnerText);
                            break;
                    }
                }
                map.ID = id;
            }
            
            
            
            
            
            
            foreach (var item in list)
            {
                
            }


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

        private void test(XmlNodeList nodList)
        {
            foreach (XmlElement e in nodList)
            {
                switch (e.Name)
                {
                    case "surname":
                        surnameList.Add(e.InnerText);
                        break;
                    case "man":
                        manList.Add(e.InnerText);
                        break;
                    case "woman":
                        womanList.Add(e.InnerText);
                        break;
                }
            }
        }
        */

        public static Vector3 ConvertToVector3(string str,char separator = ',')
        {
            var vectorList = str.Split(separator);
            if (vectorList.Length >= 3)
            {
                return new Vector3(float.Parse(vectorList[0]), float.Parse(vectorList[1]),float.Parse(vectorList[2]));
            }

            return Vector3.zero;
        }
        
    }
}


   