using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;

[Serializable]
public class DialogueSettings
{
    public Vector2 Position;
    public bool FlipX;
    public bool FlipY;
}

public class RestaurantDialogueView : DialogueViewBase
{
    public static Action OnNewRestaurantDialogue;

    [SerializeField] private Utilities.SerializedDictionary<string, DialogueSettings> _spawnPositions = new Utilities.SerializedDictionary<string, DialogueSettings>();
    [SerializeField] private GameObject _dialogueBox;
    [SerializeField] private float _timeToAdvance = 5.0f;

    private Coroutine currentAnimation;

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        if(currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            currentAnimation = null;
        }

        currentAnimation = this.Tween(
            0.0f, 1.0f, _timeToAdvance, (from, to, t) => Mathf.Lerp(from, to, t),
            () =>
            {
                OnNewRestaurantDialogue?.Invoke();
                DialogueSettings dialogueSettings = _spawnPositions[dialogueLine.CharacterName];
                DialogueText newText = Instantiate(_dialogueBox, dialogueSettings.Position, Quaternion.identity).GetComponent<DialogueText>();
                newText.SetText(dialogueLine.TextWithoutCharacterName.Text);
                newText.SetSpriteFlip(dialogueSettings.FlipX, dialogueSettings.FlipY);

                currentAnimation = null;
                Debug.Log($"{dialogueLine.CharacterName} is speaking: {dialogueLine.TextWithoutCharacterName.Text}");
                onDialogueLineFinished();
            });
    }

    private void OnDrawGizmosSelected()
    {
        foreach (KeyValuePair<string, DialogueSettings> kvp in _spawnPositions) {
            Gizmos.DrawSphere(new Vector3(kvp.Value.Position.x, kvp.Value.Position.y), 1f);
        }
    }
}
