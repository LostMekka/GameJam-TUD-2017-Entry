using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	public int GeneratedSize = 10;
	public float TileDistance = 1f;
	public float WallProbability = 0.1f;
	public GameObject FloorTilePrefab;
	public GameObject WallTilePrefab;

	private Dictionary<int, GameObject> tiles = new Dictionary<int, GameObject>();
	private const int MaxMapSize = 9999;

	void Start()
	{
		GenerateNewMap();
	}

	public GameObject this[int x, int y]
	{
		get { return tiles[x + y * MaxMapSize]; }
		set { tiles[x + y * MaxMapSize] = value; }
	}

	public void GenerateNewMap()
	{
		for (int y = 0; y < GeneratedSize; y++)
		{
			for (int x = 0; x < GeneratedSize; x++)
			{
				GameObject prefab = UnityEngine.Random.value < WallProbability
					? WallTilePrefab
					: FloorTilePrefab;
				GameObject tile = Instantiate(prefab);
				tile.transform.position = GetTilePosition(x, y);
				this[x, y] = tile;
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