using UnityEngine;
using TMPro;
using Ink.Runtime;
using System.Collections;
using UnityEngine.UI;

public class InkManager : MonoBehaviour
{
    public static InkManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public Image portraitImage;
    public GameObject continueIndicator;

    [Header("Choices")]
    public GameObject choicesPanel;
    public GameObject choiceButtonPrefab;

    [Header("Typing")]
    public float typingSpeed = 0.02f;

    [System.Serializable]
    public class CharacterData
    {
        public string characterName;
        public Sprite portrait;
        public Color nameColor;
    }

    public CharacterData[] characters;

    public static bool IsInputBlocked = false;

    private Story story;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string currentLine;
    private bool choicesActive = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartStory(TextAsset inkJSON)
    {
        if (dialoguePanel.activeSelf)
            return;

        story = new Story(inkJSON.text);

        story.BindExternalFunction("RevealKey", () =>
        {
            FindObjectOfType<BedKeyInteractable>()?.RevealKey();
        });

        dialoguePanel.SetActive(true);
        IsInputBlocked = true;
        ClearChoices();

        ContinueStory();
    }

    void Update()
    {
        if (dialoguePanel == null)
            return;

        if (!dialoguePanel.activeSelf)
            return;

        if (choicesActive)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = currentLine;
                isTyping = false;

                if (continueIndicator != null)
                    continueIndicator.SetActive(true);

                if (story.currentChoices.Count > 0)
                    ShowChoices();

                return;
            }

            ContinueStory();
        }
    }

    void ContinueStory()
    {
        ClearChoices();

        if (story.canContinue)
        {
            currentLine = story.Continue();
            HandleSpeaker(ref currentLine);

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeLine(currentLine));
        }
        else if (story.currentChoices.Count > 0)
        {
            ShowChoices();
        }
        else
        {
            EndStory();
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        if (continueIndicator != null)
            continueIndicator.SetActive(false);

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;

            if (letter == '.' || letter == ',' || letter == '!' || letter == '?')
                yield return new WaitForSeconds(typingSpeed * 4);
            else
                yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        if (story.currentChoices.Count > 0)
        {
            ShowChoices();
        }
        else if (continueIndicator != null)
        {
            continueIndicator.SetActive(true);
        }
    }

    void ShowChoices()
    {
        choicesActive = true;

        if (continueIndicator != null)
            continueIndicator.SetActive(false);

        choicesPanel.SetActive(true);

        foreach (Choice choice in story.currentChoices)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choicesPanel.transform);

            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = choice.text;

            Button button = buttonObj.GetComponent<Button>();
            int choiceIndex = choice.index;

            button.onClick.AddListener(() =>
            {
                story.ChooseChoiceIndex(choiceIndex);
                choicesActive = false;
                ClearChoices();
                ContinueStory();
            });
        }
    }

    void ClearChoices()
    {
        if (choicesPanel == null)
            return;

        foreach (Transform child in choicesPanel.transform)
        {
            Destroy(child.gameObject);
        }

        choicesPanel.SetActive(false);
        choicesActive = false;
    }

    void EndStory()
    {
        dialoguePanel.SetActive(false);

        if (continueIndicator != null)
            continueIndicator.SetActive(false);

        ClearChoices();
        IsInputBlocked = false;
    }

    void HandleSpeaker(ref string line)
    {
        int index = line.IndexOf(":");

        if (index == -1)
        {
            nameText.text = "";
            portraitImage.enabled = false;
            return;
        }

        string speaker = line.Substring(0, index).Trim();
        string content = line.Substring(index + 1).Trim();

        line = content;

        foreach (var character in characters)
        {
            if (character.characterName.ToUpper() == speaker.ToUpper())
            {
                nameText.text = character.characterName;
                nameText.color = character.nameColor;
                portraitImage.sprite = character.portrait;
                portraitImage.enabled = true;
                return;
            }
        }

        nameText.text = "";
        portraitImage.enabled = false;
    }

    public bool IsDialogueActive()
    {
        if (dialoguePanel == null)
            return false;

        return dialoguePanel.activeSelf;
    }
}