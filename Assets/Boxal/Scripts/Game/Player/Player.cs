using Boxal.Util;
using System.Collections;
using UnityEngine;

namespace Boxal.Game
{
    public class Player : Singleton<Player>
    {
        #region Variables
        public int maxLife = 6;
        private int life = 0;
        private bool isDead = false;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckDistance = 1.1f;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private float enemyCheckDistance = 1.1f;

        [Header("무적시간")]
        [SerializeField] private float invulnerabilityPeriod = 3f;
        [SerializeField] private float blinkingInterval = 0.25f;
        private bool isInvulnerable = false;
        private Renderer rend;

        private Rigidbody rb;
        #endregion

        #region Properties
        public bool IsGrounded { get; private set; }
        public bool IsEnemyDetected { get; private set; }
        public long Atk { get; set; }

        public Orbit Orbit { get; private set; }
        public Rigidbody detectedEnemyRb { get; private set; }
        #endregion

        #region Unity Event Methods
        protected override void Awake()
        {
            base.Awake();
            groundLayer = LayerMask.GetMask("Ground");
            enemyLayer = LayerMask.GetMask("Enemy");

            rb = GetComponent<Rigidbody>();
            Orbit = GetComponent<Orbit>();

            rend = GetComponent<Renderer>();
        }

        private void Update()
        {
            GroundCheck();
            EnemyCheck();
            //Debug.Log(IsGrounded);
            if (IsEnemyDetected && IsGrounded && !isInvulnerable)
            {
                TakeDamage(1);
            }
                
        }
        #endregion

        #region Custom Methods
        public void PlayerInitSettings()
        {
            isDead = false;
            //Debug.Log("추가 전" + life);
            AddLife(3);
            //Debug.Log("추가 후" + life);
            Atk = 1;
        }
        
        //무적타임
        private IEnumerator StartInvulnerabilityPeriod()
        {
            isInvulnerable = true;
            float timer = 0f;
            bool isVisible = true;
            while (timer <= invulnerabilityPeriod)
            {
                isVisible = !isVisible;
                rend.enabled = isVisible;
                timer += blinkingInterval;
                yield return new WaitForSeconds(blinkingInterval);
            }
            rend.enabled = true;
            isInvulnerable = false;
        }

        private void EnemyCheck()
        {
            RaycastHit hit;

            IsEnemyDetected = Physics.Raycast(transform.position, Vector3.up,out hit, enemyCheckDistance, enemyLayer);
            if (IsEnemyDetected)
            {
                detectedEnemyRb = hit.collider.attachedRigidbody;
            }
            else
            {
                detectedEnemyRb = null;
            }
        }

        private void GroundCheck()
        {
            IsGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
            //Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.red);
        }

        public void TakeDamage(int amount)
        {
            if (isDead)
                return;

            //Debug.Log("life left" + life);
            life -= amount;

            //Sfx,Vfx,Ui 업데이트
            //...
            Orbit.RemoveWeapon(amount);
            if (life <= 0)
            {
                Die();
                return;
            }
            StartCoroutine(StartInvulnerabilityPeriod());
        }

        private void Die()
        {
            Debug.Log("죽음!");
            isDead = true;
            GameManager.Instance.GameOver();
        }

        public void AddLife(int amount = 1)
        {
            if (isDead)
                return;

            //서순 반대로 + else 안넣었다가 엉망잔칭됐었음
            if (life + amount >= maxLife) life = maxLife;
            else life += amount;

            Orbit.AddWeapon(amount);
            //Ui 업데이트
            //...
        }
        #endregion
    }

}
