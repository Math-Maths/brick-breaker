using System;
using System.Collections;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    float MaxMovement = 2.0f;

    [SerializeField] float Speed = 2.0f;
    [SerializeField] Transform levelSizeReference;
    [SerializeField] MainManager mainManager;

    public Action DuplicateBall;

    private bool isGameRunning = true;
    private Vector3 initialSize;

    void Start()
    {
        initialSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            DuplicateBall();
        }

        if(isGameRunning)
        {
            float input = Input.GetAxis("Horizontal");

            Vector3 pos = transform.position;
            pos.x += input * Speed * Time.deltaTime;

            MaxMovement = (levelSizeReference.localScale.x /2) - (gameObject.transform.localScale.x /2);

            if (pos.x > MaxMovement)
                pos.x = MaxMovement;
            else if (pos.x < -MaxMovement)
                pos.x = -MaxMovement;

            transform.position = pos;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(mainManager.GetGameState("end") == false && mainManager.GetGameState("start") == true)
        {
            if(other.gameObject.tag == "powerup-ball")
            {
                DuplicateBall();
            }
            else if(other.gameObject.tag == "powerup-size")
            {
                StopCoroutine(PowerUpSize());
                StartCoroutine(PowerUpSize());
            }
            else if(other.gameObject.tag == "powerup-speed")
            {
                Speed *= 1.5f;
            }
            Destroy(other.gameObject);
        }

    }

    IEnumerator PowerUpSize()
    {
        transform.localScale = new Vector3(transform.localScale.x * 2f, transform.localScale.y, transform.localScale.z);
        yield return new WaitForSeconds(5f);
        transform.localScale = initialSize;
    }

    public void StopPaddle()
    {
        isGameRunning = false;
    }
}
