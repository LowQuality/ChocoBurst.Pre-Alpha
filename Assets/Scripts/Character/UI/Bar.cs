using UnityEngine;

namespace Character.UI
{
    public class Bar : MonoBehaviour
    {
        [SerializeField] private Camera uiCamera;
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private RectTransform hpBar;

        [HideInInspector] public Vector3 offset;
        [HideInInspector] public Transform target;

        private void Start()
        {
            uiCamera = GameObject.Find("Camera").GetComponent<Camera>();
            canvas = GameObject.Find("W_UI").GetComponent<Canvas>();
            rectTransform = canvas.GetComponent<RectTransform>();
            hpBar = GetComponent<RectTransform>();
        }

        private void LateUpdate()
        {
            var screenPos = uiCamera.WorldToScreenPoint(target.position + offset);

            if (screenPos.z < 0) screenPos *= -1;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPos, uiCamera,
                out var localPos);
            hpBar.localPosition = localPos;
        }
    }
}