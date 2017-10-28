using System;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{
	public Character Character;
	public Map Map;
	public GameController GameController;

	private TileInfo selectedTile;
	private const float MinAnalogStrngth = 0.1f;


	public void Update()
	{
		if (Character == null || Map == null || GameController == null)
			throw new Exception("controller input not initialized");
		Unselect();
		if (!GameController.AllowsInput) return;

		var dx = Input.GetAxis("Horizontal");
		var dy = Input.GetAxis("Vertical");
		if (dy * dy + dx * dx < MinAnalogStrngth * MinAnalogStrngth) return;

		var angle = Mathf.Atan2(dy, dx);
		var direction = (int) Mathf.Round(3 * angle / Mathf.PI);
		var tile = Map.GetTileInDirection(Character.OccupiedTile, direction);
		if (tile == null || !tile.IsWalkable || tile.CharacterStandingThere != null) return;

		Select(tile);
		if (Input.GetButtonDown("Fire1"))
		{
			ConfirmActionSequence(new ActionSequence(ActionDefinition.Walk, direction));
		}
		else if (Input.GetButtonDown("Fire2"))
		{
			ConfirmActionSequence(new ActionSequence(ActionDefinition.SimpleAttack, direction));
		}
	}

	private void ConfirmActionSequence(ActionSequence sequence)
	{
		// TODO: need to check if we can abort the current sequence
		Character.CurrentActionSequence = sequence;
		GameController.ExecuteNextTurn();
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