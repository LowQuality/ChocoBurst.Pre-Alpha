using Management;
using UnityEngine;

namespace Character
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField] private GameObject hit;
        [SerializeField] private float damage;
        public float attackDelay;
        [SerializeField] private float attackSDelay;
        [SerializeField] private Vector2 locationOffset;
        [SerializeField] private FloatingJoystick joystick;
        [SerializeField] private Animator animator;
        private Vector3 _attackRotation;

        private float _attackTimer;
        private bool _isAttacking;
        private static readonly int IsAttack = Animator.StringToHash("isAttack");

        private void Update()
        {
            StoreLastAttack();

            _attackTimer += Time.deltaTime;
            if (_attackTimer >= attackDelay) Attack();
        }

        private void Attack()
        {
            hit.SetActive(true);
            _isAttacking = true;
            animator.SetBool(IsAttack, true);
        }

        public void AniEnd()
        {
            animator.SetBool(IsAttack, false);
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
    horizontal = Mathf.RoundToInt(joystick.Horizontal);
    vertical = Mathf.RoundToInt(joystick.Vertical);
#endif
            if (horizontal != 0 || vertical != 0) _attackRotation = new Vector3(horizontal, vertical, 0);

            var x = Mathf.CeilToInt(_attackRotation.x);
            var y = Mathf.CeilToInt(_attackRotation.y);

            if (ButtonManager.IsPaused) return;
            switch (x)
            {
                // >
                case 1 when y == 0:
                    hit.transform.localPosition = new Vector3(0.23f, -0.15f, 0);
                    hit.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    hit.GetComponent<SpriteRenderer>().flipX = true;
                    hit.GetComponent<SpriteRenderer>().sortingOrder = 0;
                    break;
                // <
                case -1 when y == 0:
                    hit.transform.localPosition = new Vector3(-0.23f, -0.15f, 0);
                    hit.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    hit.GetComponent<SpriteRenderer>().flipX = false;
                    hit.GetComponent<SpriteRenderer>().sortingOrder = 0;
                    break;
                // v
                case 0 when y == -1:
                    hit.transform.localPosition = new Vector3(-0.12f, -0.16f, 0);
                    hit.transform.localRotation = Quaternion.Euler(0, 0, 180);
                    hit.GetComponent<SpriteRenderer>().flipX = true;
                    hit.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    break;
                // ^
                case 0 when y == 1:
                    hit.transform.localPosition = new Vector3(0.004f, 0.092f, 0);
                    hit.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    hit.GetComponent<SpriteRenderer>().flipX = true;
                    hit.GetComponent<SpriteRenderer>().sortingOrder = 0;
                    break;
                // // >^
                // case 1 when y == 1:
                //     hit.transform.localPosition = new Vector3(locationOffset, locationOffset, 0);
                //     hit.transform.localRotation = Quaternion.Euler(0, 0, 45);
                //     break;
                // // <^
                // case -1 when y == 1:
                //     hit.transform.localPosition = new Vector3(-locationOffset, locationOffset, 0);
                //     hit.transform.localRotation = Quaternion.Euler(0, 0, -45);
                //     break;
                // // >v
                // case 1 when y == -1:
                //     hit.transform.localPosition = new Vector3(locationOffset, -locationOffset, 0);
                //     hit.transform.localRotation = Quaternion.Euler(0, 0, 135);
                //     break;
                // // <v
                // case -1 when y == -1:
                //     hit.transform.localPosition = new Vector3(-locationOffset, -locationOffset, 0);
                //     hit.transform.localRotation = Quaternion.Euler(0, 0, -135);
                //     break;
            }
        }

        public void Attack(GameObject t)
        {
            t.GetComponent<HpManager>().TakeDamage(damage, t);
        }
    }
}