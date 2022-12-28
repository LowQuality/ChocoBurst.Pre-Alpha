using System;
using System.Collections;
using Enemy;
using Management;
using UnityEngine;

namespace Character
{
    public class HpManager : MonoBehaviour
    {
        [SerializeField] private float maxHp;
        [SerializeField] private float hpRegenTime;
        [SerializeField] private float hpRegenAmount;
        [SerializeField] private float invincibilityTime;
        [SerializeField] private float invincibilityFlashTime;
        [SerializeField] private GameObject gameOverScreen;
        [SerializeField] private bool isEnemy;

        public bool isInvincibility;

        private float _currentHp;
        private float _hpRegenTimer;
        private bool _isDamaged;

        private Material _meshRenderer;

        private void Awake()
        {
            try
            {
                _meshRenderer = gameObject.GetComponent<SpriteRenderer>().material;
            }
            catch (Exception e)
            {
                _meshRenderer = GameObject.Find("A").GetComponent<SpriteRenderer>().material;
            }
        }

        private void Start()
        {
            _currentHp = maxHp;
            _hpRegenTimer = hpRegenTime;
        }

        private void Update()
        {
            if (isEnemy) return;
            var bar = GameObject.Find("Camera").GetComponent<CameraMovement>();
            bar.hpBar.value = _currentHp / maxHp;

            if (!(_currentHp <= 0)) return;
            GameOver();
        }

        private void FixedUpdate()
        {
            if (!(_currentHp < maxHp) || _isDamaged) _hpRegenTimer = hpRegenTime;
            _hpRegenTimer -= Time.deltaTime;
            if (!(_hpRegenTimer <= 0)) return;
            _currentHp += hpRegenAmount;
            _hpRegenTimer = hpRegenTime;

            if (_currentHp > maxHp) _currentHp = maxHp;
        }

        private void GameOver()
        {
            if (gameOverScreen.activeSelf) return;
            Time.timeScale = 0;
            Movement.Instance.localMoveDisable = true;
            CameraMovement.Hide = true;

            gameOverScreen.SetActive(true);
        }

        public void TakeDamage(float damage, GameObject enemy = null)
        {
            if (_isDamaged || isInvincibility || damage <= 0) return;
            _currentHp -= damage;
            _isDamaged = true;
            StartCoroutine(DamagedEffect());
            Invoke(nameof(ResetDamage), invincibilityTime);

            if (!isEnemy || enemy == null || !(_currentHp <= 0)) return;
            StopCoroutine(DamagedEffect());
            CancelInvoke(nameof(ResetDamage));
            ScoreManager.Instance.AddScore(enemy.GetComponent<Tracker>().dropScore);
            Destroy(gameObject);
        }

        public void Heal(float heal)
        {
            _currentHp += heal;
            if (_currentHp > maxHp) _currentHp = maxHp;
        }

        private IEnumerator DamagedEffect()
        {
            _meshRenderer.color = new Color(1, 0, 0, 1);
            yield return new WaitForSeconds(0.5f);
            _meshRenderer.color = new Color(1, 1, 1, 1);

            while (_isDamaged)
            {
                _meshRenderer.color = new Color(1, 1, 1, 0.5f);
                yield return new WaitForSeconds(invincibilityFlashTime);
                _meshRenderer.color = new Color(1, 1, 1, 1f);
                yield return new WaitForSeconds(invincibilityFlashTime);
            }
        }

        private void ResetDamage()
        {
            _isDamaged = false;
            StopAllCoroutines();
            _meshRenderer.color = new Color(1, 1, 1, 1f);
        }
    }
}