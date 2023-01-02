using System.Collections;
using UnityEngine;

namespace Character.Skill
{
    public class Flash : MonoBehaviour
    {
        public static bool IsCooldown;
        [SerializeField] private Transform player;
        [SerializeField] private FloatingJoystick joystick;
        public int rechargeTime;
        [SerializeField] private GameObject flashAttack;
        public float flashAttackDistance;
        public float flashAttackDamage;
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

            StartCoroutine(FlashAttack());
            StartCoroutine(Cooldown(rechargeTime));
        }

        private static IEnumerator Cooldown(int time)
        {
            var bar = GameObject.Find("Camera").GetComponent<CameraMovement>();
            var timer = 0f;
            IsCooldown = true;

            while (timer < time)
            {
                bar.skill1Bar.value = timer / time;
                yield return new WaitForSeconds(1f);
                timer++;
            }

            bar.skill1Bar.value = 1f;
            IsCooldown = false;
        }
        
        private IEnumerator FlashAttack()
        {
            FlashAttackUpdater.Set(flashAttackDistance, flashAttackDamage);
            var flash = Instantiate(flashAttack, player.position, Quaternion.identity);
            
            yield return new WaitForSeconds(0.5f);
            Destroy(flash);
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

            StartCoroutine(FlashAttack());
            StartCoroutine(Cooldown(rechargeTime));
        }
    }
}