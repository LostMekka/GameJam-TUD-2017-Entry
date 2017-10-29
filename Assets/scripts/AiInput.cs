using UnityEngine;

public class AiInput : MonoBehaviour, IInputScript
{
	public Character Character { get { return Test; } set { Test = value; } }
	public Character Test;


	public void OnRequestInput()
	{
		// TODO: behaviour
		Debug.Log("asked AI");
		Character.OnFinishedInput(new ActionSequence(ActionDefinition.Walk, (int)(Random.value * 6)));
	}

}