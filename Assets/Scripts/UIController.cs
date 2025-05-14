using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
        public static UIController Instance;
        [SerializeField] private Slider playerHealthSlider;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text scoreText;

        public GameObject gameOverPanel;
        public GameObject pausePanel;

        public GameObject loginPanel;
        public GameObject registerPanel;
        public TextMeshProUGUI usernameDisplay;
        
   


     

        void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(this);
            } else {
                Instance = this;
            }
        
        }

        public void UpdateHealthSlider() {
            playerHealthSlider.maxValue = PlayerController.Instance.playerMaxHealth;
            playerHealthSlider.value = PlayerController.Instance.playerHealth;
            healthText.text = playerHealthSlider.value + " / " + playerHealthSlider.maxValue;
        }

        public void ShowLoggedInUser() {
            usernameDisplay.text = "Logged in as: " +  PlayerPrefs.GetString("username");;
        }

        public void UpdateScoreText(int score)
            {
                scoreText.text = "Score: " + score;
            }

        public void UpdateTimer(float timer) {
            float min = Mathf.FloorToInt(timer / 60f);
            float sec = Mathf.FloorToInt(timer % 60f);

            timerText.text = min + ":" + sec.ToString("00");
        }
    


}
