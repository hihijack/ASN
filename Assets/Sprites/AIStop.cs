using UnityEngine;
using System.Collections;

public class AIStop : IAIAction {
	public float durTime;
	
	public AIStop(float durTime){
		this.aiType = EAIAction.Stop;
		this.durTime = durTime;
	}
}
