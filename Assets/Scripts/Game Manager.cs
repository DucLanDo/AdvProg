using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string loggedInUsername;
    public float gameTime;
    public bool gameActive;


    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    void Start() {
        gameActive = true;
    }

    public void Update(){
        if (gameActive) {
            gameTime += Time.deltaTime;
            UIController.Instance.UpdateTimer(gameTime);

            if (Input.GetKeyDown(KeyCode.Escape)) {
                Pause();
            }
        }

    }

    public void GameOver() {
        gameActive = false;
        StartCoroutine(ShowGameOverScreen());
        
    }

    IEnumerator ShowGameOverScreen(){
        yield return new WaitForSeconds(1.5f);
        UIController.Instance.gameOverPanel.SetActive(true);
    }

    public void Restart() {
        SceneManager.LoadScene("Game");
    }

    public void Pause() {
        if (UIController.Instance.pausePanel.activeSelf == false && UIController.Instance.gameOverPanel.activeSelf == false) {
            UIController.Instance.pausePanel.SetActive(true);
            Time.timeScale = 0f;
        } else {
            UIController.Instance.pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void GoToMainMenu(){
        SceneManager.LoadScene("Main Menu");
    }

    public void ShowRegisterPanel(){
        UIController.Instance.registerPanel.SetActive(true);
    }

    public void CloseRegisterPanel(){
        UIController.Instance.registerPanel.SetActive(false);
    }





}
