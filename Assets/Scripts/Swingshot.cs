using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swingshot : MonoBehaviour
{
    public float hookRange = 10f;
    public float shootCooldown = 1f;
    public float maxSwingDuration = 5f;

    private bool isSwinging = false;
    private Vector3 hookPoint;
    private SpringJoint joint;
    private LineRenderer lineRenderer;
    private float currentSwingTime;
    private float lastShootTime;

    [SerializeField]
    private LayerMask hookableLayers;



    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0) && Time.time - lastShootTime > shootCooldown)
        {
            UpdateLineRenderer();
            ShootHook();
            lastShootTime = Time.time;
        }
        */
        /*

        if (Input.GetMouseButtonDown(0)) // Assuming left mouse button for shooting
        {
            if (!isSwinging)
                ShootHook();
            else
                ReleaseHook();
        }
        */
        /*
        if (isSwinging)
        {
            HandleSwinging();
            UpdateLineRenderer();
        }



        */

        if (isSwinging)
        {
            currentSwingTime += Time.deltaTime;

            if (currentSwingTime >= maxSwingDuration)
            {
                ReleaseHook();
                return;
            }

            UpdateLineRenderer();
        }
        else if (Input.GetMouseButtonDown(0) && Time.time - lastShootTime > shootCooldown)
        {
            ShootHook();
            lastShootTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReleaseHook();
        }
    }
    private void FixedUpdate()
    {
        Vector3 targetPosition = transform.position - transform.forward * 5f + Vector3.up * 2f; // Adjust as needed
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, Time.deltaTime * 5f);
        Camera.main.transform.LookAt(transform.position);
    }

    void ShootHook()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Enlarge the cast radius for more forgiving detection
        float castRadius = 5f;

        if (Physics.SphereCast(ray, castRadius, out hit, hookRange, hookableLayers))
        {
            hookPoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = hookPoint;

            float distance = Vector3.Distance(transform.position, hookPoint);

            // Adjust spring settings based on your preference
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.maxDistance = distance * 0.8f;

            isSwinging = true;
            currentSwingTime = 0f;
        }
    }

    void ReleaseHook()
    {
        isSwinging = false;
        Destroy(joint);
        lineRenderer.enabled = false;
    }

    void HandleSwinging()
    {
        Vector3 playerToHookDirection = (hookPoint - transform.position).normalized;
        Vector3 perpendicularDirection = Vector3.Cross(playerToHookDirection, Vector3.up);
        Vector3 forceDirection = Vector3.Cross(perpendicularDirection, playerToHookDirection);

        // Apply a force to simulate swinging
        float swingForce = 5f;
        GetComponent<Rigidbody>().AddForce(forceDirection * swingForce, ForceMode.Acceleration);

        // You may also want to limit the swing speed for a better feel
        float maxSwingSpeed = 10f;
        GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, maxSwingSpeed);

        float maxSwingDistance = 20f;
        if (Vector3.Distance(transform.position, hookPoint) > maxSwingDistance)
        {
            ReleaseHook();
            return;
        }
    }
    void UpdateLineRenderer()
    {
        // Update the LineRenderer positions to visualize the swingshot trajectory
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, hookPoint);
    }

}