using Boxal.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Boxal.Game
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        #region Variables
        public List<Boxmon> aliveBoxmons;
        public int currentEnemies = 0;
        [SerializeField] private GameObject boxmonPrefab;

        private float yOffset = 2f;
        private Transform lastSpawned;
        private float initOffset = 20f;

        private GameObjectPool pool;
        private TransformSnapshot snapshot;
        #endregion


        #region Unity Event Methods
        private void Start()
        {
            pool = new GameObjectPool(boxmonPrefab, 20, transform);
            aliveBoxmons = new List<Boxmon>();
            snapshot = new TransformSnapshot();
            snapshot.SaveSnapshot(boxmonPrefab);
        }

        #endregion
        
        #region Custom Methods
        public void Spawn(long maxHp)
        {
            currentEnemies++;
            GameObject obj = pool.Get();

            //Boxmon 개별 체력 설정
            Boxmon boxmon = obj.GetComponent<Boxmon>();
            boxmon.MaxHp = maxHp;
            boxmon.ResetBox();


            Vector3 spawnPos;

            if (lastSpawned == null)
            {
                // 첫 스폰이면 기본 위치 + 20f
                spawnPos = Player.Instance.transform.position + Vector3.up * initOffset;
            }
            else
            {
                // 마지막 오브젝트 위치 기준 + y
                spawnPos = lastSpawned.position + Vector3.up * yOffset;
            }

            obj.transform.position = spawnPos;
            lastSpawned = obj.transform;

            aliveBoxmons.Add(boxmon);
        }

        public void Despawn(Boxmon boxmon)
        {
            currentEnemies--;
            if (currentEnemies == 0) lastSpawned = null;
            snapshot.ResetState(boxmon);
            pool.Release(boxmon.gameObject);
        }
        #endregion

    }

}
