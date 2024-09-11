using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Deal.Data;
using Druid;
using UnityEngine;

namespace Deal
{
    public class MathUtils
    {
        /// <summary>
        /// 格式化数字
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ToKBM(long num)
        {
            string[] symbol = { "", "K", "M", "B", "T", "aa", "ab", "ac", "ad" };

            float sNum = num;
            int symbolId = 0;

            while (sNum > 1000)
            {
                sNum /= 1000;
                symbolId++;
            }


            string bkmNum = Math.Floor(sNum * 10) / 10 + symbol[symbolId];

            //Debug.Log("ToKBM num" + num + " bkmNum " + bkmNum);

            return bkmNum;

        }

        public static Vector3 GetRect8Point()
        {
            int r = Druid.Utils.MathUtils.RandomInt(0, 8);
            if (r >= 4)
            {
                r++;
            }

            int x = r % 3 - 1;
            int y = r / 3 - 1;

            return new Vector3(x, y, 0);
        }

        public static Vector3 GetCirclePoint(float r)
        {
            double x = Druid.Utils.MathUtils.RandomDouble(-1, 1);
            double y = Druid.Utils.MathUtils.RandomDouble(-1, 1);
            return new Vector3((float)x, (float)y, 0).normalized * r;
        }

        public static Vector3 GetBottomHalfCirclePoint(float r)
        {
            double x = Druid.Utils.MathUtils.RandomDouble(-1, 1);
            double y = Druid.Utils.MathUtils.RandomDouble(-1, 0);
            return new Vector3((float)x, (float)y, 0).normalized * r;
        }

        public static Vector2 Bezier(float t, Vector2 a, Vector2 b, Vector2 c)
        {
            var ab = Vector2.Lerp(a, b, t);
            var bc = Vector2.Lerp(b, c, t);
            return Vector2.Lerp(ab, bc, t);
        }

        /// <summary>
        /// 装备基础属性计算公式
        /// </summary>
        /// <param name="origin">配置表读取原始数据</param>
        /// <param name="quality">品质白绿蓝紫橙12345</param>
        /// <param name="level">等级</param>
        public static float GetEquipBaseValue(float origin, int quality, int level)
        {
            float[] tmp = { 0.3f, 0.2f, 0.17f, 0.15f, 0.12f };  //  白，绿，蓝，紫，橙升级系数
            return origin * quality * (1 + tmp[quality - 1] * (level - 1));
        }

        /// <summary>
        /// 角色的基础属性
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public static BattleRoleAtt GetHeroAttByLv(int lv)
        {
            BattleRoleAtt att = new BattleRoleAtt();

            att.MaxHP = 130 + 30 * (lv - 1);
            att.HP = att.MaxHP;
            att.Attack = 4 + 4 * (lv - 1);
            att.Crit = 5;
            att.DeCrit = 0;
            att.Hit = 0;
            att.Dodge = 0;
            att.HPReg = 0;

            return att;
        }

        /// <summary>
        /// 装备属性
        /// </summary>
        /// <param name="equip"></param>
        /// <returns></returns>
        public static BattleRoleAtt GetEquipAtt(Data_Equip equip)
        {
            if (equip == null) return null;

            BattleRoleAtt att = new BattleRoleAtt();

            ExcelData.Equip equipCfg = ConfigManger.I.GetEquipCfg(equip.equipId);

            float hp = MathUtils.GetEquipBaseValue(equipCfg.hp, equip.equipQuality, equip.equipLv);
            float atk = MathUtils.GetEquipBaseValue(equipCfg.atk, equip.equipQuality, equip.equipLv);
            float cri = equipCfg.cri;
            float decri = equipCfg.decri;
            float hit = equipCfg.hit;
            float dodge = equipCfg.dodge;
            float reg = equipCfg.reg;

            att.MaxHP = hp;
            att.HP = hp;
            att.Attack = atk;
            att.Crit = cri;
            att.DeCrit = decri;
            att.Hit = hit;
            att.Dodge = dodge;
            att.HPReg = reg;

            return att;
        }

        /// <summary>
        /// 雕塑加成
        /// </summary>
        /// <param name="equip"></param>
        /// <returns></returns>
        public static float GetStatueBuff(StatueEnum statueEnum)
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            int xlv = userData.GetStatueLv(statueEnum);
            if (!userData.HasStatueBulePrint(statueEnum))
            {
                // 没有
                return 0;
            }

            int lv = xlv / 100;
            int explv = xlv % 100;


            float buffVal = 0f;

            if (lv == 15 && explv == 10)
            {
                buffVal = (2 + 3 + 4 + 5 + 6) * 2f / 100f;
            }
            else if (lv > 12)
            {
                buffVal = (2 + 3 + 4 + 5 + 6) / 100f;
            }
            else if (lv > 9)
            {
                buffVal = (2 + 3 + 4 + 5) / 100f;
            }
            else if (lv > 6)
            {
                buffVal = (2 + 3 + 4) / 100f;
            }
            else if (lv > 3)
            {
                buffVal = (2 + 3) / 100f;
            }
            else
            {
                buffVal = 2f / 100f;
            }


            return buffVal;
        }

        /// <summary>
        /// 雕塑提供的总生命值
        /// </summary>
        /// <param name="lv">大等级</param>
        /// <param name="step">小等级</param>
        /// <returns>血量</returns>
        public static int GetStatueHp(int lv, int step)
        {
            int hp = 0;
            int[] arr = { 0, 500, 500, 500, 1000, 1000, 1000, 1500, 1500, 1500, 2000, 2000, 2000, 2500, 2500, 2500 };
            for (int i = 0; i < lv; i++)
            {
                hp += arr[i];
            }
            double c = System.Math.Ceiling((float)lv / 3) * 100 * System.Math.Floor((float)step / 2);
            hp += (int)c;
            return hp;
        }

        /// <summary>
        /// 雕塑提供的总攻击力
        /// </summary>
        /// <param name="lv">大等级</param>
        /// <param name="step">小等级</param>
        /// <returns>攻击力</returns>
        public static int GetStatueAtk(int lv, int step)
        {
            int atk = 0;
            int[] arr = { 0, 100, 100, 100, 200, 200, 200, 300, 300, 300, 400, 400, 400, 500, 500, 500 };
            for (int i = 0; i < lv; i++)
            {
                atk += arr[i];
            }
            double c = System.Math.Ceiling((float)lv / 3) * 20 * System.Math.Ceiling((float)step / 2);
            atk += (int)c;
            return atk;
        }

        /// <summary>
        /// 大厅能力值
        /// </summary>
        /// <param name="lv"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static float GetHallabilityValue(HallAbilityEnum hallAbility, int cLv, int baseVal, float upVal)
        {
            if (hallAbility == HallAbilityEnum.BreadFactoryLevel)
            {
                if (cLv < 12)
                {
                    return baseVal + upVal * cLv;
                }
                else if (cLv < 22)
                {
                    return baseVal + upVal * 12 + 3 * (cLv - 12);
                }
                else
                {
                    return baseVal + upVal * 12 + 3 * 10 + 5 * (cLv - 11);
                }
            }
            else
            {
                return baseVal + upVal * cLv;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipLv"></param>
        /// <returns></returns>
        public static int GetRunePrice(int equipLv)
        {
            if (equipLv == 1)
            {
                return 1;
            }
            else if (equipLv == 2)
            {
                return 2;
            }
            else if (equipLv == 3)
            {
                return 3;
            }
            else if (equipLv == 4)
            {
                return 6;
            }
            else if (equipLv == 5)
            {
                return 8;
            }
            else
            {
                return 10;
            }
        }


        /// <summary>
        /// 获取战力
        /// </summary>
        /// <param name="att"></param>
        /// <returns></returns>
        public static int GetCombat(BattleRoleAtt att)
        {
            float combat = att.Attack / att.AttackSpeed * (1 + att.Hit) * (1 + att.HPReg) + (1 + att.Crit) * att.HP * (1 + att.Dodge) * (1 + att.DeCrit);
            return (int)combat;
        }

    }

}
