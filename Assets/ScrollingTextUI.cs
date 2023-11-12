using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollingTextUI : MonoBehaviour
{
    public TextMeshProUGUI chapterTextUI;
    public ScrollRect scrollRect;
    public float scrollSpeed = 20f;
    private bool isAutoScrolling = true;

    public ChapterData[] chapters;

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            isAutoScrolling = false;
        }

        if (isAutoScrolling)
        {
            if (scrollRect.verticalNormalizedPosition > 0)
            {
                scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime;
            }
        }
    }

    private void OnEnable()
    {
        int chapterIndex = GameManager.Instance.currentChapterIndex;
        if (chapterIndex >= 0 && chapterIndex < chapters.Length)
        {
            DisplayChapter(chapters[chapterIndex]);
        }
    }

    public void DisplayChapter(ChapterData chapter)
    {
        chapterTextUI.text = chapter.chapterText;
        scrollRect.verticalNormalizedPosition = 1;
        isAutoScrolling = true;
    }
}
