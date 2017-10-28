﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
	public bool isWalkable = true;

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