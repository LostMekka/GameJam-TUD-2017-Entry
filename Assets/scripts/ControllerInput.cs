using System;
using DefaultNamespace;
using UnityEngine;

public class ControllerInput : MonoBehaviour, IInputScript
{
	public Character Character { get; set; }
	public Map Map;
	public GameController GameController;
	public bool InputActive;

	private TileInfo selectedTile;
	private const float MinAnalogStrngth = 0.1f;


	public void Update()
	{
		if (Character == null || Map == null || GameController == null)
			throw new Exception("controller input not initialized");
		Unselect();
		if (!InputActive) return;

		var dx = Input.GetAxis("Horizontal");
		var dy = Input.GetAxis("Vertical");
		if (dy * dy + dx * dx < MinAnalogStrngth * MinAnalogStrngth) return;

		var angle = Mathf.Atan2(dy, dx);
		var direction = (int) Mathf.Round(3 * angle / Mathf.PI);
		var tile = Map.GetTileInDirection(Character.OccupiedTile, direction);
		if (tile == null) return;

		Select(tile);
		if (Input.GetButtonDown("Fire1"))
		{
			EndInputPhase(new ActionSequence(ActionDefinition.Walk, direction));
		}
		else if (Input.GetButtonDown("Fire2"))
		{
			EndInputPhase(new ActionSequence(ActionDefinition.SimpleAttack, direction));
		}
		else if (Input.GetButtonDown("Fire3"))
		{
			EndInputPhase();
		}
	}

	public void BeginInputPhase() { InputActive = true; }

	public void EndInputPhase(ActionSequence desiredActionSequence = null)
	{
		InputActive = false;
		Character.OnFinishedInput(desiredActionSequence);
	}

	private void Select(TileInfo tile)
	{
		selectedTile = tile;
		selectedTile.Highlight(Color.green);
	}

	private void Unselect()
	{
		if (selectedTile != null) selectedTile.ClearHighlight();
		selectedTile = null;
	}
}