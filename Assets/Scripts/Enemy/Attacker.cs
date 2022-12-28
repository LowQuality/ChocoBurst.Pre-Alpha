using UnityEngine;

namespace Enemy
{
    public class Attacker : MonoBehaviour
    {
        private static readonly int IsAttack = Animator.StringToHash("isAttack");
        public bool isAttack;
        public bool canAttack;
        private Animator _animator;
        private GameObject _damageCollider;
        private Transform _player;
        private Tracker _tracker;

        private void Start()
        {
            _player = GameObject.Find("A").transform;
            _tracker = GetComponent<Tracker>();
            _animator = GetComponent<Animator>();
            _damageCollider = transform.GetChild(0).gameObject;
        }

        private void Update()
        {
            var position = _player.position;
            var position1 = transform.position;
            var direction = Vector2.Distance(position1, position);

            if (!isAttack)
                _damageCollider.transform.localPosition = GetComponent<SpriteRenderer>().flipX
                    ? new Vector3(-Mathf.Abs(_damageCollider.transform.localPosition.x), 0, 0)
                    : new Vector3(Mathf.Abs(_damageCollider.transform.localPosition.x), 0, 0);

            if (!(direction <= _tracker.attackDistance) || isAttack || !_tracker.canMoving || !canAttack) return;
            _tracker.canMoving = false;
            _tracker.disabled = false;
            isAttack = true;
            _animator.SetBool(IsAttack, true);
        }

        public void AniEnd()
        {
            _damageCollider.SetActive(false);
            _animator.SetBool(IsAttack, false);
            isAttack = false;
            _tracker.canMoving = true;

            if (transform.name != "Enemy3(Clone)") return;
            _tracker.disabled = false;
            _tracker.canMoving = false;
            _tracker.ResetSpeed();
        }

        public void EnableCollider()
        {
            _damageCollider.SetActive(true);
        }
    }
}