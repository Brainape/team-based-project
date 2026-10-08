using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform enemy;
    public float xoffset;
    public float yofsset;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(enemy.position.x - xoffset, enemy.position.y - yofsset, transform.position.z);
        //follows the player but creates an offset so player can still be on screen since camera is following the enemy
    }
}
