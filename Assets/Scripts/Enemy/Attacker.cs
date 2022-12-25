using UnityEngine;

namespace Enemy
{
    public class Attacker : MonoBehaviour
    {
        private static readonly int IsAttack = Animator.StringToHash("isAttack");
        private Animator _animator;
        private bool _isAttack;
        private Transform _player;
        private Tracker _tracker;
        private GameObject _damageCollider;

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

            _damageCollider.transform.localPosition = GetComponent<SpriteRenderer>().flipX
                ? new Vector3(-Mathf.Abs(_damageCollider.transform.localPosition.x), 0, 0)
                : new Vector3(Mathf.Abs(_damageCollider.transform.localPosition.x), 0, 0);

            if (direction <= _tracker.cDistance && !_isAttack && _tracker.canMoving)
            {
                _tracker.canMoving = false;
                _isAttack = true;
                _animator.SetBool(IsAttack, true);
            }
        }

        public void AniEnd()
        {
            _damageCollider.SetActive(false);
            _animator.SetBool(IsAttack, false);
            _tracker.canMoving = true;
            _isAttack = false;
        }

        public void EnableCollider()
        {
            _damageCollider.SetActive(true);
        }
    }
}