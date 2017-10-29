namespace DefaultNamespace
{
	public interface IInputScript
	{
		Character Character { get; set; }
		void BeginInputPhase();
		void EndInputPhase(ActionSequence desiredActionSequence);
	}
}