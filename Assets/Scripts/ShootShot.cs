using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootShot : MonoBehaviour {

    // Use this for initialization
    // Update is called once per frame
    [SerializeField]
    private GameObject shot;
    [SerializeField]
    private GameObject firingExp;
    [SerializeField]
    private Transform shotSpawn;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float shotForce;
    private float nextFire;

    public event System.EventHandler OnImpact;
	private AudioSource fire;
	public AudioSource cannonBall;

	void Awake()
	{
		fire = GetComponent<AudioSource> ();
	}

	void Update () {
        if (Input.GetKeyDown("left shift") && (Time.time > nextFire))
        {
            nextFire = Time.time + fireRate;
            fireTower();
        } 
	}

    public void fireTower()
    {
		fire.PlayOneShot (fire.clip);
        GameObject currShot = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        currShot.GetComponent<CannonShot>().parent = this;
        Rigidbody rb = currShot.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * shotForce, ForceMode.VelocityChange);
        GameObject explosion = Instantiate(firingExp, shotSpawn.position, shotSpawn.rotation);
        Destroy(explosion, 10);
    }

    public void Impact()
    {
		cannonBall.PlayOneShot (cannonBall.clip);
        if (OnImpact != null)
        {
            OnImpact(this, System.EventArgs.Empty);
        }
    }
}
