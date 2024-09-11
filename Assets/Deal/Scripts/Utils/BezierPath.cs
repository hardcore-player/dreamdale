using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deal
{
    public class BezierPath
    {
        public float percent = 0;
        public float percentSpeed = 1.5f;
        public Transform targetObj;
        public Vector2 startPos;
        public Vector2 endPos;
        private Vector2 _midPos;
        internal Action cb;
        public bool alive = false;

        // Start is called before the first frame update
        public void Init()
        {
            alive = true;
            _midPos = _getMiddlePosition(startPos, endPos);
        }

        // Update is called once per frame
        public void Update()
        {
            if (!alive) { return; }
            percent += percentSpeed * Time.deltaTime;
            if (percent > 1)
                percent = 1;

            targetObj.position = MathUtils.Bezier(percent, startPos, _midPos, endPos);
            if (percent == 1)
            {
                alive = false;
                cb();
            }
        }

        /// <summary>
        /// 贝塞尔曲线中间控制点
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private Vector2 _getMiddlePosition(Vector2 a, Vector2 b)
        {
            Vector2 m = Vector2.Lerp(a, b, 0.5f);    // ab向量上的中间点
            Vector2 normal = Vector2.Perpendicular(a - b).normalized;   // ab 垂线方向
            if (normal.y < 0)
            {
                normal.y *= -1;
                normal.x *= -1;
            }
            Vector2 dir = b - a;    // ab方向向量
            float angle = Vector2.Angle(dir, new Vector2(0, dir.y / Math.Abs(dir.y)));   // ab向量与y轴的夹角
            float curveRatio = 1.0f;     // 控制点的距离m点的距离最长为50%ab长度

            return m + dir.magnitude * curveRatio * (angle / 90) * normal;
        }
    }

}
