using UnityEngine;

namespace Character.UI
{
    public class Bat : MonoBehaviour
    {
        public void EndAni()
        {
            GameObject.Find("Character").GetComponent<Attacker>().AniEnd();
        }
    }
}