using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System;

public enum ECommandAI{
	COMMAND_NONE,
	COMMAND_TARGET_PLAYER,
	COMMAND_IDLE,
	COMMAND_ATTACK_PLAYER
}

public enum EActorState{
	STATE_MOVE,
	STATE_IDLE,
	STATE_ATTACK,
    STATE_ATTACKINTERVAL
}

public enum EActorAction {
    NONE,
	HERO_IDLE,
	HERO_RUN,
	HERO_ONAIR_DOWN,
	HERO_ONAIR_UP,
	HERO_SHIN,
	HERO_CATCHPOINT,
	HERO_ATTACK,
	NPC_IDLE,
	NPC_WALK,
	NPC_WARN,
	NPC_DIE
}

public enum ERuneStoneType{
	NONE,
	JUMP
}

public enum EActorType{
	Gard,
	Hero
}

public enum EAIAction{
	MoveBy,
	Stop,
	Turn
}