using UnityEngine;
using System.Collections;

public class IActor : MonoBehaviour{
	public int id;
	public EActorType actor_type;
	public bool isEnermy;
	public IActorState state;
	public IActorAction action;
	public tk2dAnimatedSprite ani_sprite;

	public Vector2 GetWorldPos(){
		if(gameObject != null){
			float x = gameObject.transform.position.x;
			float y = gameObject.transform.position.y;
			Bounds bounds = gameObject.renderer.bounds;
			if(ani_sprite.scale.x > 0){
				x += bounds.size.x / 2;
			}else{
				x -= bounds.size.x / 2;
			}
			
			y += bounds.size.y / 2;
				
			Vector2 v2Pos = new Vector2(x, y);
			return v2Pos;
		}else{
			return Vector2.zero;
		}
	}
	
	/// <summary>
	/// Sets the position. lower left
	/// </summary>
	/// <param name='x'>
	/// X.
	/// </param>
	/// <param name='y'>
	/// Y.
	/// </param>
	public void SetCenterPos(float x, float y){
		int i = ani_sprite.scale.x > 0 ? 1 : -1;
		Bounds bounds = gameObject.renderer.bounds;
		float xOffset = -1 * i * bounds.size.x / 2;
		float yOffset = -1 *bounds.size.y / 2;
		gameObject.transform.position = new Vector3(x + xOffset, y + yOffset, 0f);
		
	}
	
	public float GetActorDistance(IActor actorOther){
		Vector2 posThis = this.GetWorldPos();
		Vector2 posOther = actorOther.GetWorldPos();
		return Vector2.Distance(posThis, posOther);
	}
	
	public bool IsFaceToFace(IActor actorOther){
		return this.ani_sprite.scale.x * actorOther.ani_sprite.scale.x < 0;
	}

    public void updataState(IActorAction action) {
        if(action.actiontype != EFSMAction.NONE){
//            Debug.Log("updataState - " + this.state + " by action:" + action.actiontype);//########
            IActorState asCur = this.state;
            IActorState asNext = asCur.toNextState(action.actiontype);
            if (asNext != null)
            {
                this.state = asNext;
				this.action = action;
                this.state.OnEnter();
            }
        }
    }
	
	public bool IsFacingRight(){
		return ani_sprite.scale.x > 0;
	}

    public bool IsInState(System.Type type) {
        return state.GetType() == type;
    }
	
	public virtual void DoUpdateAttack() {
       
    }

    public virtual void DoUpdateIdle() { }
	
	public virtual void DoUpdateMove(){}
	
	
	public virtual void DoUpdateRun(){}
	
	public virtual void OnEnterRun(){}
	
	public virtual void OnEnterAttack(){}
	
	public virtual void OnEnterUnAttack(){}
	
	public virtual void OnEnterIdle(){}
	
	public virtual void OnEnterMove(){}
	
	public virtual void OnEnterHeroFlash(){}
	
	public virtual void DoUpdateHeroFlash(){}
	
	public virtual void OnEnterHeroFlashAttack(){}
	
	public virtual void OnEnterUnAttack_By_Flash(){}
	
	public virtual void OnEnterOnAirDown(){}
	public virtual void DoUpdateOnAirDown(){}
	
	public virtual void OnEnterOnAirUp(){}
	public virtual void DoUpdateOnAirUp(){}

	public virtual void OnEnterShin(){}
	public virtual void DoUpdateShin(){}
	
	public virtual void OnEnterCatchPoint(){}
	public virtual void DoUpdateCatchPoint(){}
}
