// Step 3, adding the startTimer and stopTimer scripts to where they will work
// Luckily it's all in a single class, QuestService

// For the start one, you need to place it in two methods, one for Leaky Landing and another for the other ILs
// First method is onStartRequest

private bool onStartQuestRequest(QuestEvents.StartQuest evt)
{
	//put it at the very start
	ZoneTransitionService.startTimeNow = true;
	// etc...
}

//Second method is ShowStartQuestSplashscreen

public IEnumerator ShowStartQuestSplashscreen(Quest quest)
{
//	 ....
	if (gameObject != null && gameObject.GetComponentInChildren<PopupManager>() != null)
	{
		//... at the very end of this if statement
		ZoneTransitionService.startTimeNow = true;
	}
	yield break;
}

// For the ending one, you need to place it in the method EndQuest at the very start. However, we must also make sure it only ends if the proper modes are met so we use the questName argument

public void EndQuest(GameObject player, string questName)
{
	ZoneTransitionService.stopTimer(questName);

	// If leaky landing, prevent from splitting again
	if (questName == "AAC001Q001LeakyShip")
	{
		ZoneTransitionService.finishedLeakyLandingNormally = true;
	}
}
