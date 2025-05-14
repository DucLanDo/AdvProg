using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public TextMeshProUGUI messageText;

    public void OnLoginClick() {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        //check
        Debug.Log("username: " + username);
        Debug.Log("password: " + password);

        string loginCheckMessage = CheckLoginInfo(username, password);

        if (string.IsNullOrEmpty(loginCheckMessage)) {
            //try login
            StartCoroutine(TryLogin(username, password));
        }

    }

    private string CheckLoginInfo(string username, string password) {
        string returnString = "";


        if(string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password)) {
            returnString = "Username and password are empty";
        }
        else if(string.IsNullOrEmpty(username)) {
            returnString = "Username was empty";
        }
        else if (string.IsNullOrEmpty(password)) {
            returnString = "Password was empty";
        }
        else {
            returnString = "";
        }

        Debug.Log("Return String: " + returnString);
        return returnString;

    }

    IEnumerator TryLogin(string username, string password) {

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:5000/login", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Server response: " + www.downloadHandler.text);
                ServerResponse response = JsonUtility.FromJson<ServerResponse>(www.downloadHandler.text);

                if (response.status == "ok")
                {
                    messageText.text = "Login erfolgreich!";
                    GameManager.Instance.loggedInUsername = username;  
                    UIController.Instance.loginPanel.SetActive(false);
                    PlayerPrefs.SetString("username", username);
                    UIController.Instance.ShowLoggedInUser();

                }
                else
                {
                    messageText.text = "Login fehlgeschlagen!";
                }
            }
            else
            {
                Debug.LogError("Netzwerkfehler: " + www.error);
                messageText.text = "Fehler beim Verbinden mit dem Server.";
            }
        }
    }

    [System.Serializable]
    public class ServerResponse
    {
        public string status;
    }





}
