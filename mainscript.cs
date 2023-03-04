// Step 2: The IGT script. It can be any that is always present and is created at the start of the game. The one I use is ZoneTransitionService, from ClubPenguin
// If going to change it, needs to change all the references to ZoneTransitionService in other objects, and also be careful not to duplicate voids, properly add them

using System.Diagnostics;
using System. IO;

// Initialize all the variables

public static bool isMainInstance; // Keeps track on whether this instance is the main instance, which will control the timer, as opposed to an instance running in the background
public static int instanceNo; // Keeps track on which is the number of the instance in the order that it was opened
public static string timerMode; // Keeps track of the timer mode, which controls what ends the timer
public static float currentElapsedTime; // to keep track of the time each frame
public static float outsideTimeCount; // All the time spent outside the game, between closing instances
public static float insideTimeCount; // All the time spent in the actual game
public static float TimeCountStart; // We will use relative time to add to the timer, we  will update this whenever we want to start counting
public static float runTimeOnLoadEnd; // To keep track of the time at the end of each load
public static bool isSpeedrunning; // Keeps track of whether the time is running or not
public static bool isRunFinished; // Keeps track if the time is currently stopped after completing a run
public static int displayTimerFlag; // Keeps track if the time should be displayed
public static bool startedLoadTimer; // Used to keep track of whether we started the process of removing the loads already
public static int toCheatEngine; // Variables meant to be read by livesplit, use this to make finding the addresses with CheatEngine very fast
public static int toCheatEngine2;
public static int toCheatEngine3;
public static DateTime saveSystemTime; // These two variables will hold the information of the current times for when the game closes
public static float saveTimeIn;
public static bool needInit; // Will be used to keep track of whether the time between instances needs to be initialized
public static float initialTimeIn;
public static bool startTimeNow; // The timer will start in this frame if this is true
public static float splitStartTimeIn; // These two variables will be used to keep track of splits and will keep track of the time at the beginning of each split
public static float splitStartTimeOut;
public static bool shouldStartSplit;
public static string splitsString; // String that has splits split across lines
public static int displayMode; // settings for displaying the mode
public static int IGTFontSize; // settings for IGT fontsize
public static int IGTModeHeight;
public static int warningFlag; // flag for the jarring warning
public static int textAlignOption; // flag for whether text is right or left aligned

public void InitializeTimer()
{
	// This script is meant to enable all timer functions. It runs once this becomes the main instance, which is the one that controls the timer
	// We will read all the IGT data with this, unless it doesn't exist, on which we shall create it
	// There are 4 places we change when adding new data types:
	// The actual variable that holds the data type (above)
	// The fileArray contains names for the paths
	// The two switches handle the cases of reading and creating the files
	
	string timerDataPath = Application.dataPath + "/IGT_Data/";
	string[] fileArray = new string[] {"mode", "running", "ended", "timein", "timeout", "display", "splitin", "splitout", "displaymode", "fontsize", "align"};
	// mode = timer mode // running = if is in a run // ended = if have finished a run // timein = time spent inside game // timeout = time spent outside game // display = display timer
	string[] pathArray = new string[fileArray.Length];
	// Create an array for the modes, turn that into array with the file path and below we will iterate through everything to properly initialize the data
	for (int i = 0; i < fileArray.Length; i++)
	{
		pathArray[i] = timerDataPath + fileArray[i];
	}
	for (int i = 0; i < fileArray.Length; i++)
	{
		string thisPath = pathArray[i];
		// If the file exists, we follow this first switch where we just read the file content
		// If it doesn't, we follow the second switch where we create them with default values
		if (File.Exists(thisPath))
		{
			string fileContents = File.ReadAllText(thisPath);
			switch (i)
			{
				case 0:
					fileContents = fileContents.Trim(); // Remove whitespace
					timerMode = fileContents;
					break;
				case 1:
					isSpeedrunning = bool.Parse(fileContents);
					break;
				case 2:
					isRunFinished = bool.Parse(fileContents);
					break;
				case 3:
					insideTimeCount = float.Parse(fileContents);
					initialTimeIn = insideTimeCount;
					runTimeOnLoadEnd = insideTimeCount;
					TimeCountStart = currentElapsedTime;
					break;
				case 4:
					outsideTimeCount = float.Parse(fileContents);
					break;
				case 5:
					displayTimerFlag = int.Parse(fileContents);
					break;
				case 6:
					splitStartTimeIn = float.Parse(fileContents);
					break;
				case 7:
					splitStartTimeOut = float.Parse(fileContents);
					break;
				case 8:
					displayMode = int.Parse(fileContents);
					break;
				case 9:
					IGTFontSize = int.Parse(fileContents);
					IGTModeHeight = DecideModeTextHeight(IGTFontSize);
					break;
				case 10:
					textAlignOption = int.Parse(fileContents);
					break;
				default:
					break;
			}
		}
		else
		{
			// Default values
			StreamWriter initWriter = new StreamWriter(thisPath);
			string initContent = "";
			switch (i)
			{
				case 0:
					initContent = "IL";
					timerMode = initContent;
					break;
				case 1:
					initContent = "False";
					isSpeedrunning = false;
					break;
				case 2:
					initContent = "False";
					isRunFinished = false;
					break;
				case 3:
					initContent = "0";
					insideTimeCount = 0f;
					break;
				case 4:
					initContent = "0";
					outsideTimeCount = 0f;
					break;
				case 5:
					initContent = "1";
					displayTimerFlag = 1;
					break;
				case 6:
					initContent = "0";
					splitStartTimeIn = 0f;
					break;
				case 7:
					initContent = "0";
					splitStartTimeOut = 0f;
					break;
				case 8:
					initContent = "4";
					displayMode = 4;
					break;
				case 9:
					initContent = "28";
					IGTFontSize = 28;
					IGTModeHeight = 40;
					break;
				case 10:
					initContent = "1";
					textAlignOption = 1;
					break;
				default:
					break;
			}
			initWriter.WriteLine(initContent);
			initWriter.Close();
		}
	}
	if (isSpeedrunning)
	{
		// If in a run, we must fetch when the last instance closed and add it to the count
		// We will do this in a different place though
		needInit = true;
		// Also read the splits value
		splitsString = File.ReadAllText(Application.dataPath + "/IGT_Data/splits");
	}
	
}

public int countOpenInstances()
{
	//Add 1 because the length being 0 means you're the first.
	return Directory.GetFiles(Application.dataPath + "/IGT_Data/InstancesLog").Length + 1;
}

private void OnApplicationQuit()
{
	// The script will handle transmitting information to the next instances
	// Deletes the instance log file when the game closes
	File.Delete(Application.dataPath + "/IGT_Data/InstancesLog/" + instanceNo);
	// Write what time this is being closed and the timer data for this run
	// Since this script actually runs a bit after the frames stop updating, I have prepared the variables saveSystemTime and insideTimeCount at a specific snapshot
	saveData("close", saveSystemTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
	saveData("timein", insideTimeCount.ToString());
	saveData("timeout", outsideTimeCount.ToString());
	saveData("splitf", splitStartTimeIn.ToString());
	saveData("splitt", splitStartTimeOut.ToString());
}

public void LateUpdate()
{
	// The main timer script
	// This script keeps track of when the game is loading and updates the time when it's not
	// It's what basically takes care of the game, we use LateUpdate because LoadingController won't update on time for us and we want to wait for the loading variable to be updated
	// We lock this script only for the main instance as it controls the timer
	// And of course lock it in case we are not in a run
	if (isMainInstance && isSpeedrunning)
	{
		if(needInit && insideTimeCount > initialTimeIn + 0.050f)
		{
			// Because this script doesn't update very well in the start, we wait until it starts updating to actually use the real time
			// So we keep track of the initialTimeIn to see when it starts moving for real
			// 50 ms can be tweaked possibly but I think this should be fine
			needInit = false;
			DateTime lastInstanceClosed = DateTime.Parse(File.ReadAllText(Application.dataPath + "/IGT_Data/close"));
			TimeSpan instanceDelta = saveSystemTime - lastInstanceClosed;
			outsideTimeCount += (float)instanceDelta.TotalSeconds;
			//Add the to the real time the time spent between instances
			insideTimeCount = initialTimeIn;
			TimeCountStart = currentElapsedTime;
			runTimeOnLoadEnd = insideTimeCount;
			// Reset the frameCount because before we were using it as a dummy variable
			return;
			// Don't want the variable increment in this frame
		}
		bool loadFlag = LoadingController.isSpeedrunLoading; //shorthand
		if (loadFlag && !startedLoadTimer)
		{
			// First frame the flag is updated, and when the loading screen appears
			// In this frame we add time to the count to consider the frame that just elapsed
			insideTimeCount = runTimeOnLoadEnd + currentElapsedTime - TimeCountStart; // Add elapsed time to the total time
			// But activate the other flag to keep track of it
			startedLoadTimer = true;
			// For the autosplitter
			toCheatEngine = 6969;
			//Save info in case we need to close the game
			// We need to save it here because after OnApplicationQuit runs, things don't run as intended, so we will snapshot the last point that things worked basically
			// Saving it every part in here
			saveTimeIn = insideTimeCount;
			return;
		}
		if (!loadFlag && startedLoadTimer)
		{
			// This is the first frame the loading screen disappears
			// So here we officially exit the loading mode
			startedLoadTimer = false;
			toCheatEngine = 420420;
			saveTimeIn = insideTimeCount;
			// And we capture this snapshot in Time
			TimeCountStart = currentElapsedTime;
			runTimeOnLoadEnd = insideTimeCount;
			return;
		}
		if (!startedLoadTimer && !loadFlag)
		{
			// The loading screen isn't present, so we update the frame count timer
			insideTimeCount = runTimeOnLoadEnd + currentElapsedTime - TimeCountStart; // Add elapsed time to the total time
			saveTimeIn = insideTimeCount;
		}
	}
	// Check every frame if we should start the timer
	// Put this at the very end here because the first frame must have 0 frames.
	if (startTimeNow)
	{
		startTimer();
		startTimeNow = false;
	}
}

// The Update script, which runs every frame and does most of the things

public void Update()
{
	// BE CAREFL WITH THE RETURN EXPRESSIONS, the object I'm using didn't originally have an Update so I'm not skipping anything important but otherwise you'll need to change this
	//
	// This code takes care of the multiple instances, and keeps watching out for when this instance can become the main one
	// It follows a type of rank ladder
	//
	saveSystemTime = DateTime.Now;
	// Since realtimeSinceStartup depends on when in the frame you are we'll define it at the beginning of each frame
	currentElapsedTime = realtimeSinceStartup;
	//Save the time variable (don't do it at LateUpdate because there it'll save the time of almost the next frame)
	if (!isMainInstance)
	{
		int processesNo = Process.GetProcessesByName("ClubPenguinIsland").Length; // Check for the number of instances currently open
		if (processesNo == 1) // If it's equal to 1, we make this the main one, initialize timer and finish this script 
		{
			InitializeTimer();
			isMainInstance = true;
			return;
		}
		string logPath = Application.dataPath + "/IGT_Data/InstancesLog/"; // If it's not equal to 1, we will check if the instance with one number lower (one rank higher) than this intance is still opened so we can "promote" this to update and make the instance numbers accurate
		int higherRankInstance = (instanceNo - 1); // So we get the higher rank instance by just subtracing one from our instance number
		if (!File.Exists(logPath + higherRankInstance.ToString())) // If the higher rank instance doesn't exist, we'll promote this one
		{
			new StreamWriter(logPath + higherRankInstance.ToString()).Close(); // Log the file for the current instance number
			File.Delete(logPath + instanceNo.ToString()); // Delete the older file
			instanceNo = higherRankInstance; // Update the instance number
			if (instanceNo == 1) // If we got promoted to the first instance, we initialize the timer
			{
				isMainInstance = true;
				InitializeTimer();
				return;
			}
		}
	}
}

public static void startTimer()
{
	//This script starts the timer when called. We will place this in the scripts that start the runs later
	// It will initialize all the important variables as well
	if (!isSpeedrunning)
	{
		toCheatEngine2 = ((ZoneTransitionService.toCheatEngine2 == 888) ? 777 : 888);
		// Only run this script if the run hasn't started yet
		// First part is the regular things necessary to start the time and save it
		isSpeedrunning = true;
		isRunFinished = false;
		outsideTimeCount = 0f;
		insideTimeCount = 0;
		TimeCountStart = Time.realtimeSinceStartup;
		runTimeOnLoadEnd = 0f;
		saveData("running", "True");
		// Command to start splits
		shouldStartSplit = true;
		splitsString = "";
		//If the game is starting in a loading screen, this code will stop the timer from updating until we leave the loading
		if (LoadingController.isSpeedrunLoading)
		{
			startedLoadTimer = true;
		}
	}
	if (shouldStartSplit)
	{
		splitStartTimeOut = outsideTimeCount;
		splitStartTimeIn = insideTimeCount;
		shouldStartSplit = false;
	}
}

public static void stopTimer(string questName)
{
	// Take an argument which is the quest name and will decide if timer should stop
	bool reallyStop = false;
	switch (timerMode)
	{
		// Go through the different modes and check if we are in the last quest for it
		case "AA":
			if (questName == "AAC002Q010Skyberg")
			{
				reallyStop = true;
			}
			break;
		case "RH":
			if (questName == "RHC001C010CaptainsShare")
			{
				reallyStop = true;
			}
			break;
		case "RK":
			if (questName == "RKC002Q005Colder")
			{
				reallyStop = true;
			}
			break;
		case "DJ":
			if (questName == "DJC001Q005Concert")
			{
				reallyStop = true;
			}
			break;
		case "IL":
			// IL mode always ends when called
			reallyStop = true;
			break;
		default:
			break;
	}
	// Information related to ending this split
	string thisSplitTime = getTimer(insideTimeCount - splitStartTimeIn, outsideTimeCount - splitStartTimeOut);
	splitsString += thisSplitTime + "\n";
	saveData("splits", splitsString);
	shouldStartSplit = true;
	toCheatEngine3 = ((toCheatEngine3 == 222) ? 333 : 222);
	if (reallyStop)
	{
		isRunFinished = true;
		isSpeedrunning = false;
		saveData("ended", isRunFinished.ToString());
		saveData("running", "False");
	}
}


public static string getTimer(float insidetime, float outsidetime)
{
	// This script gives the time based on the time between instances (real time in seconds) and the time inside the game
	// And converts it into a neat string time
	float total = insidetime + outsidetime; // total in seconds
	//rest is smart conversions to print the timer elegantly
	int hours = (int)(total / 3600f);
	int minutes = (int)(total % 3600f / 60f);
	int seconds = (int)(total % 60f);
	int miliseconds = (int)Mathf.Round(total % 1f * 1000f);
	if (miliseconds == 10000)
	{
		miliseconds = 0;
	}
	string timerFormat;
	if (hours == 0)
	{
		if (minutes == 0)
		{
			timerFormat = "{2}.{3:000}";
		}
		else
		{
			timerFormat = "{1}:{2:00}.{3:000}";
		}
	}
	else
	{
		timerFormat = "{0}:{1:00}:{2:00}.{3:000}";
	}
	return string.Format(timerFormat, new object[]
	{
		hours,
		minutes,
		seconds,
		miliseconds
	});
}

public static void textOutline(int xx, int y, string text, int fontsize)
{
	// Function for drawing readable text with outline
	int x = xx;
	int thick = 2;
	int width = x+250;
	int height = y+40;
	GUIStyle outlineStyle = new GUIStyle(GUI.skin.label);
	if (textAlignOption == 1) // right aligned so we change the x value relative to the right and change the alignment of the text as well
	{
		x = Screen.width - xx - 260;
		outlineStyle.alignment = TextAnchor.UpperRight;
	}
	outlineStyle.normal.textColor = Color.black;
	outlineStyle.fontSize = fontsize;
	GUI.Label(new Rect(x - thick, y, width, height), text, outlineStyle);
	GUI.Label(new Rect(x + thick, y, width, height), text, outlineStyle);
	GUI.Label(new Rect(x, y - thick, width, height), text, outlineStyle);
	GUI.Label(new Rect(x, y + thick, width, height), text, outlineStyle);
	GUIStyle textStyle = new GUIStyle(GUI.skin.label);
	textStyle.normal.textColor = Color.white;
	textStyle.fontSize = fontsize;
	if (textAlignOption == 1)
	{
		textStyle.alignment = TextAnchor.UpperRight;
	}
	GUI.Label(new Rect(x, y, width, height), text, textStyle);
}

public static int DecideModeTextHeight(int fontsize)
{
	if (fontsize == 40)
	{
		return 60;
	}
	if (fontsize == 28)
	{
		return 40;
	}
	return 30;
}

// Text to print jarring impossible to notice warning

public static void WarningText(string text)
{
	GUIStyle warningStyle = new GUIStyle(GUI.skin.label);
	warningStyle.fontSize = 100;
	warningStyle.normal.textColor = Color.red;
	warningStyle.alignment = TextAnchor.MiddleCenter;
	Vector2 textSize = warningStyle.CalcSize(new GUIContent(text));
	float x = ((float)Screen.width - textSize.x) / 2f;
	float y = ((float)Screen.height - textSize.y) / 2f;
	GUI.Label(new Rect(x, y, textSize.x, textSize.y), text, warningStyle);
}

void OnGUI() // Drawing the timer
{
	if (!isMainInstance)
	{
		if (warningFlag == 1 || (warningFlag == 2 && instanceNo > 2))
		{
			WarningText("WARNING\nTHIS IS INSTANCE\n# " + instanceNo.ToString());
		}
	}
	string timerText = ""; // define the text for the timer
	bool displayText = false;
	bool displaySecondText = false;
	//The different situations for the timer
	if (!isSpeedrunning)
	{
		//If not running, both times Always and OutsideRuns display text
		if (displayTimerFlag == 1 || displayTimerFlag == 2)
		{
			displayText = true;
		}
		if (displayMode == 1 || displayMode == 2)
		{
			displaySecondText = true;
		}
	}
	else
	{
		//During runs only displays text with always
		if (displayTimerFlag == 1)
		{
			displayText = true;
		}
		if (displayMode == 1)
		{
			displaySecondText = true;
		}
	}
	if (displayMode == 4)
	{
		displaySecondText = displayText;
	}
	if (displayText)
	{
		if (!isSpeedrunning && !isRunFinished)
		{
			// Before runs
			timerText = "0.000"; // Standard 0 second time display
		}
		else
		{
			// After starting timer get the proper time
			timerText = getTimer(insideTimeCount, outsideTimeCount);
		}
	}
	if (isMainInstance && !needInit)
		//Only show timer if the right display flag is set, is main timer and the timer isn't messed up from the first frames initializing
	{
		if (displayText)
		{
			textOutline(10, 10, timerText, IGTFontSize); // the speedrun timer
		}
		if (displaySecondText)
		{
			textOutline(10, IGTModeHeight, ZoneTransitionService.timerMode, IGTFontSize); // the mode of the timer
		}
	}
}

public static void saveData(string dataname, string datacontent)
{
	// Handy script for saving data
	StreamWriter streamWriter = new StreamWriter(Application.dataPath + "/IGT_Data/" + dataname);
	streamWriter.Write(datacontent);
	streamWriter.Close();
}


// The awake script, which runs when the game is started

void Awake()
{
		//...Other stuff
		if (!Directory.Exists(Application.dataPath + "/IGT_Data"))
		{
			Directory.CreateDirectory(Application.dataPath + "/IGT_Data");
		}
		if (!Directory.Exists(Application.dataPath + "/IGT_Data/InstancesLog"))
		{
			Directory.CreateDirectory(Application.dataPath + "/IGT_Data/InstancesLog");
		}
		int processesNo = Process.GetProcessesByName("ClubPenguinIsland").Length; //Gets the number of processes open when the game is opened
		// The way we'll keep track of what game is open is by storing each instance as a file in a folder, and when the instance is closed, we delete the file, so the other instances check for that file and then reorganize each other so it's accurate
		// We'll log all the instances with simply an empty file with the nubmer of the instances
		string logPath = Application.dataPath + "/IGT_Data/InstancesLog/"; // path for the places we'll use to keep track of how many game instances are open
		if (processesNo == 1) // If there is only one game open when you open, we don't need to worry about others and we make this instance the main one
		{
			new StreamWriter(logPath + "1").Close(); //Logs this instance
			instanceNo = 1; // Update our instance number //Is this used though?
			isMainInstance = true; // Make this main instance
			InitializeTimer(); // Run function for initializing the variables
		}
		else
		{
			instanceNo = countOpenInstances(); // Run function for counting instances open, instead of just looking at the processes, which doesn't give the accurate order
			if (instanceNo == 1) // If there is more than 1 process but this is the first one, we also make it the main instance and initialize the timer properly
			{
				isMainInstance = true;
				InitializeTimer();
			}
			new StreamWriter(logPath + instanceNo.ToString()).Close(); // Writes the log for this file so the other instances can keep track of it
		}
		string warningPath = Application.dataPath + "/IGT_Data/warning";
		if (!File.Exists(warningPath))
		{
			saveData("warning", "1");
			warningFlag = 1;
		}
		else
		{
			warningFlag = int.Parse(File.ReadAllText(warningPath));
		}
}
