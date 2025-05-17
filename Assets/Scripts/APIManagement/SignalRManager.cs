using Microsoft.AspNetCore.SignalR.Client;
using System;
using UnityEngine;

public class SignalRManager : MonoBehaviour
{
    private HubConnection _connection;

    public static SignalRManager Instance { get; private set; }

    public event Action<string, string> OnMessageReceived;

    async void Start()
    {
        Instance = this;

        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5039/NotificationHub")
            .WithAutomaticReconnect()
            .Build();

        _connection.On<string, string>("OnMessageReceived", (user, message) =>
        {
            Debug.Log($"[{user}]: {message}");
            OnMessageReceived?.Invoke(user, message); 
        });

        try
        {
            await _connection.StartAsync();
            Debug.Log("SignalR подключение установлено.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка подключения: {ex.Message}");
        }
    }

    public async void SendMessage(string user, string message)
    {
        if (_connection.State == HubConnectionState.Connected)
        {
            try
            {
                await _connection.InvokeAsync("BroadcastMessage", user, message);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка отправки сообщения: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning("Нет подключения к SignalR.");
        }
    }

    private async void OnApplicationQuit()
    {
        if (_connection != null)
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
        }
    }
}