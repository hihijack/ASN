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
	void Start ()
	{
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
}