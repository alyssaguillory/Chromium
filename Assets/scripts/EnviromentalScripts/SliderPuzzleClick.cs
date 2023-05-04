using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPuzzleClick : MonoBehaviour
{
    public Puzzle master;
    public int tileNum;
    private void OnMouseDown()
    {
        master.Click(tileNum);
    }
}
