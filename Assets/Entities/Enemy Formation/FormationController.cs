using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour {
 	public GameObject enemyPrefab;
 	public float width = 10f;
 	public float height = 5f;
	public float speed = 10f;
	public float spawnDelay = 0.5f;

	private bool movingRight = true;

	float xmin;
	float xmax;


	// Use this for initialization
	void Start ()
	{ 
		speed = speed * Time.deltaTime;

		SpawnUntilFull();

		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftEdge = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distanceToCamera));
		Vector3 rightEdge = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distanceToCamera));
		xmax = rightEdge.x;
		xmin = leftEdge.x;

	}

	void SpawnEnemies(){
		foreach (Transform child in transform) {
			GameObject enemy = Instantiate (enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
 		}
	 }

	 void SpawnUntilFull () {
		Transform freePosition = NextFreePosition ();
		if (freePosition) {
		GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
		enemy.transform.parent = freePosition;
		}
		if (NextFreePosition ()) {
			Invoke ("SpawnUntilFull", spawnDelay);
		}
	}

	public void OnDrawGizmos (){
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
	}
	
	// Update is called once per frame
	void Update ()
	{

		// make the enemies move left to right
		if (movingRight) {
			transform.position += Vector3.right * speed;
		} else {
			transform.position += Vector3.left * speed;
		}

		float rightEdgeOfFormation = transform.position.x + (0.5f * width);
		float leftEdgeOfFormation = transform.position.x - (0.5f * width);
		if (leftEdgeOfFormation < xmin) {
			movingRight = true;
		} else if (rightEdgeOfFormation > xmax) {
			movingRight = false;
		}

		// restrict the enemies to the game space
		float newX = Mathf.Clamp (transform.position.x, xmin, xmax);
		transform.position = new Vector3 (newX, 0, transform.position.z);

		if (AllMembersDead ()) {
			Debug.Log("Empty formation");
			SpawnUntilFull();
		} 
	}

	Transform NextFreePosition () {
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount == 0 ){
				return childPositionGameObject;
			}
		}
		return null;
	}

	bool AllMembersDead () {
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0 ){
				return false;
			}
		}
		return true;

	}
}
