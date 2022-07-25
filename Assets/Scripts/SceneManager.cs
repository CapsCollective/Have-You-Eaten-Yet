using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static Action OnDumplingsSceneLoad;
    public static Action OnRestaurantSceneLoad;

    public SpriteRenderer fade;
    public GameObject dumplings, restaurant, arrows, menu;

    private CameraPan _pan;

    private void Start()
    {
        _pan = GetComponent<CameraPan>();
        _pan.enabled = false;
    }

    [Button]
    void ToDumplings()
    {
        fade.DOFade(1, 1f).OnComplete(() =>
        {
            _pan.enabled = false;
            dumplings.SetActive(true);
            restaurant.SetActive(false);
            arrows.SetActive(false);
            menu.SetActive(false);
            transform.position = Vector3.back;
            fade.DOFade(0, 1f).OnComplete(() => OnDumplingsSceneLoad?.Invoke());
        });
    }

    [Button]
    void ToRestaurant()
    {
        fade.DOFade(1, 1f).OnComplete(() =>
        {
            dumplings.SetActive(false);
            restaurant.SetActive(true);
            arrows.SetActive(true);
            menu.SetActive(false);
            fade.DOFade(0, 1f).OnComplete(() => { 
                _pan.enabled = true;
                OnRestaurantSceneLoad?.Invoke();
            });
        });
    }
}
