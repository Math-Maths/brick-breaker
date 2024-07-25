using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public Ball ball;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text textPlayerName;
    public Text textHighScore;
    public TMP_Text level;
    public GameObject instructionText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private int _bricksCount = 0;
    private int _levelCount = 0;
    
    private bool m_GameOver = false;
    private int m_ballCount = 1;

    [SerializeField] private Paddle paddle;

    public int NumOfBall { get { return m_ballCount; } set { m_ballCount = value; } }
   
    // Start is called before the first frame update
    void Start()
    {
        _levelCount = 1;
        level.text = "level " + _levelCount;
        DataManager.Instance.LoadPlayerData();
        AudioManager.instance.AmbientMusic(SceneManager.GetActiveScene().name);

        if(DataManager.Instance != null)
        {
            textPlayerName.text = "name: " + DataManager.Instance.CurrentPlayer;
            textHighScore.text = String.Format("best score: {0} name: {1}", DataManager.Instance.PlayerScore, DataManager.Instance.PlayerName);
        }
        else
        {
            textPlayerName.text = "name: no name";
            textHighScore.text = "best score: 0 name: no name";
        }

        SpawnBricks();
    }

    void SpawnBricks()
    {
        _bricksCount = 0;
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
                _bricksCount ++;
            }
        }

        ball.maxVelocity *= 2;
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                instructionText.SetActive(false);
                m_Started = true;
                float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void IncreaseNumBall()
    {
        m_ballCount ++;
    }

    public void DecreaseNumBall()
    {
        if(m_ballCount > 1)
        {
            m_ballCount --;
        }
        else if(m_ballCount <= 1)
        {
            m_ballCount --;
            GameOver();
        }
        
    }

    public bool GetGameState(string value)
    {
        Debug.Log(m_GameOver +" " + m_Started);

        if(value == "start")
            return m_Started;
        else if (value == "end")
            return m_GameOver;
        else
            return true;
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = m_Points.ToString();
        if(_bricksCount <= 1)
        {
            StartCoroutine(WinCoroutine());
        }
        else
        {
            _bricksCount --;
        }

    }


    public void GameOver()
    {
        paddle.StopPaddle();

        AudioManager.instance.PlayClip("lose");
        AudioManager.instance.AmbientMusic("end");
        if(m_Points > DataManager.Instance.PlayerScore)
            DataManager.Instance.SavePlayerData(m_Points);

        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    IEnumerator WinCoroutine()
    {
        AudioManager.instance.PlayClip("win");
        _levelCount ++;
        level.text = "level " + _levelCount;

        yield return new WaitForSeconds(1.5f);
        SpawnBricks();
    }
}
