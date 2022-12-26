using UnityEngine;

namespace Enemy
{
    public class Tracker : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D trackerRb;
        [SerializeField] private float speed;
        [SerializeField] private Transform player;
        [SerializeField] private Animator anim;
        public int dropScore;
        public float cDistance;
        public bool canMoving = true;

        private SpriteRenderer _spriteRenderer;
        private static readonly int IsWalk = Animator.StringToHash("isWalk");

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            var position = player.position;
            var position1 = transform.position;
            var direction = Vector2.Distance(position1, position);

            if (canMoving) _spriteRenderer.flipX = position.x < position1.x;

            if (direction > cDistance && canMoving)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                anim.SetBool(IsWalk, true);
            }
            else
            {
                trackerRb.velocity = Vector2.zero;
                anim.SetBool(IsWalk, false);
            }
        }
    }
}