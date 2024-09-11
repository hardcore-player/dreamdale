using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Druid
{
    public class ResManager : Singleton<ResManager>
    {
        #region 同步

        public GameObject GetInstantiateSync(string path, Transform parent = null, bool instantiateInWorldSpace = false)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(path, parent, instantiateInWorldSpace);
            GameObject go = handle.WaitForCompletion();
            return go;
        }

        public T GetResourcesSync<T>(string path) where T : UnityEngine.Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(path);
            return handle.WaitForCompletion();
        }

        #endregion

        #region 异步

        public async Task<GameObject> GetInstantiate(string path, Vector3 pos, Transform parent = null,
    bool instantiateInWorldSpace = false)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(path, pos, Quaternion.identity, parent, instantiateInWorldSpace);
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }

            Debug.Log("GetInstantiateAsync err:" + handle.Status);
            return null;
        }

        public async Task<GameObject> GetInstantiate(string path, Transform parent = null,
            bool instantiateInWorldSpace = false)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(path, parent, instantiateInWorldSpace);
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }

            Debug.Log("GetInstantiateAsync err:" + handle.Status);
            return null;
        }

        public async Task<T> GetResources<T>(string path) where T : UnityEngine.Object
        {
            Debug.Log("GetResources :" + path);
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(path);
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }

            Debug.Log("GetResourcesAsync err:" + handle.Status);
            return null;
        }

        public void InstantiateAsync(string path, Action<AsyncOperationHandle<GameObject>> onCompleted,
            Transform parent = null,
            bool instantiateInWorldSpace = false)
        {
            Addressables.InstantiateAsync(path, parent, instantiateInWorldSpace).Completed += onCompleted;
        }

        public void LoadAssetAsync<T>(string path, Action<AsyncOperationHandle<T>> onCompleted) where T : UnityEngine.Object
        {
            Addressables.LoadAssetAsync<T>(path).Completed += onCompleted;
        }

        #endregion


        public void Release(GameObject gameObject)
        {
            if (!Addressables.ReleaseInstance(gameObject))
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}