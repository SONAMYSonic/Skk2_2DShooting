using UnityEngine;
public class Test : MonoBehaviour
{
    private int Speed = 3;
    public int _ang = 1;
    void Update()
    {
        transform.position += _MovePosition();
        Debug.Log("dfdaf" + Speed);
    }
    private Vector3 _MovePosition()
    {
        return Vector3.up * Time.deltaTime * Speed;
    }
}