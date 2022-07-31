using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using DG.Tweening;
using TMPro;

[Serializable]
public class DialogueSettings
{
    public Vector2 Position;
    public bool FlipX;
    public bool FlipY;
    public GameObject Character;
    public TMP_FontAsset Font;
}

public class RestaurantDialogueView : DialogueViewBase
{
    public Action OnNewRestaurantDialogue;

    [SerializeField] private SerializedDictionary<string, DialogueSettings> spawnPositions = new SerializedDictionary<string, DialogueSettings>();
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private float timeToWait = 3.0f;
    [SerializeField] private float timePerCharacter = 0.15f;
    [SerializeField] private float timeToFade = 5.0f;

    private Coroutine _currentAnimation;

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        if(_currentAnimation != null)
        {
            StopCoroutine(_currentAnimation);
            _currentAnimation = null;
        }

        OnNewRestaurantDialogue?.Invoke();
        DialogueSettings dialogueSettings = spawnPositions[$"{dialogueLine.CharacterName}_{SceneManager.Night}"];
        Transform box = Instantiate(dialogueBox, transform).transform;
        DialogueText dialogue = box.GetComponent<DialogueText>();
        dialogue.Setup(this);
        dialogue.SetFont(dialogueSettings.Font);
        dialogue.SetText(dialogueLine.TextWithoutCharacterName.Text);
        dialogue.SetSpriteFlip(dialogueSettings.FlipX, dialogueSettings.FlipY);
        box.SetParent(dialogueSettings.Character.transform);
        box.localPosition = dialogueSettings.Position;

        _currentAnimation = null;
        _currentAnimation = this.RunText(
            dialogueLine.TextWithoutCharacterName.Text,
            timePerCharacter,
            timeToWait,
            i =>  dialogue.RevealText(i),
            onDialogueLineFinished
        );
    }

    public override void DialogueComplete()
    {
        base.DialogueComplete();

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
        foreach (KeyValuePair<string, DialogueSettings> kvp in spawnPositions) {
            if (kvp.Value.Character != null)
            {
                Gizmos.DrawSphere(kvp.Value.Character.transform.position + new Vector3(kvp.Value.Position.x, kvp.Value.Position.y), 1f);
            }
        }
    }
}
