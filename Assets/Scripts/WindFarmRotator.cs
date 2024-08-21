using UnityEngine;

public class WindFarmRotator : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 15f;

    private void Update()
    {
        float _angle = transform.eulerAngles.z;
        _angle += rotationSpeed * Time.deltaTime;

        transform.localEulerAngles = new Vector3(0, 0, _angle); 
    }
}
