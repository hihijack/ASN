using UnityEngine;
using System.Collections;
using System.Reflection;

public class Hero : IActor
{
	
	public float speed = 10.0f;
	
	public float shinSpeed = 4f;
	
	public float jumpSpeed = 8.0f;
	
	public float gravity = 20.0f;
	
	public float pushPow = 1f;
	
	private CharacterController cc;
	
	private GameObject g_gobjCurStepOn;
	
	private Vector2 moveDir = Vector2.zero;
	
	GameView gameView;
	
	int axisH = 0;
	int axisV = 0;
	int btnA = 0;
	int btnB = 0;
	
	
	private int g_Clock_Times = 0;
	
	private int axisHCancelShin;
	
	void Start(){
		isEnermy = false;
		actor_type = EActorType.Hero;
		cc = GetComponent<CharacterController>();
		ani_sprite = GetComponent<tk2dAnimatedSprite>();
		state = new HeroActorState_Idle(this);
		gameView = GameObject.Find("CPU").GetComponent<GameView>();
	}
	
	void Update(){
		axisH = gameView.VCInput_Axis;
		axisV = gameView.VCInput_Ver_Axis;
		btnA = gameView.VCInput_BtnA;
		btnB = gameView.VCInput_BtnB;
		moveDir.x = 0f;
		this.state.DoUpdata();
		gameView.VCInput_BtnA = 0;
		gameView.VCInput_BtnB = 0;
	}
	
	void OnGUI(){
	}
	
	public override void DoUpdateIdle ()
	{
		moveDir.y = -0.1f;
		cc.SimpleMove(moveDir);
		if(!cc.isGrounded){
			updataState(new IActorAction(EFSMAction.HERO_ONAIR_DOWN));
		}else{
			
			if(gameView.IsInGameState(EGameState.Running)){
			
				if(axisH != 0){
					updataState(new IActorAction(EFSMAction.HERO_RUN));
				}else if(btnA > 0){
					updataState(new IActorAction(EFSMAction.HERO_ONAIR_UP));
				}
			}
		}
	}
	
	public override void OnEnterShin ()
	{
		ani_sprite.Play("idle");
	}
	
	public override void DoUpdateShin ()
	{
		moveDir.y = axisV * shinSpeed * Time.deltaTime;
		cc.Move(moveDir);
		if(axisH == axisHCancelShin){
			updataState(new IActorAction(EFSMAction.HERO_ONAIR_DOWN));
		}
	}
	
	public override void DoUpdateRun ()
	{
		if(cc.isGrounded){
			if(axisH != 0){
				moveDir.x = axisH * speed;
				cc.Move(moveDir * Time.deltaTime);
				if(axisH > 0){
					SetFace(true);
				}else{
					SetFace(false);
				}
			}else{
				updataState(new IActorAction(EFSMAction.HERO_IDLE));
			}
			
			if(btnA > 0){
				updataState(new IActorAction(EFSMAction.HERO_ONAIR_UP));
			}	
		}else{
			updataState(new IActorAction(EFSMAction.HERO_ONAIR_DOWN));
		}
		
	}
	
	public override void OnEnterIdle ()
	{
		ani_sprite.Play("idle");
	}
	
	public override void OnEnterRun ()
	{
		ani_sprite.Play("run");
	}
	
	public override void OnEnterOnAirDown ()
	{
		ani_sprite.Play("jump_down");
	}
	
	public override void DoUpdateOnAirDown ()
	{
		moveDir.y -= gravity * Time.deltaTime;
		if(axisH != 0){
			moveDir.x = axisH * speed;
			if(axisH > 0){
				SetFace(true);
			}else{
				SetFace(false);
			}
		}
		cc.Move(moveDir * Time.deltaTime);
		if(cc.isGrounded){
			updataState(new IActorAction(EFSMAction.HERO_IDLE));
		}
		
//		if(btnA > 0){
//			updataState(new IActorAction(EFSMAction.HERO_ONAIR_UP));
//		}
	}
	
	public override void OnEnterOnAirUp ()
	{
		ani_sprite.Play("jump_up");
		float jumpSpeedTemp = jumpSpeed;
		
		moveDir.y = jumpSpeedTemp;
		
	} 
	
	public override void DoUpdateOnAirUp ()
	{
		moveDir.y -= gravity * Time.deltaTime;
		if(axisH != 0){
			moveDir.x = axisH * speed;
			if(axisH > 0){
				SetFace(true);
			}else{
				SetFace(false);
			}
		}
		if(moveDir.y > 0){
			cc.Move(moveDir * Time.deltaTime);
		}else{
			updataState(new IActorAction(EFSMAction.HERO_ONAIR_DOWN));
		}
//		
//		if(btnA > 0){
//			updataState(new IActorAction(EFSMAction.HERO_ONAIR_UP));
//		}
	}
	
	public void SetFace(bool isRigth){
		if(isRigth && ani_sprite.scale.x < 0){
			ani_sprite.FlipX();
		}else if(!isRigth && ani_sprite.scale.x > 0){
			ani_sprite.FlipX();
		}
	}
	
	public bool IsHitSomeThing(){
		return false;
	}
	
	public GameObject GetCurBGGameObject(){
		GameObject gobjR = null;
		RaycastHit[] hits;
		Vector3 posOri = gameView.main_camera.transform.position;
		Vector3 direction  = Vector3.forward;
		hits = Physics.RaycastAll(posOri, direction, 100.0f);
		if(hits.Length == 2 && hits[1].transform.name.Equals("hero")){
			gobjR = hits[0].transform.gameObject;
		}else{
//			Debug.LogError("hits.Length != 2" + hits.Length);
		}
		return gobjR;
	}
	
	public GameObject GetSpetOnGameObject(){
		return g_gobjCurStepOn;
	}
	#region Game Mehtods
	public void OnClockAniEnd(){
		g_Clock_Times++;
		if(g_Clock_Times == 3){
			GameObject gobjClock = GameObject.Find("clock");
			GameObject gobjLockRune_1_Lock = GameObject.Find("LockRune_1_Lock");
			gobjLockRune_1_Lock.name = "LockRune_1_UnLock";
			Tools.SetGameObjMaterial(gobjLockRune_1_Lock, "rune_norm");
			BoxCollider coll = Tools.GetComponentInChildByPath<BoxCollider>(gobjClock, "clock_active");
			Destroy(coll);
		}
	}
	
	public void InteractiveHandle(){
		if(btnB > 0 && gameView.IsInGameState(EGameState.Running)){
			GameObject gobjInteractive =  GetCurBGGameObject();
			if(gobjInteractive != null){
				string name = gobjInteractive.name;
				// call event handle func
				string funcName = "InteractiveEventHandleByGameObjName_" + name;
				MethodInfo  mi = this.GetType().GetMethod(funcName);
				if(mi != null){
					mi.Invoke(this, null);
				}else{
					Debug.LogError("can't find method:" + funcName);
				}
			}else{
//				Debug.LogError("Can't find Interactive");
			}
		}
	}
	#endregion
	
	#region InteractiveEventHandle
	#endregion
	
	void OnControllerColliderHit(ControllerColliderHit hit) {
//		if(hit.moveDirection.y < 0 && Mathf.Abs(hit.moveDirection.x) < 0.3f ){
//			g_gobjCurStepOn = hit.gameObject;
//		}
//		
//		// killed when hit enermy
//		if(hit.collider.CompareTag("enermy")){
//			Killed();
//		}
		
		if(hit.gameObject.CompareTag("shinable")){
			
			if(Tools.IsInTheRight(gameObject, hit.gameObject)? (axisH < 0) : (axisH > 0)){
				updataState(new IActorAction(EFSMAction.HERO_SHIN));
				axisHCancelShin = -1 * axisH;
			}
		}
	}
	
	void OnTriggerEnter(Collider other){
		GameObject gobjOther = other.gameObject;
	}

	
	void Killed(){
		
	}
	
}
