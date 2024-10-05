using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    private void Update()
    {
        Vector3 rotationVector = Time.deltaTime * _rotateSpeed * Vector3.up;
        transform.Rotate(rotationVector);
    }
}
