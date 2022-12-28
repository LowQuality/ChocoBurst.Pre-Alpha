using System;
using System.Linq;
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
        [SerializeField] private TextMeshProUGUI scoreDiff;

        private int _currentLv;
        private int _currentMaxScore;
        private int _currentScore;
        public int totalScore;

        public static ScoreManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            StartCoroutine(LeaderboardManager.GetLeaderboard());
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
            totalScore += score;
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

        private void FixedUpdate()
        {
            if (LeaderboardManager.LastCode is 200 or 201)
            {
                string topColor;
                int top;
                var tmp = LeaderboardManager.StoredScore;
                try
                {
                    tmp["@@@YOU@@@"] = totalScore;
                } catch (Exception e)
                {
                    tmp.Add("@@@YOU@@@", totalScore);
                }
                var sorted = tmp.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                top = sorted.ToList().FindIndex(x => x.Key == "@@@YOU@@@") + 1;
                if (totalScore == 0) top = LeaderboardManager.StoredScore.Count;

                switch (top)
                {
                    case 1:
                        topColor = "ffd700";
                        break;
                    case >= 2:
                    case <= 5:
                        topColor = "c0c0c0";
                        break;
                }
                var topScoreDIff = LeaderboardManager.StoredScore.ElementAt(0).Value - totalScore;
                if (topScoreDIff < 0) topScoreDIff = 0;
                scoreDiff.text = $"당신은 현제 <color=#{topColor}>{top}등</color>입니다 <color=#000000>|</color> <color=#ffd700>1등</color>과의 격차는 <color=#ff0000>{topScoreDIff}점</color> 입니다.";
            }
            else
            {
                scoreDiff.text = "";
            }
        }
    }
}