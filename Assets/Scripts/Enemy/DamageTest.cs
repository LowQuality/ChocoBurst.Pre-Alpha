using Character;
using UnityEngine;

namespace Enemy
{
    public class DamageTest : MonoBehaviour
    {
        private bool _isPlayerInRange;

        private void Update()
        {
            if (!_isPlayerInRange) return;
            if (transform.parent.name.Contains("Enemy1"))
            {
                GameObject.Find("Character").GetComponent<HpManager>().TakeDamage(10);
            } else if (transform.parent.name.Contains("Enemy2"))
            {
                GameObject.Find("Character").GetComponent<HpManager>().TakeDamage(35);
            }
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