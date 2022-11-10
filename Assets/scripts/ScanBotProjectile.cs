using UnityEngine;

public class ScanBotProjectile : MonoBehaviour
{
    public float Speed = 2.5f;

    public void Update()
    {
        transform.position += transform.right * Time.deltaTime * Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
