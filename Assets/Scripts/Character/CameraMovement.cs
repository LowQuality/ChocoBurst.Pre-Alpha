using Character.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Character
{
    public class CameraMovement : MonoBehaviour
    {
        public static bool StaticBounds;

        public static CameraMovement Instance;
        public bool fixedCamera;
        public GameObject target;
        public BoxCollider2D cameraBounds;
        public Camera thisCamera;
        public float cameraSpeed;

        public Slider hpBar;
        public Slider skill1Bar;

        [SerializeField] private GameObject hpBarPrefab;
        [SerializeField] private Vector3 hpOffset;
        [SerializeField] private GameObject skill1BarPrefab;
        [SerializeField] private Vector3 skill1Offset;
        [SerializeField] private GameObject p;

        public static bool Hide;
        
        private float _halfHeight;
        private float _halfWidth;
        private Vector3 _maxBound;
        private Vector3 _minBound;

        private Vector3 _playerPosition;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            var wUI = GameObject.Find("W_UI").GetComponent<Canvas>();
            var hpBarO = Instantiate(hpBarPrefab, wUI.transform);
            var skill1BarO = Instantiate(skill1BarPrefab, wUI.transform);
            
            hpBar = hpBarO.GetComponent<Slider>();
            skill1Bar = skill1BarO.GetComponent<Slider>();

            hpBarO.GetComponent<Bar>().target = target.transform;
            hpBarO.GetComponent<Bar>().offset = hpOffset;
            skill1BarO.GetComponent<Bar>().target = target.transform;
            skill1BarO.GetComponent<Bar>().offset = skill1Offset;
        }

        private void Update()
        {
            p.SetActive(!Hide);
            
            if (target == null && fixedCamera) return;
            var playerTPosition = target.transform.position;
            var position = transform.position;
            var bounds = cameraBounds.bounds;

            _minBound = bounds.min;
            _maxBound = bounds.max;
            _halfHeight = thisCamera.orthographicSize;
            _halfWidth = thisCamera.aspect * _halfHeight;

            _playerPosition.Set(playerTPosition.x, playerTPosition.y, position.z);
            position = Vector3.Lerp(position, _playerPosition, Time.deltaTime * cameraSpeed);

            var clampedX = Mathf.Clamp(position.x, _minBound.x + _halfWidth, _maxBound.x - _halfWidth);
            var clampedY = Mathf.Clamp(position.y, _minBound.y + _halfHeight, _maxBound.y - _halfHeight);
            position = new Vector3(clampedX, clampedY, position.z);
            transform.position = position;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (StaticBounds) return;
            var bounds = GameObject.FindGameObjectsWithTag("CameraBounds")[0].GetComponent<BoxCollider2D>();
            Instance.SetCameraBounds(bounds);
        }


        public void SetTarget(GameObject t)
        {
            target = t;
        }

        public void SetCameraBounds(BoxCollider2D bounds)
        {
            cameraBounds = bounds;
        }
    }
}