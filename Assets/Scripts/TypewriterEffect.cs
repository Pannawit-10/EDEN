using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    [Header("การตั้งค่าบทบรรยาย")]
    [TextArea(3, 5)]
    public string[] sentences;
    public float typeSpeed = 0.05f;
    public float timeToStay = 2.0f;

    [Header("การตั้งค่าเสียงบรรยาย (Voice Over)")]
    public AudioSource voiceSource;    // ลำโพงสำหรับเสียงคนพูด
    public AudioClip[] voiceOvers;     // ไฟล์เสียงพูด (เรียงตามลำดับข้อความ)

    [Header("การตั้งค่าเสียงพิมพ์ (SFX)")]
    public AudioSource sfxSource;      // ลำโพงสำหรับเสียงพิมพ์ดีด
    public AudioClip typingSound;

    private TMP_Text textComponent;
    private int index = 0;
    private bool isTyping = false;
    private bool skipRequested = false;

    void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
        textComponent.text = "";
    }

    void Start()
    {
        StartCoroutine(PlayStorySequence());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            skipRequested = true;
        }
    }

    IEnumerator PlayStorySequence()
    {
        for (index = 0; index < sentences.Length; index++)
        {
            skipRequested = false;

            // --- 1. เล่นเสียงบรรยายของประโยคปัจจุบัน (ถ้ามี) ---
            if (voiceSource != null && voiceOvers.Length > index && voiceOvers[index] != null)
            {
                voiceSource.Stop(); // หยุดเสียงประโยคเก่า (เผื่อพูดยังไม่จบ)
                voiceSource.clip = voiceOvers[index];
                voiceSource.Play();
            }

            // 2. เริ่มพิมพ์ประโยค
            yield return StartCoroutine(TypeSentence(sentences[index]));

            // 3. รอเวลาให้อ่าน (กดข้ามได้)
            float waitTimer = 0f;
            while (waitTimer < timeToStay && !skipRequested)
            {
                waitTimer += Time.deltaTime;
                yield return null;
            }

            // --- 4. ถ้าผู้เล่นกดข้าม ให้สั่งหยุดเสียงคนพูดทันที ---
            if (skipRequested && voiceSource != null)
            {
                voiceSource.Stop();
            }

            // ล้างหน้าจอก่อนขึ้นประโยคใหม่
            textComponent.text = "";
            yield return new WaitForSeconds(0.3f);
        }

        // เมื่อจบทุกประโยค ให้เฟดจอออก
        StartCoroutine(FadeOutCanvas());
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        textComponent.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            if (skipRequested)
            {
                textComponent.text = sentence;
                break;
            }

            textComponent.text += letter;

            // เล่นเสียงพิมพ์ดีด (ใช้ PlayOneShot เพื่อให้เสียงซ้อนกันได้)
            if (sfxSource != null && typingSound != null && letter != ' ')
            {
                sfxSource.PlayOneShot(typingSound);
            }

            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
        skipRequested = false;
    }

    IEnumerator FadeOutCanvas()
    {
        CanvasGroup group = transform.parent.GetComponent<CanvasGroup>();
        if (group == null) group = transform.parent.gameObject.AddComponent<CanvasGroup>();

        float duration = 2.0f;
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            group.alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            yield return null;
        }

        transform.parent.gameObject.SetActive(false);
        Debug.Log("เริ่มเกม!");
    }
}