using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character;
using Character.Skill;
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
        [SerializeField] private GameObject levelCompletePanel;
        [SerializeField] private List<GameObject> cards;
        [SerializeField] private List<GameObject> selCardPanel;
        [SerializeField] private List<int> cardsRate;
        [SerializeField] private int UnDMaxRate;
        [SerializeField] private int UnDMinRate;

        private int _currentLv;
        private int _stackedLv;
        private int _currentMaxScore;
        private int _currentScore;
        public int totalScore;

        public static bool Selected;
        
        private bool _wait;
        private int _count;

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
            scoreText.text = $"Score: {_currentScore} / {_currentMaxScore} | Total : {totalScore}";
        }

        public void AddScore(int score)
        {
            _count += 1;
            StartCoroutine(AddScoreC(score));
        }

        private IEnumerator AddScoreC(int score)
        {
            if (_wait) yield break;
            _wait = true;
            while (_count > 0)
            {
                _currentScore += score;
                totalScore += score;
                scoreSlider.value = _currentScore;

                if (_currentScore >= _currentMaxScore)
                {
                    _currentLv++;
                    _stackedLv++;
                    yield return StartCoroutine(LevelUpEvent());
                }

                scoreText.text = $"Score: {_currentScore} / {_currentMaxScore} | Total : {totalScore}";
                _count--;
                if (_count == 0)
                {
                    _wait = false;
                }
            }
        }

        private IEnumerator LevelUpEvent()
        {
            while (_stackedLv > 0)
            {
                _stackedLv--;
                List<GameObject> tmpSelCard = new();
                var tmpScore = _currentScore - _currentMaxScore;
                _currentScore = _currentMaxScore;
                scoreText.text = $"Score: {_currentScore} / {_currentMaxScore} | Total : {totalScore}";
                scoreSlider.value = _currentMaxScore;

                // Pause
                Time.timeScale = 0;
                Movement.Instance.localMoveDisable = true;
                CameraMovement.Hide = true;
                ButtonManager.isPaused = true;
                // Pause

                // Pick a random card by rate
                var random = new System.Random();
                var pickedCard = 0;
                while (pickedCard != 2)
                {
                    var randomCard = random.Next(0, cards.Count);
                    var randomCardRate = random.Next(1, 100);

                    var a = GameObject.Find("Character").GetComponent<Flash>().flashAttackDamage;
                    if (randomCardRate > cardsRate[randomCard]) continue;
                    if (a == 0 && (cards[randomCard].transform.name.Contains("SkillAttackDowngrade") ||
                                   cards[randomCard].transform.name.Contains("SkillAttackUpgrade(%)"))) continue;
                    if (GameObject.Find("A").GetComponent<Movement>().moveSpeed > 5 && cards[randomCard].transform.name.Contains("MoveSpeed")) continue;
                    if (pickedCard == 1 && tmpSelCard[0].transform.name.Replace("(Clone)", "") == cards[randomCard].transform.name) continue;

                    tmpSelCard.Add(Instantiate(cards[randomCard], selCardPanel[pickedCard].transform.position,
                        Quaternion.identity, selCardPanel[pickedCard].transform));
                    tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().rate = random.Next(UnDMinRate, UnDMaxRate);
                    
                    if (tmpSelCard[pickedCard].transform.name.Contains("AttackSpeed"))
                    {
                        var atkDelay = GameObject.Find("Character").GetComponent<Attacker>().attackDelay;
                        tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().beforeChange = atkDelay;

                        if (tmpSelCard[pickedCard].transform.name.Contains("Upgrade"))
                            tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().afterChange =
                                atkDelay - atkDelay * (tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().rate / 100f);
                        else
                            tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().afterChange =
                                atkDelay + atkDelay * (tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().rate / 100f);
                    }
                    else if (tmpSelCard[pickedCard].transform.name.Contains("SkillAttackRange"))
                    {
                        var skillRange = GameObject.Find("Character").GetComponent<Flash>().flashAttackDistance;
                        tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().beforeChange = skillRange;

                        if (tmpSelCard[pickedCard].transform.name.Contains("Upgrade"))
                            tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().afterChange =
                                skillRange + skillRange * (tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().rate / 100f);
                        else
                            tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().afterChange =
                                skillRange - skillRange * (tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().rate / 100f);
                    }
                    else if (tmpSelCard[pickedCard].transform.name.Contains("SkillAttack"))
                    {
                        var skillAtk = GameObject.Find("Character").GetComponent<Flash>().flashAttackDamage;
                        tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().beforeChange = skillAtk;

                        if (tmpSelCard[pickedCard].transform.name.Contains("Upgrade"))
                        {
                            if (tmpSelCard[pickedCard].transform.name.Contains("(+)"))
                            {
                                tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().afterChange = skillAtk + 5;
                            }
                            else
                            {
                                tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().afterChange =
                                    skillAtk + skillAtk * (tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().rate / 100f);
                            }
                        }
                        else
                        {
                            tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().afterChange =
                                skillAtk - skillAtk * (tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().rate / 100f);
                        }
                    }
                    else if (tmpSelCard[pickedCard].transform.name.Contains("MoveSpeed"))
                    {
                        var spd = GameObject.Find("A").GetComponent<Movement>().moveSpeed;
                        tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().beforeChange = spd;

                        if (tmpSelCard[pickedCard].transform.name.Contains("Upgrade"))
                            tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().afterChange =
                                spd + spd * (tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().rate / 100f);
                        else
                            tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().afterChange =
                                spd - spd * (tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().rate / 100f);
                    }
                    else if (tmpSelCard[pickedCard].transform.name.Contains("SkillDelay"))
                    {
                        var skillDelay = GameObject.Find("Character").GetComponent<Flash>().rechargeTime;
                        tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().beforeChange = skillDelay;

                        if (tmpSelCard[pickedCard].transform.name.Contains("Upgrade"))
                            tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().afterChange = skillDelay - 1;
                        else
                            tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().afterChange = skillDelay + 1;
                    }

                    tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().afterChange = 
                        (float) (Math.Truncate(tmpSelCard[pickedCard].GetComponent<UpNDownGradeManager>().afterChange * 100) / 100);

                    pickedCard++;
                }

                levelCompletePanel.SetActive(true);

                yield return new WaitUntil(() => Selected);
                // Last
                levelCompletePanel.SetActive(false);
                Selected = false;
                Destroy(tmpSelCard[0]);
                Destroy(tmpSelCard[1]);
                _currentScore = tmpScore;
                _currentMaxScore += _currentMaxScore * maxScoreIRPL / 100;
                scoreSlider.maxValue = _currentMaxScore;
                scoreSlider.value = _currentScore;
                scoreText.text = $"Score: {_currentScore} / {_currentMaxScore} | Total : {totalScore}";

                // Resume
                Time.timeScale = 1;
                Movement.Instance.localMoveDisable = false;
                CameraMovement.Hide = false;
                ButtonManager.isPaused = false;
                // Resume
            }
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