using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public enum EGameState{
	Running,
	UIInfoing,
	UIPaging
}

public class GameView : MonoBehaviour
{
	
	public int VCInput_Axis;
	public int VCInput_Ver_Axis;
	public int VCInput_BtnA;
	public int VCInput_BtnB;
	
	public EGameState gameState;
	
	public Camera main_camera;
	
	private IActor[] g_Enemys;
	
	public GameObject gobjUIPanle;
	
	private UILabel txtTip;
	
	void Awake(){
		InitNPCAI();
	}
	
	void Start ()
	{
		txtTip = Tools.GetComponentInChildByPath<UILabel>(gobjUIPanle, "LabelTip");
	}
	
	// Update is called once per frame
	void Update ()
	{
		// keyboard controll
		/// for test.when build， close it
		#if UNITY_EDITOR||UNITY_STANDALONE_WIN
		if(Input.GetKey(KeyCode.LeftArrow)){
			VCInput_Axis = -1;
		}else if(Input.GetKey(KeyCode.RightArrow)){
			VCInput_Axis = 1;
		}else{
			VCInput_Axis = 0;
		}
		
		if(Input.GetKey(KeyCode.UpArrow)){
			VCInput_Ver_Axis = 1;
		}else if(Input.GetKey(KeyCode.DownArrow)){
			VCInput_Ver_Axis = -1;
		}else{
			VCInput_Ver_Axis = 0;
		}
		
		if(Input.GetKeyDown(KeyCode.Z)){
			VCInput_BtnA = 1;
		}else if(Input.GetKeyUp(KeyCode.Z)){
			VCInput_BtnA = 0;
		}
		
		if(Input.GetKeyDown(KeyCode.X)){
			VCInput_BtnB = 1;
		}else if(Input.GetKeyUp(KeyCode.X)){
			VCInput_BtnB = 0;
		}
		#endif
		
	}

	//×××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××××
	
	public void OnBtnPress(string btnname, bool isDown){
		if("btn_down".Equals(btnname)){
			if(isDown){
				VCInput_Ver_Axis = -1;
			}else{
				VCInput_Ver_Axis = 0;
			}
		}
		if("btn_up".Equals(btnname)){
			if(isDown){
				VCInput_Ver_Axis = 1;
			}else{
				VCInput_Ver_Axis = 0;
			}
		}
		if("btn_left".Equals(btnname)){
			if(isDown){
				VCInput_Axis = -1;
			}else{
				VCInput_Axis = 0;
			}
		}
		if("btn_right".Equals(btnname)){
			if(isDown){
				VCInput_Axis = 1;
			}else{
				VCInput_Axis = 0;
			}
		}
		
		if("btn_A".Equals(btnname)){
			if(isDown){
				VCInput_BtnA = 1;
			}else{
				VCInput_BtnA = 0;
			}
		}
		if("btn_B".Equals(btnname)){
			if(isDown){
				VCInput_BtnB = 1;
			}else{
				VCInput_BtnB = 0;
			}
		}
	}
	
	public bool IsInGameState(EGameState gameState){
		return this.gameState == gameState;
	}
	
	public void InitNPCAI(){
		
		RegisterAllEnemys();
		
		
		
		Enemy enemy001 = GetEnemyByName("Enemy_Gard_001");
		enemy001.InitAIActions(new AIMoveBy(1.28f), new AIStop(3f), new AITurn(), new AIMoveBy(-1.28f), new AIStop(3f), new AITurn());
		
		Enemy enemy002 = GetEnemyByName("Enemy_Gard_002");
		enemy002.InitAIActions(new AIMoveBy(1.44f), new AIStop(1.5f), new AITurn(), new AIMoveBy(-1.44f), new AIStop(1.5f), new AITurn());
		
		Enemy enemy003 = GetEnemyByName("Enemy_Gard_003");
		enemy003.InitAIActions(new AIMoveBy(1.12f), new AIStop(2f), new AITurn(), new AIMoveBy(-1.12f), new AIStop(2f), new AITurn());
		
		Enemy enemy004 = GetEnemyByName("Enemy_Gard_004");
		enemy004.InitAIActions(new AIMoveBy(1f), new AIStop(2f), new AITurn(), new AIMoveBy(-1), new AIStop(2f), new AITurn());
		
		Enemy enemyTarget = GetEnemyByName("Enemy_Target_005");
		
	}
	
	public IActor[] GetEnemys(){
		return g_Enemys;
	}
	
	public Enemy GetEnemyByName(string gobjName){
		Enemy r = null;
		GameObject gobjEnemy = GameObject.Find(gobjName);
		if(gobjEnemy != null){
			r = gobjEnemy.GetComponent<Enemy>();
		}
		return r;
	}
	
	public void RegisterAllEnemys(){
		GameObject[] gobjEnemys = GameObject.FindGameObjectsWithTag("enemy");
		int count = gobjEnemys.Length;
		g_Enemys = new Enemy[count];
		for (int i = 0; i < gobjEnemys.Length; i++) {
			GameObject gobjEnemy = gobjEnemys[i];
			Enemy enemy = gobjEnemy.GetComponent<Enemy>();
			g_Enemys[i] = enemy;
		}
	}
	
	public void FailGame(){
		NGUITools.SetActiveSelf(txtTip.gameObject, true);
	}
}