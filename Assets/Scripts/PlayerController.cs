using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("สถานะผู้เล่น")]
    public bool canMove = false;       // กุญแจล็อค! ถ้าเป็น false จะขยับไม่ได้

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float mouseSensitivity = 600f;
    public Transform playerCamera;

    private CharacterController controller;
    private float xRotation = 0f;

    // ตัวแปรสำหรับแรงโน้มถ่วง
    private Vector3 velocity;
    private float gravity = -9.81f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // ซ่อนเมาส์และล็อคเป้าไว้ตรงกลางจอ
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // ----- ระบบแรงโน้มถ่วง (Gravity) ทำงานตลอดเวลาเผื่อตัวละครตกจากที่สูง -----
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // *ถ้า canMove เป็น false ให้หยุดการทำงานตรงนี้เลย (หันหน้าและเดินไม่ได้)*
        if (!canMove) return;

        // ----- 1. ระบบหันมุมกล้อง (Mouse Look) -----
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // ----- 2. ระบบเดินและวิ่ง (Movement) -----
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float currentSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }
}