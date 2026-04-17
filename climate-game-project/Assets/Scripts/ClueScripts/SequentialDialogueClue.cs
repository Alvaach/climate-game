using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Attach to the clue root GameObject.
// Shows dialogue lines one by one via Next button, then gives the player two choices.
// The chosen option spawns as a dialogue line, then after a delay the result spawns too.
// Sets isDone = true after the result line appears.
public class SequentialDialogueClue : ClueBase
{
    [Header("Dialogue Lines")]
    public string[] dialogueLines;

    [Header("UI References")]
    public Transform textContainer;
   
    
    public TMP_Text textLinePrefab; //prefab with all text info, font size etc
    public Button nextButton;

    [Header("Options")]
    public GameObject optionsContainer;
    public Button optionAButton;
    public Button optionBButton;

    public string optionALabel;

    public string optionBLabel;

    [Header("Option Results")]
    [TextArea] public string resultTextA;
    [TextArea] public string resultTextB;


    public float resultDelay = 1.5f;
    private int currentLine = 0;

    void Awake()
    {
        nextButton.onClick.AddListener(OnNextClicked);
        optionAButton.onClick.AddListener(() => OnOptionChosen(optionALabel, resultTextA));
        optionBButton.onClick.AddListener(() => OnOptionChosen(optionBLabel, resultTextB));
    }

    void Start()
    {
        TMP_Text labelA = optionAButton.GetComponentInChildren<TMP_Text>();
        if (labelA != null) labelA.text = optionALabel;

        TMP_Text labelB = optionBButton.GetComponentInChildren<TMP_Text>();
        if (labelB != null) labelB.text = optionBLabel;
    }

    public override void OnClueOpen()
    {
        isDone = false;
        currentLine = 0;

        foreach (Transform child in textContainer)
            Destroy(child.gameObject);

        optionsContainer.SetActive(false);
        nextButton.gameObject.SetActive(true);

        // Show the first line right away
        SpawnLine(dialogueLines[currentLine]);
        currentLine++;

        if (currentLine >= dialogueLines.Length)
        {
            nextButton.gameObject.SetActive(false);
            optionsContainer.SetActive(true);
        }
    }

    void OnNextClicked()
    {
        if (currentLine >= dialogueLines.Length) return;

        SpawnLine(dialogueLines[currentLine]);
        currentLine++;

        if (currentLine >= dialogueLines.Length)
        {
            nextButton.gameObject.SetActive(false);
            optionsContainer.SetActive(true);
        }
    }

    void OnOptionChosen(string choiceText, string resultText)
    {
        optionsContainer.SetActive(false);
        StartCoroutine(ShowOptionThenResult(choiceText, resultText));
    }

    IEnumerator ShowOptionThenResult(string choiceText, string resultText)
    {
        TMP_Text choiceLine = SpawnLine(choiceText);

        yield return new WaitForSeconds(resultDelay);

        // remove all lines except the chosen option
        foreach (Transform child in textContainer)
        {
            if (child != choiceLine.transform)
                Destroy(child.gameObject);
        }

        SpawnLine(resultText);

        isDone = true;
    }

    TMP_Text SpawnLine(string text)
    {
        TMP_Text newLine = Instantiate(textLinePrefab, textContainer);
        newLine.text = text;
        StartCoroutine(FadeIn(newLine));
        return newLine;
    }

    IEnumerator FadeIn(TMP_Text target)
    {
        Color c = target.color;
        c.a = 0f;
        target.color = c;

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed);
            target.color = c;
            yield return null;
        }

        c.a = 1f;
        target.color = c;
    }
}
