﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour {

	public float speed;
	public float damage;

	private Rigidbody rb;

	void Start(){
		rb = GetComponent<Rigidbody> ();
		rb.AddForce(transform.forward * speed);
	}

	void OnCollisionEnter(Collision other){

		if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.GetComponent<Rigidbody> ().AddForce (-other.transform.forward * 100f);
			other.gameObject.GetComponent<PlayerController> ().takeDamage (damage);
		}

		if (other.gameObject.CompareTag ("Base")) {
			other.gameObject.GetComponent<BaseController> ().takeDamage (damage);
		}

		Destroy (gameObject);
			
	}

}
