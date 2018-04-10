using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour {

	public Transform target;
	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//float step = speed * Time.deltaTime;
		//transform.rotation = Quaternion.RotateTowards (transform.rotation, target.rotation, step);

		transform.LookAt (new Vector3(target.position.x,target.localScale.y, target.position.z)); // = Quaternion.FromToRotation(transform.position, target.position);
	}
}
