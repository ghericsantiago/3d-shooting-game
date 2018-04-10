using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon{
	public GameObject prefabs;
	public string weaponName;
	public float rateOfFire;
	public GameObject bullets;
}

public class ItemController : MonoBehaviour {

	public Weapon[] weapons;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
