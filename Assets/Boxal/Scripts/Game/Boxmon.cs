using Boxal.Util;
using MoreMountains.Feedbacks;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Boxal.Game
{
    public class Boxmon : MonoBehaviour
    {
        #region Variables
        [Header("대미지처리")]
        public TextMeshPro hpTmp;
        public Material boxMat;
        [SerializeField] private MMF_Player ftPlayer;
        [SerializeField] private Transform ftTransform;

        [Header("파괴 연출")]
        public GameObject originalObject;
        public GameObject demolished;
        [SerializeField] private float despawnDelay = 2f;

        [Header("*밸런싱용 임시 직렬화")]
        [SerializeField] private long maxHp = 1;
        
        //TODO: 테스트 인풋 대미지
        [SerializeField] private long tempDmg = 99;
        private long currentHp = 0;


        private Color startColor;
        [SerializeField] private Color targetColor;
        private Color currentColor;

        private Renderer rend;
        private MaterialPropertyBlock mpb;
        private MMF_Player feelPlayer;


        //참조
        private Rigidbody rb;
        private Collider originalCollider;
        #endregion

        #region Properties
        public long MaxHp
        { 
            get { return maxHp; }
            set { maxHp = value; } 
        }

        public bool IsDead { get; private set; }

        public Transform[] Transforms { get; private set; }
        public Rigidbody[] Rigidbodies { get; private set; }
        #endregion

        #region Unity Event Methods
        private void Awake()
        {
            startColor = boxMat.color;
            originalCollider = GetComponent<Collider>();
            rb = GetComponent<Rigidbody>();
            rend = originalObject.GetComponent<Renderer>();
            mpb = new MaterialPropertyBlock();
            feelPlayer = GetComponent<MMF_Player>();

            Transforms = GetComponentsInChildren<Transform>(true);
            Rigidbodies = GetComponentsInChildren<Rigidbody>(true);
        }
        private void Start()
        {
            //ResetBox();
        }


        private void Update()
        {
            //TODO: 테스트 인풋
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                Debug.Log("키 입력");
                TakeDamage(tempDmg);
            }
        }

        private void OnEnable()
        {
            ResetBox();
        }
        #endregion

        #region Custom Methods
        public void TakeDamage(long dmg)
        {
            if (IsDead) return;

            if(currentHp - dmg <= 0)
            {
                StartCoroutine(BreakBox());
                return;
            }
            //대미지 처리
            currentHp -= dmg;
            //Fx (오브젝트 색, 숫자 표시)
            MMF_FloatingText floatingTextFeedback = ftPlayer.GetFeedbackOfType<MMF_FloatingText>();
            floatingTextFeedback.Value = $"-{NumberUtil.FormatNumber(dmg)}";
            
            ftPlayer?.PlayFeedbacks(ftTransform.position);

            UpdateHpText();
            feelPlayer?.PlayFeedbacks();
            UpdateColor(dmg);
        }

        private void ResetBox()
        {
            IsDead = false;
            hpTmp.enabled = true;
            originalCollider.enabled = true;
            currentHp = maxHp;
            currentColor = startColor;
            UpdateHpText();
            UpdateColor();
        }

        //막타대미지 고려해야됨 (처형처리 비슷하게)
        private void UpdateColor(long dmg = 0)
        {
            float t = (float)currentHp / (MaxHp - dmg);
            currentColor = Color.Lerp(targetColor, startColor, t);

            //mpb 사용
            rend.GetPropertyBlock(mpb);
            mpb.SetColor("_Color", currentColor);
            rend.SetPropertyBlock(mpb);
        }

        private void UpdateHpText()
        {
            //UI 업데이트
            hpTmp.text = NumberUtil.FormatNumber(currentHp);
        }

        private IEnumerator BreakBox()
        {
            IsDead = true;

            //파편화연출
            //1. rb끄기(Kinematic) 2. 콜라이더 끄기 3. Hp 패널 / original 끄기 4. 파편 키기
            rb.isKinematic = true;
            originalCollider.enabled = false;

            hpTmp.enabled = false;
            originalObject.SetActive(false);
            demolished.SetActive(true);
            //TODO: 오브젝트 풀 Return 구현시 대체
            //gameObject.SetActive(false);
            yield return new WaitForSeconds(despawnDelay);
            SpawnManager.Instance.Despawn(this);
        }
        #endregion
    }

}
