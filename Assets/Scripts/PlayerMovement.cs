using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
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
    private float movementX;
    private float movementZ;
    private float rotationX;
    private float rotationY;
    private float playerRotation;
    private float cameraRotation;
    private float targetRotation;
    private float rotationDelta;
    private bool jump;
    private bool hasJump;
    private float jumpVelocity;
    private Ray footRay;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        jumpVelocity = Mathf.Sqrt(Physics.gravity.magnitude * 2 * jumpHeight);
    }

    // Update is called once per frame
    void Update()
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
        movementX = Input.GetAxisRaw("Horizontal");
        movementZ = Input.GetAxisRaw("Vertical");
        if (hasJump && Input.GetKeyDown(KeyCode.Space))
        {
            hasJump = false;
            jump = true;
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

            Vector3 movement = Quaternion.AngleAxis((cameraRotation + 360) % 360, Vector3.up) * new Vector3(movementX, 0, movementZ).normalized;

            footRay = new Ray(transform.position, movement);
            Debug.DrawRay(footRay.origin, footRay.direction * 0.5f, Color.red);
            if (!Physics.Raycast(footRay, out RaycastHit footHit, 0.5f) || hasJump)
            {
                rb.AddForce(movement * movementSpeed - horizontalVelocity, ForceMode.VelocityChange);
            }
        }
        else
        {
            // If the player isn't issuing movement, slow down to a stop
            rb.AddForce(-horizontalVelocity, ForceMode.VelocityChange);
        }
        if (jump)
        {
            jump = false;
            rb.AddForce(Vector3.up * jumpVelocity, ForceMode.VelocityChange);
            anim.SetTrigger("Jump");
        }
        anim.SetFloat("Forwards", movementZ);
        anim.SetFloat("Sideways", movementX);

        Ray cameraRay = new Ray(cameraAnchor.transform.position, -cameraAnchor.transform.forward);
        Debug.DrawRay(cameraRay.origin, cameraRay.direction * maxCameraDistance);
        if (Physics.Raycast(cameraRay, out RaycastHit cameraHit, maxCameraDistance))
        {
            playerCam.transform.localPosition = new Vector3(0, 0, -cameraHit.distance + minCameraDistance);
        }
        else
        {
            playerCam.transform.localPosition = new Vector3(0, 0, -maxCameraDistance);
        }
    }

    private void LateUpdate()
    {
        // Make the amera follow the player after both the camera's rotation and the player's position have been updated
        cameraAnchor.position = transform.position + Vector3.up;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            hasJump = true;
        }
    }
}
