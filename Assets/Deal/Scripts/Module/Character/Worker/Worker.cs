using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using Deal.Env;
using Deal.Tools;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Druid;

namespace Deal
{
    enum WorkerState
    {
        Idle, // 空闲
        Moving2Work, // 移动
        Moving2Storage, // 移动
        Working, // 收集
        Trans, // 放仓库
        WaitStorage, // 等待仓库空闲
    }

    public class Worker : RoleBase
    {

        //
        public TextMeshPro textNum;

        public Transform aniBody;

        public Data_Worker Data;

        public float MoveSpeed = 1f;
        // 移动路径
        private List<Vector3> roadPath;

        private Deal.Tools.AssetFlyNumTool _flyNumTool;

        private int _farmNeedTime = 3;
        private int _farmTime = 0;

        public override void OnAwake()
        {
        }

        /// <param name="data"></param>
        public virtual void SetData(Data_Worker data)
        {
            this.Data = data;
            //this.UpdateView();
            this.textNum.text = data.AssetNum + "/" + data.BagTotal;
        }


        private WorkerState _workState = WorkerState.Idle;
        private Vector2 _nextMoveCommand;

        // 动画时间
        private float _aniInterval = 0.0f;
        private float _aniSpeed = 1.0f;
        private bool _aniPlay = false;

        private float _flyInterval = 0.0f;

        // 树石头等需要停下在收集等建筑
        private List<CollectableRes> collectableAsset = new List<CollectableRes>();

        // 仓库
        private Building_Storage storage;

        #region  override

        /// <summary>
        /// 是否可以采集
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <returns></returns>
        public override bool CanCollectAsset(AssetEnum assetEnum)
        {
            return this.Data.AssetId == assetEnum;
        }

        #endregion  override


        void Update()
        {
            UpdateMoveState();
            UpdateAnimation();
        }

        void UpdateMoveState()
        {
            if (this._workState == WorkerState.Moving2Work || this._workState == WorkerState.Moving2Storage)
            {
                // 去工作等路上
                //if (this.CanCollect())
                //{
                //    this._workState = WorkerState.Idle;
                //}

                if (this.roadPath == null || this.roadPath.Count == 0)
                {
                    this._workState = WorkerState.Idle;
                    this.PlayIdle();
                }
                else
                {
                    Vector3 toPos = this.roadPath[0];
                    Vector3 dir = toPos - this.transform.position;
                    Vector3 dst = dir.normalized * Time.deltaTime * this.MoveSpeed;

                    this.transform.Translate(dst);

                    if (Vector3.Distance(toPos, this.transform.position) < 0.1f)
                    {
                        this.roadPath.RemoveAt(0);
                    }

                    this.PlayRun();
                    this.LookDir(dir);
                }

                return;
            }



            if (storage != null)
            {
                // 在仓库边上
                bool isStorageFull = false;
                if (isStorageFull == true)
                {
                    // 仓库满

                    if (this.Data.AssetNum > this.Data.BagTotal / 2)
                    {
                        // 原地等
                        this._workState = WorkerState.WaitStorage;

                        this.PlayIdle();
                    }
                    else
                    {
                        // 去工作
                        this.FindCollectAsset();
                    }
                }
                else
                {
                    if (this.Data.AssetNum == 0)
                    {
                        // 去工作
                        this.FindCollectAsset();
                    }
                    else
                    {
                        // 运输
                        if (this._workState != WorkerState.Trans)
                        {
                            this._workState = WorkerState.Trans;
                            this._flyInterval = 0;

                            if (this._flyNumTool == null)
                            {
                                this._flyNumTool = new AssetFlyNumTool();
                            }
                            this._flyNumTool.stop();
                        }

                    }

                    if (this._workState == WorkerState.Trans)
                    {
                        this._flyInterval += Time.deltaTime;
                        if (this._flyInterval >= 0.1f)
                        {
                            this.Fly2Storage();
                            this._flyInterval = 0;
                        }
                    }
                }

            }
            else if (this.IsIdleNoAni())
            {
                if (this.Data.AssetNum < this.Data.BagTotal)
                {
                    // 背包有空间
                    if (this.CanCollect())
                    {
                        // 采集
                        this.DoCollectAsset();
                    }
                    else
                    {
                        // 找树或者石头
                        this.FindCollectAsset();
                    }

                }
                else
                {
                    // 背包满了
                    this.FindStorage();
                }
            }
        }

        void UpdateAnimation()
        {
            if (this._aniPlay == true)
            {
                this._aniInterval += Time.deltaTime;
                if (this._aniInterval >= this._aniSpeed)
                {
                    this._aniPlay = false;

                    this.PlayIdle();
                }
            }
        }


        public override void PlayWeapon(WorkshopToolEnum workshopTool)
        {
            this._aniPlay = true;
            this._aniInterval = 0;

            float inverval = 0.5f / this.aniSpeedRate;
            this._aniSpeed = inverval;

            base.PlayWeapon(workshopTool);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsIdleNoAni()
        {
            return this._aniPlay == false && this._workState == WorkerState.Idle;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log("OnTriggerEnter2D" + collision.gameObject.tag);

            if (collision.gameObject.tag == "CollectableRes")
            {
                CollectableRes prop = collision.gameObject.GetComponent<CollectableRes>();
                if (prop != null)
                {
                    Data_CollectableRes _Data = prop.GetData<Data_CollectableRes>();
                    if (_Data != null && _Data.AssetId == this.Data.AssetId)
                    {
                        this.collectableAsset.Add(prop);
                    }
                }
            }
            else if (collision.gameObject.tag == "Storage")
            {
                this.storage = collision.gameObject.GetComponent<Building_Storage>();
            }

        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            //Debug.Log("OnTriggerExit2D" + collision.gameObject.tag);

            if (collision.gameObject.tag == "CollectableRes")
            {
                CollectableRes prop = collision.gameObject.GetComponent<CollectableRes>();
                if (prop != null)
                {
                    if (this.collectableAsset.Contains(prop))
                    {
                        this.collectableAsset.Remove(prop);
                    }

                }
            }
            else if (collision.gameObject.tag == "Storage")
            {
                this.storage = null;
            }

        }


        /// <summary>
        /// 可以采集
        /// </summary>
        /// <returns></returns>
        private bool CanCollect()
        {
            if (this.collectableAsset.Count <= 0)
            {
                return false;
            }

            for (int i = 0; i < this.collectableAsset.Count; i++)
            {
                var item = this.collectableAsset[i];
                if (item.CanCollect())
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// 采集
        /// </summary>
        private void DoCollectAsset()
        {
            //Debug.Log("DoFarm");

            AssetEnum collectAsset = DealUtils.getCollectAssetEnum(this, this.collectableAsset);

            if (collectAsset != AssetEnum.None)
            {
                UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

                WorkshopToolEnum tool = userData.GetCollectResTool(collectAsset);
                int fallNum = (int)this.Data.AbilityVal;
                if (fallNum > 0)
                {
                    this.PlayWeapon(tool);

                    bool fall = false;
                    this._farmTime++;
                    if (this._farmTime >= this._farmNeedTime)
                    {
                        this._farmTime = 0;
                        fall = true;
                    }

                    List<CollectableRes> _list = DealUtils.RoleCollectAsset(this, this.collectableAsset, collectAsset, tool, fallNum, fall);

                    if (_list != null && _list.Count > 0)
                    {
                        Vector3 dir = _list[0].transform.position - this.transform.position;
                        this.LookDir(dir);
                    }
                }
            }

        }

        /// <summary>
        /// 找资源
        /// </summary>
        private void FindCollectAsset()
        {
            MapRender mapRender = MapManager.I.mapRender;

            float nearst = 10000f;
            CollectableRes nearstItem = null;
            foreach (CollectableRes item in mapRender.collectableRes.Values)
            {
                Data_CollectableRes _Data = item.GetData<Data_CollectableRes>();

                if (_Data.AssetId == this.Data.AssetId && item.CanCollect())
                {
                    float dis = Vector3.Distance(transform.position, item.transform.position);

                    if (nearst > dis)
                    {
                        nearst = dis;
                        nearstItem = item;
                    }
                }
            }

            if (nearstItem != null)
            {
                this._workState = WorkerState.Moving2Work;
                Data_CollectableRes _Data = nearstItem.GetData<Data_CollectableRes>();
                Data_Point data_Point = _Data.StartGrid();

                int sx = data_Point.x;
                int sy = data_Point.y;

                //mapRender.GeneratePathTiles();

                byte[][] Mapdata = mapRender.Mapdata;

                if (_Data.AssetId == AssetEnum.Pumpkin || _Data.AssetId == AssetEnum.Carrot)
                {
                    // 胡萝卜南瓜直接去中心点，而且如果距离小1，直接走
                    Debug.Log("南瓜 xx" + sx + "yyy" + sy);

                    if (nearst < 3f)
                    {
                        // 一块胡萝卜南瓜地力
                        this.roadPath = new List<Vector3>();
                        this.roadPath.Add(nearstItem.transform.position);
                    }
                    else
                    {
                        // 去找胡萝卜南瓜地
                        this.FindPath(new Data_Point(sx, sy));
                    }
                }
                else
                {
                    if (Mapdata[sy + 100][sx + 100 - 1] == 0)
                    {
                        this.FindPath(new Data_Point(sx - 1, sy));
                    }
                    else if (Mapdata[sy + 100][sx + 100 + 1] == 0)
                    {
                        this.FindPath(new Data_Point(sx + 1, sy));
                    }
                    else if (Mapdata[sy + 100 + 1][sx + 100] == 0)
                    {
                        this.FindPath(new Data_Point(sx, sy + 1));
                    }
                    else if (Mapdata[sy + 100 - 2][sx + 100] == 0)
                    {
                        this.FindPath(new Data_Point(sx, sy - 1));
                    }
                    else
                    {
                        this.FindPath(new Data_Point(sx, sy));
                    }
                }
            }
        }


        /// <summary>
        /// 找仓库
        /// </summary>
        private void FindStorage()
        {
            //Debug.Log("FindStorage");
            Building_Storage _Storage = MapManager.I.GetSingleBuilding(BuildingEnum.Storage) as Building_Storage;

            if (_Storage)
            {
                this._workState = WorkerState.Moving2Storage;
                Data_Storage _Data = _Storage.GetData<Data_Storage>();
                Data_Point data_Point = _Data.CenterGrid();

                Data_Point dataP = new Data_Point(data_Point.x + _Data.Size.x / 2, data_Point.y - 1);

                this.FindPath(dataP);
            }
        }

        private void Fly2Storage()
        {

            if (this.storage == null) return;
            Data_Storage _Storage = this.storage.GetData<Data_Storage>();

            if (this.Data.AssetNum > 0 && !_Storage.IsAseetsFull())
            {
                Data_Worker _Worker = this.Data as Data_Worker;

                int req = this.Data.AssetNum;
                if (_Storage.GetAseetCount() + this.Data.AssetNum > _Storage.AssetTotal)
                {
                    req = _Storage.AssetTotal - _Storage.GetAseetCount();
                }

                int num = this._flyNumTool.numPerTime(req, this.Data.AssetNum);

                Debug.Log("_Storage AssetTotal" + _Storage.AssetTotal);
                Debug.Log("_Storage num" + num);
                Debug.Log("_Storage GetAseetCount" + _Storage.GetAseetCount());

                bool added = _Worker.AddAsset(this.Data.AssetId, -num);
                if (added)
                {
                    this.UpdateAssetLabel(this.Data.AssetId, num);
                    int addNum = _Storage.AddAsset(this.Data.AssetId, num);
                    this.storage.UpdateAsset(this.Data.AssetId, addNum);

                    Debug.Log("_Storage GetAseetCount" + _Storage.GetAseetCount());


                    Vector3 from = center != null ? center.position : transform.position;
                    DealUtils.NewDropPropBezierToTarget(this.Data.AssetId, num, from, this.storage.center.position);
                }
            }
        }


        public void UpdateAssetLabel(AssetEnum assetEnum, int num)
        {
            Data_Worker _Worker = this.Data as Data_Worker;
            this.textNum.text = _Worker.AssetNum + "/" + _Worker.BagTotal;
        }


        public override void LookDir(Vector3 dir)
        {
            if (dir.x < 0)
            {
                this.aniBody.localScale = new Vector3(-1, 1, 0);
            }
            else if (dir.x > 0)
            {
                this.aniBody.localScale = new Vector3(1, 1, 0);
            }
        }

        private void FindPath(Data_Point data_Point)
        {
            MapRender mapRender = MapManager.I.mapRender;

            int sx = (int)Mathf.Round(this.transform.position.x - 0.5f) + 100;
            int sy = (int)Mathf.Round(this.transform.position.y - 0.5f) + 100;

            int dx = (int)data_Point.x + 100;
            int dy = (int)data_Point.y + 100;

            //mapRender.GeneratePathTiles();

            List<AstarPosVo> rst = Astar.instance.find(mapRender.Mapdata, NumDefine.MapTileHeight, NumDefine.MapTileWidth, sx, sy, dx, dy, 50);

            if (rst != null)
            {
                this.roadPath = new List<Vector3>();
                for (int i = 0; i < rst.Count; i++)
                {
                    AstarPosVo vo = rst[i];
                    this.roadPath.Add(new Vector3(vo.x - 100 + 0.5f, vo.y - 100 + 0.5f, 0));
                }
            }
            else
            {
                this._workState = WorkerState.Idle;
            }

        }

    }
}

