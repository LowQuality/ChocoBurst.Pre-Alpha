using UnityEngine;

namespace Character.Skill
{
    public class FlashAttackUpdater : MonoBehaviour
    {
        [SerializeField] private Transform transform;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private static float _range;
        private static float _damage;
        private void Awake()
        {
            transform.localScale = new Vector3(_range / 5, _range / 5, 1);
            if (_damage == 0) spriteRenderer.enabled = false;
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