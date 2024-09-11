using System;
using UnityEngine;

namespace Framework.Math
{
    /// <summary>
    /// 底数和指数的大数字，底数最高包含2位整数和小数包含四位小数
    /// </summary>
    public class BigNumber
    {
        public static string[] bigNumberLabel = new[] {"", "K", "M", "G", "T"};

        //精度
        public static int degree = 10000;


        //底数
        private int baseNum = 0;

        //指数
        private int expNum = 0;

        public int BaseNum
        {
            get => baseNum;
            set => baseNum = value;
        }

        public int ExpNum
        {
            get => expNum;
            set => expNum = value;
        }

        public BigNumber()
        {
        }

        public BigNumber Clone()
        {
            return new BigNumber(BaseNum * 1.0f / degree, ExpNum);
        }

        /// <summary>
        /// 1.2345 = 12345  12.3= 123000
        /// </summary>
        /// <param name="b"></param>
        /// <param name="e"></param>
        public BigNumber(float b, int e)
        {
            if (b >= 1000)
            {
                Debug.LogError("整数部分不能超过1000 b=" + b);
                return;
            }

            baseNum = (int) (b * degree);
            expNum = e;
        }


        public static BigNumber operator +(BigNumber b_, BigNumber c_)
        {
            BigNumber result = new BigNumber();
            BigNumber b = b_.Clone();
            BigNumber c = c_.Clone();

            if (b.ExpNum - c.ExpNum >= 2)
            {
                result.BaseNum = b.BaseNum;
                result.ExpNum = b.ExpNum;
            }
            else if (c.ExpNum - b.ExpNum >= 2)
            {
                result.BaseNum = c.BaseNum;
                result.ExpNum = c.ExpNum;
            }
            else
            {
                BigNumber big = b;
                BigNumber small = c;

                if (b.ExpNum < c.ExpNum)
                {
                    big = c;
                    small = b;
                }

                int bigBaseNum = big.BaseNum + (int) (small.BaseNum / Mathf.Pow(1000, big.ExpNum - small.ExpNum));
                int bigExpNum = big.ExpNum;

                // if (bigBaseNum >= 1000)
                // {
                //     bigBaseNum = bigBaseNum / 1000;
                //     bigExpNum++;
                // }

                result.BaseNum = bigBaseNum;
                result.ExpNum = bigExpNum;
                result.CheckBig();
            }

            return result;
        }


        /// <summary>
        /// 转化成指定指数
        /// </summary>
        /// <param name="toExp"></param>
        public void Parse2Exp(int toExp)
        {
            BaseNum = (int) (BaseNum / Mathf.Pow(1000, toExp - ExpNum));
            ExpNum = toExp;
        }

        public static BigNumber operator -(BigNumber b_, BigNumber c_)
        {
            BigNumber result = new BigNumber();
            BigNumber b = b_.Clone();
            BigNumber c = c_.Clone();

            if (b.ExpNum - c.ExpNum >= 2)
            {
                result.BaseNum = b.BaseNum;
                result.ExpNum = b.ExpNum;
            }
            else if (c.ExpNum - b.ExpNum >= 2)
            {
                result.BaseNum = -c.BaseNum;
                result.ExpNum = c.ExpNum;
            }
            else
            {
                int bigExp = b.ExpNum > c.ExpNum ? b.ExpNum : c.ExpNum;
                b.Parse2Exp(bigExp);
                c.Parse2Exp(bigExp);

                result.BaseNum = b.BaseNum - c.BaseNum;
                result.ExpNum = b.ExpNum;
                result.ValideBaseNum();
            }

            return result;
        }


        /// <summary>
        /// 就这个除法只是比较b是c的百分数
        /// </summary>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float operator /(BigNumber b, BigNumber c)
        {
            if (b.ExpNum - c.ExpNum >= 2)
            {
                return 100f;
            }

            if (c.ExpNum - b.ExpNum >= 2)
            {
                return 0f;
            }

            int bBaseNumAsCExp = (int) (b.BaseNum / Mathf.Pow(1000, c.ExpNum - b.ExpNum));

            return bBaseNumAsCExp * 1.0f / c.BaseNum;
        }

        public static BigNumber operator *(BigNumber b, float c)
        {
            BigNumber result = new BigNumber();

            int bNum = b.BaseNum;
            int eNum = b.ExpNum;


            bNum = (int) (c * bNum);

            while (bNum >= 1000 * degree)
            {
                bNum = bNum / 1000;
                eNum++;
            }

            result.BaseNum = bNum;
            result.ExpNum = eNum;

            return result;
        }


        public static BigNumber operator /(BigNumber b, float c)
        {
            BigNumber result = new BigNumber();

            int bNum = b.BaseNum;
            int eNum = b.ExpNum;


            bNum = (int) (bNum / c);

            while (bNum < 1 * degree && eNum > 0)
            {
                bNum = bNum * 1000;
                eNum--;
            }

            result.BaseNum = bNum;
            result.ExpNum = eNum;

            return result;
        }


        public static bool operator >(BigNumber b, BigNumber c)
        {
            if (b.ExpNum > c.ExpNum)
            {
                return true;
            }
            else if (b.ExpNum == c.ExpNum && b.BaseNum > c.BaseNum)
            {
                return true;
            }

            return false;
        }

        public static bool operator <(BigNumber b, BigNumber c)
        {
            if (b.ExpNum < c.ExpNum)
            {
                return true;
            }
            else if (b.ExpNum == c.ExpNum && b.BaseNum < c.BaseNum)
            {
                return true;
            }

            return false;
        }

        public static bool operator >=(BigNumber b, BigNumber c)
        {
            if (b.ExpNum > c.ExpNum)
            {
                return true;
            }
            else if (b.ExpNum == c.ExpNum && b.BaseNum >= c.BaseNum)
            {
                return true;
            }

            return false;
        }

        public static bool operator <=(BigNumber b, BigNumber c)
        {
            if (b.ExpNum < c.ExpNum)
            {
                return true;
            }
            else if (b.ExpNum == c.ExpNum && b.BaseNum <= c.BaseNum)
            {
                return true;
            }

            return false;
        }

        private void CheckBig()
        {
            if (BaseNum < 1000 * degree)
            {
                return;
            }

            BaseNum = BaseNum / 1000;
            ExpNum++;
            CheckBig();
        }

        /// <summary>
        /// 校验整数部分在1-1000之间
        /// </summary>
        private void ValideBaseNum()
        {
            if (System.Math.Abs(BaseNum) >= 1 * degree)
            {
                return;
            }

            if (ExpNum == 0)
            {
                return;
            }

            BaseNum = BaseNum * 1000;
            ExpNum--;
            ValideBaseNum();
        }

        public override string ToString()
        {
            // return String.Format("({0}, {1}, {2})", length, breadth, height);
            return String.Format("{0}{1}", BaseNum * 1f / degree, bigNumberLabel[ExpNum]);
        }

        public string ToString(int format)
        {
            return String.Format("{0}{1}", System.Math.Round(BaseNum * 1f / degree, format), bigNumberLabel[ExpNum]);
        }
    }
}