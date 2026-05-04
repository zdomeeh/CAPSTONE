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

    [Header("Final Scene")]
    public Image finalImage;
    public float fadeSpeed = 2f;

    public System.Action OnStoryEnd;

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

    public bool IsWaitingForExternalAction { get; private set; } = false;

    void Awake()
    {
        // Controlla se esiste gia' un altro InkManager
        if (Instance != null && Instance != this)
        {
            // Elimina questo oggetto per evitare duplicati
            Destroy(gameObject);
            return;
        }

        // Imposta questo oggetto come istanza principale
        Instance = this;

        // Mantiene questo oggetto anche quando cambia scena
        DontDestroyOnLoad(gameObject);
    }

    public void StartStory(TextAsset inkJSON)
    {
        // Se il pannello dei dialoghi e' gia' attivo, non avvia un nuovo dialogo
        if (dialoguePanel.activeSelf)
            return;

        // Crea una nuova storia usando il file Ink
        story = new Story(inkJSON.text);

        // Collega la funzione Ink alla funzione che mostra la chiave
        story.BindExternalFunction("RevealKey", () =>
        {
            FindObjectOfType<BedKeyInteractable>()?.RevealKey();
        });

        // Collega la funzione Ink alla funzione che mostra la lettera
        story.BindExternalFunction("RevealLetter", () =>
        {
            FindObjectOfType<PaintingLetterInteractable>()?.RevealLetter();
        });

        // Collega la funzione Ink alla funzione che sposta la carriola
        story.BindExternalFunction("MoveWheelbarrow", () =>
        {
            // Mette in pausa il dialogo durante l'azione esterna
            PauseForExternalAction();
            FindObjectOfType<WheelbarrowInteractable>()?.MoveWheelbarrow();
        });

        // Collega la funzione Ink alla funzione che mostra l'immagine finale
        story.BindExternalFunction("ShowFinalImage", () =>
        {
            ShowFinalImage();
        });

        // Collega la funzione Ink alla funzione che nasconde l'immagine finale
        story.BindExternalFunction("HideFinalImage", () =>
        {
            HideFinalImage();
        });

        // Mostra il pannello del dialogo
        dialoguePanel.SetActive(true);

        // Blocca gli input del giocatore
        IsInputBlocked = true;

        // Rimuove eventuali scelte precedenti
        ClearChoices();

        // Continua la storia
        ContinueStory();
    }

    void Update()
    {
        // Se il pannello non esiste, esce dal metodo
        if (dialoguePanel == null)
            return;

        // Se il dialogo non e' attivo, esce dal metodo
        if (!dialoguePanel.activeSelf)
            return;

        // Se sta aspettando un'azione esterna, blocca il proseguimento
        if (IsWaitingForExternalAction)
            return;

        // Se ci sono scelte attive, blocca il click per continuare
        if (choicesActive)
            return;

        // Controlla se il giocatore clicca con il mouse
        if (Input.GetMouseButtonDown(0))
        {
            // Se il testo sta ancora comparendo
            if (isTyping)
            {
                // Ferma la scrittura graduale
                StopCoroutine(typingCoroutine);

                // Mostra subito tutta la frase
                dialogueText.text = currentLine;
                isTyping = false;

                // Mostra l'indicatore per continuare
                if (continueIndicator != null)
                    continueIndicator.SetActive(true);

                // Se ci sono scelte, le mostra
                if (story.currentChoices.Count > 0)
                    ShowChoices();

                return;
            }

            // Passa alla parte successiva del dialogo
            ContinueStory();
        }
    }

    void ContinueStory()
    {
        // Pulisce le scelte precedenti
        ClearChoices();

        // Se la storia puo' continuare
        if (story.canContinue)
        {
            // Prende la prossima riga del dialogo
            currentLine = story.Continue();

            // Gestisce il nome e il ritratto del personaggio
            HandleSpeaker(ref currentLine);

            // Ferma una scrittura precedente, se esiste
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            // Avvia la scrittura graduale della riga
            typingCoroutine = StartCoroutine(TypeLine(currentLine));
        }
        // Se ci sono scelte disponibili, le mostra
        else if (story.currentChoices.Count > 0)
        {
            ShowChoices();
        }
        // Se non c'e' altro da mostrare, termina il dialogo
        else
        {
            EndStory();
        }
    }

    IEnumerator TypeLine(string line)
    {
        // Indica che il testo sta venendo scritto
        isTyping = true;
        dialogueText.text = "";

        // Nasconde l'indicatore mentre il testo viene scritto
        if (continueIndicator != null)
            continueIndicator.SetActive(false);

        // Scrive una lettera alla volta
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;

            // Fa una pausa piu' lunga dopo la punteggiatura
            if (letter == '.' || letter == ',' || letter == '!' || letter == '?')
                yield return new WaitForSeconds(typingSpeed * 4);
            else
                yield return new WaitForSeconds(typingSpeed);
        }

        // Indica che la scrittura e' finita
        isTyping = false;

        // Se ci sono scelte, le mostra
        if (story.currentChoices.Count > 0)
        {
            ShowChoices();
        }
        // Altrimenti mostra l'indicatore per continuare
        else if (continueIndicator != null)
        {
            continueIndicator.SetActive(true);
        }
    }

    void ShowChoices()
    {
        // Indica che le scelte sono attive
        choicesActive = true;

        // Nasconde l'indicatore per continuare
        if (continueIndicator != null)
            continueIndicator.SetActive(false);

        // Mostra il pannello delle scelte
        choicesPanel.SetActive(true);

        // Crea un bottone per ogni scelta disponibile
        foreach (Choice choice in story.currentChoices)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choicesPanel.transform);

            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = choice.text;

            buttonText.alignment = TextAlignmentOptions.Center;
            buttonText.enableWordWrapping = true;

            Button button = buttonObj.GetComponent<Button>();
            int choiceIndex = choice.index;

            // Quando il bottone viene premuto, seleziona quella scelta
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
        // Se il pannello delle scelte non esiste, esce dal metodo
        if (choicesPanel == null)
            return;

        // Elimina tutti i bottoni delle scelte
        foreach (Transform child in choicesPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Nasconde il pannello delle scelte
        choicesPanel.SetActive(false);

        // Indica che non ci sono scelte attive
        choicesActive = false;
    }

    void EndStory()
    {
        // Nasconde il pannello del dialogo
        dialoguePanel.SetActive(false);

        // Nasconde l'indicatore per continuare
        if (continueIndicator != null)
            continueIndicator.SetActive(false);

        // Pulisce le scelte
        ClearChoices();

        // Sblocca gli input del giocatore
        IsInputBlocked = false;

        // Esegue eventuali azioni collegate alla fine della storia
        OnStoryEnd?.Invoke();

        // Rimuove le azioni collegate alla fine della storia
        OnStoryEnd = null;
    }

    void HandleSpeaker(ref string line)
    {
        // Cerca i due punti per capire se c'e' un nome prima della frase
        int index = line.IndexOf(":");

        // Se non trova un nome, nasconde nome e ritratto
        if (index == -1)
        {
            nameText.text = "";
            portraitImage.enabled = false;
            return;
        }

        string speaker = line.Substring(0, index).Trim();
        string content = line.Substring(index + 1).Trim();

        // Lascia solo il testo del dialogo
        line = content;

        // Cerca il personaggio corrispondente
        foreach (var character in characters)
        {
            if (character.characterName.ToUpper() == speaker.ToUpper())
            {
                // Mostra nome, colore e ritratto del personaggio
                nameText.text = character.characterName;
                nameText.color = character.nameColor;
                portraitImage.sprite = character.portrait;
                portraitImage.enabled = true;
                return;
            }
        }

        // Se il personaggio non viene trovato, nasconde nome e ritratto
        nameText.text = "";
        portraitImage.enabled = false;
    }

    public bool IsDialogueActive()
    {
        // Se il pannello non esiste, il dialogo non e' attivo
        if (dialoguePanel == null)
            return false;

        // Restituisce se il pannello del dialogo e' attivo
        return dialoguePanel.activeSelf;
    }

    public void PauseForExternalAction()
    {
        // Mette in pausa il dialogo
        IsWaitingForExternalAction = true;
    }

    public void ResumeAfterExternalAction()
    {
        // Toglie la pausa al dialogo
        IsWaitingForExternalAction = false;

        // Continua la storia
        ContinueStory();
    }

    public void ShowFinalImage()
    {
        // Se l'immagine finale non esiste, esce dal metodo
        if (finalImage == null) return;

        // Ferma tutte le coroutine attive
        StopAllCoroutines();

        // Fa comparire l'immagine finale
        StartCoroutine(FadeImage(1f));
    }

    public void HideFinalImage()
    {
        // Se l'immagine finale non esiste, esce dal metodo
        if (finalImage == null) return;

        // Ferma tutte le coroutine attive
        StopAllCoroutines();

        // Fa scomparire l'immagine finale
        StartCoroutine(FadeImage(0f));
    }

    IEnumerator FadeImage(float targetAlpha)
    {
        float startAlpha = finalImage.color.a;
        float t = 0f;

        // Cambia gradualmente la trasparenza dell'immagine
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            finalImage.color = new Color(1f, 1f, 1f, alpha);

            yield return null;
        }

        // Imposta la trasparenza finale precisa
        finalImage.color = new Color(1f, 1f, 1f, targetAlpha);
    }
}