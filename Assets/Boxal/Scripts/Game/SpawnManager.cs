using Boxal.Util;
using UnityEngine;

namespace Boxal.Game
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        #region Variables
        [SerializeField] private GameObject boxmonPrefab;
        [SerializeField] private Transform spawnpoint;
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
        public void Spawn()
        {
            GameObject obj = pool.Get();
            obj.transform.position = spawnpoint.position;
        }
        
        public void Despawn(Boxmon boxmon)
        {
            snapshot.ResetState(boxmon);
            pool.Release(boxmon.gameObject);
        }
        #endregion

    }

}
