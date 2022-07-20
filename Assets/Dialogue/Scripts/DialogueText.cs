using TMPro;
using DG.Tweening;
using UnityEngine;

public class DialogueText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private float moveTime = 10;
    [SerializeField] private float fadeTime = 1.5f;
    [SerializeField] private float sineStrength = 3.0f;
    [SerializeField] private float sineFrequency = 1.0f;

    private bool _startFloat = false;
    private float sine;
    private float startX;

    private void Awake()
    {
        RestaurantDialogueView.OnNewRestaurantDialogue += OnNewRestaurantDialogue;
    }

    private void Start()
    {
        //sine = Random.value;
    }

    private void OnNewRestaurantDialogue()
    {
        if (_startFloat) return;
        _startFloat = true;

        startX = transform.localPosition.x;
        transform.DOMoveY(35, moveTime).SetEase(Ease.InSine).OnUpdate(() =>
        {
            transform.localPosition = new Vector3(startX + (Mathf.Sin(sine) * sineStrength), transform.localPosition.y);
        });
        GetComponent<CanvasGroup>().DOFade(0, fadeTime);
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
            sine += Time.deltaTime * sineFrequency;
    }
}
