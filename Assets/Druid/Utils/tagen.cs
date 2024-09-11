using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Druid.Utils
{
    /// <summary>
    /// ag编号生成器,用来给所有用户显示面板来自动生成Tag
    /// </summary>
    public class tagen
    {
        public static int counter = 10000;

        public static int get()
        {
            return counter++;
        }
    }
}