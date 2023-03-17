using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.Events;

public class OnHoverIntensity : MonoBehaviour
{
    private Light2D lightChanger;
    [SerializeField] float baseLight = 0.08f;
    [SerializeField] float hoverLight = 1.0f;
    [SerializeField] float checkRadius = 1.0f;
    public UnityEvent buttonClick;
    [SerializeField] AudioSource hoverSound;
    [SerializeField] AudioSource clickSound;
    private bool hover = false;
    // Start is called before the first frame update
    void Start()
    {
        lightChanger = GetComponent<Light2D>();
    }
    private void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if ((mousePosition - (Vector2)transform.position).magnitude <= checkRadius) { 
            MouseIsOver();
            if (Input.GetKeyDown(KeyCode.Mouse0)) { buttonClick.Invoke(); clickSound.Play(); }
        }
        else { MouseExit(); }
    }
    private void MouseIsOver()
    {
        lightChanger.intensity = hoverLight;
        if (!hover) { hoverSound.Play(); }
        hover = true;
    }
    private void MouseExit()
    {
        lightChanger.intensity = baseLight;
        hover = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawSphere(transform.position, checkRadius);
    }
}
