using UnityEngine;
using System.Collections;

public enum EViewType{
	A,
	B
}

public class CameraControll : MonoBehaviour {

	public GameObject target;
	public EViewType viewType;
	GameView gameView;
	private float upSpeed = 5.0f;
	private float uplimitOffset = 4.0f;
	private float posY;
	// Use this for initialization
	void Start () {
		viewType = EViewType.A;
		gameView = GameObject.Find("CPU").GetComponent<GameView>();
	}
	
	// Update is called once per frame
	void Update () {
		float x = target.transform.position.x;
		float y = target.transform.position.y;
		
		float zSelf = transform.position.z;
		
		// look up
//		if(gameView.VCInput_Ver_Axis > 0){
//			posY += upSpeed * Time.deltaTime;
//			if(posY > (y + uplimitOffset)){
//				posY = (y + uplimitOffset);
//			}
//		}else{
//			posY -= upSpeed * Time.deltaTime;
//			if(posY < y){
//				posY = y;
//			}
//		}
//		
		if(viewType == EViewType.A){
			transform.position = new Vector3(x, y, zSelf);
		}
//			else if(viewType == EViewType.B){
//			transform.position = new Vector3(xSelf, y, z);
//		}
		
		
//		if(Input.GetKeyDown(KeyCode.R)){
//			if(viewType == EViewType.A){
//				viewType = EViewType.B;
//				transform.RotateAround(target.transform.position, 90);
//			}else{
//				viewType = EViewType.A;
//				transform.RotateAround(target.transform.position, -90);
//			}
//		}
	}
}
