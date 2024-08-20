# Club Penguin Island IGT & Load Remover

This repository contains the code for a Club Penguin Island mod which adds an in-game timer with load remover, meant for speedrunning.

# Installing

Download the dlls provided in the releases tab, place them in your game's `Client\ClubPenguinIsland_Data\Managed` folder.

# Features

The in-game timer is displayed on the screen. You can change the timer settings by opening the game's offline mode menu, which explains in detail what each settings does. The timer runs a timer that stopts at each loading screen, and also saves the time between instances.

## Splits

The game will log the splits of the current run in the `Client\ClubPenguinIsland_Data\IGT_Data\splits` file. This is just to spare the time of performing operations and checking in case you think you got a PB in one of the ILs.

# Limitations

* Changing the computer time when the game isn't open will break the timer. Changing it while the game is open is fine.

* The load remover only works in one instance of the game at a time. Make sure you are on the correct instance when you have to open more than one game. More details can be found in the timer settings menu in-game.

* Because of Unity's time functions, the accuracy of the real time can't be of more than 1 ms, which means that in a run where you open and close the timer multiple times, the accuracy might be off by 10-20 ms (around 1 frame at 60 fps).

* If you manage to somehow close the game and swap to a new instance in less than 150 ms, you could technically lose time because the load remover won't be working yet. It shouldn't be possible for a human to swap the instances that quickly, but it's important to keep that in mind for TASing.

* The timer will not work properly if you alter the IGT_Data folder, and it may either stop working or display innacurate times.

# Build Steps

The repository contains C# code, but you must modify the DLLs manually. There are three DLLs which will be edited for the mod: `UnityShared`, `ReMix-Game` and `ClubPenguinClient`. You can independently edit each DLL, but the intended order is "UnityShared", "ClubPenguinClient" and then "ReMix-Game", so if you have any reference errors go on that order. Below are the instructions for modifying each `dll`, using dnSpy v6.1.8.

## UnityShared

1. Open DLL in dnSpy.
2. Go to `Disney.Kelowna.Common` -> `LoadingController`.
3. Right click, add class members, copy and paste everything in `LoadingController.cs` as new members.
4. Edit (in any order) `OnEnable` and `Update` methods with the code from `LoadingController.OnEnable.cs` and `LoadingController.Update.cs`.
5. Save the DLL.

## ClubPenguinClient

1. Open the DLL in dnSpy.
2. Go to `ClubPenguin.Net.Client` -> `SetStatusOperation`.
3. Edit the `SetStatus` method with the code from `SetStatusOperation.SetStatus.cs`.
4. Save the DLL.


## ReMix-Game

1. Open the DLL in dnSpy.
2. Go to `ClubPenguin` -> `ZoneTransitionService`
3. Right click, add class members, copy and paste everything in `ZoneTransitionService.cs` as new members (will need to move the `using` part up).
4. Edit the `Awake` method with the code from `ZoneTransitionService.Awake.cs`.
5. Save the DLL, close it from `dnSpy`, and open it again (this step is because you will run into decompilation issues with `QuestService` otherwise).
6. Open the DLL again.
7. Go to `ClubPenguin.Adventure` -> `QuestService`.
8. Edit the `ShowStartQuestSplashscreen` method as `QuestService.ShowStartQuestSplashscreen.cs` says
9. Edit (in any order) the `onStartQuestRequest` and `EndQuest` methods as `QuestService.onStartQuestRequest.cs` and `QuestService.EndQuest.cs` say
10. Go to `ClubPenguin` -> `GameSettings`
11. Right click, add class members, copy and paste all of `GameSettings.cs` as new fields.
12. Save the DLL.

