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

The repository contains C# code which is injected via dnSpy. There are three `dll`s which will be edited for the mod: `UnityShared`, `ReMix-Game` and `ClubPenguinClient`. Below are the instructions for modifying each `dll`, using dnSpy v6.1.8

## UnityShared

1. Go to `Disney.Kelowna.Common` -> `LoadingController`.
2. Right click, add class members, add the fields from `loadingcontroller.cs`
3. Edit each of the methods listed in accordance with the comments
4. Save the `dll`

## ReMix-Game

1. Go to `ClubPenguin` -> `ZoneTransitionService`
2. Right click, add class members, copy and paste everything inside `mainscript.cs`, except for the awake method (which is at the bottom)
3. Edit the awake method and then copy the code as the comments say
4. Go to `ClubPenguin.Adventure` -> `QuestService`
5. Edit the `onStartQuestRequest` as `questservice.cs` says
6. You will need to save the `dll`, close it and open it again
7. Return to `ClubPenguin.Adventure` -> `QuestService`
8. Edit the `ShowStartQuestSplashscreen` and `EndQuest` methods as `questservice.cs` requires
9. Go to `ClubPenguin` -> `GameSettings`
10. Edit add class members, copy and paste all of `gamesettings.cs` into and compile
11. Save the `dll`

# ClubPenguinClient

1. Go to `ClubPenguin.Net.Client` -> `SetStatusOperation`
2. Edit the `SetStatus` method as in `setstatus.cs`
3. Save the `dll`
