using System;
using UnityEngine;
using Discord;

public class DiscordManager : MonoBehaviour
{
    public Discord.Discord discord;

    public string State;
    public string largeImage = "icon";

    private void Awake()
    {
        discord = new Discord.Discord(865333464316379147, (System.UInt64)Discord.CreateFlags.Default);
        UpdateStatus(State);
    }

    private void Update()
    {
        discord.RunCallbacks();
    }

    public void UpdateStatus(string state = "", string details = "")
    {
        if (state.Length < 1) state = State;
        var activity = new Discord.Activity
        {
            State = state,
            Details = details,

            Assets =
            {
                LargeImage = largeImage
            },
            Timestamps =
            {
                Start = DateTimeOffset.Now.ToUnixTimeSeconds()
            }
        };
        var activityManager = discord.GetActivityManager();
        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Discord.Result.Ok)
                Debug.Log("Discord status succ");
            else
                Debug.LogError($"Discord status failed, {res}");
        });
    }

    private void OnApplicationQuit()
    {
        discord.Dispose();
    }
}
