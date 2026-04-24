using UnityEngine;
using Ink.Runtime;
using TMPro;

public class InkManager : MonoBehaviour
{
    public static InkManager Instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    private Story story;

    void Awake()
    {
        Instance = this;
    }

    public void StartStory(TextAsset inkJSON)
    {
        story = new Story(inkJSON.text);
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            ContinueStory();
        }
    }

    void ContinueStory()
    {
        if (story.canContinue)
        {
            dialogueText.text = story.Continue();
        }
        else
        {
            dialoguePanel.SetActive(false);
        }
    }
}