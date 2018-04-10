using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	[HideInInspector]
	public Rigidbody rb;
	[HideInInspector]
	public ItemController weaponController;

	public Transform target;
	public Transform turret;
	public float jumpForce;
	public float movementSpeed;
	public float rotateSpeed;
	public float mouseSensitivity;
	public GameObject firePoint;

	private int currentWeaponIndex;
	private bool stun = false;
	private Vector3 moveDirection;
	protected Weapon currentWeapon;
	protected GameObject currentWeaponObject;
	protected GameManager gm;

	// Use this for initialization
	void Start () {
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

		if (Input.GetButtonDown ("Jump")) {
			Jump ();
		}
		RotateTurret ();

		if (Input.GetKeyDown(KeyCode.E)) {
			SwitchWeapon ();
		}

		if (Input.GetButtonDown ("Fire1")) {
			Fire ();
		}

	}

	void FixedUpdate(){
		
		if (gm.gameEnded)
			return;
		
		Move ();
	}

	void SwitchWeapon(){

		Destroy (currentWeaponObject);
		currentWeaponIndex = (currentWeaponIndex + 1) % weaponController.weapons.Length;
		currentWeapon = weaponController.weapons [currentWeaponIndex];
		currentWeaponObject = (GameObject) Instantiate (currentWeapon.prefabs, turret.position, turret.rotation, turret);

	}

	void Fire (){
		
		GameObject bullet = (GameObject) Instantiate (currentWeapon.bullets, firePoint.transform.position, firePoint.transform.rotation);
		//Destroy (bullet, 5f);
	}

	void Jump(){
		rb.velocity = new Vector3 (rb.velocity.x, jumpForce, rb.velocity.z);
	}

	void Move(){

		if (stun)
			return;

		// Move Player Forward/Backward

		transform.Translate(transform.forward * movementSpeed * Input.GetAxis ("Vertical") * Time.deltaTime, Space.World);

		/*
		 * float yStore = rb.velocity.y;
		rb.velocity = transform.forward * Input.GetAxis ("Vertical") * movementSpeed;
		rb.velocity = new Vector3 (rb.velocity.x, yStore, rb.velocity.z);
		*/

		transform.Translate(transform.right * movementSpeed * Input.GetAxis ("Horizontal") * Time.deltaTime, Space.World);

		float horizontal = Input.GetAxis("Horizontal") * rotateSpeed;
		transform.Rotate (0, horizontal, 0);

	}

	void RotateTurret(){
		
		if (turret.rotation.y < -45) {
			turret.Rotate (0 , -45, 0);
		} else {
			float horizontalMouse = Input.GetAxis ("Mouse X") * mouseSensitivity;
			turret.Rotate (0 , horizontalMouse, 0);
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Bullet")) {
			StartCoroutine (Stun());
		}
	}


	IEnumerator Stun(){
		stun = true;
		yield return new WaitForSeconds (1);
		stun = false;
	}


	void OnCollisionEnter(Collision other){

		if (other.gameObject.CompareTag ("Enemy")) {
			gm.GameOver ();
		}

	}

}
