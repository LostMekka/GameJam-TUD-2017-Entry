using System;
using UnityEngine;

public class Character : MonoBehaviour
{
	public int Health;
	public int MaxHealth = 100;
	public int Stamina;
	public int MaxStamina = 100;
	public float moveTime = 0.1f;
	private float inverseMoveTime;

	public ActionSequence CurrentActionSequence;
	public TileInfo OccupiedTile;
	public bool InAnimation;

	public float ModelScale = 1;
	public GameObject ModelPrefab;
	public GameObject OutermostGameObject;
	private int direction;

	private Transform unitTransform;

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
		Debug.Log(animator);


		//By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
		inverseMoveTime = 1f / moveTime;
	}

	void Update()
	{
		InAnimation = InAnimation && !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
	}

	//start movement and animation
	public void StartTurnAnimation(TileInfo EndTile)
	{
		// TODO STEVE: fix animations
		MoveToPosition(EndTile);
		InAnimation = true;
	}

	//just plays animation
	public void StartHitAnimation()
	{
		// TODO: trigger hit animation instead-> !after! switch to idle
		animator.SetTrigger(GetAnimationNameForActionType(CurrentActionSequence.CurrentTurnActionAtom.Type));
		InAnimation = true;
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

	private void MoveToPosition(TileInfo EndTile)
	{
		//Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
		//Square magnitude is used instead of magnitude because it's computationally cheaper.
		float sqrRemainingDistance = (EndTile.transform.position - EndTile.GlobalMidpointPosition).sqrMagnitude;

		//--start continuous animation
		animator.SetTrigger(GetAnimationNameForActionType(CurrentActionSequence.CurrentTurnActionAtom.Type));

		//While that distance is greater than a very small amount (Epsilon, almost zero):
		while (sqrRemainingDistance > float.Epsilon)
		{
			//Find a new position proportionally closer to the end, based on the moveTime
			Vector3 newPostion = Vector3.MoveTowards(unitTransform.position, EndTile.GlobalMidpointPosition, inverseMoveTime * Time.deltaTime);

			//Call MovePosition on attached Rigidbody2D and move it to the calculated position.
			unitTransform.Translate(newPostion);

			//Recalculate the remaining distance after moving.
			sqrRemainingDistance = (transform.position - EndTile.GlobalMidpointPosition).sqrMagnitude;

			Debug.Log("Moving");
		}

		//--end Animation
		animator.SetTrigger("unitIdle");

		Debug.Log("EndOfMovement");

		return;
	}
}
