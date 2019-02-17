using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tetris;

[RequireComponent(typeof(TetrisBoard))]
public class TetrisGame : MonoBehaviour
{
	public Color ShadowColor;

	public ControllerInputManager InputManager;
    public AI TetrisAi;

    public bool AIControlled = false;

	[Header("Queue Parameters")]
	[SerializeField]
	private int queueSize = 5;
	[SerializeField]
	private int queueLookback = 4;
	[SerializeField]
	private int queueTries = 4;

	public Tetris.Game game;
	private bool initialized = false;
		
	public MinoType HeldBlock{ get{ return game.HeldBlock; } }
	public MinoType[] QueuedBlocks{ get{ return game.QueuedBlocks; } }
	public RectangularTetrisBoard BoardController{ get{ return (RectangularTetrisBoard)game.Board; } }
	public TetrisBoard Board{ get { return GetComponent<TetrisBoard> (); } } 

	public event System.EventHandler BlockHeld;
	public event System.EventHandler BlockDropped;
	public event System.EventHandler Started;

    /// <summary>
    /// Creates a new Occupied array based on if a mino is dropped
    /// All params are optional, if any aren't used then the current mino/occupied is used
    /// </summary>
    /// <param name="position">Position to drop from, if null use current mino position</param>
    /// <param name="rotation">Rotation to drop from, if null use current mino rotation</param>
    /// <param name="type">Block to drop, if null use current mino block</param>
    /// <param name="occupied">Result from previous call, if null use board array</param>
    /// <returns></returns>
    public bool[,] TestDrop(Point position = null, Tetris.Rotation? rotation = null, MinoType type = null, bool[,] occupied = null)
    {
        if (position == null) position = game.CurrentBlock.Position;
        if (rotation == null) rotation = game.CurrentBlock.Rotation;
        if (type == null) type = game.CurrentBlock.BlockType;
        if (occupied == null) occupied = Board.Occupied;

        occupied = (bool[,])occupied.Clone();

        while (IsValidPlacement(occupied, Mino.GetBlockLocations(type, position, (int)rotation)))
            position.y++;
        position.y--;
        if (IsValidPlacement(occupied, Mino.GetBlockLocations(type, position, (int)rotation)))
        {
            foreach(Point p in Mino.GetBlockLocations(type, position, (int)rotation))
            {
                if (p.y >= 0)
                    occupied[p.y, p.x] = true;
            }
        }
        return occupied;
    }

    static bool IsValidPlacement(bool[,] occupied, IEnumerable<Point> blockLocations)
    {
        int height = occupied.GetLength(0);
        int width = occupied.GetLength(1);

        foreach(Point p in blockLocations)
        {
            if(p.x < 0 || p.x >= width || p.y >= height ||
                (p.y >= 0 && occupied[p.y, p.x]))
            {
                return false;
            }
        }
        
        return true;
    }

    void Awake()
    {
        MainGameScreen ms = FindObjectOfType<MainGameScreen>();
        if(ms != null && tag == "AIPlayer")
        {
            AIControlled = ms.SetAI;
        }
    }

    void Start()
    {
        Init();
    }

	// Use this for initialization
	public void Init()
	{
		Mino.ShadowColor = ShadowColor.ToBlockColor();
		RandomItemGenerator<MinoType> queue = new RandomItemGenerator<MinoType>( Tetromino.TETROMINO_TYPES, queueSize, queueLookback, queueTries );
		BaseTetrisBoard board = Board.Controller;
        if (AIControlled)
            game = new Game(TetrisAi.CurrentState, board, queue);
        else
            game = new Game(InputManager, board, queue);
		initialized = true;
		OnEnabled();
		game.GameStart();
		game.OnDisable();
	}

	void OnEnabled()
	{
		if (initialized) {
			//game.OnEnable();
			game.BlockHeld += Game_BlockHeld;
			game.BlockDropped += Game_BlockDropped;
			game.Started += Game_Started;
			Board.BoardChanged += Board_BoardChanged;
		}
	}

	void Reset()
	{
		game.Reset ();
	}

	void Board_BoardChanged (object sender, System.EventArgs e)
	{
		if (Board.Controller.Lost) 
		{
			Invoke ("Reset", 3f);
		}
	}

	void Game_BlockHeld (object sender, System.EventArgs e)
	{
		if (BlockHeld != null )
			BlockHeld (this, e);
	}

	void Game_BlockDropped (object sender, System.EventArgs e)
	{
		if (BlockDropped != null)
			BlockDropped (this, e);
	}

	void Game_Started (object sender, System.EventArgs e)
	{
		if (Started != null)
			Started (this, e);
	}

	void OnDisabled()
	{
		//game.OnDisable();
		game.BlockHeld -= Game_BlockHeld;
		game.BlockDropped -= Game_BlockDropped;
		game.Started -= Game_Started;
		Board.BoardChanged -= Board_BoardChanged;
	}

	void Update()
	{
		if(!Board.Controller.Lost)
			game.Update (Time.deltaTime);
	}
}
