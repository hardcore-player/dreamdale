using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using TMPro;
using Druid.Utils;
using System;
using Deal.UI;
using PathologicalGames;

namespace Deal
{
    public class PlayManager : PersistentSingleton<PlayManager>
    {
        #region inspector

        public Transform dropItem;
        public Transform dropToolItem;
        public Transform dropNumItem;

        //public Transform dmgHpItem;
        //public Transform dmgMissItem;
        //public Transform dmgCritItem;
        //public Transform recoverHpItem;

        #endregion inspector

        public bool firstLoad = true;

        // 英雄
        public Hero mHero;

        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            InitPool();
            double timestamp = TimeUtils.TimeNowMilliseconds();

            Debug.Log("DateTime.Now.Date" + TimeUtils.DateTimeFromSeconds(timestamp / 1000));
            Debug.Log("DateTime.Now.TimeOfDay" + DateTime.Now.DayOfYear);
        }

        #region pool
        public void InitPool()
        {
            SpawnPool pool = ObjectPool.GetPool(PoolDefine.POOL_DROPITEM);

            //if (pool.GetPrefabPool(dropItem) == null)
            //{
            // 加载元素池子
            ObjectPool.CreatePrefabPool(PoolDefine.POOL_DROPITEM, dropItem);
            ObjectPool.CreatePrefabPool(PoolDefine.POOL_DROPITEM, dropToolItem);
            ObjectPool.CreatePrefabPool(PoolDefine.POOL_DROPITEM, dropNumItem);

        }

        public DropProp SpawnDropItem()
        {
            Transform go = ObjectPool.Spawn(PoolDefine.POOL_DROPITEM, dropItem);
            DropProp dropProp = go.GetComponent<DropProp>();
            dropProp.OnSpawn();
            return dropProp;
        }

        public void DespawnDropItem(Transform prefab)
        {
            DropProp dropProp = prefab.GetComponent<DropProp>();
            dropProp.OnDespawn();
            // 加载元素池子
            ObjectPool.Despawn(PoolDefine.POOL_DROPITEM, prefab);
        }

        public TextMeshPro SpawnDropNumItem()
        {
            Transform go = ObjectPool.Spawn(PoolDefine.POOL_DROPITEM, dropNumItem);
            return go.GetComponent<TextMeshPro>();
        }

        public void DespawnDropNumItem(Transform prefab)
        {
            // 加载元素池子
            ObjectPool.Despawn(PoolDefine.POOL_DROPITEM, prefab);
        }

        #endregion pool

        /// <summary>
        /// 
        /// </summary>
        public void LoadGameScene()
        {
            StartCoroutine(this.TransToScene(AddressbalePathEnum.UNITY_Main));
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadBattleScene()
        {
            StartCoroutine(this.TransToScene(AddressbalePathEnum.UNITY_Dungeon));
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadPvpScene()
        {
            StartCoroutine(this.TransToScene(AddressbalePathEnum.UNITY_Arena));
        }


        private IEnumerator TransToScene(string scenePath)
        {
            TransitionManager transManager = TransitionManager.I;
            transManager.ShowTransition();

            UITransition uiTransing = transManager.uiTransiton;
            SceneInstance scene = new SceneInstance();

            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(scenePath, LoadSceneMode.Single, false);
            handle.Completed += (obj) =>
            {
                scene = ((AsyncOperationHandle<SceneInstance>)obj).Result;

                Debug.LogWarning($"Load async scene complete{obj.Status}");
            };

            while (!handle.IsDone || !transManager.IsTransionInOver())
            {
                // 在此可使用handle.PercentComplete进行进度展示
                yield return null;
            }

            scene.ActivateAsync();
        }

        /// <summary>
        /// 进地牢的位置
        /// </summary>
        public Vector3 enterDungeonPos = Vector3.zero;


    }

}

