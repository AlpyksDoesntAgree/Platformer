using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //Serializable
    //Login
    [System.Serializable]
    public class LoginRequest
    {
        public string Login;
        public string Password;
    }

    [System.Serializable]
    public class LoginResponse
    {
        public bool status;
        public UserData data;
    }

    [System.Serializable]
    public class UserData
    {
        public int id;
        public string name;
    }

    //Reg
    [System.Serializable]
    public class CreateNewUser
    {
        public string Name;
        public string Login;
        public string Password;

        public CreateNewUser(string name, string login, string password)
        {
            Name = name;
            Login = login;
            Password = password;
        }
    }

    [System.Serializable]
    public class RegistrationResponse
    {
        public bool status;
        public int id;
        public string name;
    }

    //UIElements
    [Header("Login Elements")]
    [SerializeField] private InputField loginInput;
    [SerializeField] private InputField passwordInput;

    [Header("Reg Elements")]
    [SerializeField] private InputField loginField;
    [SerializeField] private InputField nameField;
    [SerializeField] private InputField passField;
    [SerializeField] private InputField repeatPassField;

    [Header("Chat Elements")]
    [SerializeField] private InputField _chatMessageInput;
    [SerializeField] private Button _sendButton;
    [SerializeField] private Text _chatText;
    [SerializeField] private ScrollRect _scrollRect;



    private string apiURL = "http://localhost:5039/api/UsersLogins";
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Login
    public void AuthButton()
    {
        string login = loginInput.text;
        string password = passwordInput.text;
        StartCoroutine(SendLoginReq(login, password));
    }

    private IEnumerator SendLoginReq(string login, string password)
    {
        LoginRequest request = new LoginRequest { Login = login, Password = password };
        string jsonData = JsonUtility.ToJson(request);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        using (UnityWebRequest www = new UnityWebRequest(apiURL + "/getUser", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(jsonResponse);

                if (response.status)
                {
                    Debug.Log($"Logged In! ID: {response.data.id} Name: {response.data.name}");

                    PlayerPrefs.SetInt("id", response.data.id);
                    PlayerPrefs.SetString("UserName", response.data.name);
                    PlayerPrefs.Save();

                    SceneManager.LoadScene("MainMenu");
                }
                else
                {
                    Debug.Log("Error: " + jsonResponse);
                }
            }
            else
            {
                Debug.LogError($"Response Code: {www.responseCode}");
                Debug.LogError($"Download Handler: {www.downloadHandler.text}");
            }
        }
    }

    //Registration
    public void RegisterNewUser()
    {
        if (passField.text == repeatPassField.text && passField.text != "")
        {
            string name = nameField.text;
            string login = loginField.text;
            string password = passField.text;

            CreateNewUser newUser = new CreateNewUser(name, login, password);
            StartCoroutine(SendRegistrationRequest(newUser));
        }
    }

    private IEnumerator SendRegistrationRequest(CreateNewUser newUser)
    {
        string json = JsonUtility.ToJson(newUser);

        UnityWebRequest request = new UnityWebRequest(apiURL + "/createNewUser", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            RegistrationResponse response = JsonUtility.FromJson<RegistrationResponse>(responseText);

            PlayerPrefs.SetInt("id", response.id);
            PlayerPrefs.SetString("UserName", response.name);
            PlayerPrefs.Save();

            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.LogError("Registration failed: " + request.error);
        }
    }
}
