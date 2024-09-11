using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Druid;
using Deal.Data;
using DG.Tweening;
using Deal.UI;
using System;

namespace Deal.Env
{

    public class BuildingAdConfig
    {
        // 冷却时间
        public int refreshSecond;
        // 奖励资源
        public AssetEnum rewardAsset;
        // 奖励数量
        public int assetNum;

        // 看完回调
        public Action onAdCallback;
        // 视频冷却
        public Action onAdRestore;
    }

    /// <summary>
    /// 建筑的基础类
    /// </summary>
    public class BuildingBase : InteractiveBase
    {

        // 建筑类型
        public BuildingEnum buildingEnum = BuildingEnum.None;
        public BluePrintEnum bluePrintEnum = BluePrintEnum.None;
        public StatueEnum statueEnum = StatueEnum.None;
        public string buildingName = "";
        // 解锁任务ID
        public int UnlockTask = -1;


        /// <summary>
        /// 建筑的广告配置
        /// </summary>
        public virtual BuildingAdConfig GetAdConfig()
        {
            return new BuildingAdConfig();
        }


        /// <summary>
        /// 建筑升级大厅能力
        /// </summary>
        /// <param name="hallAbility"></param>
        public async void OnHallAbilityUpClick(string haStr)
        {
            HallAbilityEnum hallAbility = (HallAbilityEnum)Enum.Parse(typeof(HallAbilityEnum), haStr);

            UIFactoryLvUpPop ui = await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIFactoryLvUpPop) as UIFactoryLvUpPop;
            ui.SetData(hallAbility);
        }

    }
}

