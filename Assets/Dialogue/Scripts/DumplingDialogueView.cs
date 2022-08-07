using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using DG.Tweening;

public class DumplingDialogueView : DialogueViewBase
{
    public static Action OnNewDumplingDialogue;

    [SerializeField] private SerializedDictionary<string, DialogueSettings> spawnPositions = new SerializedDictionary<string, DialogueSettings>();
    [SerializeField] private GameObject dialogueBoxPrefab;
    
    private const float timeToWait = 1.0f;
    private float timePerCharacter = 0.03f;
    private const float timeToFade = 2.0f;

    [SerializeField] private GameObject optionButton;
    [SerializeField] private Transform optionButtonParent;
    [SerializeField] private CanvasGroup optionButtonCanvasGroup;
    [SerializeField] private float timeToShowOptions = 1.0f;
    [SerializeField] private float timeToHideOptions = 1.0f;

    private Coroutine _currentAnimation;
    private Action<int> OnOptionSelected;

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        if (_currentAnimation != null)
        {
            StopCoroutine(_currentAnimation);
            _currentAnimation = null;
        }

        OnNewDumplingDialogue?.Invoke();
        DialogueSettings dialogueSettings = spawnPositions[dialogueLine.CharacterName];
        Transform box = Instantiate(dialogueBoxPrefab, transform).transform;
        DialogueText dialogue = box.GetComponent<DialogueText>();
        dialogue.Setup(this);
        dialogue.SetFont(dialogueSettings.Font);
        dialogue.SetText(dialogueLine.TextWithoutCharacterName.Text);
        dialogue.SetSpriteFlip(dialogueSettings.FlipX, dialogueSettings.FlipY);
        dialogue.HookAction(onDialogueLineFinished);
        
        if(dialogueSettings.Character != null) box.SetParent(dialogueSettings.Character.transform);
        box.localPosition = dialogueSettings.Position;
        box.GetComponentInChildren<Hover>().enabled = true;

        _currentAnimation = null;
        _currentAnimation = this.RunText(dialogueLine.TextWithoutCharacterName.Text, timePerCharacter, timeToWait, (i) =>
        {
            dialogue.RevealText(i);
        }, OnNewDumplingDialogue);
    }

    public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
    {
        // create new ones
        foreach(DialogueOption option in dialogueOptions)
        {
            CreateButton(option);
        }
        optionButtonCanvasGroup.DOFade(1, timeToShowOptions).OnComplete(() =>
        {
            optionButtonCanvasGroup.interactable = true;
        });

        OnOptionSelected = onOptionSelected;

        OptionButton CreateButton(DialogueOption option)
        {
            var button = Instantiate(optionButton, optionButtonParent).GetComponent<OptionButton>();
            button.SetupButton(option);
            button.transform.localScale = Vector3.one;
            button.OnOptionSelected = OptionWasSelected;

            return button;
        }
    }

    private void OptionWasSelected(int optionNum)
    {
        optionButtonCanvasGroup.DOFade(0, timeToHideOptions).OnComplete(() =>
        {
            OnOptionSelected?.Invoke(optionNum);
            // cleanup old options
            foreach(Transform t in optionButtonParent)
            {
                Destroy(t.gameObject);
            }
        });
    }
    public override void DialogueStarted()
    {
        base.DialogueStarted();
        foreach (KeyValuePair<string, DialogueSettings> kvp in spawnPositions)
        {
            if (!kvp.Value.Character) continue;
            kvp.Value.Character.transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(0.4f, timeToFade);
            kvp.Value.Character.GetComponent<SpriteRenderer>().DOFade(1, timeToFade);
        }
    }

    public override void DialogueComplete()
    {
        base.DialogueComplete();
        optionButtonCanvasGroup.interactable = false;
        
        foreach (KeyValuePair<string, DialogueSettings> kvp in spawnPositions)
        {
            if (!kvp.Value.Character) continue;
            kvp.Value.Character.transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(0, timeToFade);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        foreach (KeyValuePair<string, DialogueSettings> kvp in spawnPositions)
        {
            if(kvp.Value.Character != null)
                Gizmos.DrawSphere(kvp.Value.Character.transform.position + new Vector3(kvp.Value.Position.x, kvp.Value.Position.y), 1f);
            else
                Gizmos.DrawSphere(new Vector3(kvp.Value.Position.x, kvp.Value.Position.y), 1f);
        }
    }
}
