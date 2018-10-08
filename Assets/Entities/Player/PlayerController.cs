using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float speed = 10f;
	public GameObject projectile;
	public float projectileSpeed;
	public float firingRate = 0.2f;
	public float health = 300f;

	public AudioClip laserSound;
	public AudioClip destroyedSound;
	   
	float xmin;
	float xmax;


	// Use this for initialization
	void Start () 
	{ 	
		speed = speed * Time.deltaTime;
		 
		 
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0,0, distanceToCamera));
		Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1,0, distanceToCamera));
  		xmax = rightEdge.x;
		xmin = leftEdge.x;

	}

	void OnTriggerEnter2D (Collider2D collider) {
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile) {
			health -= missile.GetDamage ();
			missile.Hit();
			if (health <= 0) {
				Die();

			} 
		}
	}

	void Die(){
		LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		man.LoadLevel("Win Screen");
		Destroy(gameObject);
		AudioSource.PlayClipAtPoint(destroyedSound, transform.position);
	}

	void Fire () {
		GameObject playerLaser = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		playerLaser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
		AudioSource.PlayClipAtPoint(laserSound, transform.position); 
	}

	// Update is called once per frame
	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating ("Fire", 0.000001f, firingRate);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke("Fire");
		}

		// move the player
		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.position += Vector3.left * speed;
 		} else if (Input.GetKey (KeyCode.RightArrow)) {
			transform.position += Vector3.right * speed; 
		} 

		// restrict the player to the game space
		float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}
}
