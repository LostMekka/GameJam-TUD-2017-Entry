using System.Collections.Generic;

public class ActionDefinition
{
	public static readonly ActionDefinition Idle = new ActionDefinition(
		"Idle",
		new List<ActionAtom>
		{
			new ActionAtom(ActionType.Idle),
		},
		true
	);

	public static readonly ActionDefinition Walk = new ActionDefinition(
		"Walk",
		new List<ActionAtom>
		{
			new ActionAtom(ActionType.Move),
		}
	);

	public static readonly ActionDefinition Roll = new ActionDefinition(
		"Roll",
		new List<ActionAtom>
		{
			new ActionAtom(ActionType.Roll),
			new ActionAtom(ActionType.Recover),
		}
	);

	public static readonly ActionDefinition SimpleAttack = new ActionDefinition(
		"SimpleAttack",
		new List<ActionAtom>
		{
			new ActionAtom(ActionType.Buildup),
			new ActionAtom(ActionType.AttackSingleTile),
			new ActionAtom(ActionType.Recover),
		}
	);


	public readonly string Name;
	public readonly bool LoopsWithoutInput;
	public readonly List<ActionAtom> Atoms;

	private ActionDefinition(string name, List<ActionAtom> atoms, bool loopsWithoutInput = false)
	{
		Atoms = atoms;
		LoopsWithoutInput = loopsWithoutInput;
		Name = name;
	}
}