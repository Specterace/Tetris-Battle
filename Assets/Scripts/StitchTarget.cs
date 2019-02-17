using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StitchTarget : MonoBehaviour {

    public string SceneName = "Battlefield";
    private StitchSource source;

    public StitchSource Source { get { return source; } }

    void Awake()
    {
        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
    }

	// Use this for initialization
	void Start () {
        source = FindObjectOfType<StitchSource>();
	}
}
