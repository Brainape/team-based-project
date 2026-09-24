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
    }
}
