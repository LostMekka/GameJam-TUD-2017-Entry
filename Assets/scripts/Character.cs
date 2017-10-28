using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public int Health;
	public int MaxHealth = 100;
	public int Stamina;
	public int MaxStamina = 100;

	public Action CurrentAction;
	public int Direction;
	public TileInfo OccupiedTile;
	public bool InAnimation;

	public float ModelScale = 1;
	public GameObject ModelPrefab;


	public string CurrentActionName { get { return CurrentAction.Name; } }
	public bool IsDead { get { return Health <= 0; } }


	private GameObject model;
	private Animator animator;


	// Use this for initialization
	void Start()
	{
		model = Instantiate(ModelPrefab);
		model.transform.localScale = new Vector3(ModelScale, ModelScale, ModelScale);
		model.transform.parent = gameObject.transform;
		model.transform.localPosition = Vector3.zero;

		Health = MaxHealth;
		Stamina = MaxStamina;

		animator = GetComponentInChildren<Animator>();
	}

	void Update() { InAnimation = InAnimation && !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"); }

	public void StartTurnAnimation()
	{
		animator.SetTrigger(GetAnimationNameForActionType(CurrentAction.CurrentTurnActionAtom.Type));
		InAnimation = true;
	}

	public void StartHitAnimation()
	{
		// TODO: trigger hit animation instead of idle
		animator.SetTrigger("unitIdle");
		InAnimation = true;
	}

	public void GoToNextActionAtom()
	{
		CurrentAction.Tick();
		if (CurrentAction.IsDone) CurrentAction = new Action(ActionDefinition.Idle);
	}

	public void DealDamage(int amount) { Health -= amount; }
	public void Heal(int amount) { Health += amount; }

	public void MoveToTile(TileInfo tile)
	{
		OccupiedTile.CharacterStandingThere = null;
		tile.CharacterStandingThere = this;
		OccupiedTile = tile;
	}

	private string GetAnimationNameForActionType(ActionType type)
	{
		switch (type)
		{
			case ActionType.Idle: return "unitIdle";
			case ActionType.Move: return "unitWalk";
			case ActionType.Roll: return "unitWalk";
			case ActionType.Block: return "unitIdle";
			case ActionType.Buildup: return "unitVictory";
			case ActionType.Recover: return "unitVictory";
			case ActionType.AttackSingleTile: return "unitAttack01";
			case ActionType.Attack3Tiles: return "unitAttack01";
			case ActionType.Attack5Tiles: return "unitAttack02";
			case ActionType.AttackAllTiles: return "unitAttack02";
			case ActionType.Rotate: return "unitIdle";
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
		}
	}
}