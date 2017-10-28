using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
	public readonly ActionDefinition Definition;

	public int CurrentTurnIndex { get { return IsDone ? -1 : currentTurnIndex; } }

	public ActionAtom CurrentTurnActionAtom { get { return IsDone ? null : Definition.Atoms[currentTurnIndex]; } }

	public bool IsDone { get { return currentTurnIndex >= Definition.Atoms.Count; } }

	public string Name { get { return Definition.Name; } }

	public bool CanLoop { get { return Definition.LoopsWithoutInput; } }

	private int currentTurnIndex;

	public Action(ActionDefinition definition) { Definition = definition; }

	public void Tick()
	{
		currentTurnIndex++;
		if (IsDone && CanLoop) currentTurnIndex = 0;
	}
}