using UnityEngine;
using UnityEngine.PlayerLoop;

public class PowerUpBehaviour : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }
}