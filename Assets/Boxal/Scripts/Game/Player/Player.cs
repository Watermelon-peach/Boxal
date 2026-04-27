using Boxal.Util;
using UnityEngine;

namespace Boxal.Game
{
    public class Player : Singleton<Player>
    {
        #region Variables
        public int maxLife = 5;
        private int life = 3;
        private bool isDead = false;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckDistance = 1.1f;

        private Rigidbody rb;
        #endregion

        #region Properties
        public bool IsGrounded { get; private set; }
        public long Atk { get; set; }

        public Orbit Orbit { get; private set; }
        #endregion

        #region Unity Event Methods
        protected override void Awake()
        {
            base.Awake();
            groundLayer = LayerMask.GetMask("Ground");
            rb = GetComponent<Rigidbody>();
            Orbit = GetComponent<Orbit>();
        }

        private void Update()
        {
            GroundCheck();
        }
        #endregion

        #region Custom Methods
        private void GroundCheck()
        {
            IsGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
            Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.red);
        }

        public void TakeDamage()
        {
            if (isDead)
                return;
            life--;

            //Sfx,Vfx,Ui 업데이트
            //...

            if (life <= 0)
                Die();

        }

        private void Die()
        {
            isDead = true;
            GameManager.Instance.isGameOver = true;
        }

        public void AddLife(int amount = 1)
        {
            life += amount;
            if (life + amount >= maxLife) life = maxLife;
            //Ui 업데이트
            //...
        }

        public void OnJump()
        {

        }
        #endregion
    }

}
