using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Druid.Utils
{
    public class MathUtils
    {
        public static System.Random r = new System.Random();


        /// <summary>
        /// 获取一个随机整数【min,max）
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int RandomInt(int min, int max)
        {
            return r.Next(min, max);
        }

        public static double RandomDouble(double min, double max)
        {
            return r.NextDouble() * (max - min) + min;
        }


        /// <summary>
        /// 注视方法1
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void LookAt2D(Transform source, Transform target)
        {
            Vector3 v = (target.position - source.transform.position).normalized;
            source.transform.right = v;
        }

        /// <summary>
        /// 注视方法2
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void LookAt2DMath(Transform source, Transform target)
        {
            Vector2 direction = target.transform.position - source.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            source.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}