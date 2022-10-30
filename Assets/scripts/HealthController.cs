using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthController : MonoBehaviour
{
    public int playerHealth;
    [SerializeField] private Image[] gears;
    // Start is called before the first frame update
    void Start()
    {
        UpdateHealth();
    }

    // Update is called once per frame
    public void UpdateHealth()
    {
        for(int i = 0; i < gears.Length; i++)
        {
            if(i < playerHealth)
            {
                gears[i].color = new Color32(91, 26, 0, 255);
                //Color.red;
            } else{
                gears[i].color = new Color32(0, 0, 0, 100);
                //Color.black;

            }
        }
        
    }
}
