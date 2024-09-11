using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deal.Data
{
    [Serializable]
    public class Data_Point
    {
        public int x;
        public int y;

        public Data_Point()
        {
        }

        public Data_Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            Data_Point p = obj as Data_Point;
            return this.x == p.x && this.y == p.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.x, this.y);
            //return this.x + this.y;
        }

        public override string ToString()
        {
            return $"{this.x}_{this.y}";
        }

        public static Data_Point ToObject(string str)
        {
            string[] arr = str.Split('_');
            return new Data_Point(Int32.Parse(arr[0]), Int32.Parse(arr[1]));
        }
    }

}
