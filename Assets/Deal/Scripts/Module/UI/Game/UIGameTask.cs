using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using Deal.Env;
using Deal.Data;
using DG.Tweening;
using TMPro;

namespace Deal.UI
{
    public class UIGameTask : UIBase
    {
        #region Inspector
        public TextMeshProUGUI txtTaskInfo;
        public TextMeshProUGUI txtProgress;
        public Slider sldProgress;
        public GameObject goTaskBar;
        public GameObject goOverBar;
        public GameObject goCompolete;
        public GameObject goFinger;

        private GameObject guideArrow;

        #endregion Inspector

        public override void OnUIAwake()
        {

            Debug.Log("App.I.CurScene.sceneName" + App.I.CurScene.sceneName);
            if (App.I.CurScene.sceneName == SceneEnum.dungeon)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                this.guideArrow = MapManager.I.mapRender.guideArrow;

                // 监听变化
                TaskManager.I.OnDataTaskChange += OnDataTaskChange;
                PrefabsUtils.OnBuildingLoad += OnBuildingLoad;

                Druid.Utils.UIUtils.AddBtnClick(transform, "goTaskBar", this.OnCompoleteClick);
                Druid.Utils.UIUtils.AddBtnClick(transform, "goTaskBar/finishedbg", this.OnCompoleteClick);
            }
        }

        public override void OnUIStart()
        {
            if (App.I.CurScene.sceneName == SceneEnum.main)
            {
                this.goTaskBar.SetActive(false);
                StartCoroutine(DoRefreshTask());
            }
        }

        public override void OnUIDestroy()
        {
            TaskManager.I.OnDataTaskChange -= OnDataTaskChange;
            PrefabsUtils.OnBuildingLoad -= OnBuildingLoad;

        }


        IEnumerator DoRefreshTask()
        {
            yield return new WaitForSeconds(1f); //wait one frame

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            this.UpdateTask(userData.Data.Data_Task);
        }

        /// <summary>
        /// 任务变化
        /// </summary>
        /// <param name="task"></param>
        public void OnDataTaskChange(Data_Task task, bool isCreate = false)
        {
            this.UpdateTask(task, isCreate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingBase"></param>
        /// <param name="building"></param>
        public void OnBuildingLoad(Data_BuildingBase buildingBase, BuildingBase building)
        {
            Data_Task task = TaskManager.I.GetTask();

            int taskId = task.TaskId;

            long taskGuideUId = MapManager.I.GetTaskUniqueId(taskId);

            if (buildingBase.UniqueId() == taskGuideUId)
            {
                if (building.guide != null)
                {
                    this.guideArrow.transform.position = building.guide.transform.position;
                }
            }
        }

        private void UpdateTask(Data_Task task, bool isCreate = false)
        {
            // 米有任务或者任务奖励已经领取
            if (task == null || task.HasReward == false)
            {
                this.goTaskBar.SetActive(false);
                this.guideArrow.SetActive(false);
                return;
            }

            if (task.TaskId > Config.VersionMaxTaskId)
            {
                this.goTaskBar.SetActive(false);
                this.goOverBar.SetActive(true);
                return;
            }

            this.goTaskBar.SetActive(true);
            this.goOverBar.SetActive(false);

            Debug.Log("task.TaskInfo" + task.TaskInfo);
            this.txtTaskInfo.text = $"{ task.TaskInfo}";
            this.txtProgress.text = $"{task.CurProgress}/{task.TotalProgress}";
            this.sldProgress.value = task.CurProgress / 1.0f / task.TotalProgress;

            if (task.IsDone && !isCreate)
            {
                this.guideArrow.SetActive(false);
                this.goCompolete.SetActive(true);

                this.goFinger.SetActive(task.TaskId <= 8);

                if (task.Auto > 0)
                {
                    Hero hero = PlayManager.I.mHero;
                    hero.Controller.SetStatePause();

                    Sequence s = DOTween.Sequence();
                    s.AppendInterval(0.5f);
                    s.AppendCallback(() =>
                    {
                        hero.Controller.SetStateIdle();
                        //  自动完成
                        this.OnCompoleteClick();
                    });
                }
            }
            else
            {
                // 更新箭头
                this.UpdateTaskGuide(task);
                this.goCompolete.SetActive(false);
            }
        }

        private void UpdateTaskGuide(Data_Task task)
        {
            if (App.I.CurScene.sceneName == SceneEnum.dungeon)
            {
                return;
            }

            int taskId = task.TaskId;

            MapRender mapRender = MapManager.I.mapRender;
            SO_MapData sO_MapData = mapRender.SO_MapData;


            this.guideArrow.SetActive(false);

            long taskGuideUId = MapManager.I.GetTaskUniqueId(taskId);

            if (taskGuideUId != 0)
            {
                BindingSaveData go = mapRender.GetBuyUniqueId(taskGuideUId);
                if (go != null)
                {
                    this.guideArrow.SetActive(true);
                    if (go.guide != null)
                    {
                        this.guideArrow.transform.position = go.guide.transform.position;
                    }
                    else
                    {
                        this.guideArrow.transform.position = go.transform.position + new Vector3(0, 1, 0);
                    }

                }
                else
                {
                    for (int i = 0; i < mapRender.SO_MapData.Guisdes.Count; i++)
                    {
                        MapTileGuild guild = mapRender.SO_MapData.Guisdes[i];

                        if (guild.uniqueId == taskGuideUId)
                        {
                            this.guideArrow.SetActive(true);
                            this.guideArrow.transform.position = new Vector3(guild.Pos2x.x / 2f, guild.Pos2x.y / 2f) + new Vector3(0, 1, 0);
                        }
                    }
                }

                //if (this.guideArrow.activeInHierarchy && task.CurProgress == 0)
                //{
                //    this.LookAtGuideTarget();
                //}
            }
            else
            {
                this.guideArrow.SetActive(false);
            }
        }


        /// <summary>
        /// 点击完成
        /// </summary>
        private void OnCompoleteClick()
        {
            Data_Task data = TaskManager.I.GetTask();

            if (data != null && data.IsDone == true && data.HasReward == true)
            {
                TaskManager.I.OnCompleteTask();

                SoundManager.I.playEffect(AddressbalePathEnum.WAV_bilngGameSound10);

                UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
                userData.AddLandExp(4);

                // 提交统计
                NetUtils.postTaskStats(data.TaskId);
            }
            else
            {
                TaskManager.I.LookAtGuideTarget(0, 0, false);
            }

        }

    }

}
