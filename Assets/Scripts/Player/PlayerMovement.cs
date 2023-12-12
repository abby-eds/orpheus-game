using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    private float movementMultiplier;
    public float rotateSpeed;
    public float jumpHeight = 1;
    public Transform cameraAnchor;
    public Camera playerCam;
    public float cameraSpeed;
    public float minCameraDistance = 0.5f;
    public float maxCameraDistance = 5;
    public float minCameraAngle = 10;
    public float maxCameraAngle = 80;

    private Rigidbody rb;
    private Animator anim;
    private RingMusic ringMusic;
    private PlayerHealth playerHealth;
    private float movementX;
    private float movementZ;
    private float rotationX;
    private float rotationY;
    private float playerRotation;
    private float cameraRotation;
    private float targetRotation;
    private float rotationDelta;
    private bool jump;
    private float jumpVelocity;
    public bool grounded;
    public bool blocked;
    // private Ray footRay;
    private Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        ringMusic = GetComponent<RingMusic>();
        playerHealth = GetComponent<PlayerHealth>();
        jumpVelocity = Mathf.Sqrt(Physics.gravity.magnitude * 2 * jumpHeight);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            // Get the X and Y delta of the mouse
            rotationX = Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1);
            rotationY = Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1);

            // Rotate the camera around the "Up" axis based on mouse X
            cameraAnchor.Rotate(Vector3.up, rotationX * cameraSpeed, Space.World);

            // Rotate the camera by X euler angles to look up/down
            cameraAnchor.eulerAngles += new Vector3(-rotationY * cameraSpeed, 0, 0);

            // Put bounds on how far up/down the camera can look
            if (cameraAnchor.eulerAngles.x > maxCameraAngle) cameraAnchor.eulerAngles = new Vector3(maxCameraAngle, cameraAnchor.eulerAngles.y, cameraAnchor.eulerAngles.z);
            if (cameraAnchor.eulerAngles.x < minCameraAngle) cameraAnchor.eulerAngles = new Vector3(minCameraAngle, cameraAnchor.eulerAngles.y, cameraAnchor.eulerAngles.z);

            // Save the camera's rotation for later movement calculations
            cameraRotation = cameraAnchor.eulerAngles.y;

            // Get the player's movement input
            if (!playerHealth.dead)
            {
                movementX = Input.GetAxisRaw("Horizontal");
                movementZ = Input.GetAxisRaw("Vertical");
                if (grounded && Input.GetKeyDown(KeyCode.Space))
                {
                    jump = true;
                }
            }
            else
            {
                movementX = 0;
                movementZ = 0;
                jump = false;
            }
        }
    }

    private void FixedUpdate()
    {
        // Calculate the player's velocity in regards to movement (ignoring jumping/gravity)
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        // If the player issues movement, calculate movement
        if (movementX != 0 || movementZ != 0)
        {
            // targetRotation is the rotation value the player wants to end up at given the camera's rotation
            targetRotation = cameraRotation;
            if (movementZ > 0)
            {
                if (movementX < 0) targetRotation -= 45;
                if (movementX > 0) targetRotation += 45;
            }
            else if (movementZ < 0)
            {
                if (movementX < 0) targetRotation += 45;
                if (movementX > 0) targetRotation -= 45;
            }
            targetRotation = (targetRotation + 720) % 360;

            // playerRotation is the player's current rotation
            playerRotation = (transform.eulerAngles.y + 360) % 360;

            // Rotation delta is the difference between the target rotation and the current rotation (how many degrees to turn)
            rotationDelta = targetRotation - playerRotation;

            // Math to make sure that the shortest path is always taken (30 -> 360 should only be a 30 degree turn, not a 330 degree one)
            if (rotationDelta > 0)
            {
                if (rotationDelta > 180) rotationDelta -= 360;
            }
            else
            {
                if (rotationDelta < -180) rotationDelta += 360;
            }

            // Clamp rotation speed to the maximum rotation speed
            if (rotationDelta > rotateSpeed) rotationDelta = rotateSpeed;
            else if (rotationDelta < -rotateSpeed) rotationDelta = -rotateSpeed;

            // Rotate the player accordingly
            transform.Rotate(Vector3.up, rotationDelta);
            cameraAnchor.Rotate(Vector3.up, -rotationDelta, Space.World);

            movement = Quaternion.AngleAxis((cameraRotation + 360) % 360, Vector3.up) * new Vector3(movementX, 0, movementZ).normalized;

            // footRay = new Ray(transform.position + Vector3.up, movement);
            // Debug.DrawRay(footRay.origin, footRay.direction * 0.5f, Color.red);
            // movementMultiplier = ((ringMusic.songLevel > 0) ? 0.75f : 1);
            movementMultiplier = 1;
            if (!blocked) // This looks dumb but it excludes the case of when you're in contact with a slope
            {
                rb.AddForce(movement * movementSpeed * movementMultiplier - horizontalVelocity, ForceMode.VelocityChange);
            }
        }
        else
        {
            // If the player isn't issuing movement, slow down to a stop
            rb.AddForce(-horizontalVelocity, ForceMode.VelocityChange);
            movement = Vector3.zero;
        }
        if (jump && jumpHeight > 0)
        {
            jump = false;
            rb.AddForce(Vector3.up * jumpVelocity, ForceMode.VelocityChange);
            anim.SetTrigger("Jump");
        }
        anim.SetFloat("Forwards", movementZ * movementSpeed);
        anim.SetFloat("Sideways", movementX * movementSpeed);
        anim.SetFloat("Movement Speed", movementMultiplier);
        
        // Debug.Log("Grounded: " + grounded);
        grounded = false;
        blocked = false;
    }

    private void LateUpdate()
    {
        // Make the amera follow the player after both the camera's rotation and the player's position have been updated
        cameraAnchor.position = transform.position + Vector3.up;
        Ray cameraRay = new Ray(cameraAnchor.transform.position, -cameraAnchor.transform.forward);
        Debug.DrawRay(cameraRay.origin, cameraRay.direction * (maxCameraDistance + 1));
        RaycastHit[] cameraHits = Physics.RaycastAll(cameraRay, maxCameraDistance + 1, LayerMask.GetMask("Terrain"));
        float distance = maxCameraDistance;
        foreach (RaycastHit hit in cameraHits)
        {
            if (!hit.collider.isTrigger && hit.distance - minCameraDistance < distance) distance = hit.distance - minCameraDistance;
        }
        playerCam.transform.localPosition = new Vector3(0, 0, -distance);
        UIManager.UI.UpdatePopups();
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.7f)
            {
                grounded = true;
            }
            Vector2 movement2d = new Vector2(movement.x, movement.z);
            Vector2 contact2d = new Vector2(contact.normal.x, contact.normal.z);
            if(Vector2.Dot(movement2d, contact2d) < -0.1f && contact2d.magnitude > 0.7f && contact2d.magnitude < 0.99f)
            {
                blocked = true;
            }
        }
    }
}
