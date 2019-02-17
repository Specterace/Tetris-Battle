using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShot : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private float throwForce;

    public ShootShot parent;

	void Start () {
       //GetComponent<Rigidbody>().velocity = transform.forward * speed;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Tower")
        {
            parent.Impact();
            Destroy(gameObject);
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }


}
