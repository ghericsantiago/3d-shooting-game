using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


	public Text countdown;
	public Text win;
	public bool gameEnded = false;
	public float timer;


	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.R)) {
			SceneManager.LoadScene ("Main");
			gameEnded = false;
		}

		if (Input.GetKeyDown (KeyCode.Q)) {
			Application.Quit();
		}

		countdown.text = "Count Down: " + timer.ToString("F2");

		if (timer <= 0) {
			Win ();
		} else {
			timer -= Time.deltaTime;
		}

	}

	void Win(){
		win.text = "You win";
		countdown.text = "Count Down: 00:00";
		gameEnded = true;
	}

	void Pause(){
		Time.timeScale = 0;
	}

	void Resume(){
		Debug.Log ("Resume");
		Time.timeScale = 1;
	}

	public void GameOver(){
		float lastTimer = timer;
		win.text = "You lose!";
		countdown.text = "Count Down: " + lastTimer.ToString("F2");
		gameEnded = true;
	}
}
