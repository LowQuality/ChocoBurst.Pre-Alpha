using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Management
{
    public class FadeManager : MonoBehaviour
    {
        [SerializeField] private Image whiteFX;
        [SerializeField] private Image blackFX;

        public static FadeManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void WhiteFXFadeOut(float time)
        {
            StartCoroutine(WhiteFXFadeOutCoroutine(time));
        }

        public void BlackFXFadeOut(float time)
        {
            StartCoroutine(BlackFXFadeOutCoroutine(time));
        }

        public void FadeIn(float time)
        {
            StartCoroutine(FadeInCoroutine(time));
        }

        private IEnumerator WhiteFXFadeOutCoroutine(float time)
        {
            if (time == 0)
                whiteFX.color = new Color(1, 1, 1, 1);
            else
                while (whiteFX.color.a < 1.0f)
                {
                    whiteFX.color = new Color(1, 1, 1, whiteFX.color.a + Time.deltaTime / time);
                    yield return null;
                }
        }

        private IEnumerator BlackFXFadeOutCoroutine(float time)
        {
            if (time == 0)
                blackFX.color = new Color(0, 0, 0, 1);
            else
                while (blackFX.color.a < 1.0f)
                {
                    blackFX.color = new Color(0, 0, 0, blackFX.color.a + Time.deltaTime / time);
                    yield return null;
                }
        }

        private IEnumerator FadeInCoroutine(float time)
        {
            if (time == 0)
            {
                blackFX.color = new Color(0, 0, 0, 0);
                whiteFX.color = new Color(1, 1, 1, 0);
            }
            else
            {
                while (blackFX.color.a > 0.0f)
                {
                    blackFX.color = new Color(0, 0, 0, blackFX.color.a - Time.deltaTime / time);
                    whiteFX.color = new Color(1, 1, 1, whiteFX.color.a - Time.deltaTime / time);
                    yield return null;
                }
            }
        }
    }
}