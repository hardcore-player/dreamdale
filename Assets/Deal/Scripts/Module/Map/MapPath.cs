using System.Collections.Generic;
using UnityEngine;
using Deal.Data;

namespace Deal
{
    public class MapPath
    {
        private int[,] _mapWeight;  // 记录每个格子的权重
        private Dictionary<int, List<string>> _dicWeight; // key权重/value坐标
        /// <summary>
        /// 寻路算法
        /// </summary>
        /// <param name="mapInfo">可移动地块的信息，1可以，0不可</param>
        /// <param name="start">起始坐标</param>
        /// <param name="targets">多个目标点</param>
        /// <returns></returns>
        public List<Data_Point> FindPath(int[,] mapInfo, Data_Point start, Data_Point[] targets)
        {
            this._mapWeight = new int[mapInfo.GetLength(0), mapInfo.GetLength(1)];
            this._dicWeight = new Dictionary<int, List<string>>();
            foreach (Data_Point tar in targets)
            {
                this._setMapWeight(tar, 0);
            }

            int n = 0;
            while (this._dicWeight[n] != null)
            {
                foreach (string str in this._dicWeight[n])
                {
                    Data_Point pos = Data_Point.ToObject(str);
                    this._calNeighborWeight(mapInfo, pos);
                }
                n++;
            }
            return this._getPath(start);
        }

        private void _setMapWeight(Data_Point pos, int weight)
        {
            int x = pos.x;
            int y = pos.y;
            if (this._mapWeight[x, y] == weight)
            {
                return;
            }
            if (this._mapWeight[x, y] >= 0)
            {
                this._dicWeight[this._mapWeight[x, y]].Remove(pos.ToString());
            }
            this._mapWeight[x, y] = weight;
            this._setDicWeight(pos, weight);
        }

        private void _setDicWeight(Data_Point pos, int weight)
        {
            if (!this._dicWeight.ContainsKey(weight))
            {
                this._dicWeight.Add(weight, new List<string>());
            }
            string str = pos.ToString();
            if (!this._dicWeight[weight].Contains(str))
            {
                this._dicWeight[weight].Add(str);
            }
        }

        private void _calNeighborWeight(int[,] mapInfo, Data_Point pos)
        {
            int x = pos.x;
            int y = pos.y;
            int weight = this._mapWeight[x, y] + 1;
            int[,] neighbors = new int[4, 2] { { x - 1, y }, { x + 1, y }, { x, y + 1 }, { x, y - 1 } };
            for (int i = 0; i < 4; i++)
            {
                int dx = neighbors[i, 0];
                int dy = neighbors[i, 1];
                int curWeight = this._mapWeight[dx, dy];
                if (mapInfo[dx, dy] == 1 && weight < curWeight)
                {
                    this._setMapWeight(new Data_Point(dx, dy), weight);
                }
            }
        }

        private List<Data_Point> _getPath(Data_Point start)
        {
            List<Data_Point> path = new List<Data_Point>();
            path.Add(start);
            int weight = this._mapWeight[start.x, start.y];
            for (int i = weight - 1; i >= 0; i--)
            {
                int x = start.x;
                int y = start.y;
                int[,] neighbors = new int[4, 2] { { x - 1, y }, { x + 1, y }, { x, y + 1 }, { x, y - 1 } };
                for (int k = 0; k < 4; k++)
                {
                    int dx = neighbors[k, 0];
                    int dy = neighbors[k, 1];
                    int curWeight = this._mapWeight[dx, dy];
                    if (i == curWeight)
                    {
                        path.Add(new Data_Point(dx, dy));
                        break;
                    }
                }
            }
            return path;
        }
    }

}
