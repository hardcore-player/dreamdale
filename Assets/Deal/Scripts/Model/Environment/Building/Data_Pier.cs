using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;
using UnityEngine.Tilemaps;

namespace Deal.Data
{

    /// <summary>
    /// 工坊
    /// </summary>
    [Serializable]
    public class Data_Pier : Data_BuildingBase
    {

        public override void Load()
        {

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                int x = this.Pos.x / 2;
                int y = this.Pos.y / 2;

                PrefabsUtils.NewPier(this, mapRender.Builds.transform, this.WorldPos, (obj) =>
                {
                    for (int i = 0; i < 3; i++)
                    {
                        mapRender.Ground.SetColliderType(new Vector3Int(x - 1 + i, y, 0), Tile.ColliderType.None);
                    }
                });
            }
            else if (this.StateEnum == BuildingStateEnum.Building)
            {
                PrefabsUtils.NewDefaultUnlocked(this, mapRender.Builds.transform, this.WorldPos);
            }
            else
            {
                PrefabsUtils.NewDefaultNone(this, mapRender.Builds.transform, this.WorldPos);
            }
        }
    }
}