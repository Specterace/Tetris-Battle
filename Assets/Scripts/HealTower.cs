using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTower : MonoBehaviour {

    [SerializeField]
    private GameObject healAura1;
    [SerializeField]
    private GameObject healAura2;
    [SerializeField]
    private Transform healSpawn1;
    [SerializeField]
    private Transform healSpawn2;
    [SerializeField]
    private Transform healSpawn3;
    [SerializeField]
    private Transform healSpawn4;
    [SerializeField]
    private Transform healSpawn5;
    [SerializeField]
    private float healRate;
    private float nextHeal;
    // Use this for initialization
	public AudioClip HealClip;
	private AudioSource audio;

	void Awake()
	{
		audio = GetComponent<AudioSource> ();
	}

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && (Time.time > nextHeal))
        {
            nextHeal = Time.time + healRate;
            healTower();
        }
    }

    public void healTower()
    { 
		audio.PlayOneShot (HealClip);
		GameObject currHeal = Instantiate(healAura1, healSpawn1.position, healSpawn1.rotation, gameObject.transform);
		GameObject currHeal1 = Instantiate(healAura2, healSpawn2.position, healSpawn2.rotation, gameObject.transform);
		GameObject currHeal2 = Instantiate(healAura2, healSpawn3.position, healSpawn3.rotation, gameObject.transform);
		GameObject currHeal3 = Instantiate(healAura2, healSpawn4.position, healSpawn4.rotation, gameObject.transform);
		GameObject currHeal4 = Instantiate(healAura2, healSpawn5.position, healSpawn5.rotation, gameObject.transform);
    }
}
