using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using DG.Tweening;

public class DumplingDialogueView : DialogueViewBase
{
    public static Action OnNewRestaurantDialogue;

    [SerializeField] private Utilities.SerializedDictionary<string, DialogueSettings> spawnPositions = new Utilities.SerializedDictionary<string, DialogueSettings>();
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Transform dialogueParent;
    [SerializeField] private float timeToWait = 3.0f;
    [SerializeField] private float timePerCharacter = 0.15f;
    [SerializeField] private float timeToFade = 5.0f;
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

        OnNewRestaurantDialogue?.Invoke();
        DialogueSettings dialogueSettings = spawnPositions[dialogueLine.CharacterName];
        Transform box = Instantiate(dialogueBox, Vector3.zero, Quaternion.identity).transform;
        DialogueText dialogue = box.GetComponent<DialogueText>();
        dialogue.SetText(dialogueLine.TextWithoutCharacterName.Text);
        dialogue.SetSpriteFlip(dialogueSettings.FlipX, dialogueSettings.FlipY);
        if(dialogueSettings.Character != null)
            box.SetParent(dialogueSettings.Character.transform);
        box.localPosition = dialogueSettings.Position;

        _currentAnimation = null;
        _currentAnimation = this.RunText(dialogueLine.TextWithoutCharacterName.Text, timePerCharacter, timeToWait, (i) =>
        {
            dialogue.RevealText(i);
        },
        () => onDialogueLineFinished());
    }

    public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
    {
        // cleanup old options
        foreach(Transform t in optionButtonParent.transform)
        {
            Destroy(t.gameObject);
        }

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
            var button = Instantiate(optionButton).GetComponent<OptionButton>();
            button.SetupButton(option);
            button.transform.SetParent(optionButtonParent);
            button.transform.localScale = Vector3.one;
            button.OnOptionSelected = OptionWasSelected;

            return button;
        }
    }

    void OptionWasSelected(int optionNum)
    {
        optionButtonCanvasGroup.DOFade(0, timeToHideOptions).OnComplete(() =>
        {
            OnOptionSelected?.Invoke(optionNum);
        });
    }

    public override void DialogueComplete()
    {
        base.DialogueComplete();

        optionButtonCanvasGroup.interactable = false;
        foreach (KeyValuePair<string, DialogueSettings> kvp in spawnPositions)
        {
            kvp.Value.Character.GetComponent<SpriteRenderer>().DOFade(0, timeToFade).OnComplete(() =>
            {
                OnNewRestaurantDialogue?.Invoke();
            });
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
