using UnityEngine;

public class FeetEnemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float enemySpeed;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * enemySpeed * Time.deltaTime);
    }
}
