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
	
	public float killRange;
	
	private Enemy curFacous;
	
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
		
		CheckEnemyInRange(killRange);
		
		gameView.VCInput_BtnA = 0;
		gameView.VCInput_BtnB = 0;
	}
	
	void OnGUI(){
	}
	
	#region FSM Handler
	public override void OnEnterAttack ()
	{
		transform.position = curFacous.transform.position;
		Kill(curFacous);
	}
	
	public override void DoUpdataAttack ()
	{
		updataState(new IActorAction(EActorAction.HERO_IDLE));
	}
	
	public override void OnEnterCatchPoint ()
	{
		ani_sprite.Play("idle");
	}
	
	public override void DoUpdateCatchPoint ()
	{
		if(btnA > 0){
			updataState(new IActorAction(EActorAction.HERO_ONAIR_UP));
		}else if(axisV < 0){
		    updataState(new IActorAction(EActorAction.HERO_ONAIR_DOWN));
		}
	}
	
	public override void DoUpdateIdle ()
	{
		moveDir.y = -0.1f;
		cc.SimpleMove(moveDir);
		if(!cc.isGrounded){
			updataState(new IActorAction(EActorAction.HERO_ONAIR_DOWN));
		}else{
			
			if(gameView.IsInGameState(EGameState.Running)){
			
				if(axisH != 0){
					updataState(new IActorAction(EActorAction.HERO_RUN));
				}else if(btnA > 0){
					updataState(new IActorAction(EActorAction.HERO_ONAIR_UP));
				}else if(btnB > 0 && curFacous != null){
					updataState(new IActorAction(EActorAction.HERO_ATTACK));
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
		
		GameObject curShinGObj = GetCurTouchGameObject(axisHCancelShin < 0);
		if(curShinGObj != null){
			if(curShinGObj.name.Equals("Box_Up") && axisV > 0){
				moveDir.y = 0;
			}else if(curShinGObj.name.Equals("Box_Down") && axisV < 0){
				moveDir.y = 0;
			}
		}
		
		cc.Move(moveDir);
		
		if(axisH == axisHCancelShin){
			updataState(new IActorAction(EActorAction.HERO_ONAIR_DOWN));
		}
		
		if(btnA > 0){
			updataState(new IActorAction(EActorAction.HERO_ONAIR_UP));
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
				updataState(new IActorAction(EActorAction.HERO_IDLE));
			}
			
			if(btnA > 0){
				updataState(new IActorAction(EActorAction.HERO_ONAIR_UP));
			}
			
			if(btnB > 0 && curFacous != null){
				updataState(new IActorAction(EActorAction.HERO_ATTACK));
			}
		}else{
			updataState(new IActorAction(EActorAction.HERO_ONAIR_DOWN));
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
			updataState(new IActorAction(EActorAction.HERO_IDLE));
		}
		
		if(btnB > 0 && curFacous != null){
			updataState(new IActorAction(EActorAction.HERO_ATTACK));
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
			updataState(new IActorAction(EActorAction.HERO_ONAIR_DOWN));
		}
//		
		
		if(btnB > 0 && curFacous != null){
			updataState(new IActorAction(EActorAction.HERO_ATTACK));	
		}
		
//		if(btnA > 0){
//			updataState(new IActorAction(EFSMAction.HERO_ONAIR_UP));
//		}
	}
	
	#endregion
	

	
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
	/// <summary>
	/// Gets the current touch game object.
	/// </summary>
	/// <returns>
	/// The current touch game object.
	/// </returns>
	/// <param name='isRight'>
	/// the dir that hero  to  other obj
	/// </param>
	public GameObject GetCurTouchGameObject(bool isRight){
		GameObject gobjR = null;
		RaycastHit hit;
		Vector3 posOri = transform.position;
		Vector3 direction;
		if(isRight){
			direction = Vector3.right;
		}else{
			direction =Vector3.left;
		}
		if(Physics.Raycast(posOri, direction, out hit, 0.5f)){
			gobjR = hit.transform.gameObject;
		}
		return gobjR;
	}
	
	public GameObject GetSpetOnGameObject(){
		return g_gobjCurStepOn;
	}
	#region Game Mehtods
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
				updataState(new IActorAction(EActorAction.HERO_SHIN));
				axisHCancelShin = -1 * axisH;
			}
		}
	}
	
	void OnTriggerEnter(Collider other){
		GameObject gobjOther = other.gameObject;
		if(gobjOther.CompareTag("catchpoint") && axisV > 0){
			updataState(new IActorAction(EActorAction.HERO_CATCHPOINT));
		}
	}

	
	void Killed(){
		
	}
	
	
	void CheckEnemyInRange(float range){
		IActor[] enemys = gameView.GetEnemys(); 
		int enemyCount = enemys.Length;
		for (int i = 0; i < enemyCount; i++) {
			Enemy enemy = enemys[i] as Enemy;
			if(enemy != null){
				float distance = GetActorDistance(enemy);
				if(distance <= range){
					FocusEnemy(enemy);
				}else{
					CancelFocusEnemy(enemy);
				}
			}
		}
	}
	
	void FocusEnemy(Enemy enemy){
		if(!enemy.IsFocused()){
			enemy.SetIsFocused(true);
			curFacous = enemy;
			GameObject gobjMark = Tools.GetGameObjectInChildByPathSimple(enemy.gameObject, "KillMark");
			if(gobjMark == null){
				gobjMark = Tools.LoadResourcesGameObject(IPath.Path_Effects + "KillMark", enemy.gameObject);
				gobjMark.transform.localPosition = new Vector3(0f, 0.41f, 0f);
				gobjMark.name = "KillMark";
			}else{
				gobjMark.SetActive(true);
			}
		}
	}
	
	void CancelFocusEnemy(Enemy enemy){
		if(enemy.IsFocused()){
			enemy.SetIsFocused(false);
			curFacous = null;
			GameObject gobjMark = Tools.GetGameObjectInChildByPathSimple(enemy.gameObject, "KillMark");
			gobjMark.SetActive(false);
		}
		
	}
	
	bool HasFocus(){
		return curFacous != null;
	}
	
	void Kill(Enemy enemy){
		enemy.updataState(new IActorAction(EActorAction.NPC_DIE));
	}
	
}
