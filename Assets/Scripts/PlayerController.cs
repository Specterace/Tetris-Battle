using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour {
	public string HealButton = "HealP1";
	public string FireButton = "FireP1";
	public string SabotageButton = "SabotageP1";

	public RemoveControl RC;
	public GameObject PauseMenu;

	//public float Sensitivity = .75f;

	private Player player;
    private TetrisGame tetrisGame;
	private bool oldSabotageDown = false;
	void Awake()
	{
		player = GetComponent<Player> ();
        tetrisGame = GetComponent<TetrisGame>();
	}

	void Update()
	{
        if (tetrisGame.AIControlled)
            return;

		if (Input.GetButtonDown(HealButton)) {
			player.Heal();
			Debug.Log (this.ToString() + "Heal");
		}
		if (Input.GetButtonDown(FireButton)) {
			player.Attack();
			Debug.Log (this.ToString() + "Attack");
		}
		if (Input.GetButtonDown ("Start")) {
			RC.Remove ();
			PauseMenu.SetActive (true);
		}
		bool newSabotageDown = Input.GetAxisRaw (SabotageButton) < -.5;
		if (!oldSabotageDown && newSabotageDown) {
            player.Sabotage();
			Debug.Log (this.ToString () + "Sabotage");
		}
		oldSabotageDown = newSabotageDown;
	}
	/*
	private var oldTriggerHeld : boolean;
 
	function Update() {
    	var newTriggerHeld = GetAxis("Fire1") > 0.0;
    	if (!oldTriggerHeld  newTriggerHeld)
        	Fire();
    	oldTriggerHeld = newTriggerHeld;
	}
	*/
}
