using System.Collections;
using UnityEngine;

namespace Character
{
    public class Movement : MonoBehaviour
    {
        private static readonly int DirX = Animator.StringToHash("DirX");
        private static readonly int DirY = Animator.StringToHash("DirY");
        private static readonly int Walking = Animator.StringToHash("Walking");

        public static Movement Instance;

        public bool localMoveDisable;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Animator animator;
        [SerializeField] private BoxCollider2D boxCollider;
        [SerializeField] private LayerMask layerMask;

        [SerializeField] private Collider2D topCollider;

        [SerializeField] private FloatingJoystick joystick;

        private Vector3 _v;
        public static bool GlobalMoveDisable { get; set; }

        private void Awake()
        {
            Instance = this;

            Application.targetFrameRate = 360;
        }

        private void FixedUpdate()
        {
            if (GlobalMoveDisable || localMoveDisable) return; // Disable movement if either is true
            float horizontal;
            float vertical;

#if UNITY_EDITOR
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
#else
    horizontal = joystick.Horizontal;
    vertical = joystick.Vertical;
#endif

            var sr = GetComponent<SpriteRenderer>();
            if (horizontal > 0)
                sr.flipX = false;
            else if (horizontal < 0) sr.flipX = true;

            _v = new Vector3(horizontal, vertical, 0).normalized;
            GameObject.Find("A").GetComponent<Rigidbody2D>().velocity = _v * moveSpeed;
        }

        private IEnumerator Move()
        {
            // if (_v.x != 0 || _v.y != 0)
            // {
            //     var start = transform.position;
            //     var end = start + _v;
            //     
            //     boxCollider.enabled = false;
            //     var hit = Physics2D.Linecast(start, end, layerMask);
            //     boxCollider.enabled = true;
            //
            //     if (hit.transform != null)
            //     {
            //         // animator.SetBool(Walking, false);
            //         yield break;
            //     }
            //     if (_v != Vector3.zero)
            //     {
            //         // animator.SetFloat(DirX, _v.x);
            //         // animator.SetFloat(DirY, _v.y);
            //         // animator.SetBool(Walking, true);
            //     }
            //
            //     transform.Translate(_v * (moveSpeed * Time.smoothDeltaTime));
            //     yield return new WaitForSeconds(0.1f);
            // }
            // else
            // {
            //     // animator.SetBool(Walking, false);
            // }

            GetComponent<Rigidbody2D>().velocity = _v * moveSpeed;
            yield break;
        }
    }
}