public class ActionAtom
{
	public readonly ActionType Type;
	public readonly int DirectionOffset;

	public ActionAtom(ActionType type, int directionOffset = 0)
	{
		Type = type;
		DirectionOffset = directionOffset;
	}
}