using System;
using Unity.VisualScripting;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

public class SignalRManager : MonoBehaviour
{
    [SerializeField] private float connectionDelay = 1f; // Задержка между попытками подключения

    public async UniTask<HubConnection> ConnectToHubAsync()
    {
        Debug.Log("ConnectToHubAsync start");

        // Создаем соединение с нашим написанным тестовым хабом
        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5039/NotificationHub")
            .WithAutomaticReconnect()
            .Build();

        Debug.Log("connection handle created");

        // Подписываемся на сообщение от хаба, чтобы проверить подключение
        connection.On<string, string>("ReceiveMessage",
            (user, message) => LogAsync($"{user}: {message}").Forget());

        while (connection.State != HubConnectionState.Connected)
        {
            try
            {
                if (connection.State == HubConnectionState.Connecting)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(connectionDelay));
                    continue;
                }

                Debug.Log("start connection");
                await connection.StartAsync();
                Debug.Log("connection finished");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        return connection;
    }

    // Метод для логирования сообщений (можно заменить на свой)
    private async UniTask LogAsync(string message)
    {
        await UniTask.SwitchToMainThread(); // Если нужно выполнить в основном потоке Unity
        Debug.Log(message);
    }
}