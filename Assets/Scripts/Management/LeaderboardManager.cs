using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Management
{
    public class LeaderboardManager : MonoBehaviour
    {
        // GetIP
        [SerializeField] private TMP_InputField ipInput;
        [SerializeField] private Button ipButton;
        private static string _url;
        public static int LastCode;
        
        // Leaderboard
        [SerializeField] private GameObject leaderboard;
        [SerializeField] private GameObject leaderboardLoading;
        [SerializeField] private GameObject leaderboardContent;
        [SerializeField] private GameObject leaderboardEntryPrefab;
        // [SerializeField] private 
        private static string _storedDataText;
        private static Dictionary<string, object> _storedData;
        public static Dictionary<string, int> StoredScore;

        // Connect Server
        public void ConnectToServer()
        {
            _url = ipInput.text;
            StartCoroutine(WaitForRequestIsOk());
        }

        private static IEnumerator WaitForRequestIsOk()
        {
            var request = UnityWebRequest.Get($"{_url}/ok");
            yield return request.SendWebRequest();

            if (request.responseCode == 200)
            {
                LastCode = (int)request.responseCode;
                _url = $"{_url}";
            }
            else
            {
                ToastMessage.Send(request.error);
                LastCode = (int)request.responseCode;
                _url = null;
            }
        }

        // Get Leaderboard
        public static IEnumerator GetLeaderboard()
        {
            if (_url == null)
            {
                ToastMessage.Send("Server is not connected");
                yield break;
            }

            var request = UnityWebRequest.Get($"{_url}/leaderboard");
            yield return request.SendWebRequest();

            if (request.responseCode == 200)
            {
                LastCode = (int)request.responseCode;
                _storedDataText = request.downloadHandler.text;
                _storedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(_storedDataText);
            }
            else
            {
                LastCode = (int)request.responseCode;
                ToastMessage.Send("Error while getting leaderboard");
            }
            
            var scores = StringToDictionary(_storedData["Scores"].ToString());
            scores = scores.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            StoredScore = scores;
        }
        
        // Post Score
        public static IEnumerator PostScore(string name, int score)
        {
            var data = $"{{ \"{name}\": {score} }}";
            var dataBytes = Encoding.UTF8.GetBytes(data);
            
            if (_url == null)
            {
                ToastMessage.Send("Server is not connected");
                yield break;
            }

            using var request = new UnityWebRequest($"{_url}/leaderboard", "POST");
            {
                request.uploadHandler = new UploadHandlerRaw(dataBytes);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

                if (request.responseCode == 201)
                {
                    LastCode = (int)request.responseCode;
                    ToastMessage.Send("스코어가 성공적으로 등록되었습니다.");
                }
                else
                {
                    LastCode = (int)request.responseCode;
                    ToastMessage.Send("Error while posting score");
                }
            }
        }

        private IEnumerator GetLeaderboardButtonC()
        {
            LastCode = 0;
            leaderboardLoading.SetActive(true);
            leaderboard.SetActive(true);
            
            var tried = 0;
            while (LastCode != 200)
            {
                if (tried <= 5)
                {
                    StartCoroutine(GetLeaderboard());
                    yield return new WaitForSeconds(1f);

                    if (LastCode == 200)
                    {
                        break;
                    }
                }
                else
                {
                    yield break;
                }
                tried++;
            }

            leaderboardLoading.SetActive(false);

            // Get Data
            foreach (var i in StoredScore)
            {
                Instantiate(leaderboardEntryPrefab, leaderboardContent.transform.position, Quaternion.identity, leaderboardContent.transform);
                TextUpdater.UpdateText((Array.IndexOf(StoredScore.Keys.ToArray(), i.Key) + 1).ToString(), i.Key, i.Value);
            }
        }

        public void GetLeaderboardButton()
        {
            StartCoroutine(GetLeaderboardButtonC());
        }
        
        // Close Leaderboard
        public void CloseLeaderboard()
        {
            StopAllCoroutines();
            var childList = leaderboardContent.GetComponentsInChildren<Transform>();
            if (childList != null) {
                for (var i = 1; i < childList.Length; i++) {
                    if (childList[i] != transform)
                        Destroy(childList[i].gameObject);
                }
            }

            leaderboard.SetActive(false);
        }

        private void Start()
        {
            StartCoroutine(WaitForRequestIsOk());
        }

        private void FixedUpdate()
        {
            if (LastCode == 200)
            {
                ipButton.interactable = false;
                ipInput.interactable = false;
            }
            else
            {
                ipButton.interactable = true;
                ipInput.interactable = true;
            }
        }
        
        private static Dictionary<string, int> StringToDictionary(string str)
        {
            str = str.Replace("{", "");
            str = str.Replace("}", "");
            str = str.Replace("\"", "");
            str = str.Replace("  ", "");
            str = str.Replace("\n", "");
            // Delete Dog Trash Characters
            
            var strArray = str.Split(',');
            var tmp = strArray.Select(s => s.Split(": ")).ToDictionary(t => t[0], t => int.Parse(t[1]));
            
            return tmp;
        }
    }
}