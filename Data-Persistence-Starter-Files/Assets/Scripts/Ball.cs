using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    [SerializeField] Paddle paddle;
    public float maxVelocity = 1.5f;

    private MainManager mainManager;

    void Start()
    {
        paddle.DuplicateBall += DuplicateBall;
        maxVelocity = 1.5f;
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        mainManager = GameObject.FindObjectOfType<MainManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "DeathZone")
        {
            paddle.DuplicateBall -= DuplicateBall;
            mainManager.DecreaseNumBall();
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        
        if(other.gameObject.tag == "ball")
        {
            m_Rigidbody.velocity = -m_Rigidbody.velocity;
            return;
        }
        var velocity = m_Rigidbody.velocity;
        
        //acelerarion
        velocity += velocity.normalized * 0.001f;
        
        
        // //check if we are not going totally vertically as this would lead to being stuck, we add a little vertical force
        // if (Vector3.Dot(velocity.normalized, Vector3.up) < 0.01f)
        // {
        //     velocity += velocity.y > 0 ? Vector3.up * 0.5f : Vector3.down * 0.5f;
        // }

        //max velocity
        if (velocity.magnitude > maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }

        velocity.Normalize();

        m_Rigidbody.velocity = velocity;
        AudioManager.instance.PlayClip("bounce");
    }

    void DuplicateBall()
    {
        mainManager.IncreaseNumBall();
        GameObject newBall = Instantiate(gameObject);
        Rigidbody newRb = newBall.GetComponent<Rigidbody>();
        newRb.velocity = -m_Rigidbody.velocity;
    }
}
