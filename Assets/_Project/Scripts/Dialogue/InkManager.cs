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

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartStory(TextAsset inkJSON)
    {
        if (dialoguePanel.activeSelf)
            return;

        story = new Story(inkJSON.text);

        dialoguePanel.SetActive(true);
        IsInputBlocked = true;

        ContinueStory();
    }

    void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            // Se sta scrivendo allora completa subito
            if (isTyping) 
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = currentLine;
                isTyping = false;

                if (continueIndicator != null)
                    continueIndicator.SetActive(true);

                return;
            }

            ContinueStory();
        }
    }

    void ContinueStory()
    {
        if (story.canContinue)
        {
            currentLine = story.Continue();

            // Gestione speaker (TO, PINO)
            HandleSpeaker(ref currentLine);

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeLine(currentLine));
        }
        else
        {
            dialoguePanel.SetActive(false);

            if (continueIndicator != null)
                continueIndicator.SetActive(false);

            IsInputBlocked = false;
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

            // pausa su punteggiatura
            if (letter == '.' || letter == ',' || letter == '!' || letter == '?')
                yield return new WaitForSeconds(typingSpeed * 4);
            else
                yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        if (continueIndicator != null)
            continueIndicator.SetActive(true);
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

        // fallback
        nameText.text = "";
        portraitImage.enabled = false;
    }

    public bool IsDialogueActive()
    {
        return dialoguePanel.activeSelf;
    }
}