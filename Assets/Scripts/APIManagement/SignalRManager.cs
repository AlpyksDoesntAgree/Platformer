using System;
using Unity.VisualScripting;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

public class SignalRManager : MonoBehaviour
{
    [SerializeField] private float connectionDelay = 1f; // �������� ����� ��������� �����������

    public async UniTask<HubConnection> ConnectToHubAsync()
    {
        Debug.Log("ConnectToHubAsync start");

        // ������� ���������� � ����� ���������� �������� �����
        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5039/NotificationHub")
            .WithAutomaticReconnect()
            .Build();

        Debug.Log("connection handle created");

        // ������������� �� ��������� �� ����, ����� ��������� �����������
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

    // ����� ��� ����������� ��������� (����� �������� �� ����)
    private async UniTask LogAsync(string message)
    {
        await UniTask.SwitchToMainThread(); // ���� ����� ��������� � �������� ������ Unity
        Debug.Log(message);
    }
}