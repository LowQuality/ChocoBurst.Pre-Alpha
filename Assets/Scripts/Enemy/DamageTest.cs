using Character;
using UnityEngine;

namespace Enemy
{
    public class DamageTest : MonoBehaviour
    {
        private bool _isPlayerInRange;

        [SerializeField] private int damage;

        private void Update()
        {
            if (!_isPlayerInRange) return;
            GameObject.Find("Character").GetComponent<HpManager>().TakeDamage(damage);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player")) _isPlayerInRange = true;
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player")) _isPlayerInRange = false;
        }
    }
}