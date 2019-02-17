using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SabotageTower : MonoBehaviour {

    [SerializeField]
    private GameObject saboAura;
    [SerializeField]
    private Transform saboSpawn;
    [SerializeField]
    private float saboRate;
    private float nextSabo;
    // Use this for initialization
	public AudioClip SabotageClip;
	private AudioSource audio;

    void Start()
    {
		audio = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && (Time.time > nextSabo))
        {
            nextSabo = Time.time + saboRate;
            saboTower();
        }
    }

    public void saboTower()
    {
		audio.PlayOneShot (SabotageClip);	
		GameObject currSabo = Instantiate(saboAura, saboSpawn.position, saboSpawn.rotation, gameObject.transform);
		Destroy (currSabo, 10f);
    }
}
