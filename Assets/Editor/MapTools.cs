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
    public class MapTools : MonoBehaviour
    {
        [MenuItem("DruidTools / 保存地图 New")]
        static void CleanSave()
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


            Tilemap Open = GameObject.Find("Grid/Open").GetComponent<Tilemap>();
            Tilemap Terrain = GameObject.Find("Grid/Terrain").GetComponent<Tilemap>();
            Tilemap Ground = GameObject.Find("Grid/Ground").GetComponent<Tilemap>();
            Tilemap Decoration = GameObject.Find("Grid/Decoration").GetComponent<Tilemap>();
            Tilemap SeaShadow = GameObject.Find("Grid/SeaShadow").GetComponent<Tilemap>();
            Tilemap Resoures = GameObject.Find("Grid/Resoures").GetComponent<Tilemap>();
            Tilemap Fence = GameObject.Find("Grid/Fence").GetComponent<Tilemap>();
            Tilemap Interactive = GameObject.Find("Grid/Interactive").GetComponent<Tilemap>();

            soMap.Open.Clear();
            soMap.Terrain.Clear();
            soMap.Ground.Clear();
            soMap.SeaShadow.Clear();
            soMap.Decoration.Clear();
            soMap.Interactive.Clear();
            soMap.Decoration1.Clear();
            soMap.Resoures.Clear();
            soMap.Fence.Clear();

            soMap.Cost.Clear();
            soMap.Builds.Clear();
            soMap.Guisdes.Clear();

            // tiles 
            for (int y = -100; y < 100; y++)
            {
                for (int x = -100; x < 100; x++)
                {
                    TileBase tileBase0 = Open.GetTile(new Vector3Int(x, y, 0));
                    if (tileBase0)
                    {
                        soMap.Open.Add(new MapTilePoint(x, y, tileBase0.name));
                    }


                    TileBase tileBase1 = Terrain.GetTile(new Vector3Int(x, y, 0));
                    if (tileBase1)
                    {
                        soMap.Terrain.Add(new MapTilePoint(x, y, tileBase1.name));
                    }

                    TileBase tileBase2 = Ground.GetTile(new Vector3Int(x, y, 0));
                    if (tileBase2)
                    {
                        soMap.Ground.Add(new MapTilePoint(x, y, tileBase2.name));
                    }

                    TileBase tileBase3 = Decoration.GetTile(new Vector3Int(x, y, 0));
                    if (tileBase3)
                    {
                        soMap.Decoration.Add(new MapTilePoint(x, y, tileBase3.name));
                    }
                    TileBase tileBase4 = Resoures.GetTile(new Vector3Int(x, y, 0));
                    if (tileBase4)
                    {
                        soMap.Resoures.Add(new MapTilePoint(x, y, tileBase4.name));
                    }
                    TileBase tileBase5 = Fence.GetTile(new Vector3Int(x, y, 0));
                    if (tileBase5)
                    {
                        soMap.Fence.Add(new MapTilePoint(x, y, tileBase5.name));
                    }

                    TileBase tileBase6 = SeaShadow.GetTile(new Vector3Int(x, y, 0));
                    if (tileBase6)
                    {
                        soMap.SeaShadow.Add(new MapTilePoint(x, y, tileBase6.name));
                    }

                }
            }



            // Decoration1

            for (int i = 0; i < Decoration.transform.childCount; i++)
            {
                Transform child = Decoration.transform.GetChild(i);

                if (child.name.Contains("Sign"))
                {
                    int x = (int)(child.localPosition.x - 0.5f);
                    int y = (int)(child.localPosition.y - 0.5f);

                    soMap.Decoration1.Add(new MapTilePoint(x, y, child.name));
                }
            }

            for (int i = 0; i < Interactive.transform.childCount; i++)
            {
                Transform child = Interactive.transform.GetChild(i);
                int x = (int)(child.localPosition.x - 0.5f);
                int y = (int)(child.localPosition.y - 0.5f);

                Interactive interactive = child.gameObject.GetComponent<Interactive>();
                if (interactive)
                {
                    soMap.Interactive.Add(new MapTilePoint(x, y, interactive.prefabPath));
                }
            }

            // cost
            Transform Cost = GameObject.Find("Grid/Cost").transform;

            for (int i = 0; i < Cost.childCount; i++)
            {
                Transform child = Cost.GetChild(i);

                int sx = (int)child.localScale.x;
                int sy = (int)child.localScale.y;

                float px = child.localPosition.x;
                float py = child.localPosition.y;


                int startX = (int)(px * 2);
                int startY = (int)(py * 2);
                int width = sx * 3;
                int height = sy * 3;

                SpaceCost cost = child.GetComponent<SpaceCost>();

                Data_SpaceCost data = new Data_SpaceCost();
                data.Pos = new Data_Point(startX, startY);
                data.Size = new Data_Point(width, height);
                data.Price = cost.unlockAssets;


                AddUnlockTask(child, data);
                AddGuideTarget(child, data, soMap.Guisdes);

                soMap.Cost.Add(data);

            }

            //buildings
            Transform Buildings = GameObject.Find("Grid/Buildings").transform;

            for (int i = 0; i < Buildings.childCount; i++)
            {
                Transform child = Buildings.GetChild(i);

                Data_BuildingBase data = new Data_BuildingBase();

                BuildingBase buildingBase = child.GetComponent<BuildingBase>();
                data.BuildingEnum = buildingBase.buildingEnum;
                data.UnlockTask = buildingBase.UnlockTask;
                data.BluePrint = buildingBase.bluePrintEnum;
                data.StatueEnum = buildingBase.statueEnum;

                int px = (int)(child.localPosition.x * 2);
                int py = (int)(child.localPosition.y * 2);
                data.Pos = new Data_Point(px, py);
                data.Size = new Data_Point(2, 2);

                AddGuideTarget(child, data, soMap.Guisdes);

                if (child.name == "Wagon")
                {
                    Wagon cost = child.GetComponent<Wagon>();

                    data.Price = cost.assets;
                }
                else
                {
                    SpaceCost cost = child.GetComponent<SpaceCost>();

                    if (cost)
                    {
                        data.Price = cost.unlockAssets;
                        if (cost.UnlockTask > 0)
                        {
                            data.UnlockTask = cost.UnlockTask;
                        }
                    }
                }

                soMap.Builds.Add(data);


            }

            // Resoures buildings
            for (int i = 0; i < Resoures.transform.childCount; i++)
            {
                Transform child = Resoures.transform.GetChild(i);

                if (child.name == "Pumpkin" || child.name == "AppleTree")
                {
                    int px = (int)(child.localPosition.x * 2);
                    int py = (int)(child.localPosition.y * 2);

                    SpaceCost cost = child.GetComponent<SpaceCost>();
                    BuildingBase buildingBase = child.GetComponent<BuildingBase>();

                    Data_BuildingBase data = new Data_BuildingBase();
                    data.Pos = new Data_Point(px, py);
                    if (child.name == "Pumpkin")
                    {
                        data.Size = new Data_Point(3, 3);
                    }
                    else
                    {
                        data.Size = new Data_Point(1, 1);
                    }

                    data.BuildingEnum = buildingBase.buildingEnum;
                    data.UnlockTask = buildingBase.UnlockTask;

                    Debug.Log("Resoures buildings " + buildingBase.buildingEnum);
                    data.Price = cost.unlockAssets;

                    AddGuideTarget(child, data, soMap.Guisdes);

                    soMap.Builds.Add(data);
                }
                else if (child.name == "Stone")
                {
                    int x = (int)(child.localPosition.x);
                    int y = (int)(child.localPosition.y);

                    soMap.Resoures.Add(new MapTilePoint(x, y, "Stone"));
                }
                else if (child.name == "Treasure")
                {
                    int x = (int)(child.localPosition.x);
                    int y = (int)(child.localPosition.y);

                    soMap.Resoures.Add(new MapTilePoint(x, y, "Treasure"));
                }

                if (child.name == "Stone" || child.name == "Tree" || child.name == "Treasure"  || child.name == "Cactus")
                {
                    int x = (int)(child.localPosition.x);
                    int y = (int)(child.localPosition.y);

                    Data_CollectableRes data = new Data_CollectableRes();
                    data.Pos = new Data_Point(x, y);

                    AddGuideTarget(child, data, soMap.Guisdes);
                }

            }

            // Terrion buildings

            for (int i = 0; i < Terrain.transform.childCount; i++)
            {
                Transform child = Terrain.transform.GetChild(i);

                Data_BuildingBase data = new Data_BuildingBase();

                int px = (int)(child.localPosition.x * 2);
                int py = (int)(child.localPosition.y * 2);

                data.Pos = new Data_Point(px, py);



                if (child.name == "Bridge")
                {
                    data.Size = new Data_Point(1, 3);
                    data.BuildingEnum = BuildingEnum.Bridge;
                }
                if (child.name == "BridgeTD")
                {
                    data.Size = new Data_Point(3, 1);
                    data.BuildingEnum = BuildingEnum.BridgeTD;
                }
                else if (child.name == "Treasure")
                {
                    //data.Size = new Data_Point(1, 1);
                    //data.BuildingEnum = BuildingEnum.Treasure;

                    //int x = (int)(child.localPosition.x);
                    //int y = (int)(child.localPosition.y);

                    //soMap.Resoures.Add(new MapTilePoint(x, y, "Treasure"));
                }

                SpaceCost cost = child.GetComponent<SpaceCost>();
                if (cost)
                {
                    data.Price = cost.unlockAssets;
                }

                AddUnlockTask(child, data);
                AddGuideTarget(child, data, soMap.Guisdes);

                soMap.Builds.Add(data);

            }


            soMap.SetDirty();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("保存成功");
        }



        private static void AddUnlockTask(Transform child, Data_BuildingBase data)
        {
            SpaceCost cost = child.GetComponent<SpaceCost>();
            if (cost)
            {
                data.UnlockTask = cost.UnlockTask;
            }
        }



        private static void AddUnlockTask(Transform child, Data_SpaceCost data)
        {
            BuildingBase buildingBase = child.GetComponent<BuildingBase>();
            if (buildingBase != null)
            {
                data.UnlockTask = buildingBase.UnlockTask;
            }

            SpaceCost spaceCost = child.GetComponent<SpaceCost>();
            if (spaceCost != null && spaceCost.UnlockTask > 0)
            {
                data.UnlockTask = spaceCost.UnlockTask;
            }

            Debug.Log("data.UnlockTask " + data.UnlockTask);
        }

        private static void AddGuideTarget(Transform child, Data_CollectableRes data, List<MapTileGuild> guisdes)
        {
            Guide_Target guide_Target = child.GetComponent<Guide_Target>();
            if (guide_Target != null)
            {
                int[] ids = guide_Target.GuideIds;

                for (int j = 0; j < ids.Length; j++)
                {
                    MapTileGuild mapTileGuild = new MapTileGuild(ids[j], data.UniqueId());
                    mapTileGuild.Pos2x = data.Pos;
                    guisdes.Add(mapTileGuild);
                }
            }
        }

        private static void AddGuideTarget(Transform child, Data_BuildingBase data, List<MapTileGuild> guisdes)
        {
            Guide_Target guide_Target = child.GetComponent<Guide_Target>();
            if (guide_Target != null)
            {
                int[] ids = guide_Target.GuideIds;

                for (int j = 0; j < ids.Length; j++)
                {
                    MapTileGuild mapTileGuild = new MapTileGuild(ids[j], data.UniqueId());
                    mapTileGuild.Pos2x = data.Pos;
                    guisdes.Add(mapTileGuild);
                }
            }
        }

        private static void AddGuideTarget(Transform child, Data_SpaceCost data, List<MapTileGuild> guisdes)
        {
            Guide_Target guide_Target = child.GetComponent<Guide_Target>();
            if (guide_Target != null)
            {
                int[] ids = guide_Target.GuideIds;

                for (int j = 0; j < ids.Length; j++)
                {
                    MapTileGuild mapTileGuild = new MapTileGuild(ids[j], data.UniqueId());
                    mapTileGuild.Pos2x = data.Pos;
                    guisdes.Add(mapTileGuild);
                }
            }
        }
    }
}