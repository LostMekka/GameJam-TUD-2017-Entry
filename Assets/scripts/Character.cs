using System;
using UnityEngine;

public class Character : MonoBehaviour
{
	public int Health;
	public int MaxHealth = 100;
	public int Stamina;
	public int MaxStamina = 100;

	public ActionSequence CurrentActionSequence;
	public TileInfo OccupiedTile;
	public bool InAnimation;

	public float ModelScale = 1;
	public GameObject ModelPrefab;
	public GameObject OutermostGameObject;
	private int direction;


	public int Direction
	{
		get { return direction; }
		set
		{
			direction = (value % 6 + 6) % 6;
			model.transform.eulerAngles = new Vector3(0, (90 + 360 - direction * 60) % 360, 0);
		}
	}

	public string CurrentActionSequenceName { get { return CurrentActionSequence.Name; } }
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
		// TODO STEVE: fix animations
//		animator.SetTrigger(GetAnimationNameForActionType(CurrentActionSequence.CurrentTurnActionAtom.Type));
//		InAnimation = true;
	}

	public void StartHitAnimation()
	{
		// TODO: trigger hit animation instead of idle
//		animator.SetTrigger("unitIdle");
//		InAnimation = true;
	}

	public void GoToNextActionAtom()
	{
		CurrentActionSequence.Tick();
		if (CurrentActionSequence.IsDone) CurrentActionSequence = new ActionSequence(ActionDefinition.Idle);
	}

	public void DealDamage(int amount) { Health -= amount; }
	public void Heal(int amount) { Health += amount; }

	public bool MoveToTile(TileInfo tile)
	{
		if (tile.CharacterStandingThere != null || !tile.IsWalkable) return false;
		OccupiedTile.CharacterStandingThere = null;
		tile.CharacterStandingThere = this;
		OccupiedTile = tile;
		OutermostGameObject.transform.position = tile.GlobalMidpointPosition;
		return true;
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