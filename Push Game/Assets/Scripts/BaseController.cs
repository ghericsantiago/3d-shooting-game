using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseController : MonoBehaviour {

	public float maxHealth = 100;
	private float health;

	public Image healthbar;

	public GameManager gm;

	// Use this for initialization
	void Start () {
		health = maxHealth;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void takeDamage(float damageToTake){

		health -= damageToTake;
		healthbar.fillAmount = health / maxHealth;
		if (health <= 0) {
			gm.GameOver ();
		}
	}
}
