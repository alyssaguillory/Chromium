using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponBase[] primaryWeapons;
    public WeaponBase[] secondaryWeapons;
    private int activePrimary = 0;
    private int activeSecondary = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            activePrimary++;
            if (activePrimary > primaryWeapons.Length - 1)
            {
                activePrimary = 0;
            }
            SwapToP(activePrimary);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            activeSecondary++;
            if (activeSecondary > secondaryWeapons.Length - 1)
            {
                activeSecondary = 0;
            }
            SwapToP(activeSecondary);
        }
    }

    void SwapToP(int newActive)
    {
        primaryWeapons[activePrimary].GetComponent<WeaponBase>().Deactivate();
        primaryWeapons[newActive].GetComponent<WeaponBase>().Activate();
    }
    void SwapToS(int newActive)
    {
        secondaryWeapons[activeSecondary].Deactivate();
        secondaryWeapons[newActive].Activate();
    }
}
