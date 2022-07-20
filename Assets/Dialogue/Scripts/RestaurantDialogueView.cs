using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using DG.Tweening;

[Serializable]
public class DialogueSettings
{
    public Vector2 Position;
    public bool FlipX;
    public bool FlipY;
    public GameObject Character;
}

public class RestaurantDialogueView : DialogueViewBase
{
    public static Action OnNewRestaurantDialogue;

    [SerializeField] private Utilities.SerializedDictionary<string, DialogueSettings> spawnPositions = new Utilities.SerializedDictionary<string, DialogueSettings>();
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Transform dialogueParent;
    [SerializeField] private float timeToAdvance = 5.0f;
    [SerializeField] private float timeToFade = 5.0f;

    private Coroutine _currentAnimation;

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        if(_currentAnimation != null)
        {
            StopCoroutine(_currentAnimation);
            _currentAnimation = null;
        }

        _currentAnimation = this.Tween(
            0.0f, 1.0f, timeToAdvance, (from, to, t) => Mathf.Lerp(from, to, t),
            () =>
            {
                OnNewRestaurantDialogue?.Invoke();
                DialogueSettings dialogueSettings = spawnPositions[dialogueLine.CharacterName];
                Transform box = Instantiate(dialogueBox, Vector3.zero, Quaternion.identity).transform;
                DialogueText dialogue = box.GetComponent<DialogueText>();
                dialogue.SetText(dialogueLine.TextWithoutCharacterName.Text);
                dialogue.SetSpriteFlip(dialogueSettings.FlipX, dialogueSettings.FlipY);
                box.SetParent(dialogueSettings.Character.transform);
                box.localPosition = dialogueSettings.Position;

                _currentAnimation = null;
                Debug.Log($"{dialogueLine.CharacterName} is speaking: {dialogueLine.TextWithoutCharacterName.Text}");
                onDialogueLineFinished();
            });
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
            Gizmos.DrawSphere(kvp.Value.Character.transform.position + new Vector3(kvp.Value.Position.x, kvp.Value.Position.y), 1f);
        }
    }
}
