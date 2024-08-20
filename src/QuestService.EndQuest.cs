// PUT AT THE START OF THE METHOD

// public EndQuest()
// {
//     <------------- CODE HERE AT THE START
//     ...
// }


// ends all the segments
ZoneTransitionService.stopTimer(questName);
// If leaky landing, prevent from splitting again
if (questName == "AAC001Q001LeakyShip")
{
    ZoneTransitionService.finishedLeakyLandingNormally = true;
}