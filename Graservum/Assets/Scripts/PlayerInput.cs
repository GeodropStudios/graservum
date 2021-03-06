﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Class for taking player input and signalling player components to update.
public class PlayerInput : MonoBehaviour {

    // --- Inspector-exposd public properties ---

    [SerializeField]
    private GameObject _playerAsteroid;
    public GameObject playerAsteroid { get => _playerAsteroid; private set => _playerAsteroid = value; }

    [SerializeField]
    private EngineController _engineController;
    public EngineController engineController { get => _engineController; private set => _engineController = value; }

    [SerializeField]
    private PlayerPhysicsController _playerPhysicsController;
    public PlayerPhysicsController playerPhysicsController { get => _playerPhysicsController; private set => _playerPhysicsController = value; }

    [SerializeField]
    private Joystick _joystick;
    public Joystick joystick { get => _joystick; private set => _joystick = value; }

    [SerializeField]
    private EasySlider _burnSlider;
    public EasySlider burnSlider { get => _burnSlider; private set => _burnSlider = value; }

	[SerializeField]
	[Range(0.0f, 100000.0f)]
	private float _maxAsteroidMass = 10000.0f;
	public float maxAsteroidMass { get => _maxAsteroidMass; private set => _maxAsteroidMass = value; }

    // --- Inspector-exposed private fields ---

#pragma warning disable
	[SerializeField]
    [Range(1.0f, 10.0f)]
    private float timeToMaxValue = 2.0f;
	[SerializeField]
	private TMP_Text scoreText;
    [SerializeField]
    private GameObject deathShip;
#pragma warning restore

    // --- Public properties ---

    public float score { get; set; } = 0.0f;

    // --- Private fields ---

    private float accumulatedTime = 0.0f;
	private Rigidbody asteroidRigidbody;

    void OnDestroy() {
        GameObject.Find("AsteroidManager").GetComponent<AsteroidManager>().DisableWarningSprites();

        Instantiate(deathShip, _engineController.transform.parent.position, Quaternion.identity);
    }

	void Start() {
		asteroidRigidbody = playerAsteroid.GetComponent<Rigidbody>();
	}

	void Update() {
		//Check if the player asteroid's mass has exceeded the maximum.
		if (asteroidRigidbody.mass > maxAsteroidMass) {
			Destroy(gameObject);
		}

        // Check for joystick held, update the accumulated time.
        if (joystick.GetInput()) {
            accumulatedTime += Time.deltaTime * playerPhysicsController.accelerationRate;
        } else {
            accumulatedTime -= Time.deltaTime * playerPhysicsController.cooldownRate;
        }
        accumulatedTime = Mathf.Clamp(accumulatedTime, 0.0f, timeToMaxValue);

        // Update slider with new value.
        float progress = accumulatedTime / timeToMaxValue * burnSlider.maximumValue;
        burnSlider.sliderValue = progress;

		playerPhysicsController.currentlyAccelerating = burnSlider.sliderValue > burnSlider.minimumValue;

        engineController.UpdateEngine();

        // Update score text.
        if (scoreText != null) {
            scoreText.text = ((int) score).ToString();
        }

		// Move the ship according to playerAsteroid position.
		engineController.transform.parent.position = asteroidRigidbody.position;
	}
}
