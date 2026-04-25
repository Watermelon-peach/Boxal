using System.Collections.Generic;
using UnityEngine;

namespace Boxal.Game
{
    public class Orbit : MonoBehaviour
    {
        public Transform player;
        public List<Transform> weapons;
        public float radius = 2f;
        public float rotationSpeed = 100f;
        public GameObject weaponPrefab;
        float currentRotation;

        private void Start()
        {
            AddWeapon();
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
                weapon.position = player.position + offset;

                // 엉덩이가 플레이어 향하게
                Vector3 dir = player.position - weapon.position;
                weapon.up = -dir.normalized;
            }
        }

        public void AddWeapon()
        {
            Transform weapon = Instantiate(weaponPrefab).transform;
            weapons.Add(weapon);
        }
    }

}
