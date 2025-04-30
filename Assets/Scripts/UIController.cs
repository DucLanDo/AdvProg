using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
        public static UIController Instance;
        [SerializeField] private Slider playerHealthSlider;
        [SerializeField] private TMP_Text healthText;
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
            usernameDisplay.text = "Logged in as: " + GameManager.Instance.loggedInUsername;
        }
    


}
