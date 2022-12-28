using UnityEngine;

namespace Character.Skill
{
    public class FlashAttackUpdater : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _collider;
        private static float _range;
        private static float _damage;
        private void Awake()
        {
            _collider.size = new Vector2(_range / 5, _range / 5);
        }
        
        public static void Set(float range, float damage)
        {
            _range = range;
            _damage = damage;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
                other.gameObject.GetComponent<HpManager>().TakeDamage(_damage, other.gameObject);
        }
    }
}