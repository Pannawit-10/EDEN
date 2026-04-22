using UnityEngine;
using System.Collections;

public class AwakeningController : MonoBehaviour
{
    [Header("ระบบไฟและเสียงตอนตื่น")]
    public Light roomLight;           // หลอดไฟในห้อง
    public AudioSource motherVoice;   // ลำโพงเสียง Mother

    [Header("เชื่อมต่อผู้เล่น (เอาไว้ปลดล็อคตอนตื่น)")]
    public PlayerController playerScript; // ลากวัตถุ Player มาใส่ช่องนี้ใน Unity

    void Start()
    {
        // ย้ำอีกรอบเพื่อความชัวร์ว่าตอนเริ่มเกม ไฟต้องดับอยู่
        if (roomLight != null) roomLight.enabled = false;

        // ล็อคผู้เล่นไว้ก่อนตั้งแต่เริ่มเกม (ตอนจอยังดำอยู่)
        if (playerScript != null) playerScript.canMove = false;
    }

    public void StartAwakeningSequence()
    {
        StartCoroutine(WakeUpRoutine());
    }

    IEnumerator WakeUpRoutine()
    {
        // 1. เอฟเฟกต์ไฟนีออนกะพริบ (Flicker)
        int flickerTimes = 6;
        for (int i = 0; i < flickerTimes; i++)
        {
            roomLight.enabled = !roomLight.enabled;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        }

        // บังคับให้ไฟเปิดค้างไว้ในตอนจบ
        roomLight.enabled = true;

        // 2. รอจังหวะแป๊บเดียว แล้วเปิดเสียง Mother
        yield return new WaitForSeconds(0.5f);
        if (motherVoice != null)
        {
            motherVoice.Play();
        }

        // 3. ปลดล็อคให้ Kael ขยับตัวและหันมองได้แล้ว!
        if (playerScript != null)
        {
            playerScript.canMove = true;
            Debug.Log("Kael ตื่นแล้ว! บังคับตัวละครได้");
        }
    }
}