using System;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Env;
using Deal.Data;
using DG.Tweening;

namespace Deal.UI
{
    /// <summary>
    /// 传送点地图
    /// </summary>
    public class UITeleportMap : UIBase
    {
        public List<UITeleportButton> buttons = new List<UITeleportButton>();

        public override void OnInit(UIParamStruct param)
        {
            // 所有传送点的坐标，最后一个是自己
            List<Data_BuildingBase> otherTeleport = param.param as List<Data_BuildingBase>;

            foreach (UITeleportButton item in this.buttons)
            {
                //item.gameObject.SetActive(false);

                for (int i = 0; i < otherTeleport.Count; i++)
                {
                    //Debug.Log("otherTeleport[i].UniqueId()" + otherTeleport[i].UniqueId());
                    if (otherTeleport[i].UniqueId() == item.UniqueId)
                    {
                        Debug.Log("otherTeleport[i].UniqueId() 有" + otherTeleport[i].UniqueId());

                        // 有
                        item.gameObject.SetActive(true);
                        item.SetData(otherTeleport[i]);
                    }
                }
            }

            Data_BuildingBase myPos = otherTeleport[otherTeleport.Count - 1];

            foreach (UITeleportButton item in this.buttons)
            {
                //item.gameObject.SetActive(false);

                if (myPos.UniqueId() == item.UniqueId)
                {
                    // 自己
                    item.gameObject.SetActive(true);
                    item.SetMyPos();
                }
            }

        }

        public override void OnUIAwake()
        {
        }

    }
}


