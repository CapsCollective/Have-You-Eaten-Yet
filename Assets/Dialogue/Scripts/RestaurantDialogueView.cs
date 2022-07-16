using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

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

    [SerializeField] private Utilities.SerializedDictionary<string, DialogueSettings> spawnPositions = new Utilities.SerializedDictionary<string, DialogueSettings>();
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Transform dialogueParent;
    [SerializeField] private float timeToAdvance = 5.0f;

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
                Transform box = Instantiate(dialogueBox, dialogueSettings.Position, Quaternion.identity).transform;
                box.SetParent(dialogueParent);
                DialogueText dialogue = box.GetComponent<DialogueText>();
                dialogue.SetText(dialogueLine.TextWithoutCharacterName.Text);
                dialogue.SetSpriteFlip(dialogueSettings.FlipX, dialogueSettings.FlipY);

                _currentAnimation = null;
                Debug.Log($"{dialogueLine.CharacterName} is speaking: {dialogueLine.TextWithoutCharacterName.Text}");
                onDialogueLineFinished();
            });
    }

    private void OnDrawGizmosSelected()
    {
        foreach (KeyValuePair<string, DialogueSettings> kvp in spawnPositions) {
            Gizmos.DrawSphere(new Vector3(kvp.Value.Position.x, kvp.Value.Position.y), 1f);
        }
    }
}
