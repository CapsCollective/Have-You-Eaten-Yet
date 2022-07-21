using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Yarn.Unity;

public class OptionButton : Selectable, ISubmitHandler, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI buttonText;
    public Action<int> OnOptionSelected;

    private DialogueOption Option;

    public void OnPointerClick(PointerEventData eventData)
    {
        SubmitOption();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        SubmitOption();
    }

    public void SubmitOption()
    {
        OnOptionSelected?.Invoke(Option.DialogueOptionID);
    }

    public void SetupButton(DialogueOption option)
    {
        Option = option;
        buttonText.text = option.Line.Text.Text;
    }
}
