using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
	public bool IsWalkable = true;
	public int X;
	public int Y;
	public Character CharacterStandingThere;

	private Color defaultColor;
	private MeshRenderer meshRenderer;
	private readonly List<TileInfo> neighbours = new List<TileInfo>();


	public bool Highlighted { get { return meshRenderer.material.color == defaultColor; } }
	public bool CanWalkTo { get { return IsWalkable && CharacterStandingThere == null; } }


	public List<TileInfo> Neighbours { get { return neighbours; } }

	public Vector3 GlobalMidpointPosition
	{
		get
		{
			var pos = transform.position;
			pos.y = 0;
			return pos;
		}
	}


	public void Start()
	{
		meshRenderer = GetComponentInChildren<MeshRenderer>();
		defaultColor = meshRenderer.material.color;
	}

	public void Highlight(Color color) { meshRenderer.material.color = color; }

	public void ClearHighlight() { meshRenderer.material.color = defaultColor; }

	public void ClearTileConnections() { neighbours.Clear(); }

	public void ConnectToTile(TileInfo tile)
	{
		if (!neighbours.Contains(tile)) neighbours.Add(tile);
	}

	public void DisconnectTile(TileInfo tile) { neighbours.Remove(tile); }
}