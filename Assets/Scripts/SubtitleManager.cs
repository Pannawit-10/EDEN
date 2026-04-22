using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleManager : MonoBehaviour
{
    public TMP_Text subtitleText; // ลากวัตถุ SubtitleText มาใส่ช่องนี้

    void Start()
    {
        subtitleText.text = ""; // เคลียร์ข้อความตอนเริ่มเกม
    }

    // ฟังก์ชันสำหรับสั่งให้ซับขึ้น
    public void DisplaySubtitle(string text, float duration)
    {
        StartCoroutine(ShowSubtitleRoutine(text, duration));
    }

    IEnumerator ShowSubtitleRoutine(string text, float duration)
    {
        subtitleText.text = text; // แสดงข้อความ
        yield return new WaitForSeconds(duration); // รอตามระยะเวลาที่กำหนด
        subtitleText.text = ""; // ลบข้อความ
    }
}