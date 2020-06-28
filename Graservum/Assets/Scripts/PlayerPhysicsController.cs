﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsController : MonoBehaviour
{

	// --- Inspector-exposd public properties ---

	[SerializeField]
	private float _accelerationRate;
	public float accelerationRate {
		get { return _accelerationRate; }
		private set { _accelerationRate = value; }
	}

	[SerializeField]
	private float _cooldownRate;
	public float cooldownRate {
		get { return _cooldownRate; }
		private set { _cooldownRate = value; }
	}

	// --- Inspector-exposed private fields ---

#pragma warning disable
	[SerializeField]
	private PlayerInput playerInput;
	[SerializeField]
	private float emissionSpeed = 100.0f;
	[SerializeField]
	[Range(0.01f, 1.0f)]
	private float maxEmittedMassPerSecondFraction = 0.2f;
	[SerializeField]
	private float springStiffness = 1.0f;
#pragma warning restore

	// --- Public properties ---

	public bool currentlyAccelerating { get; set; } = false;

	// --- Private fields ---
	private Bounds playerBounds;
	private Rigidbody _rigidbody;

	void Start() {
		playerBounds = Camera.main.GetComponent<CameraController>().cameraBounds;
		_rigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate() {
		// If player input has said to accelerate on last Update() of PlayerInput, accelerate.
		if (currentlyAccelerating) {
			Accelerate();
		}
		CheckBounds();
	}

	private void Accelerate() {
		// Calculate mass.
		float differentialEmittedMass = playerInput.burnSlider.sliderProgress * maxEmittedMassPerSecondFraction * _rigidbody.mass * Time.fixedDeltaTime;
		float newMass = _rigidbody.mass - differentialEmittedMass;

		// Add the emitted mass to the score.
		playerInput.score += differentialEmittedMass;

		// Get mouse position to target.
		Vector3 targetDirection = HelperFunctions.GetMouseTargetDirection(transform);

		// Calculate velocity.
		Vector3 newVelocity = (_rigidbody.mass * _rigidbody.velocity - differentialEmittedMass * targetDirection * emissionSpeed) / newMass;

		// Apply mass and velocity.
		_rigidbody.mass = newMass;
		_rigidbody.velocity = newVelocity;
	}

	private void CheckBounds() {
		// If player is not in bounds, apply spring force to return player to bounds.
		if (!playerBounds.Contains(transform.position)) {
			// Get normalized direction vector from player to closest point on bounds and scale by spring stiffness.
			Vector3 springForce = springStiffness * Vector3.Normalize(playerBounds.ClosestPoint(transform.position) - transform.position);
			_rigidbody.AddForce(springForce);
		}
	}
}