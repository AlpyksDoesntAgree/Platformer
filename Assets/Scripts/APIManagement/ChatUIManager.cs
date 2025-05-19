using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatUIManager : MonoBehaviour
{
    [SerializeField] private InputField messageInput;
    [SerializeField] private Button sendButton;
    private SignalRManager chatClient;
    [SerializeField] private ChatScroll chatScroll;

    void Start()
    {
        chatClient = FindObjectOfType<SignalRManager>();
        sendButton.onClick.AddListener(OnSendButtonClicked);

        chatClient.OnMessageReceived += OnMessageReceived;
    }

    void OnSendButtonClicked()
    {
        string user = PlayerPrefs.GetString("UserName", "Guest");
        string message = messageInput.text;
        chatClient.SendMessage(user, message);
        messageInput.text = "";
    }

    private void OnMessageReceived(string user, string message)
    {
        chatScroll.AddMessage(user, message);
    }

    private void OnDestroy()
    {
        if (chatClient != null)
            chatClient.OnMessageReceived -= OnMessageReceived;
    }
}
