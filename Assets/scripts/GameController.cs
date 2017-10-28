using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	private enum State
	{
		Input,
		TurnAnimations,
		HitAnimations,
	}


	public Map Map;

	private readonly List<Character> registeredCharacters = new List<Character>();
	private State state = State.Input;


	private void Update()
	{
		switch (state)
		{
			case State.TurnAnimations:
				break;
			case State.HitAnimations:
				break;
		}
	}

	public void RegisterCharacter(Character character)
	{
		if (!registeredCharacters.Contains(character)) registeredCharacters.Add(character);
	}

	public void ExecuteNextTurn()
	{
		foreach (var character in registeredCharacters)
		{
			character.StartTurnAnimation();
			// TODO: collect all actions, execute them, update state, trigger animation phase
		}
	}
}