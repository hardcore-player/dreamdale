using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using UnityEngine;

namespace Deal.Env
{
    /// <summary>
    /// 未解锁的南瓜地
    /// </summary>
    public class Building_PumpkinLandUnlocked : BuildingUnclockWithAsset
    {
        public override void Unclock()
        {
            base.Unclock();

            if (this.Data == null)
            {
                return;
            }

            // 

            Data_BuildingBase _Data = this.GetData<Data_BuildingBase>();

            // 开放新土地
            //MapManager.I.mapRender.OpenTiles(Data.Pos.x, Data.Pos.y, Data.Size.x, Data.Size.y);

            this.gameObject.SetActive(false);

            _Data.StateEnum = Deal.Data.BuildingStateEnum.Open;
            _Data.Load();
        }
    }

}
