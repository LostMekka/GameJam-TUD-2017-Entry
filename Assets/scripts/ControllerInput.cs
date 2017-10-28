using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class ControllerInput : MonoBehaviour
{
	public Character character = null;
	public Map map = null;
	
	private TileInfo nextTile;
	// Use this for initialization
	void Start () {
//		character = gameO.GetComponent<Character>();		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ( character == null ) return;
		
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		var currentTile = character.OccupiedTile;


		if (vertical * vertical + horizontal * horizontal < 0.2)
		{
			if (nextTile != null)
			{
				nextTile.tileState = TileInfo.TileState.None;
				nextTile.Select();
				nextTile = null;
			}
			return;
		}
		
	//	Debug.Log("X:" + currentTile.X.ToString() + "Y: " + currentTile.Y.ToString() );
		float angle = Mathf.Atan2(vertical, horizontal);
	//	Debug.Log("Angle:" + angle.ToString() );

		int index = (int)Mathf.Round(3 * angle/Mathf.PI);
		Debug.Log("Index:" + index.ToString() );

		if (map == null) return;
		
		TileInfo tile = map.GetTileInDirection(character.OccupiedTile, index);

		if (tile != null)
		{
			if (nextTile != null)
			{
				nextTile.tileState = TileInfo.TileState.None;
				nextTile.Select();
			}
			nextTile = tile;
			nextTile.tileState = TileInfo.TileState.Next;
			nextTile.Select();
		}
	}
}
