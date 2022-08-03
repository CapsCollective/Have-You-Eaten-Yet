using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Yarn.Unity;

public class SceneManager : MonoBehaviour
{
    public static Action<int> OnDumplingsSceneLoad;
    public static Action<int> OnRestaurantSceneLoad;
    public static Action OnEpilogueSceneLoad;
    public static int Night = 1;
    
    public SpriteRenderer fade;
    public GameObject dumplings, restaurant, epilogue, arrows, menu, openSign, closedSign;

    private CameraPan _pan;

    [SerializeField] private int debugNight = 1;
    [SerializeField] private List<GameObject> restaurantNights = new List<GameObject>();

    private void Start()
    {
        _pan = GetComponent<CameraPan>();
        _pan.enabled = false;
    }

    [Button]
    public void DebugToRestaurant()
    {
        ToRestaurant(debugNight);
    }

    [Button]
    public void DebugToDumplings()
    {
        ToDumplings(debugNight);
    }


    [YarnCommand("ToMenu")]
    public void ToMenu(bool closed)
    {
        fade.DOFade(1, 1f).OnComplete(() =>
        {
            _pan.enabled = false;
            dumplings.SetActive(false);
            restaurant.SetActive(false);
            epilogue.SetActive(false);
            arrows.SetActive(false);
            menu.SetActive(true);
            
            openSign.SetActive(!closed);
            closedSign.SetActive(closed);
            
            transform.position = Vector3.back;
            fade.DOFade(0, 1f);
        });
    }
    
    [YarnCommand("ToDumplings")]
    public void ToDumplings(int night)
    {
        Night = night;
        WrapperThrower.MaxSpawnCount = night == 1 ? 1 : 3;
        WrapperThrower.FaultyThrowChance = night == 3 ? 0.3f : 0;
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

    [YarnCommand("ToRestaurant")]
    public void ToRestaurant(int night)
    {
        // Disable all restaurant scenes before setting the new one up
        foreach(GameObject g in restaurantNights)
        {
            g.SetActive(false);
        }

        Night = night;
        fade.DOFade(1, 1f).OnComplete(() =>
        {
            dumplings.SetActive(false);
            restaurant.SetActive(true);
            restaurantNights[Night - 1].SetActive(true);
            arrows.SetActive(true);
            menu.SetActive(false);
            fade.DOFade(0, 1f).OnComplete(() => { 
                _pan.enabled = true;
                OnRestaurantSceneLoad?.Invoke(Night);
            });
        });
    }
    
    [YarnCommand("ToEpilogue")]
    public void ToEpilogue()
    {
        fade.DOFade(1, 1f).OnComplete(() =>
        {
            _pan.enabled = false;
            dumplings.SetActive(false);
            restaurant.SetActive(false);
            epilogue.SetActive(true);
            arrows.SetActive(false);
            menu.SetActive(false);
            fade.DOFade(0, 1f).OnComplete(() => {
                OnEpilogueSceneLoad?.Invoke();
                //Services.DialogueStarter.StartTutorialDialogue();
            });
        });
    }
}
