using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string loggedInUsername;
    public float gameTime;
    public bool gameActive;

    public int score = 0;


    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;

        }
    }

    void Start() {
        gameActive = true;
        loggedInUsername = PlayerPrefs.GetString("username");
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
        SubmitScoreToServer(loggedInUsername, score); 
        
    }

    IEnumerator ShowGameOverScreen(){
        yield return new WaitForSeconds(1.5f);
        UIController.Instance.gameOverPanel.SetActive(true);
    }

    public void Restart() {
        SceneManager.LoadScene("Game");
        score = 0;
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
        PlayerPrefs.DeleteKey("username");
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

    public void AddPoints(int amount) {
        score += amount;
        UIController.Instance.UpdateScoreText(score);
    }

    public void SubmitScoreToServer(string username, int score) {
        StartCoroutine(SubmitScoreCoroutine(username, score));
    }

    
    IEnumerator SubmitScoreCoroutine(string username, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("score", score);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:5000/submit_score", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Score erfolgreich an Server gesendet!");
            }
            else
            {
                Debug.LogError("Fehler beim Score-Upload: " + www.error);
            }
        }
    }





}
