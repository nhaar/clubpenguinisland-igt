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

# Editing The Game (Only for people who want to contribute to this mod)

The code in this repository contains C# code meant to be injected into the decompiled game code. I recommend using [dnSpy](https://github.com/dnSpy/dnSpy) to decompile and edit the code.

To inject the code properly, first change the `LoadingController` class in the `Disney.Kelowna.Common` namespace in `UnityShared.dll` just like in  `loadingcontroller.cs `. Then, change the main script, which should be one that runs every frame and is initialized at the beginning, according to  `mainscript.cs `. I've picked `ZoneTransitionService` from the `ClubPenguin` namespace in `ReMix-Game.dll`, if that is to be changed the other code will need to be changed too. The other two things to change are the `QuestService` class, in the `ClubPenguin.Adventure` namespace and the `GameSettings` class in the `ClubPenguin` namespace, both of which are in `ReMix-Game.dll`. For those, you can copy the code in  `questservice.cs ` and  `gamesettings.cs `. If you are using dnSpy and you get a compiling error, make sure you are editing a method and not the class, and if you are still getting a weird decompile issue, close the dll you are editing and open it again (this always happens to me a few times in the same few scripts).
