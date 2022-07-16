using TMPro;
using UnityEngine;

public class DialogueText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private float speed = 10;

    private bool _startFloat = false;

    private void Awake()
    {
        RestaurantDialogueView.OnNewRestaurantDialogue += OnNewRestaurantDialogue;
    }

    private void OnNewRestaurantDialogue()
    {
        if (_startFloat) return;
        _startFloat = true;
        GetComponent<Hover>().enabled = false;
    }

    public void SetSpriteFlip(bool x, bool y)
    {
        transform.localScale = new Vector3(x ? -1 : 1, y ? -1 : 1, 1);
        textMeshPro.transform.localScale = new Vector3(x ? -1 : 1, y ? -1 : 1, 1);
    }

    public void SetText(string text)
    {
        textMeshPro.text = text;
    }

    public void Update()
    {
        if(_startFloat)
            transform.Translate(Vector2.up * Time.deltaTime * speed);
    }
}
