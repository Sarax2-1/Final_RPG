using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AnalyticManager : MonoBehaviour
{
    public static AnalyticManager Instance;

    private float appStartTime;
    private float leaderboardStartTime;
    private int shopPressCount = 0;

    async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            await InitializeAnalytics();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    async Task InitializeAnalytics()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();

        appStartTime = Time.time;
        Debug.Log("Analytics Initialized");
    }

    // Flush events when app is paused/closed to prevent data loss
    void OnApplicationPause(bool paused)
    {
        if (paused)
            AnalyticsService.Instance.Flush();
    }

    public void OnOpenLeaderboard()
    {
        leaderboardStartTime = Time.time;
        Debug.Log("Enter Leaderboard");
    }

    public void OnCloseLeaderboard()
    {
        float duration = Time.time - leaderboardStartTime;

        SendEvent("leaderboard_usage", new Dictionary<string, object>
        {
            { "time_spent", Mathf.Round(duration * 10f) / 10f }
        });

        Debug.Log($"Exit Leaderboard: {duration:F1}s");
    }

    public void OnShopPressed()
    {
        shopPressCount++;

        SendEvent("shop_interaction", new Dictionary<string, object>
        {
            { "press_count", shopPressCount }
        });

        Debug.Log($"Shop Click: {shopPressCount}");
    }

    public void OnCreditOpened()
    {
        float delay = Time.time - appStartTime;

        SendEvent("credit_open_delay", new Dictionary<string, object>
        {
            { "wait_time", Mathf.Round(delay * 10f) / 10f }
        });

        Debug.Log($"Credit Open Delay: {delay:F1}s");
    }

    void SendEvent(string eventName, Dictionary<string, object> data)
    {
        var evt = new CustomEvent(eventName);

        foreach (var pair in data)
        {
            evt.Add(pair.Key, pair.Value);
        }

        AnalyticsService.Instance.RecordEvent(evt);
    }
}