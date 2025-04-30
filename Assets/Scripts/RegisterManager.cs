using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class RegisterManager : MonoBehaviour
{
    public TMP_InputField usernameField;
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TMP_InputField confirmPasswordField;
    public TextMeshProUGUI registerMessageText;

    public void OnRegisterClick()
    {
        string username = usernameField.text;
        string email = emailField.text;
        string password = passwordField.text;
        string confirmPassword = confirmPasswordField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            registerMessageText.text = "Alle Felder müssen ausgefüllt sein!";
            return;
        }

        if (password != confirmPassword)
        {
            registerMessageText.text = "Passwörter stimmen nicht überein.";
            return;
        }

        StartCoroutine(SendRegisterRequest(username, email, password));
    }

    IEnumerator SendRegisterRequest(string username, string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("email", email);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:5000/register", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                ServerResponse res = JsonUtility.FromJson<ServerResponse>(www.downloadHandler.text);
                if (res.status == "ok") {
                    registerMessageText.text = "Registrierung erfolgreich!";
                    UIController.Instance.registerPanel.SetActive(false);
                }
                else if (res.status == "exists") {
                    registerMessageText.text = "Benutzername oder E-Mail existiert bereits.";
                }
                else {
                    registerMessageText.text = "Fehler bei der Registrierung.";
                }
            }
            else
            {
                registerMessageText.text = "Verbindungsfehler: " + www.error;
            }
        }
    }

    [System.Serializable]
    public class ServerResponse { public string status; }
}
