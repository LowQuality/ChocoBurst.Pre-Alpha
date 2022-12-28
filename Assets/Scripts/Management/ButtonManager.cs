using System.Collections;
using Character;
using Character.Skill;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Management
{
    public class ButtonManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject scoreMenu;
        [SerializeField] private GameObject pauseButton;

        [SerializeField] private TMP_InputField username;
        [SerializeField] private Button postButton;
        
        public static bool IsPaused;
        
        public void Retry()
        {
            Time.timeScale = 1;
            Movement.Instance.localMoveDisable = false;
            Flash.IsCooldown = false;
            CameraMovement.Hide = false;

            SceneManager.LoadScene("Game");
        }

        public void Pause()
        {
            Time.timeScale = 0;
            Movement.Instance.localMoveDisable = true;
            CameraMovement.Hide = true;
            scoreMenu.SetActive(false);
            pauseMenu.SetActive(true);
            pauseButton.SetActive(false);
            IsPaused = true;
        }
        
        public void Resume()
        {
            Time.timeScale = 1;
            Movement.Instance.localMoveDisable = false;
            CameraMovement.Hide = false;
            scoreMenu.SetActive(true);
            pauseMenu.SetActive(false);
            pauseButton.SetActive(true);
            IsPaused = false;
        }

        public void GameStart()
        {
            Time.timeScale = 1;
            Flash.IsCooldown = false;
            CameraMovement.Hide = false;

            SceneManager.LoadScene("Game");
        }
        
        public void MainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Title");
        }
        
        public void PostScore()
        {
            StartCoroutine(PostScoreC());
        }

        private IEnumerator PostScoreC()
        {
            username.interactable = false;
            postButton.interactable = false;
            var tried = 0;
            while (tried <= 5)
            {
                if (LeaderboardManager.LastCode == 201) yield break;
                StartCoroutine(LeaderboardManager.PostScore(username.text, ScoreManager.Instance.totalScore));
                tried++;
                yield return new WaitForSeconds(1f);
            }
            username.interactable = true;
            postButton.interactable = true;
        }

        public void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            
            if (!IsPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }
}