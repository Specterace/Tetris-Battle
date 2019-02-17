using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TetrisScore))]
[RequireComponent(typeof(GarbageGenerator))]
public class Player: MonoBehaviour {
    [SerializeField]
    private StitchTarget target;
    [SerializeField]
    private int stitchNum;
	[SerializeField]
	private RemoveControl rc;

    // Other Player
    [SerializeField]
    private Player OtherPlayer;

    // Game Over Screen
    [SerializeField]
    private GameObject GameOverScreen;
    [SerializeField]
    private Text WinnerText;

    // Player's HUD
    [SerializeField]
    private RectTransform HP;
    [SerializeField]
    private Text HP_Text;
    [SerializeField]
    private RectTransform AP;
    [SerializeField]
    private Text AP_Text;

    // HP
    [SerializeField]
    private int HealthMax = 50;
    [SerializeField]
    [Range(0, 50)]
    private int Health;

    // AP
    [SerializeField]
    private int APMax = 25;
    [SerializeField]
    [Range(0, 25)]
    private int ActionPoints;

    // ActionButtons
    [SerializeField]
    private Image AttackButton;
    [SerializeField]
    private Image HealButton;
    [SerializeField]
    private Image SabotageButton;
	[SerializeField]
	private GameObject OverflowTimer;

    private TetrisScore scoringSystem;
    private GarbageGenerator garbageGenerator;
    private TetrisBoard board;
    private TetrisGame game;

	private void Awake()
	{
		scoringSystem = GetComponent<TetrisScore> ();
        garbageGenerator = GetComponent<GarbageGenerator>();
        board = GetComponent<TetrisBoard>();
        game = GetComponent<TetrisGame>();
	}

    private void Start() {
        Health = HealthMax;
        ActionPoints = 0;

		scoringSystem.PlayerScored += gainActionPoints;
        board.BoardChanged += Board_BoardChanged;
        game.Started += Game_Started;
        target.Source.shoot[stitchNum].OnImpact += Player_OnImpact;
    }

    private void Game_Started(object sender, System.EventArgs e)
    {
        once = true;
    }

    private bool once = true;

    private void Board_BoardChanged(object sender, System.EventArgs e)
    {
        if (once && board.Controller.Lost) {
            TakeDamage(25);
			OverflowTimer.SetActive (true);
            once = false;
        }
    }

    private void Update() {
        if (ActionPoints >= 5) {
            AttackButton.color = Color.red;
        } else {
            AttackButton.color = Color.white;
        }

        if (ActionPoints >= 8) {
            HealButton.color = Color.cyan;
        } else {
            HealButton.color = Color.white;
        }

        if (ActionPoints >= 16) {
            SabotageButton.color = Color.green;
        } else {
            SabotageButton.color = Color.white;
        }



        HP.offsetMax = new Vector3(-HP_Percentage(), 0);
        HP_Text.text = Health.ToString() + "/" + HealthMax.ToString();
        AP.offsetMax = new Vector3(-AP_Percentage(), 0);
        AP_Text.text = ActionPoints.ToString() + "/" + APMax.ToString();

        if (OtherPlayer.isDead()) {
            GameOverScreen.SetActive(true);
            WinnerText.text = "PLAYER " + this.gameObject.name.Substring(14, 1) + "\n WINS";
			rc.Remove ();
        }
    }

    private float HP_Percentage() {
        return (400 - (400 * Health/HealthMax));
    }

    private float AP_Percentage() {
        return (350 - (350 * ActionPoints / APMax));
    }

    private bool isDead() {
        return Health <= 0;
    }

    private void LoseAP() {
        if (ActionPoints > 5 && ActionPoints < 11) {
            ActionPoints -= 1;
        } else if (ActionPoints >= 11 && ActionPoints < 18) {
            ActionPoints -= 2;
        } else if (ActionPoints >= 19) {
            ActionPoints -= 3;
        }
    }
    
    private void TakeDamage(int damage) {
        if ((Health -= damage) < 0) {
            Health = 0;
        }
    }

    public void Attack() {
        if (ActionPoints >= 5) {
            ActionPoints -= 5;
            target.Source.shoot[stitchNum].fireTower();
        }
    }

    private void Player_OnImpact(object sender, System.EventArgs e)
    {
        OtherPlayer.TakeDamage(3);
        OtherPlayer.garbageGenerator.GenerateGarbage(3);
    }

    public void Heal() {
		if (ActionPoints >= 8) {
            ActionPoints -= 8;
            Health += 8;
			if (Health > HealthMax) {
				Health = HealthMax;
			}
            target.Source.heal[stitchNum].healTower();
        }
    }

    public void Sabotage()
    {
        if(ActionPoints >= 16)
        {
            ActionPoints -= 16;
            OtherPlayer.garbageGenerator.GenerateGarbageAtTop();
            target.Source.sabotage[stitchNum].saboTower();        }
    }

    public void gainActionPoints(object sender, int points) {
        ActionPoints += points;
        while (ActionPoints > APMax)
        {
            Attack();
        }
        ActionPoints = Mathf.Min(ActionPoints, APMax);
    }
}
