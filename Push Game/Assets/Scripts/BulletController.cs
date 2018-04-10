using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	public float speed;
	public float damage;

	private Rigidbody rb;

	void Start(){
		rb = GetComponent<Rigidbody> ();
		rb.AddForce(transform.forward * speed);
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.CompareTag ("Enemy") || other.gameObject.CompareTag ("Wall")) {

			if (other.gameObject.CompareTag ("Enemy")) {
				other.gameObject.GetComponent<Rigidbody> ().AddForce (-other.transform.forward * 700f);
				other.gameObject.GetComponent<EnemyController> ().takeDamage (Random.Range(30,50));
			}

			Destroy (gameObject);
		}
	}	
}
