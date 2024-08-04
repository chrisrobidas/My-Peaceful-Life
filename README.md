# My Peaceful Life

A cozy life-sim game where you can grow crops, chop wood, mine, fish, and hunt. Play alone or with friends!

This game was made using Unity 2022.3.29f1

The game is still a work in progress...

To avoid getting this error:
```
[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.
UnityEngine.Debug:LogError (object,UnityEngine.Object)
SteamManager:Awake () (at Assets/Scripts/Steamworks.NET/SteamManager.cs:124)
UnityEngine.GameObject:AddComponent<SteamManager> ()
SteamManager:get_Instance () (at Assets/Scripts/Steamworks.NET/SteamManager.cs:31)
SteamManager:get_Initialized () (at Assets/Scripts/Steamworks.NET/SteamManager.cs:42)
DisplayPlayerName:Start () (at Assets/Scripts/Player/DisplayPlayerName.cs:11)
```
Make sure the Steam app is running when pressing the Unity Editor play button!
