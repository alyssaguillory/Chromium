using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearSpawner : MonoBehaviour
{
    [SerializeField] GameObject GearMaster;
    [SerializeField] GameObject UICanvas;
    public List<GameObject> Gears = new List<GameObject>();
    Vector2 lastPosition = new Vector2(-70.0f, -70.0f);
    Vector2 addVector = new Vector2(150.0f, 0.0f);
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] float changeSpeed = 1.0f;
    Image currGear;
    private float timePassed;
    private float lastHealth;
    //GameObject[] Gears;
    // Start is called before the first frame update
    void Start()
    {
        //Gears.Add((GameObject)Instantiate(GearMaster, UICanvas.transform));
        //Gears[0].GetComponent<RectTransform>().anchorMin.Set(new Vector2())
    }

    // Update is called once per frame
    private void Update()
    {
        currGear = Gears[playerHealth.CurrBars-1].transform.GetChild(1).gameObject.GetComponent<Image>();
        slowHealth();
        
    }
    public void AddBar()
    {
        Gears.Add((GameObject)Instantiate(GearMaster, UICanvas.transform));
        Gears[Gears.Count - 1].GetComponent<RectTransform>().anchoredPosition = lastPosition + addVector;
        lastPosition += addVector;
        //redoHealth();
    }
    public void redoHealth()
    {
        
        for(int i = 0; i<playerHealth.CurrBars - 1; i++)
        {
            Gears[i].transform.GetChild(1).gameObject.GetComponent<Image>().fillAmount = 1;
        }
        for (int i = playerHealth.CurrBars; i < playerHealth.HealthBars; i++)
        {
            StartCoroutine(specialFill(Gears[i].transform.GetChild(1).gameObject.GetComponent<Image>()));
        }
    }
    public void slowHealth()
    {
        /*
        timePassed += Time.deltaTime;
        currGear.fillAmount = Mathf.Lerp(lastHealth, playerHealth.CurrHealth / playerHealth.MaxHealth, timePassed);
        */
        currGear.fillAmount = playerHealth.CurrHealth / playerHealth.MaxHealth;
    }
    IEnumerator specialFill(Image deadGear)
    {
        while(deadGear.fillAmount >= 0.05)
        {
            deadGear.fillAmount -= deadGear.fillAmount * changeSpeed * Time.deltaTime;
            yield return null;
        }
        deadGear.fillAmount = 0;
    }
}
