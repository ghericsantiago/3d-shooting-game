using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

	[HideInInspector]
	public Rigidbody rb;
	[HideInInspector]
	public ItemController weaponController;
	public float maxHealth = 100;
	public float health;

	public NavMeshAgent agent;
	public Transform target;
	public Transform mainTarget;
	protected Transform currentTarget;

	public Transform turret;
	public float rotateSpeed;
	public float rateOfFire;
	public float speed;
	public float maxRange;
	public float FireRange;
	public float mixRange;

	public Image healthbar;

	public GameObject firePoint;
	private int currentWeaponIndex;
	private Vector3 moveDirection;
	protected Weapon currentWeapon;
	protected GameObject currentWeaponObject;
	protected float fireCountDown = 0;
	protected bool targetInSight = false;
	protected GameManager gm;

	void Start () {
		health = maxHealth;
		agent = GetComponent<NavMeshAgent> ();
		rb = GetComponent<Rigidbody> ();
		weaponController = GetComponent<ItemController> ();
		currentWeaponIndex = 0;
		currentWeapon = weaponController.weapons [currentWeaponIndex];
		currentWeaponObject = (GameObject) Instantiate (currentWeapon.prefabs, turret.position, turret.rotation, turret);
		gm = FindObjectOfType<GameManager> ();
		currentTarget = target;
	}
	
	// Update is called once per frame
	void Update () {

		if (gm.gameEnded)
			return;

		LookAtTarget ();
		Move ();
	}

	void FixedUpdate(){

		if (gm.gameEnded)
			return;
		RaycastHit hit;

		if (Physics.Raycast (transform.position,  transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)) {

			if (hit.transform.CompareTag ("Player") || hit.transform.CompareTag ("Base")) {
				targetInSight = true;
			} else {
				targetInSight = false;
			}
		}

		if (targetInSight && GetDistance() <= FireRange) {
			Fire ();
		}

		updateTarget ();

	}

	void LateUpdate(){

		Debug.DrawRay (gameObject.transform.position, currentTarget.position , Color.red);
		Debug.DrawLine (transform.position, currentTarget.transform.position, Color.green);
	}

	public float GetDistance(){
		return Vector3.Distance (transform.position, currentTarget.position);
	}

	void updateTarget(){

		float dis1 = Vector3.Distance (transform.position, target.position);
		float dis2 = Vector3.Distance (transform.position, mainTarget.position);

		if (dis1 > maxRange || dis2 <= maxRange) {
			currentTarget = mainTarget;
		} else {
			currentTarget = target;
		}
	
	}

	void Move(){
		agent.SetDestination(currentTarget.position);
	}

	void Fire(){

		if (fireCountDown <= 0) {
			fireCountDown = 1 / rateOfFire;
			GameObject bullet = (GameObject) Instantiate (currentWeapon.bullets, firePoint.transform.position, firePoint.transform.rotation);
			Destroy (bullet, 5f);
		} else {
			fireCountDown -= Time.deltaTime;
		}

	}

	void LookAtTarget(){
		transform.LookAt (currentTarget.transform);
	}

	public void takeDamage(float damageToTake){

		health -= damageToTake;

		healthbar.fillAmount = health / maxHealth;

		if (health <= 0) {
			Destroy (gameObject);
		}
	}
}
