using System.Collections.Generic;
using UnityEngine;

namespace Boxal.Game
{
    public class Orbit : MonoBehaviour
    {
        #region Variables
        public Transform player;
        public List<Transform> weapons;
        public float radius = 2f;
        public float rotationSpeed = 100f;
        public GameObject weaponPrefab;
        private float currentRotation;
        [SerializeField] private int maxWeaponCount = 6;
        private int currentWeaponCount = 0;
        #endregion

        #region Unity Event Methods
        private void Start()
        {
            //AddWeapon();
        }

        private void Update()
        {
            currentRotation += rotationSpeed * Time.deltaTime;

            float angleStep = 360f / weapons.Count;

            for (int i = 0; i < weapons.Count; i++)
            {
                float angle = currentRotation + angleStep * i;
                float rad = angle * Mathf.Deg2Rad;

                Vector3 offset = new Vector3(
                    Mathf.Cos(rad),
                    Mathf.Sin(rad),
                    0
                ) * radius;

                Transform weapon = weapons[i];
                weapon.localPosition = offset;

                Vector3 dir = -weapon.localPosition;
                weapon.up = -dir.normalized;
            }
        }
        #endregion

        #region Cutom Methods
        public void AddWeapon()
        {
            if (currentWeaponCount >= maxWeaponCount)
                return;

            currentWeaponCount++;
            Transform weapon = Instantiate(weaponPrefab, player).transform;
            weapons.Add(weapon);
        }
        #endregion

    }

}
