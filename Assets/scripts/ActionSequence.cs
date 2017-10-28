using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequence
{
	public readonly ActionDefinition Definition;
	public readonly int? DirectionOverride;
	private int currentTurnIndex;


	public int CurrentTurnIndex { get { return IsDone ? -1 : currentTurnIndex; } }

	public ActionAtom CurrentTurnActionAtom { get { return IsDone ? null : Definition.Atoms[currentTurnIndex]; } }

	public bool IsDone { get { return currentTurnIndex >= Definition.Atoms.Count; } }

	public string Name { get { return Definition.Name; } }

	public bool CanLoop { get { return Definition.LoopsWithoutInput; } }


	public ActionSequence(ActionDefinition definition, int? directionOverride = null)
	{
		Definition = definition;
		DirectionOverride = directionOverride;
	}

	public void Tick()
	{
		currentTurnIndex++;
		if (IsDone && CanLoop) currentTurnIndex = 0;
	}
}