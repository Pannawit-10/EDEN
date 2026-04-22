using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("การตั้งค่าระบบหยิบของ")]
    public float interactRange = 3f;  // ระยะแขนที่เอื้อมหยิบถึง (3 เมตร)
    public Camera playerCamera;       // ตาของ Kael (กล้อง)

    // เราต้องเชื่อมกุญแจ canMove มาด้วย เพื่อไม่ให้คลิกของได้ตอนที่จอยังดำอยู่
    public PlayerController playerController;

    void Update()
    {
        // ตรวจสอบว่าตัวละครขยับได้แล้วหรือยัง (ตื่นหรือยัง) ถ้ายังไม่ตื่น ห้ามหยิบของ
        if (playerController != null && !playerController.canMove) return;

        // ถ้ากด "คลิกซ้าย" (เบอร์ 0)
        if (Input.GetMouseButtonDown(0))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        // สร้างเส้นเลเซอร์พุ่งออกจากจุดกึ่งกลางกล้อง ไปด้านหน้า
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hitInfo; // ตัวแปรสำหรับเก็บข้อมูลว่าเลเซอร์ไปชนอะไร

        // ยิงเลเซอร์ออกไป! ถ้าชนอะไรสักอย่างในระยะ interactRange...
        if (Physics.Raycast(ray, out hitInfo, interactRange))
        {
            // เช็คว่าสิ่งที่ชน มีสคริปต์ InteractableItem แปะอยู่ไหม?
            InteractableItem item = hitInfo.collider.GetComponent<InteractableItem>();

            if (item != null)
            {
                // ถ้ามีของชิ้นนั้นอยู่ ให้สั่งทำงานฟังก์ชัน Interact() ทันที
                item.Interact();
            }
        }
    }
}