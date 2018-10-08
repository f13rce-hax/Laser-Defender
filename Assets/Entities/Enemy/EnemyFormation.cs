using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFormation : MonoBehaviour {
	public float health = 150f;
	public GameObject projectile;
	public float projectileSpeed;
	public float firingRate = 0.2f;
	public float shotsPerSecond = 0.5f;
	public int scoreValue = 150;
	public AudioClip laserSound;
	public AudioClip destroyedSound;

	private ScoreKeeper scoreKeeper;

	void Start () {
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
	}

	void OnTriggerEnter2D (Collider2D collider) {
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile) {
			health -= missile.GetDamage ();
			missile.Hit();
			if (health <= 0) {
				Destroy(gameObject);
				scoreKeeper.Score(scoreValue);
				AudioSource.PlayClipAtPoint(destroyedSound, transform.position);
			} 
		}
	}
	void Fire () {
		GameObject enemyLaser = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		enemyLaser.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0f, - projectileSpeed, 0f);
		AudioSource.PlayClipAtPoint(laserSound, transform.position);
	}

	void Update ()
	{
		float probability = Time.deltaTime * shotsPerSecond;
		if (Random.value < probability) {
			Fire();
		}
	}
}
