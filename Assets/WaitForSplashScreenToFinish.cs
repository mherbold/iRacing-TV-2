
using UnityEngine;
using UnityEngine.Rendering;

public class WaitForSplashScreenToFinish : CustomYieldInstruction
{
	public override bool keepWaiting
	{
		get
		{
			return !SplashScreen.isFinished;
		}
	}
}
