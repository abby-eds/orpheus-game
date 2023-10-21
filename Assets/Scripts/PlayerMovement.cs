using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float rotateSpeed;
    public float jumpHeight;
    public Transform cameraAnchor;

    private Rigidbody rb;
    private float movementX;
    private float movementZ;
    private float rotationX;
    private float playerRotation;
    private float cameraRotation;
    private float targetRotation;
    private float rotationDelta;
    private bool jump;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rotationX = Input.GetAxis("Mouse X");
        cameraAnchor.Rotate(Vector3.up, rotationX);
        cameraRotation = cameraAnchor.eulerAngles.y;

        movementX = Input.GetAxisRaw("Horizontal");
        movementZ = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Vector3 velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (movementX != 0 || movementZ != 0)
        {
            targetRotation = (-(Mathf.Atan2(movementZ, movementX) * Mathf.Rad2Deg) + 90 + cameraRotation + 360) % 360;
            playerRotation = (transform.eulerAngles.y + 360) % 360;

            rotationDelta = targetRotation - playerRotation;
            if (rotationDelta > 0)
            {
                if (rotationDelta > 180) rotationDelta -= 360;
            }
            else
            {
                if (rotationDelta < -180) rotationDelta += 360;
            }
            if (rotationDelta > rotateSpeed) rotationDelta = rotateSpeed;
            else if (rotationDelta < -rotateSpeed) rotationDelta = -rotateSpeed;

            transform.Rotate(Vector3.up, rotationDelta);

            rb.AddForce(transform.forward * movementSpeed - velocity, ForceMode.VelocityChange);
        }
        else
        {
            rb.AddForce(-velocity, ForceMode.VelocityChange);
        }
    }

    private void LateUpdate()
    {
        cameraAnchor.position = transform.position;
    }
}
