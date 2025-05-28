using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
using TMPro;

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
        Time.timeScale = 1f;
    }

    public void ShowRegisterPanel(){
        UIController.Instance.registerPanel.SetActive(true);
    }

    public void CloseRegisterPanel(){
        UIController.Instance.registerPanel.SetActive(false);
    }

    public void ShowLeaderboardPanel(){
        UIController.Instance.leaderboardPanel.SetActive(true);
        LoadRawLeaderboard();
    }

    public void CloseLeaderBoardPanel(){
        UIController.Instance.leaderboardPanel.SetActive(false);
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

    // leader board
    public void LoadRawLeaderboard() {
        StartCoroutine(LoadRawLeaderboardCoroutine());
    }
/*
    IEnumerator LoadRawLeaderboardCoroutine() {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/all_scores")) {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success) {
                Debug.Log("Leaderboard JSON: " + www.downloadHandler.text);

                
                string json = www.downloadHandler.text;
                LeaderboardEntry[] entries = JsonHelper.FromJson<LeaderboardEntry>(json);

                string formattedText = "Leaderboard:\n";
                for (int i = 0; i < entries.Length; i++) {
                    formattedText += $"{i + 1}. {entries[i].username} - {entries[i].score}\n";
                }

                UIController.Instance.rawLeaderboardText.text = formattedText;
            } else {
                Debug.LogError("Fehler beim Laden der Scores: " + www.error);
                UIController.Instance.rawLeaderboardText.text = "Fehler beim Abrufen!";
            }
        }
    }
*/
    IEnumerator LoadRawLeaderboardCoroutine() {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/all_scores")) {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success) {
                LeaderboardEntry[] entries = JsonHelper.FromJson<LeaderboardEntry>(www.downloadHandler.text);

                string posText = "Pos.\n";
                string nameText = "Name\n";
                string scoreText = "Score\n";

                string currentUser = GameManager.Instance.loggedInUsername;

                int displayCount = Mathf.Min(10, entries.Length);
                for (int i = 0; i < displayCount; i++) {
                    posText += (i + 1) + ".\n";
                    nameText += entries[i].username + "\n";
                    scoreText += entries[i].score + "\n";
                }

                // Suche nach bestem Eintrag des Users
                LeaderboardEntry bestEntryOfUser = null;
                int userRank = -1;

                for (int i = 0; i < entries.Length; i++) {
                    if (entries[i].username == currentUser) {
                        if (bestEntryOfUser == null || entries[i].score > bestEntryOfUser.score) {
                            bestEntryOfUser = entries[i];
                            userRank = i + 1; // Position ist Index + 1
                        }
                    }
                }

                // Anzeigen, wenn User-Eintrag gefunden wurde
                if (bestEntryOfUser != null) {
                    posText += "\n<b>" + userRank + ".</b>\n";
                    nameText += "\n<b>" + bestEntryOfUser.username + "</b>\n";
                    scoreText += "\n<b>" + bestEntryOfUser.score + "</b>\n";
                }

                // Anzeigen
                UIController.Instance.leaderboardPosText.text = posText;
                UIController.Instance.leaderboardNameText.text = nameText;
                UIController.Instance.leaderboardScoreText.text = scoreText;
            } else {
                Debug.LogError("Fehler beim Laden der Scores: " + www.error);
                UIController.Instance.leaderboardNameText.text = "Fehler beim Abrufen!";
                UIController.Instance.leaderboardPosText.text = "";
                UIController.Instance.leaderboardScoreText.text = "";
            }
        }
    }




}

[System.Serializable]
public class LeaderboardEntry {
    public string username;
    public int score;
}

public static class JsonHelper {
    public static T[] FromJson<T>(string json) {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T> {
        public T[] array;
    }
}






