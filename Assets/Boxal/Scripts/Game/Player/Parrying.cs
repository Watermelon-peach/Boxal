using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Boxal.Game
{
    public class Parrying : MonoBehaviour
    {
        #region Variables
        public Image icon;

        [SerializeField] private float parryCoolDown = 0.5f;
        [SerializeField] private float power = 5f;
        private bool isCoolDown = false;
        #endregion

        #region Custom Method
        public void OnParry()
        {
            if (isCoolDown)
                return;

            if (Player.Instance.IsEnemyDetected)
            {
                foreach (Boxmon boxmon in SpawnManager.Instance.aliveBoxmons)
                {
                    Vector3 lv = boxmon.rb.linearVelocity;
                    boxmon.rb.linearVelocity = new Vector3(lv.x, power, lv.z);
                }
            }

            /*if (Player.Instance.detectedEnemyRb != null)
            {
                Rigidbody rb = Player.Instance.detectedEnemyRb;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, power, rb.linearVelocity.z);
            }*/
            StartCoroutine(CoolDown());
        }
        private IEnumerator CoolDown()
        {
            isCoolDown = true;
            //TODO: UI 연결
            float timer = 0f;
            while (timer <= parryCoolDown)
            {
                timer += Time.deltaTime;
                icon.fillAmount = timer / parryCoolDown;
                yield return null;
            }
            isCoolDown = false;
        }
        #endregion
    }

}
