using Boxal.Util;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Boxal.Game
{
    public class Boxmon : MonoBehaviour
    {
        #region Variables
        public TextMeshPro hpTmp;
        public Material boxMat;

        [SerializeField] private long maxHp = 0;
        private long currentHp = 0;
        private Color startColor;
        #endregion

        #region Properties
        public long MaxHp
        { 
            get { return maxHp; }
            set { maxHp = value; } 
        }
        #endregion

        #region Unity Event Methods
        private void Awake()
        {
            startColor = boxMat.color;
        }
        private void Start()
        {
            
        }
        private void OnEnable()
        {
            currentHp = maxHp;
            UpdateHpText();
        }

        private void Update()
        {
            //TODO: 테스트 인풋
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                Debug.Log("키 입력");
                TakeDamage(2);
            }
        }
        #endregion

        #region Custom Methods
        public void TakeDamage(long dmg)
        {
            if(currentHp - dmg <= 0)
            {
                BreakBox();
                return;
            }
            //대미지 처리
            currentHp -= dmg;
            //Fx (오브젝트 색, 숫자 표시)
            //mpb 사용

            UpdateHpText();
        }
        private void UpdateHpText()
        {
            //UI 업데이트
            hpTmp.text = NumberUtil.FormatNumber(currentHp);
        }

        private void BreakBox()
        {
            //파편화


            //TODO: 오브젝트 풀 Return 구현시 대체
            gameObject.SetActive(false);
        }
        #endregion
    }

}
