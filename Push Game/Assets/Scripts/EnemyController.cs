using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

	[HideInInspector]
	public Rigidbody rb;
	[HideInInspector]
	public ItemController weaponController;
	public float health = 100;


	public NavMeshAgent agent;
	public Transform target;
	public Transform turret;
	public float rotateSpeed;
	public float rateOfFire;
	public float speed;
	public float maxRange;
	public float FireRange;
	public float mixRange;

	public GameObject firePoint;
	private int currentWeaponIndex;
	private Vector3 moveDirection;
	protected Weapon currentWeapon;
	protected GameObject currentWeaponObject;
	protected float fireCountDown = 0;
	protected bool targetInSight = false;
	protected GameManager gm;

	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		agent = GetComponent<NavMeshAgent> ();
		rb = GetComponent<Rigidbody> ();
		weaponController = GetComponent<ItemController> ();
		currentWeaponIndex = 0;
		currentWeapon = weaponController.weapons [currentWeaponIndex];
		currentWeaponObject = (GameObject) Instantiate (currentWeapon.prefabs, turret.position, turret.rotation, turret);
		gm = FindObjectOfType<GameManager> ();
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
		if (Physics.Raycast (target.position, transform.forward, out hit)) {

			if (hit.transform.CompareTag ("Player")) {
				targetInSight = true;
			} else {
				targetInSight = false;
			}
		}

		if (targetInSight) {
			Fire ();
		}

		if (GetDistance () <= maxRange) {

			//Move ();
			if (GetDistance () <= FireRange) {
				//Fire ();
			}
		}


	}

	void LateUpdate(){

		Debug.DrawRay (gameObject.transform.position, target.position , Color.red);
	}

	public float GetDistance(){
		return Vector3.Distance (gameObject.transform.position, target.forward);
	}

	void Move(){
		agent.SetDestination(target.position);
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
		transform.LookAt (target.transform);
	}

	public void takeDamage(float damageToTake){

		health -= damageToTake;
		if (health <= 0) {
			Destroy (gameObject);
		}
	}
}
