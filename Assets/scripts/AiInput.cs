using UnityEngine;

public class AiInput : MonoBehaviour, IInputScript
{
	public Character Character { get; set; }


	public void OnRequestInput()
	{
		// TODO: behaviour
		Character.OnFinishedInput(new ActionSequence(ActionDefinition.Walk, (int)(Random.value * 6)));
	}

}