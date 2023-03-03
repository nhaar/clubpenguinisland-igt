// Step 4, adding the debug button settings
// They'll be used to control various things
// Add it to the GameSettings method

//Change game mode

//First the choices
public enum TimerModes
{
	AA,
	RH,
	DJ,
	RK,
	IL
}

//Then the invokable script
[PublicTweak]
[Invokable("Timer Settings.Change Mode", Description = "\n\n\n\n\n\nChange the level the timer ends.\nAA: End the run in Aunt Arctic Chapter 2 Episode 10: Skyberg Assault\nRH: End the run in Rockhopper Chapter 1 Episode 10: A Captain's Share\nRK: End the run in Rookie Chapter 2 Episode 5: Special Delivery\nDJ: End the run in DJ Cadence Chapter 1 Episode 5: Make It Big\nIL: End the run in any level (Individual Level)")]
public void ChangeTimerMode(TimerModes mode)
{
	string newMode = "";
	switch (mode)
	{
	case TimerModes.AA:
		newMode = "AA";
		break;
	case TimerModes.RH:
		newMode = "RH";
		break;
	case TimerModes.RK:
		newMode = "RK";
		break;
	case TimerModes.DJ:
		newMode = "DJ";
		break;
	case TimerModes.IL:
		newMode = "IL";
		break;
	default:
		break;
	}
	ZoneTransitionService.saveData("mode", newMode);
	ZoneTransitionService.timerMode = newMode;
}


//Reset the timer/stop speedrunning
[PublicTweak]
[Invokable("Timer Settings.Reset", Description = "\n\n\n\n\n\nReset the timer.")]
public void ResetRun()
{
	ZoneTransitionService.isSpeedrunning = false;
	ZoneTransitionService.isRunFinished = false;
	ZoneTransitionService.saveData("running", "False");
	ZoneTransitionService.saveData("ended", "False");
}

//Display Timer Settings
// Three options defined

public enum DisplayModes
{
	Always,
	OutsideRuns,
	Never
}

[PublicTweak]
[Invokable("Timer Settings.Display Timer", Description = "\n\n\n\n\n\nChanges the timer display.\nAlways: Timer is always on screen.\nOutside runs: Only when the timer is not running.\nNever: Disable the timer (not recommended because the timer must be visible for verification).")]
public void ToggleDisplay(DisplayModes mode)
{
	switch (mode)
	{
		case DisplayModes.Always:
			ZoneTransitionService.displayTimerFlag = 1;
			break;
		case DisplayModes.OutsideRuns:
			ZoneTransitionService.displayTimerFlag = 2;
			break;
		case DisplayModes.Never:
			ZoneTransitionService.displayTimerFlag = 3;
			break;
	}
	ZoneTransitionService.saveData("display", ZoneTransitionService.displayTimerFlag.ToString());
}

// Change font size

public enum FontSizes
{
	Big,
	Medium,
	Small
}

[PublicTweak]
[Invokable("Timer Settings.Font Size", Description = "\n\n\n\n\n\nChanges the font size of the timer UI.")]
public void ChangeFontSize(FontSizes size)
{
	switch (size)
	{
		case FontSizes.Big:
			ZoneTransitionService.IGTFontSize = 40;
			ZoneTransitionService.IGTModeHeight = 60;
			break;
		case FontSizes.Medium:
			ZoneTransitionService.IGTFontSize = 28;
			ZoneTransitionService.IGTModeHeight = 40;
			break;
		case FontSizes.Small:
			ZoneTransitionService.IGTFontSize = 14;
			ZoneTransitionService.IGTModeHeight = 30;
			break;
	}
	ZoneTransitionService.saveData("fontsize", ZoneTransitionService.IGTFontSize.ToString());
}

//DisplayModeText options

public enum DisplayModeText
{
	Always,
	OutsideRuns,
	Never,
	SameAsTimer
}

[PublicTweak]
[Invokable("Timer Settings.Display Mode", Description = "\n\n\n\n\n\nChanges the timer mode display (below the timer).\nAlways: You can always see the timer mode.\nOutside runs: You can only see the timer mode when the timer is not running.\nNever: Never show the mode.\nSame as timer: As it suggests, matches the display timer mode.")]
public void ToggleModeDisplay(DisplayModeText mode)
{
	switch (mode)
	{
		case DisplayModeText.Always:
			ZoneTransitionService.displayMode = 1;
			break;
		case DisplayModeText.OutsideRuns:
			ZoneTransitionService.displayMode = 2;
			break;
		case DisplayModeText.Never:
			ZoneTransitionService.displayMode = 3;
			break;
		case DisplayModeText.SameAsTimer:
			ZoneTransitionService.displayMode = 4;
			break;
	}
	ZoneTransitionService.saveData("displaymode", ZoneTransitionService.displayMode.ToString());
}

public enum WarningOptions
{
	On_second_instance_or_higher,
	On_third_instance_or_higher,
	Never
}

[PublicTweak]
[Invokable("Timer Settings.Instance Warning", Description = "\n\n\n\n\n\nWarns the player on what instance they are depending on the option. The load remover only works on the first instance, so if you play on another instance you won't be getting the time removed, and you may want a warning." +
"\nOn second instance or higher: You will get a warning if you are not in the main instance\nOn third instance or higher: you will get a warning if you are not on the main instance or the secondary instance (the one that will become main once the actual main instance is closed)\nNever: No warnings. Helpful for removing the jarring warning, but only recommended once you are used to the game.")]
public void WarningChange(WarningOptions option)
{
	switch (option)
	{
		case WarningOptions.On_second_instance_or_higher:
			ZoneTransitionService.warningFlag = 1;
			break;
		case WarningOptions.On_third_instance_or_higher:
			ZoneTransitionService.warningFlag = 2;
			break;
		case WarningOptions.Never:
			ZoneTransitionService.warningFlag = 3;
			break;
	}
	ZoneTransitionService.saveData("warning", ZoneTransitionService.warningFlag.ToString());
}

// Change text alignment

[PublicTweak]
[Invokable("Timer Settings.Change UI Alignment", Description = "\n\n\n\n\n\nChanges the UI alignment between left or right.")]
public void ChangeAlignment()
{
	if (ZoneTransitionService.textAlignOption == 1)
	{
		ZoneTransitionService.textAlignOption = 2;
	}
	else
	{
		ZoneTransitionService.textAlignOption = 1;
	}
	ZoneTransitionService.saveData("align", ZoneTransitionService.textAlignOption.ToString());
}
