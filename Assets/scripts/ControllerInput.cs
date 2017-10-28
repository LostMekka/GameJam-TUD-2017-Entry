using System;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{
	public Character Character;
	public Map Map;

	private TileInfo selectedTile;
	private const float MinAnalogStrngth = 0.1f;


	public void Update()
	{
		if (Character == null || Map == null) throw new Exception("character or map is not initialized");
		Unselect();

		var dx = Input.GetAxis("Horizontal");
		var dy = Input.GetAxis("Vertical");
		if (dy * dy + dx * dx < MinAnalogStrngth * MinAnalogStrngth) return;

		var angle = Mathf.Atan2(dy, dx);
		var direction = (int) Mathf.Round(3 * angle / Mathf.PI);
		var tile = Map.GetTileInDirection(Character.OccupiedTile, direction);
		if (tile != null) Select(tile);
	}

	private void Select(TileInfo tile)
	{
		selectedTile = tile;
		selectedTile.Highlight(Color.green);
	}

	private void Unselect()
	{
		if (selectedTile == null) return;
		selectedTile.ClearHighlight();
		selectedTile = null;
	}
}