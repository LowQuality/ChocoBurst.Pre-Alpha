using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Management
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int defaultMaxScore;
        [SerializeField] private int maxScoreIRPL;
        [SerializeField] private Slider scoreSlider;
        [SerializeField] private TextMeshProUGUI scoreText;
        
        private int _currentLv;
        private int _currentScore;
        private int _currentMaxScore;
        private int _totalScore;
        
        public static ScoreManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _currentLv = 0;
            _currentScore = 0;
            _currentMaxScore = defaultMaxScore;
            scoreSlider.maxValue = _currentMaxScore;
            scoreSlider.value = _currentScore;
            scoreText.text = $"Score: {_currentScore} / {_currentMaxScore}";
        }
        
        public void AddScore(int score)
        {
            _currentScore += score;
            _totalScore += score;
            scoreSlider.value = _currentScore;
            
            if (_currentScore >= _currentMaxScore)
            {
                _currentLv++;
                _currentScore -= _currentMaxScore;
                _currentMaxScore += _currentMaxScore * maxScoreIRPL / 100;
                scoreSlider.maxValue = _currentMaxScore;
                scoreSlider.value = _currentScore;
            }
            scoreText.text = $"Score: {_currentScore} / {_currentMaxScore}";
        }
    }
}