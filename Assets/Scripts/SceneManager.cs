using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static Action<int> OnDumplingsSceneLoad;
    public static Action<int> OnRestaurantSceneLoad;
    public static int Night = 1; 
    
    public SpriteRenderer fade;
    public GameObject dumplings, restaurant, arrows, menu;

    private CameraPan _pan;

    private void Start()
    {
        _pan = GetComponent<CameraPan>();
        _pan.enabled = false;
    }

    [Button]
    public void ToDumplings(int night)
    {
        Night = night;
        fade.DOFade(1, 1f).OnComplete(() =>
        {
            _pan.enabled = false;
            dumplings.SetActive(true);
            restaurant.SetActive(false);
            arrows.SetActive(false);
            menu.SetActive(false);
            transform.position = Vector3.back;
            fade.DOFade(0, 1f).OnComplete(() => OnDumplingsSceneLoad?.Invoke(Night));
        });
    }

    [Button]
    public void ToRestaurant(int night)
    {
        Night = night;
        fade.DOFade(1, 1f).OnComplete(() =>
        {
            dumplings.SetActive(false);
            restaurant.SetActive(true);
            arrows.SetActive(true);
            menu.SetActive(false);
            fade.DOFade(0, 1f).OnComplete(() => { 
                _pan.enabled = true;
                OnRestaurantSceneLoad?.Invoke(Night);
            });
        });
    }
}
