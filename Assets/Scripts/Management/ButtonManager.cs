using Character;
using Character.Skill;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Management
{
    public class ButtonManager : MonoBehaviour
    {
        
        
        public void Retry()
        {
            Time.timeScale = 1;
            Movement.Instance.localMoveDisable = false;
            Flash.IsCooldown = false;
            CameraMovement.Hide = false;
            
            SceneManager.LoadScene("Game");
        }
    }
}