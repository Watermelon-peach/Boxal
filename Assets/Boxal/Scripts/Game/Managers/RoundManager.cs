using UnityEngine;
using Boxal.Util;
using System.Collections;
using Boxal.Game.UI;

namespace Boxal.Game
{
    public class RoundManager : Singleton<RoundManager>
    {
        #region Variables
       

        private uint roundCount = 0;
        [SerializeField] private float roundDuration = 30f;
        [SerializeField] private int enemiesPerRound = 5;
        [Header("라운드 타이머")]
        [SerializeField] private AnimationCurve curve;
        private Color startColor;
        [SerializeField] private Color targetColor = Color.red;
        #endregion

        #region Unity Event Methods
        private void Start()
        {
            startColor = UiManager.Instance.timerUi.color;
        }
        #endregion

        #region Custom Methods
        private IEnumerator RoundTimer()
        {
            float timer = 0f;
            while (timer <= roundDuration)
            {
                timer += Time.deltaTime;
                float t = curve.Evaluate(timer / roundDuration);
                UiManager.Instance.timerUi.text = ((int)(roundDuration - timer + 1)).ToString();
                UiManager.Instance.timerUi.color = Color.Lerp(startColor, targetColor, t);
                float scale = Mathf.Lerp(1f, 2f, t);
                UiManager.Instance.timerUi.transform.localScale = Vector3.one * scale;

                //타이머끝나기 전에 클리어 시 바로 탈출
                if (SpawnManager.Instance.currentEnemies == 0)
                {
                    StartRound();
                    yield break;
                }

                yield return null;
            }
            StartRound();
            
        }
        public void StartRound()
        {
            roundCount++;
            ShowRoundCount();
            //5라운드마다 보스 스폰
            if(roundCount%5 == 0)
            {
                //TODO: 보스 크기, 색 변경
                SpawnManager.Instance.Spawn(roundCount * enemiesPerRound * 2);
                //BossAlert
            }
            else
            {
                for (int i = 0; i < enemiesPerRound; i++)
                {
                    SpawnManager.Instance.Spawn(roundCount);
                }
            }
                
            StartCoroutine(RoundTimer());
        }
        private void ShowRoundCount()
        {
            UiManager.Instance.roundUi.text = "Round " + roundCount.ToString();
        }

        #endregion
    }

}
