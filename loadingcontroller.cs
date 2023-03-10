// Step 1: detect when the game is loading

//@ UnityShared LoadingController

public static bool isSpeedrunLoading; // Variable to keep track if the game is loading
public static bool stopLeaky; // Variable to see if we should stop Leaky Landing

//turning it on because the load started

public void OnEnable() //inside OnEnable write
{
	isSpeedrunLoading = true;
}

//turning it off because the load ended

public void Update() inside Update()
{
	//Other things
	//there will be something like
	if (this.hideDelay <= 0)
	{
		//...
		//In here write
		isSpeedrunLoading = false;
	}
}
