using UnityEngine;
using System.Collections;

public class IActorState{
    public IActor actor;
    public float time;
    public IActorState() {}
    public virtual IActorState toNextState(EActorAction action) { return null; }
	/// <summary>
	/// Raises the enter event.call at the frame that to call updataState
	/// </summary>
    public virtual void OnEnter() { }
    public virtual void DoUpdata() { }
}

#region HeroFSM
public class HeroActorState_Idle : IActorState{
	public HeroActorState_Idle(IActor actor){
		this.actor = actor;
	}
	
	public override IActorState toNextState (EActorAction action)
	{
		IActorState result = null;
		if(action == EActorAction.HERO_RUN){
			result = new HeroActorState_Run(actor);
		}else if(action == EActorAction.HERO_ONAIR_DOWN){
			result = new HeroActorState_OnAir_Down(actor);
		}else if(action == EActorAction.HERO_ONAIR_UP){
			result = new HeroActorState_OnAir_Up(actor);
		}else if(action == EActorAction.HERO_ATTACK){
			result = new HeroActorState_Attack(actor);
		}
		return result;
	}
	
	public override void DoUpdata ()
	{
		actor.DoUpdateIdle();
	}
	
	public override void OnEnter ()
	{
		actor.OnEnterIdle();
	}
}


public class HeroActorState_OnAir_Up : IActorState{
	public HeroActorState_OnAir_Up(IActor actor){
		this.actor = actor;
	}
	
	public override IActorState toNextState (EActorAction action)
	{
		IActorState result = null;
		if(action == EActorAction.HERO_ONAIR_DOWN){
			result = new HeroActorState_OnAir_Down(actor);
		}
		else if(action == EActorAction.HERO_ONAIR_UP){
			result = new HeroActorState_OnAir_Up(actor);
		}else if(action == EActorAction.HERO_SHIN){
			result = new HeroActorState_Shin(actor);
		}else if(action == EActorAction.HERO_CATCHPOINT){
			result = new HeroActorState_CatchPoint(actor);
		}else if(action == EActorAction.HERO_ATTACK){
			result = new HeroActorState_Attack(actor);
		}
		return result;
	}
	
	public override void DoUpdata ()
	{
		actor.DoUpdateOnAirUp();
	}
	
	public override void OnEnter ()
	{
		actor.OnEnterOnAirUp();
	}
}

public class HeroActorState_Run : IActorState{
	public HeroActorState_Run(IActor actor){
		this.actor = actor;
	}
	
	public override IActorState toNextState (EActorAction action)
	{
		IActorState result = null;
		if(action == EActorAction.HERO_IDLE){
			result = new HeroActorState_Idle(actor);
		}else if(action == EActorAction.HERO_ONAIR_UP){
			result = new HeroActorState_OnAir_Up(actor);
		}else if(action == EActorAction.HERO_ONAIR_DOWN){
			result = new HeroActorState_OnAir_Down(actor);
		}else if(action == EActorAction.HERO_ATTACK){
			result = new HeroActorState_Attack(actor);
		}
		return result;
	}
	
	public override void DoUpdata ()
	{
		actor.DoUpdateRun();
	}
	
	public override void OnEnter ()
	{
		actor.OnEnterRun();
	}
}

public class HeroActorState_OnAir_Down : IActorState{
	public HeroActorState_OnAir_Down(IActor actor){
		this.actor = actor;
	}
	
	public override IActorState toNextState (EActorAction action)
	{
		IActorState result = null;
		if(action == EActorAction.HERO_IDLE){
			result = new HeroActorState_Idle(actor);
		}
		else if(action == EActorAction.HERO_ONAIR_UP){
			result = new HeroActorState_OnAir_Up(actor);
		}else if(action == EActorAction.HERO_SHIN){
			result = new HeroActorState_Shin(actor);
		}else if(action == EActorAction.HERO_CATCHPOINT){
			result = new HeroActorState_CatchPoint(actor);
		}else if(action == EActorAction.HERO_ATTACK){
			result = new HeroActorState_Attack(actor);
		}
		return result;
	}
	
	public override void DoUpdata ()
	{
		actor.DoUpdateOnAirDown();
	}
	
	public override void OnEnter ()
	{
		actor.OnEnterOnAirDown();
	}
}

public class HeroActorState_Shin : IActorState{
	public HeroActorState_Shin(IActor actor){
		this.actor = actor;
	}
	
	public override IActorState toNextState (EActorAction action)
	{
		IActorState result = null;
		if(action == EActorAction.HERO_ONAIR_DOWN){
			result = new HeroActorState_OnAir_Down(actor);
		}else if(action == EActorAction.HERO_ONAIR_UP){
			result = new HeroActorState_OnAir_Up(actor);
		}
		return result;
	}
	
	public override void DoUpdata ()
	{
		actor.DoUpdateShin();
	}
	
	public override void OnEnter ()
	{
		actor.OnEnterShin();
	}
}

public class HeroActorState_CatchPoint : IActorState{
	public HeroActorState_CatchPoint(IActor actor){
		this.actor = actor;
	}
	
	public override IActorState toNextState (EActorAction action)
	{
		IActorState result = null;
		if(action == EActorAction.HERO_ONAIR_UP){
			result = new HeroActorState_OnAir_Up(actor);
		}else if(action == EActorAction.HERO_ONAIR_DOWN){
			result = new HeroActorState_OnAir_Down(actor);
		}
		return result;
	}
	
	public override void DoUpdata ()
	{
		actor.DoUpdateCatchPoint();
	}
	
	public override void OnEnter ()
	{
		actor.OnEnterCatchPoint();
	}
}

public class HeroActorState_Attack : IActorState{
	public HeroActorState_Attack(IActor actor){
		this.actor = actor;
	}
	
	public override IActorState toNextState (EActorAction action)
	{
		IActorState result = null;
		if(action == EActorAction.HERO_IDLE){
			result = new HeroActorState_Idle(actor);
		}
		return result;
	}
	
	public override void OnEnter ()
	{
		actor.OnEnterAttack();
	}
	
	public override void DoUpdata ()
	{
		actor.DoUpdataAttack();
	}
}

	
#endregion
	
	#region NPC FSM
public class NPCActorState_Idle : IActorState{
	public NPCActorState_Idle(IActor actor){
		this.actor = actor;
	}
	
	public override IActorState toNextState (EActorAction action)
	{
		IActorState result = null;
		if(action == EActorAction.NPC_WALK){
			result = new NPCActorState_Walk(actor);
		}
		else if(action == EActorAction.NPC_DIE){
			result = new NPCActorState_Die(actor);
		}
		else if(action == EActorAction.NPC_WARN){
			result = new NPCActorState_Warn(actor);
		}
		return result;
	}
	
	public override void OnEnter ()
	{
		actor.OnEnterNPCIdle();
	}
	
	public override void DoUpdata ()
	{
		actor.DoUpdataNPCIdle();
	}
	
	
}

public class NPCActorState_Walk : IActorState{
	public NPCActorState_Walk(IActor actor){
		this.actor = actor;
	}
	
	public override IActorState toNextState (EActorAction action)
	{
		IActorState result = null;
		if(action == EActorAction.NPC_IDLE){
			result = new NPCActorState_Idle(actor);
		}
		else if(action == EActorAction.NPC_DIE){
			result = new NPCActorState_Die(actor);
		}
		else if(action == EActorAction.NPC_WARN){
			result = new NPCActorState_Warn(actor);
		}
		return result;
	}
	
	public override void OnEnter ()
	{
		actor.OnEnterWalk();
	}
	
	public override void DoUpdata ()
	{
		actor.DoUpdataNPCWalk();
	}
}

public class NPCActorState_Warn : IActorState{
	public NPCActorState_Warn(IActor actor){
		this.actor = actor;
	}
	
	public override IActorState toNextState (EActorAction action)
	{
		IActorState result = null;
		if(action == EActorAction.NPC_IDLE){
			result = new NPCActorState_Idle(actor);
		}
		else if(action == EActorAction.NPC_WALK){
			result = new NPCActorState_Walk(actor);
		}
		else if(action == EActorAction.NPC_DIE){
			result = new NPCActorState_Die(actor);
		}
		return result;
	}
	
	public override void OnEnter ()
	{
		actor.OnEnterWarn();
	}
	
	public override void DoUpdata ()
	{
		actor.DoUpdataWarn();
	}

}

public class NPCActorState_Die : IActorState{
	public NPCActorState_Die(IActor actor){
		this.actor = actor;
	}
	
	public override IActorState toNextState (EActorAction action)
	{
		IActorState result = null;
		return result;
	}
	
	public override void OnEnter ()
	{
		actor.OnEnterDie();
	}
	
	public override void DoUpdata ()
	{
		actor.DoUpdataDie();
	}
}
	#endregion
