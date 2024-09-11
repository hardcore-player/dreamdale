using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Druid
{
    public class DataManager : SingletonClass<DataManager>
    {

        private Dictionary<string, DataTable> datas = new Dictionary<string, DataTable>();

        public override void Destroy()
        {
            this.datas.Clear();
        }

        public void Register(DataTable data)
        {
            string key = data.TableName;
            if (!datas.ContainsKey(key))
            {
                datas.Add(key, data);
            }
        }

        public T Get<T>(string tableName) where T : DataTable
        {
            if (datas.ContainsKey(tableName))
            {
                return datas[tableName] as T;
            }

            return null;
        }

        public void Save(string tableName)
        {
            if (datas.ContainsKey(tableName))
            {
                datas[tableName].Save();
            }

        }
    }
}