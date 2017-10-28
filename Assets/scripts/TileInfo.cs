using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.WSA;

public class TileInfo : MonoBehaviour
{
	public bool IsWalkable = true;
	public int X;
	public int Y;

	public enum TileState
	{
		None, Next
	}

	public TileState tileState = TileState.None;
	// TODO: reference main scripts instead of game objects here
	public Character CharacterStandingThere;
	public GameObject PickupLyingThere;
	private Color defaultCol;

	public void Start()
	{
		var renderer = GetComponentInChildren<MeshRenderer>();
		defaultCol = renderer.material.color;
	}
	public void Select()
	{
		var renderer = GetComponentInChildren<MeshRenderer>();
		if (tileState == TileState.Next) renderer.material.color = Color.green;
		else renderer.material.color = defaultCol;
	}
	public List<TileInfo> Neighbours
	{
		get { return neighbours; }
	}

	private readonly List<TileInfo> neighbours = new List<TileInfo>();

	public void ConnectToTile(TileInfo tile)
	{
		if (!neighbours.Contains(tile)) neighbours.Add(tile);
	}

	public void DisconnectTile(TileInfo tile)
	{
		neighbours.Remove(tile);
	}
}