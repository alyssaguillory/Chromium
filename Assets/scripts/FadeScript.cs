using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private bool fadeIn;
    [SerializeField] private bool fadeOut;

    public void ShowUI()
    {
        fadeIn = true;
    }

    public void HideUI()
    {
        fadeOut = true;
    }

    private void Update()
    {
        if(fadeIn)
        {
            if(cg.alpha < 1)
            {
                cg.alpha += Time.deltaTime;
                if (cg.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }
        
    }
}
