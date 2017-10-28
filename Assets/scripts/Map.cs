using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	public int GeneratedSize = 10;
	public float TileRadius = 1f;
	public float TileGap = 0f;
	public float WallProbability = 0.1f;
	public float TileBaseHeight = -0.2f;
	public GameObject FloorTilePrefab;
	public GameObject WallTilePrefab;

	private readonly Dictionary<int, TileInfo> tiles = new Dictionary<int, TileInfo>();
	private readonly float hexInnerRadiusMultiplier = (float) Math.Sqrt(3) / 2f;
	private const int MaxMapSize = 9999;

	void Start() { GenerateNewMap(); }

	public TileInfo this[int x, int y]
	{
		get
		{
			TileInfo tile;
			tiles.TryGetValue(x + y * MaxMapSize, out tile);
			return tile;
		}
		set { tiles[x + y * MaxMapSize] = value; }
	}

	public void GenerateNewMap()
	{
		for (var y = 0; y < GeneratedSize; y++)
		{
			for (var x = 0; x < GeneratedSize; x++)
			{
				var prefab = UnityEngine.Random.value < WallProbability
					? WallTilePrefab
					: FloorTilePrefab;
				CreateTileFromPrefab(prefab, x, y);
			}
		}
		CreateTileLinks();
	}

	public void CreateTileLinks()
	{
		foreach (var tileInfo in tiles.Values)
		{
			var x = tileInfo.X;
			var y = tileInfo.Y;
			var o = tileInfo.Y % 2 * 2 - 1;
			Link(tileInfo, this[x + 1, y + 0]);
			Link(tileInfo, this[x - 1, y + 0]);
			Link(tileInfo, this[x + 0, y + 1]);
			Link(tileInfo, this[x + 0, y - 1]);
			Link(tileInfo, this[x + o, y + 1]);
			Link(tileInfo, this[x + o, y - 1]);
		}
	}

	public TileInfo GetTileInDirection(TileInfo startTile, int direction)
	{
		var x = startTile.X;
		var y = startTile.Y;
		switch ((direction % 6 + 6) % 6)
		{
			case 0: return this[x + 1, y];
			case 1: return this[x + y % 2, y + 1];
			case 2: return this[x - 1 + y % 2, y + 1];
			case 3: return this[x - 1, y];
			case 4: return this[x - 1 + y % 2, y - 1];
			case 5: return this[x + y % 2, y - 1];
			default: throw new ArgumentOutOfRangeException();
		}
	}

	private void CreateTileFromPrefab(GameObject prefab, int x, int y)
	{
		var tile = Instantiate(prefab);
		tile.transform.parent = transform;
		tile.transform.localPosition = GetTilePosition(x, y);
		var tileInfo = tile.GetComponent<TileInfo>();
		this[x, y] = tileInfo;
		tileInfo.X = x;
		tileInfo.Y = y;
	}

	private static void Link(TileInfo source, TileInfo target)
	{
		if (source != null && target != null) source.Neighbours.Add(target);
	}

	private Vector3 GetTilePosition(int x, int y)
	{
		var tileWidth = hexInnerRadiusMultiplier * TileRadius * 2 + TileGap;
		return new Vector3(
			x * tileWidth + y % 2 * tileWidth / 2f,
			TileBaseHeight,
			hexInnerRadiusMultiplier * tileWidth * y
		);
	}
}