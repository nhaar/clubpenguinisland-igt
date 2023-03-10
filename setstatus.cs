// This script is just to help split for Leaky Landing
// It's located in the ClubPenguinClient.dll, ClubPenguin.Net.Client namespace, SetStatusOperation class

// Method SetStatus

public static QuestChangeResponse SetStatus
	// ... In this method, search for the following place
{
	if (status == QuestStatus.COMPLETED)
	{
		// In here, add the line at the beginning
		if (questId == "AAC001Q001LeakyShip")
		{
			LoadingController.stopLeaky = true;
		}

	}
}
