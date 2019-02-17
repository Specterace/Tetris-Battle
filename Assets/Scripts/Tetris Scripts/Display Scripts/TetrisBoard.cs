﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tetris;

public class TetrisBoard : MonoBehaviour 
{
	[Header("Parent Objects:")]
	public GameObject BlockFolder;
	public GameObject BackgroundFolder;

	[Header("Dimensions:")]
	public int Width = 10;
	public int Height = 20;
	public int KillHeight = 1;
	public Vector3 StartingLocation = new Vector3( 0, 0 );
	public Vector2 PrefabSize = new Vector2();

	[Header("Colors:")]
	public Color BGColor1;
	public Color BGColor2;

	[Header("Prefab")]
	public GameObject BlockPrefab;

	private GameObject[,] background;
	private GameObject[,] blocks;
	private TetrisBlockScript[,] scripts;
	private RectangularTetrisBoard logic;

	public GameObject[,] Blocks{ get{ return blocks; } }
	public TetrisBlockScript[,] Scripts{ get { return scripts; } }
	public RectangularTetrisBoard Controller{ get { return logic; } }

	public event System.EventHandler BoardChanged;

	public bool[,] Occupied{ 
		get 
		{ 
			if (occupiedFresh)
				return occupied;
			else {
				Point pos = new Point();
				for( pos.y = 0; pos.y < Height; pos.y++ ) {
					for( pos.x = 0; pos.x < Width; pos.x++ ){
						Tetris.Block block = logic.GetBlockAt (pos);
						occupied [pos.y, pos.x] = logic.GetBlockAt (pos).Occupied;
					}
				}
				Debug.Log ("Occupied Generated");
				occupiedFresh = true;
				return occupied;
			}
		} 
	}
	private bool[,] occupied;
	private bool occupiedFresh = false;

	void Logic_BoardChanged (object sender, System.EventArgs e)
	{
		if (BoardChanged != null)
			BoardChanged (this, e);
		occupiedFresh = false;
	}

	public event System.EventHandler<Tetris.RowCollapseEventArgs> RowCollapsed;

	void Logic_RowCollapsed (object sender, RowCollapseEventArgs e)
	{
		if (RowCollapsed != null)
			RowCollapsed (this, e);
	}

	void Awake()
	{
		logic = new RectangularTetrisBoard( Width, Height, KillHeight );

		background = new GameObject[Height, Width];
		blocks = new GameObject[Height, Width];
		scripts = new TetrisBlockScript[Height, Width];
		occupied = new bool[Height, Width];

		for( int i = 0; i < Height; i++ )
		{
			for( int j = 0; j < Width; j++ )
			{
				// Set up the actual blocks
				Blocks[i, j] = (GameObject)Instantiate(
					BlockPrefab,
					BlockFolder.transform
				);

				if (i < 1)
					Blocks [i, j].SetActive (false);

				// Assign the scripts to the variable and make their background invisible
				Scripts[i, j] = Blocks[i, j].GetComponent<TetrisBlockScript>();
				Scripts[i, j].DefaultColor = new Color( 0, 0, 0, 0 );

				// Make the actual background
				// The background is separate because 
				background[i, j] = (GameObject)Instantiate(
					BlockPrefab,
					BackgroundFolder.transform
				);

				if (i < 1)
					background [i, j].SetActive (false);

				background[i, j].GetComponent<TetrisBlockScript>().DefaultColor = 
					(((j + i % 2) % 2 == 0) ? (BGColor1) : (BGColor2));
			}
		}
	}

	void OnEnable()
	{
		logic.BoardChanged += Logic_BoardChanged;
		logic.RowCollapsed += Logic_RowCollapsed;
	}

	void OnDisable()
	{
		logic.BoardChanged -= Logic_BoardChanged;
		logic.RowCollapsed -= Logic_RowCollapsed;
	}

	void Start()
	{
		UpdateBlocks();
	}

	void Update()
	{
		Point pos = new Point();
		// Todo: add some kind of soft board changed which updates whenever the background changes for efficency
		for( pos.y = 0; pos.y < Height; pos.y++ ) {
			for( pos.x = 0; pos.x < Width; pos.x++ ){
				Scripts[pos.y, pos.x].BlockColor = logic.GetBlockAt( pos ).DisplayedColor.ToUnityColor();
			}
		}
	}

	private void UpdateBlocks()
	{
		for( int i = 0; i < Height; i++ )
		{
			for( int j = 0; j < Width; j++ )
			{
				Vector3 position = new Vector3(
					StartingLocation.x + PrefabSize.x * j,
					StartingLocation.y - PrefabSize.y * i,
					StartingLocation.z
				);

				Blocks[i, j].transform.localPosition = position;
				background[i, j].transform.localPosition = position;
			}
		}
	}

}
