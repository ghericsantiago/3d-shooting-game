using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform target;

	protected Vector3 offset;

	void Start()
	{
		offset = target.position - transform.position ;
	}

	void LateUpdate()
	{
		//transform.LookAt (target.transform.position);
		transform.position = target.position - offset;
	}

}
