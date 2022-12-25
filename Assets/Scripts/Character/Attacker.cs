using System.Collections;
using UnityEngine;

namespace Character
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField] private GameObject hit;
        [SerializeField] private float damage;
        [SerializeField] private float attackDelay;
        [SerializeField] private float attackSDelay;
        [SerializeField] private float locationOffset;
        [SerializeField] private FloatingJoystick joystick;
        
        private float _attackTimer;
        private bool _isAttacking;
        private Vector3 _attackRotation;
        
        private void Update()
        {
            StoreLastAttack();

            _attackTimer += Time.deltaTime;
            if (_attackTimer >= attackDelay)
            {
                StartCoroutine(Attack());
            }
        }
        
        private IEnumerator Attack()
        {
            hit.SetActive(true);
            _isAttacking = true;
            yield return new WaitForSeconds(attackSDelay);
            _isAttacking = false;
            _attackTimer = 0;
            hit.SetActive(false);
        }
        
        private void StoreLastAttack()
        {
            float horizontal;
            float vertical;

#if UNITY_EDITOR
            horizontal = Mathf.CeilToInt(Input.GetAxisRaw("Horizontal")); 
            vertical = Mathf.CeilToInt(Input.GetAxisRaw("Vertical"));
#else
    horizontal = Mathf.CeilToInt(joystick.Horizontal);
    vertical = Mathf.CeilToInt(joystick.Vertical);
#endif
            if (horizontal != 0 || vertical != 0)
            {
                _attackRotation = new Vector3(horizontal, vertical, 0);
            }

            var x = Mathf.CeilToInt(_attackRotation.x);
            var y = Mathf.CeilToInt(_attackRotation.y);

            switch (x)
            {
                // >
                case 1 when y == 0:
                    hit.transform.localPosition = new Vector3(locationOffset, 0, 0);
                    hit.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                // >^
                case 1 when y == 1:
                    hit.transform.localPosition = new Vector3(locationOffset, locationOffset, 0);
                    hit.transform.localRotation = Quaternion.Euler(0, 0, 45);
                    break;
                // <
                case -1 when y == 0:
                    hit.transform.localPosition = new Vector3(-locationOffset, 0, 0);
                    hit.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                // <^
                case -1 when y == 1:
                    hit.transform.localPosition = new Vector3(-locationOffset, locationOffset, 0);
                    hit.transform.localRotation = Quaternion.Euler(0, 0, -45);
                    break;
                // ^
                case 0 when y == 1:
                    hit.transform.localPosition = new Vector3(0, locationOffset, 0);
                    hit.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    break;
                // v
                case 0 when y == -1:
                    hit.transform.localPosition = new Vector3(0, -locationOffset, 0);
                    hit.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    break;
                // >v
                case 1 when y == -1:
                    hit.transform.localPosition = new Vector3(locationOffset, -locationOffset, 0);
                    hit.transform.localRotation = Quaternion.Euler(0, 0, 135);
                    break;
                // <v
                case -1 when y == -1:
                    hit.transform.localPosition = new Vector3(-locationOffset, -locationOffset, 0);
                    hit.transform.localRotation = Quaternion.Euler(0, 0, -135);
                    break;
            } 
        }

        public void Attack(GameObject t)
        {
            t.GetComponent<HpManager>().TakeDamage(damage);
        }
    }
}