using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public Action CurrentAction;
	public int Health;
	public int MaxHealth = 100;
	public int Stamina;
	public int MaxStamina = 100;
	public int Direction;
	public TileInfo OccupiedTile;
	public float ModelScale = 1;
	public GameObject ModelPrefab;

	public readonly Dictionary<string, string> skillList = new Dictionary<string, string>();

	// TODO: change type to animation reference
	public object AnimationToPlay;

	private GameObject model;

	public string CurrentActionName { get { return CurrentAction.Name; } }
	public bool IsDead { get { return Health <= 0; } }

    //is true if Animation is playing
    public bool inAnimation = false;

    private Animator animator;


    // Use this for initialization
    void Start()
	{
		model = Instantiate(ModelPrefab);
		model.transform.localScale = new Vector3(ModelScale, ModelScale, ModelScale);
		model.transform.parent = gameObject.transform;
		model.transform.localPosition = Vector3.zero;

		Health = MaxHealth;
		Stamina = MaxStamina;

        //gets Animator component if it is on the same GameObject
        //if it is somewhere else this needs to be changed:
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() { }

	public void StartTurnAnimation()
	{
		// TODO STEVE: start animation corresponding to the currernt action atom
	}

	public void StartHitAnimation()
	{
		// TODO STEVE: start hit animation
	}
	
	public void GoToNextActionAtom()
	{
		var currAtom = CurrentAction.CurrentTurnActionAtom;
		CurrentAction.Tick();
		if (CurrentAction.IsDone) CurrentAction = new Action(ActionDefinition.Idle);
	}

	public void DealDamage(int amount) { Health -= amount; }
	public void Heal(int amount) { Health += amount; }

	public void MoveToTile(TileInfo tile)
	{
		OccupiedTile.CharacterStandingThere = null;
		tile.CharacterStandingThere = this;
		OccupiedTile = tile;
	}

    //returns true if animation is done
    private void StartAnimation() {
        if (CurrentAction.CurrentTurnActionAtom.Type == ActionType.Idle)
        {
            animator.SetTrigger("unitIdle");
        }
        else if (CurrentAction.CurrentTurnActionAtom.Type == ActionType.Move)
        {
            animator.SetTrigger("unitIdle");
        }
        else if (CurrentAction.CurrentTurnActionAtom.Type == ActionType.Roll)
            //doubled animations
        {
            animator.SetTrigger("unitIdle");

        }
        else if (CurrentAction.CurrentTurnActionAtom.Type == ActionType.Block)
            //doubled animations
        {
            animator.SetTrigger("unitIdle");

        }
        else if (CurrentAction.CurrentTurnActionAtom.Type == ActionType.Buildup)
        {
            //doubled animations
            animator.SetTrigger("unitIdle");
        }
        else if (CurrentAction.CurrentTurnActionAtom.Type == ActionType.Recover)
        {
            //doubled animations
            animator.SetTrigger("unitIdle");
        }
        else if (CurrentAction.CurrentTurnActionAtom.Type == ActionType.AttackSingleTile)
        {
            animator.SetTrigger("unitIdle");
        }
        else if (CurrentAction.CurrentTurnActionAtom.Type == ActionType.Attack3Tiles)
        {
            //doubled animations
            animator.SetTrigger("unitIdle");

        }
        else if (CurrentAction.CurrentTurnActionAtom.Type == ActionType.Attack5Tiles)
        {
            //doubled animations
            animator.SetTrigger("unitIdle");
        }
        else if (CurrentAction.CurrentTurnActionAtom.Type == ActionType.AttackAllTiles)
        {
            //doubled animations
            animator.SetTrigger("unitIdle");
        }
        else if (CurrentAction.CurrentTurnActionAtom.Type == ActionType.Rotate)
        {
            //doubled animations
            animator.SetTrigger("unitIdle");
        }
    }
}