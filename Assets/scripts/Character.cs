﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public Action CurrentAction;
	public int Health = 100;
	public int Stamina = 100;
	public TileInfo OccupiedTile;
	public int Size = 1;

	public readonly Dictionary<string, string> skillList = new Dictionary<string, string>();

	// TODO: change type to animation reference
	public object AnimationToPlay;


	public string CurrentActionName { get { return CurrentAction.Name; } }
	public bool IsDead { get { return Health <= 0; } }


	// Use this for initialization
	void Start()
	{
		// Add some elements to the dictionary.
		// TODO STEVE: initialize skill to animation mapping
		skillList.Add("txt", "notepad.exe");
		skillList.Add("bmp", "paint.exe");
		skillList.Add("dib", "paint.exe");
		skillList.Add("rtf", "wordpad.exe");
	}

	// Update is called once per frame
	void Update() { }

	public ActionAtom ExecuteTurn()
	{
		var currAtom = CurrentAction.CurrentTurnActionAtom;
		CurrentAction.ExecuteTurn();
		if (CurrentAction.IsDone) CurrentAction = new Action(ActionDefinition.Idle);
		return currAtom;
	}

	public void DealDamage(int amount) { Health -= amount; }
	public void Heal(int amount) { Health += amount; }

	public void MoveToTile(TileInfo tile)
	{
		OccupiedTile.CharacterStandingThere = null;
		tile.CharacterStandingThere = this;
		OccupiedTile = tile;
	}
}