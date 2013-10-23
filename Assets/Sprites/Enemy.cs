using UnityEngine;
using System.Collections;

public class Enemy : IActor {

	public IAIAction[] aiActions;
	
	private int curAIIndex = -1;
	
	public float speedWalk = 1f;
	
	
	private int direct = 1;
	
	private CharacterController cc;
	
	private Vector3 walkTargetPos;
	private float stopOverTime;
	
	private bool isFocused = false;
	
	private GameView gameView;
	
	// Use this for initialization
	void Start () {
		
		gameView = GameObject.Find("CPU").GetComponent<GameView>();
		
		ani_sprite = GetComponent<tk2dAnimatedSprite>();
		cc = GetComponent<CharacterController>();
		state = new NPCActorState_Idle(this);
		
		if(aiActions != null){
			ToNextAIAction();
			DoCurAction();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		this.state.DoUpdata();
		
		if(IsOverCurAction()){
			ToNextAIAction();
			DoCurAction();
		}
		
		CheckWarnHero();
	}
	
	#region FSM
	public override void OnEnterIdle ()
	{
		
	}
	
	public override void DoUpdataNPCIdle ()
	{
		
	}
	
	public override void OnEnterWalk ()
	{
		
	}
	
	public override void DoUpdataNPCWalk ()
	{
		Vector3 move = new Vector3(speedWalk * Time.deltaTime * direct, 0f, 0f);
		cc.Move(move);
	}
	
	public override void OnEnterDie ()
	{
		DestroyObject(gameObject);
	}
	#endregion
	
	public IAIAction ToNextAIAction(){
		curAIIndex ++;
		if(curAIIndex >= aiActions.Length){
			curAIIndex = 0;
		}
		
		
		return aiActions[curAIIndex];
	}
	
	public IAIAction GetCurAIAction(){
		return aiActions[curAIIndex];
	}
	
	public void DoCurAction(){
		IAIAction curAction = GetCurAIAction();
		if(curAction.aiType == EAIAction.MoveBy){
			AIMoveBy amb = curAction as AIMoveBy;
			walkTargetPos = transform.position + new Vector3(amb.x, 0f, 0f);
			updataState(new IActorAction(EActorAction.NPC_WALK));
		}else if(curAction.aiType == EAIAction.Stop){
			AIStop stopAction = curAction as AIStop;
			stopOverTime = Time.time + stopAction.durTime;
			updataState(new IActorAction(EActorAction.NPC_IDLE));
		}else if(curAction.aiType == EAIAction.Turn){
			TurnDir();
		}
	}
	
	public bool IsOverCurAction(){
		bool r = false;
		
		if(aiActions != null){
			IAIAction curAction = GetCurAIAction();
			if(curAction.aiType == EAIAction.MoveBy){
				Vector3 curPos = transform.position;
				if(Mathf.Abs(curPos.x - walkTargetPos.x) <= 0.5f){
					r = true;
				}
			}
			else if(curAction.aiType == EAIAction.Stop){
				if(Mathf.Abs(Time.time - stopOverTime) <= 0.1f){
					r = true;
				}
			}
			
			else if(curAction.aiType == EAIAction.Turn){
				r = true;
			}
		}
		
		return r;
	}
	
	public void TurnDir(){
		direct *= -1;
		SetFace(direct > 0);
	}
	
	public void CheckWarnHero(){
		Vector3 dir;
		if(IsFacingRight()){
			dir = Vector3.right;
		}else{
			dir = Vector3.left;
		}
		
		Ray rayView = new Ray(transform.position, dir);
		
		RaycastHit rh;
		
		if(Physics.Raycast(rayView, out rh)){
			if(rh.collider.CompareTag("hero")){
				gameView.FailGame();
			}
		} 
	}
	
	public void SetIsFocused(bool isFocused){
		this.isFocused = isFocused;
	}
	
	public bool IsFocused(){
		return isFocused;
	}
	
	public void InitAIActions(params IAIAction[] actions){
		this.aiActions = actions;
	}
}
