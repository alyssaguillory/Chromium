using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponBase[] primaryWeapons;
    public WeaponBase[] secondaryWeapons;
    private int activePrimary = 0;
    private int activeSecondary = 0;

    public Animator[] wheelList;
    public Animator[] weaponList;
    public GameObject selector;
    public float angleOffset = 1.0f;

    private int weapIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        foreach (WeaponBase weapon in primaryWeapons)
        {
            weapon.Deactivate();
        }
        primaryWeapons[0].GetComponent<WeaponBase>().Activate();
    }

    // Update is called once per frame
    void Update()
    {
        //While the q key is down, open the wheel and record the index, upon release swap to the weapon index
        if (Input.GetKey(KeyCode.Q))
        {
            weapIndex = WheelOpen();
        } else if (Input.GetKeyUp(KeyCode.Q)) { if (!(weapIndex > primaryWeapons.Length)) { SwapToP(weapIndex); } }
        //While the e key is down, open the wheel and record the index, upon release swap to the weapon index
        /*else if (Input.GetKey(KeyCode.E))
        {
            weapIndex = WheelOpen();
        } else if (Input.GetKeyUp(KeyCode.E)) { if (!(weapIndex > secondaryWeapons.Length)) { SwapToS(weapIndex); } }
        //Close the wheel on releas of all of them
        */
        else
        {
            foreach (Animator whel in wheelList) { whel.SetBool("swapping", false); }
            foreach (Animator whel in weaponList) { whel.SetBool("swapping", false); }
        }
    }

    void SwapToP(int newActive)
    {
        primaryWeapons[activePrimary].GetComponent<WeaponBase>().Deactivate();
        primaryWeapons[newActive].GetComponent<WeaponBase>().Activate();
        activePrimary = newActive;
    }
    void SwapToS(int newActive)
    {
        secondaryWeapons[activeSecondary].Deactivate();
        secondaryWeapons[newActive].Activate();
    }
    
    int WheelOpen()
    {
        //Activate Animations
        foreach (Animator whel in wheelList) { whel.SetBool("swapping", true); }
        foreach (Animator whel in weaponList) { whel.SetBool("swapping", true); }

        //Get the vector from one point to another
        Vector3 direction = Input.mousePosition - selector.transform.position;
        
        //Find the tangent angle of the vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
        
        //Rotate the selector Icon to that point
        selector.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
        //Determine selector Index
        int selectorIndex = (int)((-angle + 22.5) / 45);
        if (selectorIndex == -1) { selectorIndex = 6; }
        else if (angle >= 22.5 && angle <= 67.5) { selectorIndex = 7; }
        
        //Return selector
        return selectorIndex;
    }
}
