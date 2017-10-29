using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
	public enum InputType
	{
		None,
		Human,
		Computer,
	}


	public delegate void OnInputRequired();

	public int Health;
	public int MaxHealth = 100;
	public int Stamina;
	public int MaxStamina = 100;
	public float MoveTime = 10f;

	public ActionSequence CurrentActionSequence;
	public TileInfo OccupiedTile;
	public bool IsWaitingForAnimation { get; private set; }
	public bool IsWaitingForInput { get; private set; }
	public OnInputRequired OnInputRequiredCallback;

	public float ModelScale = 1;
	public GameObject ModelPrefab;
	public GameObject OutermostGameObject;

	private int direction;
	private GameObject model;
	private Animator animator;

    public GameObject ParticleEffect;

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

	public TileInfo MovementTarget
	{
		get
		{
			var atom = CurrentActionSequence.CurrentTurnActionAtom;
			return atom.Type == ActionType.Move || atom.Type == ActionType.Evade
				? OccupiedTile.GetTileInDirection(Direction + atom.DirectionOffset)
				: null;
		}
	}


	// Use this for initialization
	public void Start()
	{
		model = Instantiate(ModelPrefab);
		model.transform.localScale = new Vector3(ModelScale, ModelScale, ModelScale);
		model.transform.parent = gameObject.transform;
		model.transform.localPosition = Vector3.zero;

		Health = MaxHealth;
		Stamina = MaxStamina;

		animator = GetComponentInChildren<Animator>();
	}

	public void UpdateDirectionBasedOnActionSequence()
	{
		if (CurrentActionSequence.DirectionOverride != null && CurrentActionSequence.CurrentTurnIndex == 0)
		{
			Direction = CurrentActionSequence.DirectionOverride.Value;
		}
	}

	public void StartTurnAnimation() { StartCoroutine(TurnAnimationCoroutine(MoveTime)); }

	private string GetAnimationNameForActionType(ActionType type)
	{
		switch (type)
		{
			case ActionType.Idle: return "unitIdle";
			case ActionType.Move: return "unitWalk";
			case ActionType.Evade: return "unitWalk";
			case ActionType.Block: return "unitIdle";
			case ActionType.Buildup: return "unitIdle";
			case ActionType.Recover: return "unitIdle";
			case ActionType.AttackSingleTile: return "unitAttack01";
			case ActionType.Attack3Tiles: return "unitAttack01";
			case ActionType.Attack5Tiles: return "unitAttack02";
			case ActionType.AttackAllTiles: return "unitAttack02";
			case ActionType.Rotate: return "unitIdle";
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
		}
	}

	private IEnumerator TurnAnimationCoroutine(float seconds)
	{
		IsWaitingForAnimation = true;
		animator.SetTrigger(GetAnimationNameForActionType(CurrentActionSequence.CurrentTurnActionAtom.Type));

		float elapsedTime = 0;
		var source = OccupiedTile;
		var target = MovementTarget ?? OccupiedTile;
		if (!target.CanWalkTo) target = OccupiedTile;

		while (elapsedTime < seconds)
		{
			OutermostGameObject.transform.position = Vector3.Lerp(
				source.GlobalMidpointPosition,
				target.GlobalMidpointPosition,
				elapsedTime / seconds
			);
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		OutermostGameObject.transform.position = target.GlobalMidpointPosition;
		animator.SetTrigger("unitIdle");
		IsWaitingForAnimation = false;
	}

	public void StartHitAnimation() { StartCoroutine(HitAnimationCoroutine(MoveTime)); }

	private IEnumerator HitAnimationCoroutine(float seconds)
	{
		IsWaitingForAnimation = true;
        animator.SetTrigger("unitHit");


        GameObject newHitEffect = Instantiate(ParticleEffect);
        Destroy(newHitEffect,2.5f);

		float elapsedTime = 0;
		while (elapsedTime < seconds)
		{
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		animator.SetTrigger("unitIdle");
		IsWaitingForAnimation = false;
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

	public void RequestInput()
	{
		if (OnInputRequiredCallback != null)
		{
			OnInputRequiredCallback();
			IsWaitingForInput = true;
		}
		else
		{
			IsWaitingForInput = false;
		}
	}

	public void OnFinishedInput(ActionSequence requestedActionSequence = null)
	{
		// TODO: do not change sequence if it is not abortable
		if (requestedActionSequence != null) CurrentActionSequence = requestedActionSequence;
		IsWaitingForInput = false;
	}
}