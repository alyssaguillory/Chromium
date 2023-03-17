using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class OnHoverIntensity : MonoBehaviour
{
    private Light2D lightChanger;
    [SerializeField] float baseLight = 0.08f;
    [SerializeField] float hoverLight = 1.0f;
    [SerializeField] float checkRadius = 1.0f;
    public float mag;
    // Start is called before the first frame update
    void Start()
    {
        lightChanger = GetComponent<Light2D>();
    }
    private void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mag = (mousePosition - (Vector2)transform.position).magnitude;
        if ( mag <= checkRadius) { MouseIsOver(); }
        else { MouseExit(); }
    }
    private void MouseIsOver()
    {
        lightChanger.intensity = hoverLight;
    }
    private void MouseExit()
    {
        lightChanger.intensity = baseLight;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawSphere(transform.position, checkRadius);
    }
}
