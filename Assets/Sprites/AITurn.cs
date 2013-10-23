using UnityEngine;
using System.Collections;

public class AITurn : IAIAction {
	public AITurn(){
		this.aiType = EAIAction.Turn;
	}
}
