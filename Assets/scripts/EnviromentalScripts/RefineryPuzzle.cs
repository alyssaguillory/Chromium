using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class RefineryPuzzle : MonoBehaviour
{
    [SerializeField] GameObject Bar1;
    [SerializeField] GameObject Marker1;
    [SerializeField] GameObject Bar2;
    [SerializeField] GameObject Marker2;
    [SerializeField] OnHoverIntensity[] buttons;
    public float weight = 50;
    public int multiplier = 1;
    public float speed = 3;
    [SerializeField] AudioSource errorSound;
    [SerializeField] AudioSource increasePower;
    [SerializeField] AudioSource decreasePower;
    [SerializeField] AudioSource lever;
    [SerializeField] AudioSource engineStart;
    [SerializeField] AudioSource engineFail;
    [SerializeField] AnimationCurve colorCurve;
    [SerializeField] GameObject door;
    [SerializeField] Sprite openDoor;
    // Start is called before the first frame update
    void Start()
    {
        UpdateMarkers();
        Marker1.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.green, colorCurve.Evaluate(Bar1.transform.localScale.y / 30));
        Marker2.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.green, colorCurve.Evaluate(Bar2.transform.localScale.y / 30));
        Bar1.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.green, colorCurve.Evaluate(Bar1.transform.localScale.y / 30));
        Bar2.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.green, colorCurve.Evaluate(Bar2.transform.localScale.y / 30));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
     * When button 1 is pressed, the weight decreases
     * When button 2 is pressed, the weight incrases
     * Weight of 50 has them be exactly the same
     * Weight of 0 has the left bar be full
     * Weight of 100 has the right bar be full
     *      
     */
    private void UpdateMarkers()
    {
        StartCoroutine(UpdateBar1(Bar1));
        StartCoroutine(UpdateBar2(Bar2));
        /*
         * Length = 2*(1-weight|weight)/100
         * Max of 1
         * When the buttons are pressed, runs a method to change the lengths and colors
         */
    }
    public IEnumerator UpdateBar1(GameObject bar)
    {
        while (bar.transform.localScale.y != (30 * (100 - weight) / 100)) {
            bar.transform.localScale = new Vector2 (bar.transform.localScale.x, Mathf.MoveTowards(bar.transform.localScale.y, 30 * ((100 - weight) / 100), Time.deltaTime*speed));
            bar.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.green, colorCurve.Evaluate(bar.transform.localScale.y/30));
            Marker1.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.green, colorCurve.Evaluate(bar.transform.localScale.y / 30));
            Marker1.GetComponent<Light2D>().color = Color.Lerp(Color.red, Color.green, colorCurve.Evaluate(bar.transform.localScale.y / 30));
            yield return null;
        }
        //change bar size
        //change bar color based on current size
        //change marker color
        yield return null;
    }
    public IEnumerator UpdateBar2(GameObject bar)
    {
        while (bar.transform.localScale.y != (30 * (weight) / 100))
        {
            bar.transform.localScale = new Vector2(bar.transform.localScale.x, Mathf.MoveTowards(bar.transform.localScale.y, 30 * (weight / 100), Time.deltaTime * speed));
            bar.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.green, colorCurve.Evaluate(bar.transform.localScale.y / 30));
            Marker2.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.green, colorCurve.Evaluate(bar.transform.localScale.y / 30));
            Marker2.GetComponent<Light2D>().color = Color.Lerp(Color.red, Color.green, colorCurve.Evaluate(bar.transform.localScale.y / 30));
            yield return null;
        }
        //change bar size
        //change bar color based on current size
        //change marker color
        yield return null;
    }

    public void Button1()
    {
        if(weight > 1*multiplier) { weight = weight - 1*multiplier; increasePower.Play(); }
        else { errorSound.Play(); }
        UpdateMarkers();
    }
    public void Button2()
    {
        if (weight < 100 - 1 * multiplier) { weight = weight + 1 * multiplier; decreasePower.Play(); }
        else { errorSound.Play(); }
        UpdateMarkers();
    }
    public void Switch()
    {
        lever.Play();
        if(weight == 50) { engineStart.Play();
            door.GetComponent<SpriteRenderer>().sprite = openDoor;
            door.GetComponent<BoxCollider2D>().enabled = false;
            foreach(OnHoverIntensity button in buttons)
            {
                button.enabled = false;
            }
            gameObject.GetComponent<RefineryPuzzle>().enabled = false;
        }
        else { engineFail.Play(); }
        UpdateMarkers();
    }
}
