using Boxal.Util;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace Boxal.Game
{
    public class GameManager : Singleton<GameManager>
    {
        #region Variables
        public CinemachineCamera cineCam;

        public Transform spawnPoint;
        private Transform origin;
        #endregion

        #region Properties
        public bool IsGameOver { get; private set; }
        #endregion

        #region Unity Event Methods
        protected override void Awake()
        {
            base.Awake();
            origin = Player.Instance.transform;
        }

        private void Start()
        {
            IsGameOver = true;
        }
        #endregion

        #region Custom Methods
        public void OnGameStart()
        {
            if (!IsGameOver)
                return;
            IsGameOver = false;
            //UI 전환 추가
            //...
            //초반 체력 세팅
            Player.Instance.PlayerInitSettings();

            RoundManager.Instance.StartRound();
            //카메라 연출
            StartCoroutine(CameraWork());
        }
        
        private IEnumerator CameraWork()
        {
            Time.timeScale = 0f;
            cineCam.Follow = spawnPoint;
            yield return new WaitForSecondsRealtime(3f);
            cineCam.Follow = origin;
            Time.timeScale = 1f;
        }

        public void GameOver()
        {
            Debug.Log("게임오버!");
            IsGameOver = true;

        }
        #endregion
    }

}
