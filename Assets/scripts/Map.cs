using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	public int GeneratedSize = 10;
	public float TileDistance = 1f;
	public float WallProbability = 0.1f;
	public float TileBaseHeight = -0.2f;
	public GameObject FloorTilePrefab;
	public GameObject WallTilePrefab;

	private readonly Dictionary<int, GameObject> tiles = new Dictionary<int, GameObject>();
	private const int MaxMapSize = 9999;

	void Start()
	{
		GenerateNewMap();
	}

	public GameObject this[int x, int y]
	{
		get
		{
			GameObject tile;
			tiles.TryGetValue(x + y * MaxMapSize, out tile);
			return tile;
		}
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
				var tileInfo = tile.GetComponent<TileInfo>();
				tileInfo.x = x;
				tileInfo.y = y;
			}
		}
		CreateTileLinks();
	}

	public void CreateTileLinks()
	{
		foreach (GameObject tile in tiles.Values)
		{
			var tileInfo = tile.GetComponent<TileInfo>();
			var x = tileInfo.x;
			var y = tileInfo.y;
			var o = tileInfo.y % 2 * 2 - 1;
			Link(tileInfo, this[x + 1, y + 0]);
			Link(tileInfo, this[x - 1, y + 0]);
			Link(tileInfo, this[x + 0, y + 1]);
			Link(tileInfo, this[x + 0, y - 1]);
			Link(tileInfo, this[x + o, y + 1]);
			Link(tileInfo, this[x + o, y - 1]);
		}
	}

	private static void Link(TileInfo source, GameObject target)
	{
		if (source == null || target == null) return;
		var targetInfo = target.GetComponent<TileInfo>();
		if (targetInfo != null) source.Neighbours.Add(targetInfo);
	}

	private Vector3 GetTilePosition(int x, int y)
	{
		return new Vector3(
			x * TileDistance + y % 2 * TileDistance / 2f,
			TileBaseHeight,
			(float) Math.Sqrt(3) / 2f * TileDistance * y
		);
	}
}