using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
	public Animator anim;
	public Camera camera;

	public float maxHealth;
	protected float health;

	public Image healthbar;

	private int currentWeaponIndex;
	private bool stun = false;
	private Vector3 moveDirection;
	protected Weapon currentWeapon;
	protected GameObject currentWeaponObject;
	protected GameManager gm;

	// Use this for initialization
	void Start () {
		health = maxHealth;
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

		if (!currentWeapon.isMelee) {
			//RotateTurret ();
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			SwitchWeapon ();
		}

		if (Input.GetButtonDown ("Fire1")) {

			if (!currentWeapon.isMelee) {
				Fire ();
			} else {
				Blow ();
			}
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

	void Blow(){
		
		Transform weaponPivot = currentWeaponObject.transform.Find ("WeaponPivot");
		StartCoroutine(RotateOverTime (weaponPivot, weaponPivot, .15f ));
		//StartCoroutine(RotateOverTime (weaponPivot, weaponPivot.rotation, Quaternion.Euler (-100, weaponPivot.rotation.eulerAngles.y, weaponPivot.rotation.eulerAngles.z), .1f ));
		//weaponPivot.rotation = newRotation;
	}


	IEnumerator RotateOverTime (Transform objToRotate, Transform weaponPivot, float duration) {

		float t = 0f;
		while(t < duration)
		{
			objToRotate.rotation = Quaternion.Slerp(Quaternion.Euler (weaponPivot.rotation.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), Quaternion.Euler (100, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), t / duration);
			yield return null;
			t += Time.deltaTime;
		}

		objToRotate.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
	}

	void Fire (){

		if (currentWeapon.multiplier > 1) {
			float offset = -(1f / currentWeapon.multiplier);
			for (int i = 0; i < currentWeapon.multiplier; i++) {
				Vector3 newPosition = new Vector3 (firePoint.transform.position.x + offset, firePoint.transform.position.y, firePoint.transform.position.z);
				GameObject bullet = (GameObject)Instantiate (currentWeapon.bullets, newPosition, firePoint.transform.rotation);
				offset = (1f / currentWeapon.multiplier);
			}
		} else {
			GameObject bullet = (GameObject)Instantiate (currentWeapon.bullets, firePoint.transform.position, firePoint.transform.rotation);
		}


		//Destroy (bullet, 5f);
	}

	void Jump(){
		rb.velocity = new Vector3 (rb.velocity.x, jumpForce, rb.velocity.z);
	}

	void Move(){

		if (stun)
			return;

		// Move Player Forward/Backward

		//transform.Translate(transform.forward * movementSpeed * Input.GetAxis ("Vertical") * Time.deltaTime, Space.World);

		/*
		 * float yStore = rb.velocity.y;
		rb.velocity = transform.forward * Input.GetAxis ("Vertical") * movementSpeed;
		rb.velocity = new Vector3 (rb.velocity.x, yStore, rb.velocity.z);
		*/


		//transform.Translate(transform.right * movementSpeed * Input.GetAxis ("Horizontal") * Time.deltaTime, Space.World);

		//float horizontal = Input.GetAxis("Horizontal") * rotateSpeed;
		//transform.Rotate (0, horizontal, 0);
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		anim.SetFloat ("speed", Mathf.Abs (horizontal ) + Mathf.Abs(vertical));

		transform.Translate ((new Vector3(horizontal, 0, vertical)) * movementSpeed * Time.deltaTime, Space.World);

		/* 
		 * Vector3 moveDirection = new Vector3 (horizontal, 0f , vertical);
		if (moveDirection != Vector3.zero) {
			Quaternion newRotation = Quaternion.LookRotation (moveDirection);
			transform.rotation = Quaternion.Slerp (transform.rotation, newRotation, Time.deltaTime * 8);
		}*/
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
			foreach (ContactPoint c in other.contacts) {
				if (c.thisCollider.name == "HammerHead" || c.thisCollider.name == "HammerHandle") {

					other.gameObject.GetComponent<Rigidbody> ().AddForce (-other.transform.forward * 700f);
					other.gameObject.GetComponent<EnemyController> ().takeDamage (Random.Range(80,100));

					return;
				}
			}
		}
	}

	public void takeDamage(float damageToTake){

		health -= damageToTake;

		healthbar.fillAmount = health / maxHealth;

		if (health <= 0) {
			gm.GameOver ();
		}
	}

	public void moveUp(){
		transform.Translate ((new Vector3(0, 0, 1)) * movementSpeed * Time.deltaTime, Space.World);
	}

	public void moveDown(){
		transform.Translate ((new Vector3(0, 0, -1)) * movementSpeed * Time.deltaTime, Space.World);
	}

	public void moveLeft(){
		transform.Translate ((new Vector3(-1, 0, 0)) * movementSpeed * Time.deltaTime, Space.World);
	}

	public void moveRight(){
		transform.Translate ((new Vector3(1, 0, 0)) * movementSpeed * Time.deltaTime, Space.World);
	}

}
