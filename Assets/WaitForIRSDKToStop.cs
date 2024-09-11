
using UnityEngine;
using UnityEngine.Rendering;

using IRSDKSharper;

public class WaitForIRSDKToStop : CustomYieldInstruction
{
	private readonly IRacingSdk irsdk;

	public WaitForIRSDKToStop( IRacingSdk irsdk )
	{
		this.irsdk = irsdk;
	}

	public override bool keepWaiting
	{
		get
		{
			return irsdk.IsStarted;
		}
	}
}
