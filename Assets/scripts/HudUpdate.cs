using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UI;

public class HudUpdate : MonoBehaviour
{
	private ActionSequence LastSequence;
	private float imgPos = 0.0F;
	void Update()
	{
		var character = GetComponentInParent<Character>();
		if (character != null)
		{
			var healthValue = character.Health / (float) character.MaxHealth;
			var staminaValue = character.Stamina / (float) character.MaxStamina;
			var sliders = GetComponentsInChildren<Slider>();
			foreach (var slider in sliders)
			{
				if (slider.gameObject.name == "Health") slider.value = healthValue;
				if (slider.gameObject.name == "Stamina") slider.value = staminaValue;
			}
			if (character.CurrentActionSequence == null)
			{
				if (LastSequence != null)
				{
					var stuff = GameObject.FindGameObjectsWithTag("action_tag");
					foreach (var s in stuff)
					{
						Debug.Log("destroy");
						Destroy(s);
					}
				}
			}

			if (character.CurrentActionSequence == LastSequence) return;
			if (character.CurrentActionSequence != null)
			{
				foreach (var Action in character.CurrentActionSequence.Definition.Atoms )
				{
					if (Action.Type == ActionType.Attack3Tiles ||
					    Action.Type == ActionType.AttackSingleTile ||
					    Action.Type == ActionType.AttackAllTiles ||
					    Action.Type == ActionType.Attack5Tiles)
					{
						createIcon(character);
					}
					else if( Action.Type == ActionType.Move ) createIcon(character);
					else if( Action.Type == ActionType.Rotate ) createIcon(character);
					else if( Action.Type == ActionType.Recover ) createIcon(character);
					else if( Action.Type == ActionType.Block ) createIcon(character);
				}		
			}
			LastSequence = character.CurrentActionSequence;
		}

	}

	void Start()
	{
//		for ( int i = 0; i < 3; ++i)
//		{
//			GameObject temp = new GameObject();
//			temp.transform.parent = gameObject.transform;
//			var image = temp.AddComponent<Image>();
//			image.name = "action";
//			image.sprite = Resources.Load<Sprite>("Test");
//			image.transform.localScale = new Vector3(0.004F, 0.004F, 0.004F);
//			image.transform.position = new Vector3(0.0F + imgPos, 2.5F, 0.004F);
//			imgPos += 1.5F;
//		}
	}

	void createIcon(Character character)
	{
		GameObject temp = new GameObject();
		temp.name = "action";
		temp.tag = "action_tag";
		temp.transform.parent = gameObject.transform;
		var image = temp.AddComponent<Image>();
		image.name = "action";
		image.sprite = Resources.Load<Sprite>("Test");
		image.transform.localScale = new Vector3(0.04F, 0.04F, 0.04F);
		image.transform.position = new Vector3(0.0F + imgPos, character.transform.position.y,character.transform.position.z);
		imgPos += 0.3F;
		
	}
}