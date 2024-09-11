using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deal.Data;

namespace Deal
{
    public class SO_MapData : ScriptableObject
    {
        public List<MapTilePoint> Ground = new List<MapTilePoint>();
        public List<MapTilePoint> SeaShadow = new List<MapTilePoint>();
        public List<MapTilePoint> Terrain = new List<MapTilePoint>();
        public List<MapTilePoint> Decoration = new List<MapTilePoint>();
        public List<MapTilePoint> Decoration1 = new List<MapTilePoint>();
        public List<MapTilePoint> Interactive = new List<MapTilePoint>();
        public List<MapTilePoint> Resoures = new List<MapTilePoint>();
        public List<MapTilePoint> Fence = new List<MapTilePoint>();
        public List<MapTilePoint> Open = new List<MapTilePoint>();

        public List<Data_SpaceCost> Cost = new List<Data_SpaceCost>();

        public List<Data_BuildingBase> Builds = new List<Data_BuildingBase>();
        public List<MapTileGuild> Guisdes = new List<MapTileGuild>();



        public Dictionary<Vector2Int, string> groundTiles = new Dictionary<Vector2Int, string>();
        public Dictionary<Vector2Int, string> resourcesTiles = new Dictionary<Vector2Int, string>();
        public Dictionary<Vector2Int, string> decorationTiles = new Dictionary<Vector2Int, string>();
        public Dictionary<Vector2Int, string> decorationTiles1 = new Dictionary<Vector2Int, string>();
        public Dictionary<Vector2Int, string> interactiveTiles = new Dictionary<Vector2Int, string>();
        public Dictionary<Vector2Int, string> terrainTiles = new Dictionary<Vector2Int, string>();
        public Dictionary<Vector2Int, string> fenceTiles = new Dictionary<Vector2Int, string>();
        public Dictionary<Vector2Int, string> seaShadowTiles = new Dictionary<Vector2Int, string>();
        public Dictionary<Vector2Int, Data_BuildingBase> buildTiles = new Dictionary<Vector2Int, Data_BuildingBase>();
        public Dictionary<int, long> guides = new Dictionary<int, long>();


        public void Init()
        {
            groundTiles.Clear();
            resourcesTiles.Clear();
            decorationTiles.Clear();
            decorationTiles1.Clear();
            terrainTiles.Clear();
            fenceTiles.Clear();
            guides.Clear();
            buildTiles.Clear();
            seaShadowTiles.Clear();
            interactiveTiles.Clear();

            foreach (var item in this.Ground)
            {
                groundTiles.Add(new Vector2Int(item.x, item.y), item.tileName);
            }

            foreach (var item in this.SeaShadow)
            {
                seaShadowTiles.Add(new Vector2Int(item.x, item.y), item.tileName);
            }

            foreach (var item in this.Decoration)
            {
                decorationTiles.Add(new Vector2Int(item.x, item.y), item.tileName);
            }

            foreach (var item in this.Decoration1)
            {
                decorationTiles1.Add(new Vector2Int(item.x, item.y), item.tileName);
            }

            foreach (var item in this.Interactive)
            {
                interactiveTiles.Add(new Vector2Int(item.x, item.y), item.tileName);
            }


            foreach (var item in this.Resoures)
            {
                resourcesTiles.Add(new Vector2Int(item.x, item.y), item.tileName);
            }

            foreach (var item in this.Terrain)
            {
                terrainTiles.Add(new Vector2Int(item.x, item.y), item.tileName);
            }

            foreach (var item in this.Fence)
            {
                fenceTiles.Add(new Vector2Int(item.x, item.y), item.tileName);
            }

            foreach (var item in this.Guisdes)
            {
                guides.Add(item.guideId - 1, item.uniqueId);
            }

            foreach (var item in this.Builds)
            {
                Data_Point p = item.CenterGrid();
                buildTiles.Add(new Vector2Int(p.x, p.y), item);
            }
        }

    }
}


