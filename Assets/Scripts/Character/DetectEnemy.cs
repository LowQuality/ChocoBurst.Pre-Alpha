using UnityEngine;

namespace Character
{
    public class DetectEnemy : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
                GameObject.Find("Character").GetComponent<Attacker>().Attack(other.gameObject);
        }
    }
}