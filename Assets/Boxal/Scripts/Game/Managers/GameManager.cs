using Boxal.Util;
using UnityEngine;

namespace Boxal.Game
{
    public class GameManager : Singleton<GameManager>
    {
        #region Variables
        public bool isGameOver = false;
        #endregion

        #region Unity Event Methods
        #endregion

        #region Custom Methods
        public void OnGameStart()
        {
            //UI 전환 추가
            //...
            //초반 무기 세팅
            Player.Instance.Orbit.AddWeapon();
            Player.Instance.Atk = 1;
            RoundManager.Instance.StartRound();
        }
        #endregion
    }

}
