using System;
using System.Collections;
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
		public readonly TileInfo source;
		public readonly TileInfo target;
		public readonly int damage;

		public DamageEvent(TileInfo source, TileInfo target, int damage)
		{
			this.source = source;
			this.target = target;
			this.damage = damage;
		}
	}

	public Map Map;


	private readonly List<Character> registeredCharacters = new List<Character>();
	private State state = State.Input;


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
					var charactersHit = CalculateNextGameState();
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
					state = State.Input;
					// TODO: inform input scripts or something like that??
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	/// <returns>A list of characters that were hit during the turn.</returns>
	private IEnumerable<Character> CalculateNextGameState()
	{
		var damageEvents = new List<DamageEvent>();
		foreach (var character in registeredCharacters)
		{
			// TODO: get this from character
			var damageAmount = 1;
			switch (character.CurrentAction.CurrentTurnActionAtom.Type)
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
			var atom = character.CurrentAction.CurrentTurnActionAtom;
			var currTile = character.OccupiedTile;
			var moveTarget = atom.Type == ActionType.Move || atom.Type == ActionType.Roll
				? Map.GetTileInDirection(currTile, character.Direction + atom.DirectionOffset)
				: null;
			if (moveTarget != null) character.MoveToTile(moveTarget);

			var damageTaken = 0;
			foreach (var damageEvent in damageEvents)
			{
				if (damageEvent.target != currTile && damageEvent.target != moveTarget) continue;
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
					if (blockingSources.Contains(damageEvent.source)) continue;
				}
				// character did not block or evade so we deal damage
				damageTaken += damageEvent.damage;
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
		foreach (var character in registeredCharacters) character.StartTurnAnimation();
		state = State.TurnAnimations;
	}
}