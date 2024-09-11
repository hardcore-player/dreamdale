using System;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Data;
using Deal.Env;
using DG.Tweening;

namespace Deal
{
    /// <summary>
    /// 任务管理器
    /// </summary>
    public class TaskManager : SingletonClass<TaskManager>
    {
        public GameObject taskChest;

        /// <summary>
        ///  任务变化委托
        /// </summary>
        /// <param name="task"></param>
        public delegate void DelegateTaskChange(Data_Task task, bool isCreate = false);

        public DelegateTaskChange OnDataTaskChange;

        //private void OnDestroy()
        //{
        //    if (taskChest) Destroy(taskChest);
        //    OnDataTaskChange = null;
        //}


        private void NewTaskChest()
        {
            if (taskChest != null)
            {
                GameObject.Destroy(taskChest);
                taskChest = null;
            };

            Hero hero = PlayManager.I.mHero;

            MapRender mapRender = MapManager.I.mapRender;
            byte[][] Mapdata = mapRender.Mapdata;

            int x = (int)hero.transform.position.x;
            int y = (int)hero.transform.position.y;

            List<Vector3> vector3s = new List<Vector3>();
            vector3s.Add(new Vector3(x, y - 2, 0));
            vector3s.Add(new Vector3(x, y - 3, 0));
            vector3s.Add(new Vector3(x, y - 4, 0));
            vector3s.Add(new Vector3(x, y - 1, 0));
            vector3s.Add(new Vector3(x - 1, y - 2, 0));
            vector3s.Add(new Vector3(x - 1, y - 3, 0));
            vector3s.Add(new Vector3(x - 1, y - 4, 0));
            vector3s.Add(new Vector3(x - 1, y - 1, 0));
            vector3s.Add(new Vector3(x + 1, y - 2, 0));
            vector3s.Add(new Vector3(x + 1, y - 3, 0));
            vector3s.Add(new Vector3(x + 1, y - 4, 0));
            vector3s.Add(new Vector3(x + 1, y - 1, 0));
            vector3s.Add(new Vector3(x, y + 2, 0));
            vector3s.Add(new Vector3(x, y + 3, 0));
            vector3s.Add(new Vector3(x, y + 4, 0));
            vector3s.Add(new Vector3(x, y + 1, 0));
            vector3s.Add(new Vector3(x - 1, y + 2, 0));
            vector3s.Add(new Vector3(x - 1, y + 3, 0));
            vector3s.Add(new Vector3(x - 1, y + 4, 0));
            vector3s.Add(new Vector3(x - 1, y + 1, 0));
            vector3s.Add(new Vector3(x + 1, y + 2, 0));
            vector3s.Add(new Vector3(x + 1, y + 3, 0));
            vector3s.Add(new Vector3(x + 1, y + 4, 0));
            vector3s.Add(new Vector3(x + 1, y + 1, 0));


            foreach (var item in vector3s)
            {
                if (Mapdata[(int)item.y + 100][(int)item.x + 100] == 0)
                {
                    PrefabsUtils.NewUnlockSmoke(mapRender.Builds.transform, new Vector3(item.x + 0.5f, item.y + 0.5f, 0));
                    // 能走
                    PrefabsUtils.NewTaskChest(mapRender.Builds.transform, new Vector3(item.x + 0.5f, item.y + 0.5f, 0), (go) =>
                    {
                        this.taskChest = go;
                    });
                    break;
                }
            }

        }

        /// <summary>
        /// 任务完成
        /// </summary>
        public void OnCompleteTask()
        {
            Data_Task task = this.GetTask();

            if (task == null || task.IsDone == false || task.HasReward == false) return;


            // 奖励
            bool newTaskChest = task.Reward == 1;

            //标记
            task.HasReward = false;

            int nextTask = task.TaskId + 1;

            Data_Task newTask = DataUtils.NewTask(nextTask);
            if (newTask == null)
            {
                // 保存数据
                DataManager.I.Save(DataDefine.MapData);
                // 保存数据
                DataManager.I.Save(DataDefine.UserData);

                return;
            };

            nextTask = newTask.TaskId;

            // 更新任务数据
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            userData.Data.Data_Task = newTask;

            // 更新建筑解锁
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);

            // 设置建筑的状态
            foreach (Data_BuildingBase bb in mapData.Data.buildings)
            {
                // 配置从1开始
                if (bb.UnlockTask > 0 && nextTask >= bb.UnlockTask && bb.StateEnum == BuildingStateEnum.None)
                {
                    if (bb.Price != null && bb.Price.Count > 0)
                    {
                        bb.Building();
                        //bb.StateEnum = BuildingStateEnum.Building;
                        bb.DataBuff();
                        bb.OpenSmoke();
                    }
                    else
                    {
                        bb.StateEnum = BuildingStateEnum.Open;
                    }

                    bb.Load();
                }
            }

            foreach (Data_SpaceCost bb in mapData.Data.spaceCosts)
            {
                // 配置从1开始
                if (bb.UnlockTask > 0 && bb.UnlockTask == nextTask && bb.StateEnum == BuildingStateEnum.None)
                {
                    bb.StateEnum = BuildingStateEnum.Building;
                    bb.DataBuff();
                    bb.Load();
                }
            }

            // 解锁工具任务
            if (newTask.TaskType == TaskTypeEnum.tool.ToString())
            {
                WorkshopToolEnum toolType = (WorkshopToolEnum)Enum.Parse(typeof(WorkshopToolEnum), newTask.TargetType);
                Building_Workshop _Workshop = MapManager.I.GetSingleBuilding(BuildingEnum.Workshop) as Building_Workshop;
                if (_Workshop != null)
                {
                    Data_Workshop data_Workshop = _Workshop.GetData<Data_Workshop>();

                    data_Workshop.NewTool(toolType);
                    _Workshop.UpdateView();
                }
            }
            else if (newTask.TaskType == TaskTypeEnum.equip.ToString())
            {
                int EquipId = int.Parse(newTask.TargetType);

                Building_Workshop _Workshop = MapManager.I.GetSingleBuilding(BuildingEnum.Workshop) as Building_Workshop;
                if (_Workshop != null)
                {
                    Data_Workshop data_Workshop = _Workshop.GetData<Data_Workshop>();

                    data_Workshop.NewEquip(EquipId);
                    _Workshop.UpdateView();
                }

                //Building_Armory _Armory = MapManager.I.GetSingleBuilding(BuildingEnum.Armory) as Building_Armory;
                //if (_Armory != null)
                //{
                //    Data_Armory data_Armory = _Armory.GetData<Data_Armory>();

                //    data_Armory.EquipId = int.Parse(newTask.TargetType);
                //    ExcelData.Equip equip = ConfigManger.I.GetEquipCfg(data_Armory.EquipId);
                //    data_Armory.Price.Clear();

                //    int priceNum = 0;
                //    for (int i = 0; i < equip.assets.Length; i++)
                //    {
                //        AssetEnum asset = (AssetEnum)Enum.Parse(typeof(AssetEnum), equip.assets[i]);
                //        int assetNum = (int)equip.num[i];
                //        priceNum += assetNum;
                //        data_Armory.Price.Add(new Data_GameAsset(asset, assetNum));

                //    }
                //    data_Armory.ToolPrice = priceNum;

                //    _Armory.UpdateView();
                //}
            }
            else if (newTask.TaskType == TaskTypeEnum.tower.ToString())
            {
                // 宝石塔
                Building_GemTower _GemTower = MapManager.I.GetSingleBuilding(BuildingEnum.GemTower) as Building_GemTower;

                if (_GemTower != null)
                {
                    Data_GemTower dataGemTower = _GemTower.GetData<Data_GemTower>();

                    dataGemTower.Price.Clear();

                    int assetNum = newTask.TotalProgress;
                    AssetEnum asset = (AssetEnum)Enum.Parse(typeof(AssetEnum), newTask.TargetType);
                    dataGemTower.Price.Add(new Data_GameAsset(asset, assetNum));
                    dataGemTower.TowerPrice = assetNum;
                    dataGemTower.TowerAsset = asset;

                    _GemTower.UpdateView();
                }
            }

            this.OnDataTaskChange(newTask, true);

            // 弹出资源和金币限时礼包
            ShopUtils.newSpecialOfferGoldAndResource();

            if (!Config.IsDebug())
            {
                this.LookAtGuideTarget(1, 1, newTaskChest, () =>
                {
                    this.OnDataTaskChange(newTask);
                });
            }

            // 保存数据
            DataManager.I.Save(DataDefine.MapData);
            // 保存数据
            DataManager.I.Save(DataDefine.UserData);

        }


        public Data_Task GetTask()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            return userData.Data.Data_Task;
        }


        /// <summary>
        /// 采集任务
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <param name="assetNum"></param>
        public void OnTaskCollect(AssetEnum assetEnum, int assetNum)
        {
            Data_Task task = this.GetTask();

            if (task == null) return;
            if (task.IsDone == true) return;
            if (task.TaskType != TaskTypeEnum.collect.ToString()) return;

            if (assetEnum != (AssetEnum)Enum.Parse(typeof(AssetEnum), task.TargetType)) return;


            task.CurProgress += assetNum;

            if (task.CurProgress >= task.TotalProgress)
            {
                task.CurProgress = task.TotalProgress;
                task.IsDone = true;
            }

            this.OnDataTaskChange(task);
        }

        /// <summary>
        /// 击杀敌人任务
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <param name="assetNum"></param>
        public void OnTaskKill(int assetNum)
        {
            Data_Task task = this.GetTask();

            if (task == null) return;
            if (task.IsDone == true) return;
            if (task.TaskType != TaskTypeEnum.kill.ToString()) return;

            task.CurProgress += assetNum;

            if (task.CurProgress >= task.TotalProgress)
            {
                task.CurProgress = task.TotalProgress;
                task.IsDone = true;
            }

            this.OnDataTaskChange(task);
        }

        /// <summary>
        /// 建造任务
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <param name="assetNum"></param>
        public void OnTaskBuild(BuildingEnum buildingEnum)
        {
            Debug.Log("OnTaskBuild:" + buildingEnum);
            Data_Task task = this.GetTask();
            if (task == null) return;
            if (task.IsDone == true) return;
            if (task.TaskType != TaskTypeEnum.build.ToString()) return;

            if (buildingEnum != (BuildingEnum)Enum.Parse(typeof(BuildingEnum), task.TargetType)) return;

            task.CurProgress = task.TotalProgress;
            task.IsDone = true;

            this.OnDataTaskChange(task);
        }

        /// <summary>
        /// 建造任务
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <param name="assetNum"></param>
        public void OnTaskLand(Data_SpaceCost spaceCost)
        {
            Data_Task task = this.GetTask();
            if (task == null) return;
            if (task.IsDone == true) return;
            if (task.TaskType != TaskTypeEnum.land.ToString()) return;

            long uniqueId = MapManager.I.GetTaskUniqueId(task.TaskId);
            if (uniqueId > 0 && uniqueId != spaceCost.UniqueId())
            {
                // 不是任务指引的那块地
                return;
            }

            task.CurProgress = task.TotalProgress;
            task.IsDone = true;

            this.OnDataTaskChange(task);
        }


        /// <summary>
        /// 出售任务
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <param name="assetNum"></param>
        public void OnTaskSell(AssetEnum assetEnum, int num)
        {
            Data_Task task = this.GetTask();
            if (task == null) return;
            if (task.IsDone == true) return;
            if (task.TaskType != TaskTypeEnum.sell.ToString()) return;
            if (assetEnum != (AssetEnum)Enum.Parse(typeof(AssetEnum), task.TargetType)) return;

            task.CurProgress += num;
            if (task.CurProgress >= task.TotalProgress)
            {
                task.CurProgress = task.TotalProgress;
                task.IsDone = true;
            }

            this.OnDataTaskChange(task);
        }

        /// <summary>
        /// 升级任务，提前能做好
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <param name="assetNum"></param>
        public void OnTaskUpgrade(HallAbilityEnum abilityEnum, int lv)
        {
            Data_Task task = this.GetTask();
            if (task == null) return;
            if (task.IsDone == true) return;
            if (task.TaskType != TaskTypeEnum.upgrade.ToString()) return;

            if (abilityEnum != (HallAbilityEnum)Enum.Parse(typeof(HallAbilityEnum), task.TargetType)) return;

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            int aLv = userData.GetHallAbilityLv(abilityEnum);
            task.CurProgress = aLv + 1;

            if (task.CurProgress >= task.TotalProgress)
            {
                task.IsDone = true;
            }


            this.OnDataTaskChange(task);
        }


        /// <summary>
        /// 解锁任务
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <param name="assetNum"></param>
        public void OnTaskTool(WorkshopToolEnum toolEnum)
        {
            Data_Task task = this.GetTask();
            if (task == null) return;
            if (task.IsDone == true) return;
            if (task.TaskType != TaskTypeEnum.tool.ToString()) return;

            task.CurProgress = task.TotalProgress;
            task.IsDone = true;

            this.OnDataTaskChange(task);
        }

        /// <summary>
        /// 解锁任务
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <param name="assetNum"></param>
        public void OnTaskAction(TaskActionEnum actionEnum)
        {
            Data_Task task = this.GetTask();
            if (task == null) return;
            if (task.IsDone == true) return;
            if (task.TaskType != TaskTypeEnum.action.ToString()) return;

            if (actionEnum != (TaskActionEnum)Enum.Parse(typeof(TaskActionEnum), task.TargetType)) return;

            task.CurProgress = task.TotalProgress;
            task.IsDone = true;

            this.OnDataTaskChange(task);
        }

        /// <summary>
        /// 建造武器
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <param name="assetNum"></param>
        public void OnTaskEquip(string equipId)
        {
            Data_Task task = this.GetTask();
            if (task == null) return;
            if (task.IsDone == true) return;
            if (task.TaskType != TaskTypeEnum.equip.ToString()) return;


            task.CurProgress = task.TotalProgress;
            task.IsDone = true;

            this.OnDataTaskChange(task);
        }


        /// <summary>
        /// 建造任务
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <param name="assetNum"></param>
        public void OnTaskComplete(double dungeonId)
        {
            Data_Task task = this.GetTask();
            if (task == null) return;
            if (task.IsDone == true) return;
            if (task.TaskType != TaskTypeEnum.complete.ToString()) return;

            long uniqueId = MapManager.I.GetTaskUniqueId(task.TaskId);
            if (uniqueId > 0 && uniqueId != dungeonId)
            {
                // 不是任务指引的那块地
                return;
            }

            task.CurProgress = task.TotalProgress;
            task.IsDone = true;

            if (this.OnDataTaskChange != null)
                this.OnDataTaskChange(task);
        }

        /// <summary>
        /// 购买蓝图任务
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <param name="assetNum"></param>
        public void OnTaskResearch(string targetName)
        {
            Data_Task task = this.GetTask();
            if (task == null) return;
            if (task.IsDone == true) return;
            if (task.TaskType != TaskTypeEnum.research.ToString()) return;

            if (targetName != task.TargetType) return;

            task.CurProgress = task.TotalProgress;
            task.IsDone = true;

            if (this.OnDataTaskChange != null)
                this.OnDataTaskChange(task);
        }

        /// <summary>
        /// 买任务
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <param name="assetNum"></param>
        public void OnTaskBuy(string target, int num)
        {
            Data_Task task = this.GetTask();
            if (task == null) return;
            if (task.IsDone == true) return;
            if (task.TaskType != TaskTypeEnum.buy.ToString()) return;
            if (task.TargetType != target) return;

            task.CurProgress += num;
            if (task.CurProgress >= task.TotalProgress)
            {
                task.CurProgress = task.TotalProgress;
                task.IsDone = true;
            }

            this.OnDataTaskChange(task);
        }


        /// <summary>
        /// 买任务
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <param name="assetNum"></param>
        public void OnTaskTower(string target, int num)
        {
            Data_Task task = this.GetTask();
            if (task == null) return;
            if (task.IsDone == true) return;
            if (task.TaskType != TaskTypeEnum.tower.ToString()) return;
            if (task.TargetType != target) return;

            task.CurProgress = num;
            if (task.CurProgress >= task.TotalProgress)
            {
                task.CurProgress = task.TotalProgress;
                task.IsDone = true;
            }

            if (this.OnDataTaskChange != null)
            {
                this.OnDataTaskChange(task);
            }
        }

        /// <summary>
        /// 图书馆
        /// </summary>
        /// <param name="target"></param>
        /// <param name="num"></param>
        public void OnTaskLibrary(string target, int num)
        {
            Data_Task task = this.GetTask();
            if (task == null) return;
            if (task.IsDone == true) return;
            if (task.TaskType != TaskTypeEnum.library.ToString()) return;
            if (task.TargetType != target) return;

            task.CurProgress = num;
            if (task.CurProgress >= task.TotalProgress)
            {
                task.CurProgress = task.TotalProgress;
                task.IsDone = true;
            }

            this.OnDataTaskChange(task);
        }

        /// <summary>
        /// 安排工人采集任务
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <param name="assetNum"></param>
        public void OnTaskAssign(string assetEnum)
        {
            Data_Task task = this.GetTask();

            if (task == null) return;
            if (task.IsDone == true) return;
            if (task.TaskType != TaskTypeEnum.assign.ToString()) return;

            if (assetEnum != task.TargetType) return;

            task.CurProgress = task.TotalProgress;
            task.IsDone = true;

            if (this.OnDataTaskChange != null)
                this.OnDataTaskChange(task);
        }


        /// <summary>
        /// 任务镜头指引
        /// </summary>
        /// <param name="delay1"></param>
        /// <param name="delay2"></param>
        /// <param name="newTaskChest"></param>
        /// <param name="action"></param>
        public void LookAtGuideTarget(float delay1, float delay2, bool newTaskChest, Action action = null)
        {

            GameSceneManager gameScene = App.I.CurScene as GameSceneManager;

            GameObject guideArrow = MapManager.I.mapRender.guideArrow;
            gameScene.cinemachine2.Follow = guideArrow.transform;

            Hero hero = PlayManager.I.mHero;

            hero.Controller.SetStatePause();

            Sequence s = DOTween.Sequence();
            s.AppendInterval(delay1);
            s.AppendCallback(() =>
            {
                gameScene.cinemachine1.gameObject.SetActive(false);
                gameScene.cinemachine2.gameObject.SetActive(true);
            });
            s.AppendInterval(1f);
            s.AppendInterval(0.5f);
            s.AppendCallback(() =>
            {
                gameScene.cinemachine1.gameObject.SetActive(true);
                gameScene.cinemachine2.gameObject.SetActive(false);

            });
            s.AppendInterval(delay2);
            s.AppendCallback(() =>
            {
                hero.Controller.SetStateIdle();

                if (action != null) action();
                if (newTaskChest == true)
                {
                    SoundManager.I.playEffect(AddressbalePathEnum.WAV_bilngGameSound10);
                    this.NewTaskChest();
                }

            });
        }


        public void LookAtGuideTarget()
        {
            GameSceneManager gameScene = App.I.CurScene as GameSceneManager;

            GameObject guideArrow = MapManager.I.mapRender.guideArrow;

            if (guideArrow.activeInHierarchy == false) return;
            if (gameScene.cinemachine2.gameObject.activeInHierarchy == true) return;
            this.LookAtGuideTarget(0, 0, false);
        }

        #region  private


        #endregion private

    }
}