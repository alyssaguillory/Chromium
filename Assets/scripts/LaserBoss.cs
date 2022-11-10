using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBoss : MonoBehaviour
{
    [SerializeField] private float defDistanceRay = 100; 
    [SerializeField] private HealthController _healthController;
    private Rigidbody2D rb2D;
    public Transform laserFirePoint; 
    public LineRenderer m_LineRenderer; 
    Transform m_transform; 
    Vector2 startPos;
    Vector2 endPos; 
    public GameObject Laser;
    public GameObject Laser2;
    public GameObject Laser3;
    private int boxCount = 0; 
    
    void Start(){

    
     rb2D = GetComponent<Rigidbody2D>();
    }

   
    void Awake()
    {
        m_transform =  GetComponent<Transform>();
        
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
   {
        if(collision.tag == "fuse1")
        {
            
            Laser.SetActive(true); 
            Debug.Log("hit");

        }else if(collision.tag == "fuse2")
        {
            Laser2.SetActive(true);

        }else if (collision.tag == "fuse3")
        {
            Laser3.SetActive(true); 
        }
        

   }
    private void Update() {
         ShootLaser(); 
      
    }
    
    
    void ShootLaser()
    {
        RaycastHit2D hit = Physics2D.Raycast(m_transform.position,transform.right);
        m_LineRenderer.SetPosition(0,laserFirePoint.position);
        m_LineRenderer.SetPosition(1,laserFirePoint.position);
        
        /*if(hit) 
        {
            m_LineRenderer.SetPosition(1,hit.point);
            _healthController.playerHealth -= 1; 
        }
        else{
            m_LineRenderer.SetPosition(1,transform.right*100);
        }*/
        m_LineRenderer.SetPosition(1,transform.right*100);
        
    }
    
    
    void Draw2DRay(Vector2 startPosPoint, Vector2 endPos ) {
        m_LineRenderer.SetPosition(0, startPos);
        m_LineRenderer.SetPosition(1, endPos); 

    }
    

    // Start is called before the first frame update
    
}

