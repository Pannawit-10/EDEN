using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;       // ความเร็วตอนเดินปกติ
    public float runSpeed = 10f;       // ความเร็วตอนวิ่ง
    public float mouseSensitivity = 600f; // ความไวของเมาส์
    public Transform playerCamera;     // ช่องสำหรับใส่กล้อง

    private CharacterController controller;
    private float xRotation = 0f;

    // ตัวแปรสำหรับแรงโน้มถ่วง
    private Vector3 velocity;
    private float gravity = -9.81f;

    void Start()
    {
        // ดึงคอมโพเนนต์ CharacterController ในตัว Player มาใช้งาน
        controller = GetComponent<CharacterController>();

        // ซ่อนเมาส์และล็อคเป้าไว้ตรงกลางจอ
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // ----- 1. ระบบหันมุมกล้อง (Mouse Look) -----
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // ล็อคมุมกล้อง

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // ----- 2. ระบบเดินและวิ่ง (Movement) -----
        float x = Input.GetAxis("Horizontal"); // ปุ่ม A, D
        float z = Input.GetAxis("Vertical");   // ปุ่ม W, S

        // เช็คว่าผู้เล่นกดปุ่ม Left Shift ค้างไว้หรือไม่
        float currentSpeed = walkSpeed; // ตั้งค่าเริ่มต้นเป็นความเร็วเดิน
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;    // ถ้ากด Shift ให้เปลี่ยนเป็นความเร็ววิ่ง
        }

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // ----- 3. ระบบแรงโน้มถ่วง (Gravity) -----
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}