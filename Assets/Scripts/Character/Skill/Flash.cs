using System.Collections;
using UnityEngine;

namespace Character.Skill
{
    public class Flash : MonoBehaviour
    {
        public static bool IsCooldown;
        [SerializeField] private Transform player;
        [SerializeField] private FloatingJoystick joystick;
        private Collider2D _bounds;

        private void Start()
        {
            _bounds = GameObject.FindGameObjectWithTag("CameraBounds").GetComponent<Collider2D>();
        }

        private void Update()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            var pp = player.position;

            var hR = Mathf.RoundToInt(horizontal);
            var vR = Mathf.RoundToInt(vertical);
            var newPos = new Vector3(pp.x + horizontal * 2, pp.y + vertical * 2, 0);

            if ((hR == 0 && vR == 0) || !Input.GetKeyDown(KeyCode.E) || IsCooldown ||
                !_bounds.bounds.Contains(newPos)) return;
            player.position = newPos;

            StartCoroutine(Cooldown(5));
        }

        private static IEnumerator Cooldown(int time)
        {
            var bar = GameObject.Find("Camera").GetComponent<CameraMovement>();
            var timer = 0f;
            IsCooldown = true;

            while (timer < time)
            {
                bar.skill1Bar.value = timer / time;
                yield return new WaitForSeconds(1);
                timer++;
            }

            bar.skill1Bar.value = 1f;
            IsCooldown = false;
        }

        public void A()
        {
            var horizontal = joystick.Horizontal;
            var vertical = joystick.Vertical;
            var pp = player.position;

            var hR = Mathf.RoundToInt(horizontal);
            var vR = Mathf.RoundToInt(vertical);
            var newPos = new Vector3(pp.x + horizontal * 2, pp.y + vertical * 2, 0);

            if ((hR == 0 && vR == 0) || IsCooldown || !_bounds.bounds.Contains(newPos)) return;
            player.position = newPos;

            StartCoroutine(Cooldown(5));
        }
    }
}