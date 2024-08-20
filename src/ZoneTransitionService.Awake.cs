// PUT AT THE END OF THE METHOD

// public Awake()
// {
//     ...
//     <------------- CODE HERE AT THE END
// }

using System.Diagnostics;
using System.IO;
using System.Globalization;

toCheatEngine2 = 999;
toCheatEngine3 = 111; // Make getting memory adress easier
string dataPath = Application.dataPath;
string splitsPath = dataPath.Substring(0, dataPath.Length - 22) + "Splits";
if (!Directory.Exists(Application.dataPath + "/IGT_Data"))
{
    Directory.CreateDirectory(Application.dataPath + "/IGT_Data");
}
if (!Directory.Exists(Application.dataPath + "/IGT_Data/InstancesLog"))
{
    Directory.CreateDirectory(Application.dataPath + "/IGT_Data/InstancesLog");
}
if (!Directory.Exists(splitsPath))
{
    Directory.CreateDirectory(splitsPath);
    Directory.CreateDirectory(splitsPath + "/SplitsHistory");
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