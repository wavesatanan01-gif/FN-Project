using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float sensitivity = 180f;
    public Transform playerBody;

    float rotationX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotationX -= mouseY;

        // เงยได้เยอะ ก้มได้นิดเดียว
        rotationX = Mathf.Clamp(rotationX, -80f, 30f);

        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

    }
}

