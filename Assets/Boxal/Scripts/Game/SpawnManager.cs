using Boxal.Util;
using UnityEngine;

namespace Boxal.Game
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        #region Variables
        public int currentEnemies = 0;
        [SerializeField] private GameObject boxmonPrefab;

        [SerializeField] private Transform spawnpoint;
        private float yOffset = 4f;
        private Transform lastSpawned;


        private GameObjectPool pool;
        private TransformSnapshot snapshot;
        #endregion


        #region Unity Event Methods
        private void Start()
        {
            pool = new GameObjectPool(boxmonPrefab, 20, transform);
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
                // 첫 스폰이면 기본 위치
                spawnPos = spawnpoint.position;
            }
            else
            {
                // 마지막 오브젝트 위치 기준 + y
                spawnPos = lastSpawned.position + Vector3.up * yOffset;
            }

            obj.transform.position = spawnPos;
            lastSpawned = obj.transform;
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
