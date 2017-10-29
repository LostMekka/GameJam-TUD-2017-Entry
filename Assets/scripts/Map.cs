using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
	public int GeneratedSize = 10;
	public int EnemiesToPlace = 10;
	public float TileRadius = 1f;
	public float TileGap;
	public float WallProbability = 0.1f;
	public float TileBaseHeight = -0.2f;
	public GameObject FloorTilePrefab;
	public GameObject WallTilePrefab;
	public GameObject PlayerPrefab;
	public GameObject EnemyPrefab;
	public GameController GameController;
	public Character Player;

	private readonly Dictionary<int, TileInfo> tiles = new Dictionary<int, TileInfo>();
	private readonly float hexInnerRadiusMultiplier = (float) Math.Sqrt(3) / 2f;
	private const int MaxMapSize = 9999;


	void Start()
	{
		GameController = FindObjectOfType<GameController>();

		GenerateNewMap();

		Player = GameController.CreateCharacter(PlayerPrefab, 4, 4);
		var controllerInput = Player.gameObject.AddComponent<ControllerInput>();
		controllerInput.Character = Player;
		controllerInput.Map = this;
		controllerInput.GameController = GameController;
		
		// place a predefined amount of enemies
		var count = EnemiesToPlace;
		var tries = EnemiesToPlace * 10; // make sure we do not loop forever

		while (count > 0 && tries > 0)
		{
			var Random = new Random();
			var posX = (int) (GeneratedSize * Random.value);
			var posY = (int) (GeneratedSize * Random.value);
			
			if (controllerInput.Map[posX, posY].CanWalkTo == true)
			{
				var Enemy = GameController.CreateCharacter(EnemyPrefab, posX, posY);
				--count;
			}

			--tries;
		}
	}

	public TileInfo this[int x, int y]
	{
		get
		{
			TileInfo tile;
			tiles.TryGetValue(x + y * MaxMapSize, out tile);
			return tile;
		}
		set
		{
			tiles[x + y * MaxMapSize] = value;
			CreateTileLinks();
		}
	}

	public void GenerateNewMap()
	{
		for (var y = 0; y < GeneratedSize; y++)
		{
			for (var x = 0; x < GeneratedSize; x++)
			{
				var prefab = Random.value < WallProbability
					? WallTilePrefab
					: FloorTilePrefab;
				CreateTileFromPrefab(prefab, x, y);
				
			}
		}
		CreateTileLinks();
		var midPosition = GetTilePosition(GeneratedSize / 2 - 1, GeneratedSize / 2 - 1);
		transform.position = new Vector3(-midPosition.x, 0, -midPosition.z);
	}

	private void CreateTileLinks()
	{
		foreach (var tileInfo in tiles.Values)
		{
			tileInfo.ClearTileConnections();
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

	private static void Link(TileInfo source, TileInfo target)
	{
		if (source != null && target != null) source.ConnectToTile(target);
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
			default: throw new ArgumentOutOfRangeException("direction", direction, null);
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
		tileInfo.Map = this;
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