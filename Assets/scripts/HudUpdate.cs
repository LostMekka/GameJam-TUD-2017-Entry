using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudUpdate : MonoBehaviour
{
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
		}
	}
}