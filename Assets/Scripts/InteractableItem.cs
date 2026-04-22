using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    [Header("ข้อมูลสิ่งของ")]
    public string itemName = "ไอเทมปริศนา"; // ชื่อของสิ่งชิ้นนี้

    // ฟังก์ชันนี้จะถูกเรียกเมื่อ Kael คลิกเมาส์ใส่
    public void Interact()
    {
        Debug.Log("Kael หยิบ: " + itemName);

        // ตรงนี้สามารถใส่เสียงตอนหยิบของได้ (ถ้ามี)

        // เมื่อหยิบแล้ว ให้ทำลายวัตถุนี้ทิ้งไปจากฉาก (เสมือนว่าเข้ากระเป๋าไปแล้ว)
        Destroy(gameObject);
    }
}