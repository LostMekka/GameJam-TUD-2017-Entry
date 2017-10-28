using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
	private enum State
	{
		Input,
		TurnAnimations,
		HitAnimations,
	}

	private class DamageEvent
	{
		public readonly TileInfo Source;
		public readonly TileInfo Target;
		public readonly int Damage;

		public DamageEvent(TileInfo source, TileInfo target, int damage)
		{
			Source = source;
			Target = target;
			Damage = damage;
		}
	}

	public Map Map;


	private readonly List<Character> registeredCharacters = new List<Character>();
	private State state = State.Input;


	public bool AllowsInput { get { return state == State.Input; } }
	private bool EveryCharacterIsIdle { get { return registeredCharacters.All(c => !c.InAnimation); } }


	private void Update()
	{
		switch (state)
		{
			case State.Input:
				// nothing to do
				break;
			case State.TurnAnimations:
				if (EveryCharacterIsIdle)
				{
					Debug.Log("game controller starts state calculation");
					var charactersHit = CalculateNextGameState();
					Debug.Log("game controller starts hit animations");
					foreach (var character in charactersHit)
					{
						character.GoToNextActionAtom();
						character.StartHitAnimation();
					}
					state = State.HitAnimations;
				}
				break;
			case State.HitAnimations:
				if (EveryCharacterIsIdle)
				{
					Debug.Log("game controller starts input phase");
					state = State.Input;
					// TODO: inform input scripts or something like that??
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	public Character CreateCharacter(GameObject prefab, int xPos, int yPos)
	{
		var instance = Instantiate(prefab);
		var character = instance.GetComponentInChildren<Character>();
		if (character == null)
		{
			Destroy(instance);
			throw new ArgumentException("given prefab has no character script component");
		}

		var targetTile = Map[xPos, yPos];
		if (targetTile == null || !targetTile.IsWalkable || targetTile.CharacterStandingThere != null)
		{
			Destroy(instance);
			throw new ArgumentException("given coordinates do not point to a valid tile");
		}

		targetTile.CharacterStandingThere = character;
		character.OccupiedTile = targetTile;
		character.OutermostGameObject = instance;
		instance.transform.SetParent(Map.transform);
		instance.transform.position = targetTile.GlobalMidpointPosition;
		registeredCharacters.Add(character);
		return character;
	}

	/// <returns>A list of characters that were hit during the turn.</returns>
	private IEnumerable<Character> CalculateNextGameState()
	{
		var damageEvents = new List<DamageEvent>();
		foreach (var character in registeredCharacters)
		{
			// TODO: get this from character
			var damageAmount = 1;
			switch (character.CurrentActionSequence.CurrentTurnActionAtom.Type)
			{
				case ActionType.AttackSingleTile:
					AddDamageEvent(character, new[] {0}, damageAmount, damageEvents);
					break;
				case ActionType.Attack3Tiles:
					AddDamageEvent(character, new[] {-1, 0, 1}, damageAmount, damageEvents);
					break;
				case ActionType.Attack5Tiles:
					AddDamageEvent(character, new[] {-2, -1, 0, 1, 2}, damageAmount, damageEvents);
					break;
				case ActionType.AttackAllTiles:
					AddDamageEvent(character, new[] {0, 1, 2, 3, 4, 5}, damageAmount, damageEvents);
					break;
			}
		}

		var damagedCharacters = new List<Character>();
		foreach (var character in registeredCharacters)
		{
			var atom = character.CurrentActionSequence.CurrentTurnActionAtom;
			var currTile = character.OccupiedTile;
			var moveTarget = atom.Type == ActionType.Move || atom.Type == ActionType.Roll
				? Map.GetTileInDirection(currTile, character.Direction + atom.DirectionOffset)
				: null;
			Debug.Log("name: " + character.CurrentActionSequenceName);
			Debug.Log(moveTarget == null ? "target: null" : "target: " + moveTarget.X + ", " + moveTarget.Y);
			if (moveTarget != null) character.MoveToTile(moveTarget);

			var damageTaken = 0;
			foreach (var damageEvent in damageEvents)
			{
				if (damageEvent.Target != currTile && damageEvent.Target != moveTarget) continue;
				// evade: rolling characters are invulvnerable
				if (atom.Type == ActionType.Roll) continue;
				// block: blocking characters ignore damage from the 3 tiles in front of them
				if (atom.Type == ActionType.Block)
				{
					var blockingSources = new[]
					{
						Map.GetTileInDirection(currTile, character.Direction - 1),
						Map.GetTileInDirection(currTile, character.Direction),
						Map.GetTileInDirection(currTile, character.Direction + 1),
					};
					if (blockingSources.Contains(damageEvent.Source)) continue;
				}
				// character did not block or evade so we deal damage
				damageTaken += damageEvent.Damage;
			}
			if (damageTaken > 0)
			{
				damagedCharacters.Add(character);
				character.DealDamage(damageTaken);
			}
		}

		return damagedCharacters;
	}

	private void AddDamageEvent(
		Character source, int[] globalDirections, int damageAmount,
		List<DamageEvent> targetList)
	{
		var startTile = source.OccupiedTile;
		foreach (var globalDirection in globalDirections)
		{
			var endTile = Map.GetTileInDirection(startTile, globalDirection);
			if (endTile != null) targetList.Add(new DamageEvent(startTile, endTile, damageAmount));
		}
	}

	public void RegisterCharacter(Character character)
	{
		if (!registeredCharacters.Contains(character)) registeredCharacters.Add(character);
	}

	public void ExecuteNextTurn()
	{
		Debug.Log("game controller starts turn animations");
		foreach (var character in registeredCharacters)
		{
			var actionSequence = character.CurrentActionSequence;
			if (actionSequence.DirectionOverride != null && actionSequence.CurrentTurnIndex == 0)
			{
				character.Direction = actionSequence.DirectionOverride.Value;
			}
			character.StartTurnAnimation();
		}
		state = State.TurnAnimations;
	}
}