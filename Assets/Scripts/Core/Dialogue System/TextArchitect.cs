using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DIALOGUE;

public class TextArchitect
{
    private TextMeshProUGUI tmproUGUI;
    private TextMeshPro tmproText;

    public TMP_Text tmpAsset => tmproUGUI != null ? tmproUGUI : tmproText;

    public string currentText => tmpAsset.text;
    public string targetText { get; private set; } = "";
    public string preText { get; private set; } = "";

    private int preTextLength = 0;

    public string fullTargetText => preText + " " + targetText;

    public enum TypeMethod { INSTANT, TYPEWRITER, FADE }
    public TypeMethod typeMethod = TypeMethod.TYPEWRITER;

    public Color textColor { get { return tmpAsset.color; } set { tmpAsset.color = value; } }

    public float Speed { get { return baseSpeed * speedMultiplier; } set { speedMultiplier = value; } }
    private const float baseSpeed = 1;
    private float speedMultiplier = 1;

    public int characterPerCycle { get { return Speed <= 2f ? characterMultiplier : Speed <= 2.5f ? characterMultiplier * 2 : characterMultiplier * 3; } }
    public int characterMultiplier = 1;

    public bool textSkip = false;

    /// <summary>
    /// Initialize TextArchitect with TEXTMESHPROUGUI Parameter
    /// </summary>
    /// <param name="tmpUI"></param>
    public TextArchitect(TextMeshProUGUI tmpUI)
    {
        this.tmproUGUI = tmpUI;

    }

    /// <summary>
    /// Initialize TextArchitect with TextMeshPro asset
    /// </summary>
    /// <param name="tmproText"></param>
    public TextArchitect(TextMeshPro tmproText)
    {
        this.tmproText = tmproText;
    }

    /// <summary>
    /// Type out text without any pretext
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public Coroutine Type(string text)
    {
        preText = "";
        targetText = text;

        Stop();
        typeProcess = tmpAsset.StartCoroutine(Typing());
        return typeProcess;
    }

    /// <summary>
    /// Append text to the end of current text asset
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public Coroutine AppendType(string text)
    {
        preText = tmpAsset.text + " ";
        targetText = text;

        Stop();
        typeProcess = tmpAsset.StartCoroutine(Typing());
        return typeProcess;
    }

    public Coroutine typeProcess = null;
    public bool isTyping => typeProcess != null;

    /// <summary>
    /// Stop the current typing process and end the coroutine
    /// </summary>
    public void Stop()
    {
        if (!isTyping)
            return;

        tmpAsset.StopCoroutine(typeProcess);
        typeProcess = null;
    }

    /// <summary>
    /// Begin Typing text 
    /// </summary>
    /// <returns></returns>
    IEnumerator Typing()
    {
        Prepare();

        switch (typeMethod)
        {
            case TypeMethod.TYPEWRITER:
                yield return TypeWriter();
                break;
            case TypeMethod.FADE:
                yield return Fade();
                break;
            default:
                break;
        }

        OnComplete();
    }

    private void Prepare()
    {
        switch (typeMethod)
        {
            case TypeMethod.INSTANT:
                PrepareInstant();
                break;
            case TypeMethod.TYPEWRITER:
                PrepareTypewriter();
                break;
            case TypeMethod.FADE:
                PrepareFade();
                break;
        }
    }

    public void OnComplete()
    {
        typeProcess = null;
        textSkip = false;
    }

    public void ForceComplete()
    {
        switch (typeMethod)
        {
            case TypeMethod.TYPEWRITER:
                tmpAsset.maxVisibleCharacters = tmpAsset.textInfo.characterCount;
                break;
            case TypeMethod.FADE:
                break;
            default:
                break;
        }
    }

    private void PrepareInstant()
    {
        tmpAsset.color = tmpAsset.color;
        tmpAsset.text = fullTargetText;
        tmpAsset.ForceMeshUpdate(); //Forces the textmesh to update with correct colors
        tmpAsset.maxVisibleCharacters = tmpAsset.textInfo.characterCount; //Forces the textmesh to make sure all characters are on screen

    }

    private void PrepareTypewriter()
    {
        tmpAsset.color = tmpAsset.color;
        tmpAsset.maxVisibleCharacters = 0;
        tmpAsset.text = preText;

        if (preText != "")
        {
            tmpAsset.ForceMeshUpdate();
            tmpAsset.maxVisibleCharacters = tmpAsset.textInfo.characterCount;
        }

        tmpAsset.text += targetText;
        tmpAsset.ForceMeshUpdate();
    }

    private void PrepareFade()
    {

    }

    private IEnumerator TypeWriter()
    {
        while (tmpAsset.maxVisibleCharacters < tmpAsset.textInfo.characterCount)
        {
            tmpAsset.maxVisibleCharacters += textSkip ? characterPerCycle * 5 : characterPerCycle;

            yield return new WaitForSeconds(0.015f / Speed);
        }
    }

    private IEnumerator Fade()
    {
        yield return null;
    }

}
