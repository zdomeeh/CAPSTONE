using UnityEngine;
using Ink.Runtime;
using TMPro;

public class InkManager : MonoBehaviour
{
    public static InkManager Instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    public GameObject continueIndicator;

    private Story story;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartStory(TextAsset inkJSON)
    {
        if (dialoguePanel.activeSelf)
            return;

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

            // mostra indicatore
            continueIndicator.SetActive(true);
        }
        else
        {
            dialoguePanel.SetActive(false);

            // nasconde indicatore
            continueIndicator.SetActive(false);
        }
    }

    public bool IsDialogueActive()
    {
        return dialoguePanel.activeSelf;
    }
}