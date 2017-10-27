using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	public int GeneratedSize = 10;
	public float TileDistance = 1f;
	public GameObject TilePrefab;

	// Use this for initialization
	void Start()
	{
		GenerateNewMap();
	}

	// Update is called once per frame
	void Update()
	{
	}

	public void GenerateNewMap()
	{
		for (int y = 0; y < GeneratedSize; y++)
		{
			for (int x = 0; x < GeneratedSize; x++)
			{
				GameObject tile = Instantiate(TilePrefab);
				tile.transform.position = GetTilePosition(x, y);
			}
		}
	}

	private Vector3 GetTilePosition(int x, int y)
	{
		return new Vector3(
			x * TileDistance + (y % 2) * TileDistance / 2f,
			0,
			(float) Math.Sqrt(3) / 2f * TileDistance * y
		);
	}
}