using UnityEngine;
using System.Collections;
using System.Collections.Generic; // สำหรับใช้ List เก็บข้อความ

public class StoryManager : MonoBehaviour
{
    // คิวของไดอะล็อก (text: ข้อความ, duration: เวลา)
    [System.Serializable]
    public struct DialogueLine
    {
        public string text;
        public float duration;
    }

    public List<DialogueLine> dialogueSequence; // รายการไดอะล็อกทั้งหมดในคัดซีนนี้
    public SubtitleManager subtitleManager; // เชื่อมต่อกับระบบซับ

    private int currentDialogueIndex = 0; // ลำดับไดอะล็อกปัจจุบัน
    private bool isCutsceneActive = false; // ตรวจสอบว่าคัดซีนกำลังเล่นอยู่ไหม

    void Start()
    {
        // สามารถสั่งเริ่มคัดซีนจากตรงนี้ได้เลยสำหรับการทดสอบ
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        isCutsceneActive = true;

        // เล่นไดอะล็อกทีละข้อความจนครบใน List
        while (currentDialogueIndex < dialogueSequence.Count)
        {
            DialogueLine line = dialogueSequence[currentDialogueIndex];
            subtitleManager.DisplaySubtitle(line.text, line.duration); // สั่งซับขึ้น

            // รอจนกว่าซับจะหายไป (บวกเวลาเว้นจังหวะนิดหน่อย)
            yield return new WaitForSeconds(line.duration + 0.5f);
            currentDialogueIndex++;
        }

        EndCutscene(); // จบคัดซีนเมื่อเล่นครบ
    }

    // ฟังก์ชันที่จะเรียกใช้งานเมื่อกดปุ่ม Skip
    public void SkipToNextDialogue()
    {
        if (isCutsceneActive && currentDialogueIndex < dialogueSequence.Count - 1)
        {
            currentDialogueIndex++; // ข้ามไปไดอะล็อกถัดไปทันที
            StopAllCoroutines(); // หยุดการรอเวลาของข้อความเดิม
            StartCoroutine(PlayCutscene()); // เริ่มคัดซีนจากไดอะล็อกใหม่
        }
    }

    void EndCutscene()
    {
        isCutsceneActive = false;
        Debug.Log("คัดซีนจบลลงแล้ว, ผู้เล่นสามารถเริ่มสำรวจได้!");
        // ตรงนี้สามารถใส่คำสั่งเปิดระบบควบคุมตัวละคร, เสียง Mother หรือไฟในห้องได้
    }
}