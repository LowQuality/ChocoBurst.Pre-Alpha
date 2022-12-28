using TMPro;
using UnityEngine;

namespace Management
{
    public class TextUpdater : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI top;
        [SerializeField] private TextMeshProUGUI username;
        [SerializeField] private TextMeshProUGUI score;
        
        public static TextUpdater Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }
        
        public static void UpdateText(string top, string username, int score)
        {
            Instance.top.text = $"#{top}";
            Instance.username.text = username;
            Instance.score.text = $"Score: {score}";
        }
    }
}