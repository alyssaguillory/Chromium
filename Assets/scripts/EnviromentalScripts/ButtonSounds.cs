using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource AudioS;
    public AudioClip HoverSound;
    public AudioClip ClickSound;
    public void OnHover()
    {
        AudioS.PlayOneShot(HoverSound);
    }
    public void OnClick()
    {
        AudioS.PlayOneShot(ClickSound);
    }
}
