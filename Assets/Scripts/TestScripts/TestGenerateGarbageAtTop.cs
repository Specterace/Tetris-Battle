using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGenerateGarbageAtTop : MonoBehaviour {

	public GarbageGenerator gg;

	void OnGUI()
	{
		if (GUI.Button (new Rect (0, 0, 500, 500), "Print stuff")) {
			gg.GenerateGarbageAtTop ();
		}
	}
}
