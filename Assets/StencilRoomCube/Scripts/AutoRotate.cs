using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField]
    private Vector3 angles;

    void Update()
    {
        transform.Rotate(angles * Time.deltaTime);    
    }
}