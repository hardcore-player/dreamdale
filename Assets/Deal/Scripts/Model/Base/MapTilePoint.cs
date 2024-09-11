using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deal.Data
{
    [Serializable]
    public class MapTilePoint
    {
        public int x;
        public int y;
        public int uniqueId;

        public string tileName;

        public MapTilePoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public MapTilePoint(int x, int y, string tileName)
        {
            this.x = x;
            this.y = y;
            this.tileName = tileName;
        }
    }

    [Serializable]
    public class MapTileGuild
    {
        public int guideId;
        public long uniqueId;
        // 记录一个2倍坐标
        public Data_Point Pos2x;

        public MapTileGuild()
        {

        }

        public MapTileGuild(int x, long y)
        {
            this.guideId = x;
            this.uniqueId = y;
        }


    }
}

