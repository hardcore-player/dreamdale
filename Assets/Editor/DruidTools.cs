using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using Deal;
using Deal.Env;
using Deal.Data;

namespace Druid
{
    public class DruidTools : MonoBehaviour
    {
        [MenuItem("DruidTools / 删除存档")]
        static void CleanSave()
        {
            PlayerPrefs.DeleteAll();
            SaveLoadManager.DeleteSave("Mary_UserData");
        }

        [MenuItem("DruidTools / 保存地图")]
        static void Clean1Save()
        {
            string assetPath = "Assets/Deal/GameResFirst/Config/MapData.asset";

            SO_MapData soMap = AssetDatabase.LoadAssetAtPath<SO_MapData>(assetPath);
            if (soMap == null)
            {
                soMap = ScriptableObject.CreateInstance<SO_MapData>();
                AssetDatabase.CreateAsset(soMap, assetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            soMap.Guisdes.Clear();

            soMap.Open.Clear();
            seekData("Open", soMap.Open);

            soMap.Ground.Clear();
            seekData("Ground", soMap.Ground);

            soMap.Decoration.Clear();
            seekData("Decoration", soMap.Decoration);

            soMap.Terrain.Clear();
            seekData("Terrain", soMap.Terrain);

            soMap.Fence.Clear();
            seekData("Fence", soMap.Fence);

            soMap.Resoures.Clear();
            seekData("Resoures", soMap.Resoures);

            soMap.Cost.Clear();
            seekCost("Cost", soMap.Cost, soMap);

            soMap.Builds.Clear();
            seekResuild("Resoures", soMap.Builds, soMap);
            seekBuild("Buildings", soMap.Builds, soMap);
            seekBridge("Terrain", soMap.Builds, soMap);

            soMap.SetDirty();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("保存成功");
        }

        /// <summary>
        /// 建筑地坐标x2 ，是为了记录真实坐标，（int）x/2 就似火格子坐标 x/2 就是真实坐标
        /// </summary>
        /// <param name="tilemapName"></param>
        /// <param name="list"></param>

        public static void seekBridge(string tilemapName, List<Data_BuildingBase> list, SO_MapData soMap)
        {
            Transform Cost = GameObject.Find("Grid/" + tilemapName).transform;

            for (int i = 0; i < Cost.childCount; i++)
            {
                Transform child = Cost.GetChild(i);
                Debug.Log(child.name);


                int px = (int)(child.localPosition.x * 2);
                int py = (int)(child.localPosition.y * 2);

                Data_BuildingBase data = new Data_BuildingBase();
                data.Pos = new Data_Point(px, py);

                if (child.name == "Bridge")
                {
                    //int px = (int)(child.localPosition.x - 0.5f);
                    //int py = (int)(child.localPosition.y - 0.5f);

                    data.Size = new Data_Point(1, 3);
                    data.BuildingEnum = BuildingEnum.Bridge;


                    BuildingBase buildingBase = child.GetComponent<BuildingBase>();
                    if (buildingBase)
                    {
                        data.UnlockTask = buildingBase.UnlockTask;
                    }

                    SpaceCost cost = child.GetComponent<SpaceCost>();
                    data.Price = cost.unlockAssets;

                    list.Add(data);

                    recordGuide(soMap, child, data);
                }
                else
                {
                }



            }
        }


        public static void seekBuild(string tilemapName, List<Data_BuildingBase> list, SO_MapData soMap)
        {
            Transform Cost = GameObject.Find("Grid/" + tilemapName).transform;

            for (int i = 0; i < Cost.childCount; i++)
            {
                Transform child = Cost.GetChild(i);
                Debug.Log(child.name);

                Data_BuildingBase data = new Data_BuildingBase();

                SpaceCost cost = child.GetComponent<SpaceCost>();
                data.Price = cost.unlockAssets;
                BuildingBase buildingBase = child.GetComponent<BuildingBase>();
                data.BuildingEnum = buildingBase.buildingEnum;
                data.UnlockTask = buildingBase.UnlockTask;

                Debug.Log("seekBuild UnlockTask" + buildingBase.UnlockTask);

                if (child.name == "Hall")
                {
                    //int px = (int)(child.localPosition.x * 2);
                    //int py = (int)(child.localPosition.y * 2) + 2;
                    //data.Pos = new Data_Point(px, py);

                    //data.Size = new Data_Point(3, 2);
                }
                else if (child.name == "House")
                {
                    int px = (int)(child.localPosition.x * 2);
                    int py = (int)(child.localPosition.y * 2);
                    data.Pos = new Data_Point(px, py);
                    data.Size = new Data_Point(2, 2);
                }

                list.Add(data);

                recordGuide(soMap, child, data);
            }
        }

        public static void seekResuild(string tilemapName, List<Data_BuildingBase> list, SO_MapData soMap)
        {
            Transform Cost = GameObject.Find("Grid/" + tilemapName).transform;

            for (int i = 0; i < Cost.childCount; i++)
            {
                Transform child = Cost.GetChild(i);

                Data_BuildingBase data = null;
                Debug.Log("seekResuild==" + child.name);
                if (child.name == "Pumpkin")
                {
                    //int px = (int)(child.localPosition.x - 1.5f);
                    //int py = (int)(child.localPosition.y - 1.5f);

                    int px = (int)(child.localPosition.x * 2);
                    int py = (int)(child.localPosition.y * 2);

                    SpaceCost cost = child.GetComponent<SpaceCost>();

                    data = new Data_BuildingBase();
                    data.Pos = new Data_Point(px, py);
                    data.Size = new Data_Point(3, 3);
                    data.BuildingEnum = BuildingEnum.Farm;


                    data.Price = cost.unlockAssets;

                    list.Add(data);
                }
                else if (child.name == "AppleTree")
                {
                    int px = (int)(child.localPosition.x * 2);
                    int py = (int)(child.localPosition.y * 2);

                    SpaceCost cost = child.GetComponent<SpaceCost>();


                    data = new Data_BuildingBase();
                    data.Pos = new Data_Point(px, py);
                    data.Size = new Data_Point(1, 1);
                    data.BuildingEnum = BuildingEnum.AppleTree;

                    data.Price = cost.unlockAssets;

                    list.Add(data);
                }
                else if (child.name == "Tree")
                {
                    // 苹果的坐标用的是格子
                    int px = (int)(child.localPosition.x);
                    int py = (int)(child.localPosition.y);

                    SpaceCost cost = child.GetComponent<SpaceCost>();

                    Data_CollectableRes data1 = new Data_CollectableRes();
                    data1.Pos = new Data_Point(px, py);

                    //树记载在格子里了

                    recordGuide(soMap, child, data1);
                }
                else if (child.name == "Stone")
                {
                    int x = (int)(child.localPosition.x);
                    int y = (int)(child.localPosition.y);

                    soMap.Resoures.Add(new MapTilePoint(x, y, "Stone"));
                }
                else
                {
                    int px = (int)(child.localPosition.x * 2);
                    int py = (int)(child.localPosition.y * 2);

                    data = new Data_BuildingBase();
                    data.Pos = new Data_Point(px, py);
                    data.Size = new Data_Point(1, 1);
                }

                if (data != null)
                {
                    BuildingBase buildingBase = child.GetComponent<BuildingBase>();
                    if (buildingBase)
                    {
                        data.UnlockTask = buildingBase.UnlockTask;
                    }

                    recordGuide(soMap, child, data);
                }

            }
        }

        public static void seekCost(string tilemapName, List<Data_SpaceCost> list, SO_MapData soMap)
        {
            Transform Cost = GameObject.Find("Grid/" + tilemapName).transform;

            for (int i = 0; i < Cost.childCount; i++)
            {
                Transform child = Cost.GetChild(i);

                int sx = (int)child.localScale.x;
                int sy = (int)child.localScale.y;

                float px = child.localPosition.x;
                float py = child.localPosition.y;

                //int startX = (int)(px - (sx * 3) / 2.0f);
                //int startY = (int)(py - (sy * 3) / 2.0f);

                int startX = (int)(px * 2);
                int startY = (int)(py * 2);
                int width = sx * 3;
                int height = sy * 3;

                SpaceCost cost = child.GetComponent<SpaceCost>();

                Data_SpaceCost data = new Data_SpaceCost();
                data.Pos = new Data_Point(startX, startY);
                data.Size = new Data_Point(width, height);
                data.Price = cost.unlockAssets;

                list.Add(data);

                BuildingBase buildingBase = child.GetComponent<BuildingBase>();
                if (buildingBase)
                {
                    data.UnlockTask = buildingBase.UnlockTask;
                }


                Guide_Target guide_Target = child.GetComponent<Guide_Target>();
                if (guide_Target != null)
                {
                    int[] ids = guide_Target.GuideIds;

                    for (int j = 0; j < ids.Length; j++)
                    {
                        soMap.Guisdes.Add(new MapTileGuild(ids[j], data.UniqueId()));
                    }
                }
            }
        }

        public static void seekData(string tilemapName, List<MapTilePoint> list)
        {
            Tilemap Ground = GameObject.Find("Grid/" + tilemapName).GetComponent<Tilemap>();

            for (int y = -100; y < 100; y++)
            {
                for (int x = -100; x < 100; x++)
                {
                    TileBase tileBase = Ground.GetTile(new Vector3Int(x, y, 0));
                    if (tileBase)
                    {
                        Debug.Log("tileBase" + tileBase.name);
                        list.Add(new MapTilePoint(x, y, tileBase.name));
                    }

                }
            }
        }

        /// <summary>
        /// 记录引导信息
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="data"></param>
        private static void recordGuide(SO_MapData soMap, Transform gameObject, Data_BuildingBase data)
        {
            Guide_Target guide_Target = gameObject.GetComponent<Guide_Target>();
            if (guide_Target != null)
            {
                int[] ids = guide_Target.GuideIds;

                for (int j = 0; j < ids.Length; j++)
                {
                    MapTileGuild mapTileGuild = new MapTileGuild(ids[j], data.UniqueId());
                    mapTileGuild.Pos2x = data.Pos;
                    soMap.Guisdes.Add(mapTileGuild);
                }
            }
        }


        private static void recordGuide(SO_MapData soMap, Transform gameObject, Data_CollectableRes data)
        {
            Guide_Target guide_Target = gameObject.GetComponent<Guide_Target>();
            if (guide_Target != null)
            {
                int[] ids = guide_Target.GuideIds;

                for (int j = 0; j < ids.Length; j++)
                {
                    MapTileGuild mapTileGuild = new MapTileGuild(ids[j], data.UniqueId());
                    mapTileGuild.Pos2x = data.Pos;
                    soMap.Guisdes.Add(mapTileGuild);
                }
            }
        }



    }

}
