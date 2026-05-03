using UnityEngine;

namespace Boxal.Game
{
    /// <summary>
    /// 차징점프 클래스
    /// </summary>
    public class ChargeJump : MonoBehaviour
    {
        #region Variables
        public Rigidbody rb;

        public float minForce = 5f;
        public float maxForce = 15f;
        public float maxChargeTime = 2f;

        public float fallMultiplier = 2.5f;

        private float chargeTime;
        private bool isCharging;

        [Header("게이지UI")]
        [SerializeField] private ImgsFillDynamic imgsFill;
        private CanvasGroup group;
        #endregion


        #region Unity Event Methods
        private void Awake()
        {
            group = imgsFill.GetComponent<CanvasGroup>();
        }
        void Update()
        {
            if (isCharging)
            {
                chargeTime += Time.deltaTime;
                chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);
                // Image fillAmount 구현
                float ratio = chargeTime / maxChargeTime;
                imgsFill.SetValue(ratio,true);
            }
        }

        void FixedUpdate()
        {
            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }
        }
        #endregion

        #region Custom Methods
        public void StartCharge()
        {
            isCharging = true;
            chargeTime = 0f;
            group.alpha = 1f;
        }

        public void ReleaseJump()
        {
            group.alpha = 0;
            if (!isCharging|| !Player.Instance.IsGrounded)
                return;

            imgsFill.SetValue(0, true);
            isCharging = false;

            float ratio = chargeTime / maxChargeTime;
            float force = Mathf.Lerp(minForce, maxForce, ratio);

            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
        }
        #endregion

    }

}