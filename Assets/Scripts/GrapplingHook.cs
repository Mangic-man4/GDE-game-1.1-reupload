using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking.Types;

public class GrapplingHook : MonoBehaviour
{
    private Camera playerCamera;
    private float maxDistance = 10f;
    private Vector3 grapplingHookPosition;
    private bool isGrappling = false;
    private LineRenderer lineRenderer;
    private GameObject player;
    // private Rigidbody playerRigidbody;
    private CharacterController characterController;
    public float swingForce = 5f; // Adjust this value based on your game's feel
    public float releaseDistance = 0.5f;
    private Vector3 detachedPosition;


    void Start()
    {
        // Initialize necessary components
        playerCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        player = GameObject.FindWithTag("Player"); // Replace "Player" with your player tag
        // playerRigidbody = player.GetComponent<Rigidbody>();
        characterController = player.GetComponent<CharacterController>();
        lineRenderer.enabled = false;
    }

    void FixedUpdate()
    {
        //Debug.Log("FixedUpdate called");

        if (Input.GetButton("Fire1"))
        {
            FireGrapplingHook();
        }

        if (isGrappling)
        {
            // Code for swinging logic
            Swing();

            if (Input.GetButtonUp("Fire1") || Vector3.Distance(player.transform.position, grapplingHookPosition) < releaseDistance)
            {
                ReleaseGrapplingHook();
            }
        }
    }

    void FireGrapplingHook()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxDistance))
        {
            Debug.Log("Hit object: " + hit.collider.gameObject.name); 

            if (hit.collider.CompareTag("GrapplingNode"))
            {
                float distanceToHook = Vector3.Distance(player.transform.position, hit.point);
                if (distanceToHook <= maxDistance)
                {
                    ConnectGrapplingHook(hit.point);
                }
            }
        }
    }

    void ConnectGrapplingHook(Vector3 targetPoint)
    {
        grapplingHookPosition = targetPoint;

        // Additional code for grappling hook connection logic

        // Set player position to the stored detached position
        player.transform.position = detachedPosition;

        // Enable grappling after setting the position
        isGrappling = true;

        // Update the line renderer positions
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, player.transform.position);
        lineRenderer.SetPosition(1, targetPoint);
    }

    void Swing()
    {
        Vector3 playerToHook = (grapplingHookPosition - player.transform.position).normalized;

        // Ensure the player is not directly above the hook point to avoid shooting backward
        if (Vector3.Dot(playerToHook, Vector3.up) < 0.95f)
        {
            // Calculate a force directed downward
            Vector3 downwardForce = Vector3.ProjectOnPlane(Vector3.down, playerToHook).normalized;

            // Adjust player position based on swinging motion using CharacterController
            characterController.Move(downwardForce * swingForce * Time.fixedDeltaTime);
        }

        // Adjust player position and rotation based on swinging motion
        // Example using a Rigidbody (you may need to adjust force values):
        // playerRigidbody.AddForce(playerToHook * swingForce, ForceMode.Force);

        // Example using a CharacterController:
        // characterController.Move(playerToHook * swingForce * Time.deltaTime);

        // Debug.DrawRay(player.transform.position, playerToHook * swingForce, Color.red);

    }

    void ReleaseGrapplingHook()
    {
        isGrappling = false;
        lineRenderer.enabled = false;

        // Store the position when detaching only if not already grappling
        if (!isGrappling)
        {
            detachedPosition = player.transform.position;
        }

        // Additional code for releasing the grappling hook
    }
}
