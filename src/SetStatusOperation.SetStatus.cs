// PUT CODE IN THE 4TH BLOCK INSIDE THE METHOD

// This script is just to help split for Leaky Landing
// It's located in the ClubPenguinClient.dll, ClubPenguin.Net.Client namespace, SetStatusOperation class

// public static QuestChangeResponse SetStatus
// {
// 	... search for the following place:
// 	if (status == QuestStatus.COMPLETED)
// 	{
//       <------------ CODE HERE AT THE START
//       ...
// 	}
// }


// arbitrary point where leaky leanding skip will count as finished
if (questId == "AAC001Q001LeakyShip")
{
    LoadingController.stopLeaky = true;
}