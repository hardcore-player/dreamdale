using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

namespace Druid
{
    public class ObjectPool
    {
        public static void CreatePool(string poolName)
        {
            if (PoolManager.Pools.ContainsKey(poolName))
            {
                return;
            }

            SpawnPool pool = PoolManager.Pools.Create(poolName);
        }

        public static SpawnPool GetPool(string poolName)
        {
            if (!PoolManager.Pools.ContainsKey(poolName))
            {
                CreatePool(poolName);
            }

            return PoolManager.Pools[poolName];
        }

        public static void CreatePrefabPool(string poolName, Transform Prefab)
        {
            SpawnPool pool = GetPool(poolName);
            if (pool.GetPrefabPool(Prefab) != null)
            {
                return;
            }

            PrefabPool prefabPool = new PrefabPool(Prefab);
            prefabPool.preloadAmount = 20; // This is the default so may be omitted
            prefabPool.cullDespawned = true;
            prefabPool.cullAbove = 100;
            prefabPool.cullDelay = 1;
            prefabPool.limitInstances = true;
            prefabPool.limitAmount = 1000;
            prefabPool.limitFIFO = true;

            pool.CreatePrefabPool(prefabPool);
        }


        public static Transform Spawn(string poolName, Transform Prefab)
        {
            SpawnPool pool = GetPool(poolName);

            if (pool.GetPrefabPool(Prefab) == null)
            {
                CreatePrefabPool(poolName, Prefab);
            }

            Transform inst = pool.Spawn(Prefab, Vector3.zero, Quaternion.identity);

            return inst;
        }

        public static void Despawn(string poolName, Transform Prefab)
        {
            SpawnPool pool = GetPool(poolName);
            pool.Despawn(Prefab);
        }

        public static void DespawnAll(string poolName)
        {
            SpawnPool pool = GetPool(poolName);
            pool.DespawnAll();
        }

        public static void DestroyPool(string poolName)
        {
            PoolManager.Pools.Destroy(poolName);

        }

        public static void RemovePool(string poolName)
        {
            // SpawnPool pool = GetPool(poolName);
            // PoolManager.Pools.Remove(pool);

        }
    }
}