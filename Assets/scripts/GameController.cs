using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public Map Map;

	private readonly List<Character> registeredCharacters = new List<Character>();

	public void RegisterCharacter(Character character)
	{
		if (!registeredCharacters.Contains(character)) registeredCharacters.Add(character);
	}

	public void ExecuteNextTurn()
	{
		foreach (var character in registeredCharacters)
		{
			var actionAtom = character.ExecuteTurn();
			// TODO: collect all actions, execute them, update state, trigger animation phase
		}
	}
}