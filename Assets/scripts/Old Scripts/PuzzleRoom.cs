using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class PuzzleRoom : MonoBehaviour
{
    public TextMeshProUGUI Text;

    public Color[] colors = {Color.magenta,Color.green,Color.red,Color.blue};
    public bool magenta_hit = false;
    public bool green_hit = false; 
    public bool red_hit = false; 
    public bool blue_hit = false; 
    public string levelName;

    // Start is called before the first frame update
    void Start()
    {
        Text = FindObjectOfType<TextMeshProUGUI>();
        StartCoroutine(WaitBeforeShow());
        
    }

    // Update is called once per frame
    void Update()
    {   
        
       
    }
    
    private IEnumerator WaitBeforeShow()
    {
        for (int i = 1; i < 5; i++){
            Text.text = i+"";
            Text.color = colors[i-1];
            yield return new WaitForSeconds(5);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "magenta"){
            magenta_hit = true; 
           
            Debug.Log("magenta");
        }
        else if(collision.tag == "green" && magenta_hit == true && red_hit == false && blue_hit == false)
        {
            Debug.Log("green");
            green_hit = true; 
        }
        else if(collision.tag == "red" && magenta_hit == true && green_hit == true && blue_hit == false)
        {
            Debug.Log("red");
            red_hit = true; 
        }
        else if(collision.tag == "blue" && magenta_hit == true && green_hit == true && red_hit == true)
        {
            Debug.Log("blue");
            SceneManager.LoadScene(levelName);
            blue_hit = true; 
        }
    }
}
