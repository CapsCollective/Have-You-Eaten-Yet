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
    [SerializeField] private Transform tray, wrapperThrower, meatBucket;
    
    private bool _transitioning;
    
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

    [Button]
    public void DebugToMenu()
    {
        ToMenu(false);
    }

    [YarnCommand("ToMenu")]
    public void ToMenu(bool closed)
    {
        if (_transitioning) return;
        _transitioning = true;
        fade.DOFade(1, 1f).OnComplete(() =>
        {
            _transitioning = false;

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
        if (_transitioning) return;
        _transitioning = true;
        Night = night;
        WrapperThrower.MaxSpawnCount = night == 1 ? 1 : 3;
        WrapperThrower.FaultyThrowChance = night == 3 ? 0.3f : 0;
        WrapperThrower.MinDelay = night == 3 ? 4.0f : 2.0f;
        WrapperThrower.MaxDelay = night == 3 ? 7.0f : 4.0f;

        foreach (Transform t in wrapperThrower) Destroy(t.gameObject);
        foreach (Transform t in meatBucket) Destroy(t.gameObject);
        foreach (Transform t in tray) Destroy(t.GetComponent<DropTarget>().dropped);
        WrapperThrower.SpawnedWrappers = 0;
        Tray.PlacedDumplings = 0;

        fade.DOFade(1, 4f).OnComplete(() =>
        {
            _transitioning = false;
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
        if (_transitioning) return;
        _transitioning = true;
        
        NightSpriteToggle.OnSpriteToggle?.Invoke(true);
        
        // Disable all restaurant scenes before setting the new one up
        foreach(GameObject g in restaurantNights)
        {
            g.SetActive(false);
        }

        Night = night;
        fade.DOFade(1, 1f).OnComplete(() =>
        {
            _transitioning = false;
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
        if (_transitioning) return;
        _transitioning = true;
        fade.DOFade(1, 1f).OnComplete(() =>
        {
            _transitioning = false;
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
