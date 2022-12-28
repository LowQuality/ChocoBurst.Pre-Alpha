using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Enemy
{
    public class Tracker : MonoBehaviour
    {
        private static readonly int IsWalk = Animator.StringToHash("isWalk");
        [SerializeField] private Rigidbody2D trackerRb;
        [SerializeField] private float speed;
        [SerializeField] private Transform player;
        [SerializeField] private Animator anim;
        public int dropScore;
        public float stopDistance;
        public float attackDistance;
        public bool canMoving = true;
        public bool prowling;
        public bool disabled;
        private float _deg;

        private bool _isStarted;
        private Vector3 _newPos;
        private float _originalSpeed;

        private bool _tmp;

        private SpriteRenderer _spriteRenderer;
        private static readonly int IsAttack = Animator.StringToHash("isAttack");

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalSpeed = speed;
        }

        private void Update()
        {
            var position = player.position;
            var position1 = transform.position;
            var direction = Vector2.Distance(position1, position);

            _spriteRenderer.flipX = canMoving switch
            {
                true when !prowling => position.x < position1.x,
                true when prowling => _newPos.x < position1.x,
                _ => _spriteRenderer.flipX
            };

            if (direction > stopDistance && canMoving && !prowling)
            {
                _newPos = position;
                transform.position = Vector2.MoveTowards(position1, _newPos, speed * Time.deltaTime);
                anim.SetBool(IsWalk, true);
            }
            else if (Vector2.Distance(position1, _newPos) < 0.3f && canMoving && prowling)
            {
                anim.SetBool(IsWalk, false);
            }
            else if (canMoving && prowling)
            {
                transform.position = Vector2.MoveTowards(position1, _newPos, speed * Time.deltaTime);
                anim.SetBool(IsWalk, true);
            }
            else
            {
                trackerRb.velocity = Vector2.zero;
                anim.SetBool(IsWalk, false);
            }

            switch (transform.name)
            {
                case "Enemy3(Clone)" when !_isStarted:
                    StartCoroutine(RandomStopDistance());
                    break;
                case "Enemy3(Clone)" when GetComponent<Attacker>().isAttack:
                    StopCoroutine(RandomStopDistance());
                    break;
            }
        }

        public IEnumerator RandomStopDistance()
        {
            var position = player.position;
            
            if (disabled) yield break;

            anim.SetBool(IsAttack, false);
            _isStarted = true;
            canMoving = true;
            prowling = true;

            var random = Random.Range(0, 100);
            if (random >= 80)
            {
                prowling = false;
                disabled = true;
                canMoving = true;
                speed = speed * 2 + 0.25f;
                GetComponent<Attacker>().isAttack = false;
                GetComponent<Attacker>().canAttack = true;
            }
            else
            {
                _newPos = new Vector3(position.x + Random.Range(-10, 10), position.y + Random.Range(-10, 10), 0);
                GetComponent<Attacker>().canAttack = false;
            }

            yield return new WaitForSeconds(5f);
            _isStarted = false;
        }

        public void ResetSpeed()
        {
            speed = _originalSpeed;
        }
    }
}