using UnityEngine;
using System.Collections;

public class AIMoveBy : IAIAction {
	public float x;
	
	public AIMoveBy(float x){
		this.aiType = EAIAction.MoveBy;
		this.x = x;
	}
}
