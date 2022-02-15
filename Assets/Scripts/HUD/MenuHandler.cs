﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class MenuHandler : MonoBehaviour
{
    
    
    Canvas canvas;
    CanvasGroup canvasGroupData;


    public MenuHandler parentMenu { get; private set; }
    public MenuHandler root { get; private set; }
    MenuHandler[] children;



    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasGroupData = GetComponent<CanvasGroup>();
        Debug.Log(name + ", " + canvasGroupData);

        canvasGroupData.ignoreParentGroups = true;

        parentMenu = null;
        // If the object has a parent, check for a higher layer MenuHandler using GetComponentInParent
        if (transform.parent != null)
        {
            // This object runs this function on the parent transform so it doesn't include itself.
            parentMenu = transform.parent.GetComponentInParent<MenuHandler>();
        }
        
        if (parentMenu == null) // If this menu doesn't have a parent, it is the root.
        {
            root = this;
            // Reference the root in all children at the start so the calculations are only performed once.
            children = GetComponentsInChildren<MenuHandler>(true);
            for (int i = 0; i < children.Length; i++)
            {
                children[i].root = this;
            }
        }
    }

    public void SetWindowActiveState(bool active)
    {
        //Debug.Log(name + ", " + canvasGroupData + ", " + enabled);
        canvasGroupData.alpha = active ? 1 : 0; // Essentially says "set to one if active, otherwise set to zero"
        canvasGroupData.interactable = active;
        canvasGroupData.blocksRaycasts = active;
    }

    public void SwitchMenu(MenuHandler newMenu)
    {
        newMenu.gameObject.SetActive(true);

        for(int i = 0; i < root.children.Length; i++)
        {
            root.children[i].SetWindowActiveState(false);
        }

        newMenu.SetWindowActiveState(true);
    }

    public void ReturnToParentMenu()
    {
        SwitchMenu(parentMenu);
    }

    public void ReturnToTopLayer()
    {
        SwitchMenu(root);
    }

    public void LoadSceneSimply(string name)
    {
        //SceneManager.LoadScene(name);
        LoadingScreen.LoadScene(name);
    }

    public void ResetTime(float scale)
    {
        Time.timeScale = scale;
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
