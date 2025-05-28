using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
        public static UIController Instance;
        [SerializeField] private Slider playerHealthSlider;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Slider playerExperienceSlider;
        [SerializeField] private TMP_Text experienceText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text scoreText;
        public LevelUpButton[] levelUpButtons;

        public GameObject gameOverPanel;
        public GameObject pausePanel;
        public GameObject levelUpPanel;

        public GameObject loginPanel;
        public GameObject registerPanel;
        public GameObject leaderboardPanel;
        public TextMeshProUGUI usernameDisplay;
        [SerializeField] private TMP_Text leaderboardTextField;

        public TMP_Text rawLeaderboardText;
        public TMP_Text leaderboardPosText;
        public TMP_Text leaderboardNameText;
        public TMP_Text leaderboardScoreText;

        
   


     

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

        public void UpdateExpericenceSlider() {
            playerExperienceSlider.maxValue = PlayerController.Instance.playerLevels[PlayerController.Instance.currentLevel-1];
            playerExperienceSlider.value = PlayerController.Instance.experience;
            experienceText.text = playerExperienceSlider.value + " / " + playerExperienceSlider.maxValue;
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

        public void levelUpPanelOpen() {
            levelUpPanel.SetActive(true);
            Time.timeScale = 0;
        }

        public void levelUpPanelClose() {
            levelUpPanel.SetActive(false);
            Time.timeScale = 1;
        }

        //leaderboard


}

