using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_Text errorMessage;

    private string _playerName;
    private float _playerScore;
    private string _currentPlayerName;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code
    
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public string PlayerName
    {
        get
        { 
            if(_playerName != null) 
                return _playerName;
            else 
                return "no name"; 
        }
    }

    public string CurrentPlayer
    {
        get { return _currentPlayerName; }
    }

    public float PlayerScore
    {
        get{ return _playerScore;}
    }

    bool SaveName()
    {
        if(!String.IsNullOrEmpty(nameField.text))
        {
            _currentPlayerName = nameField.text;
            return true;
        }

        return false;
    }

    public void StartGame()
    {
        if(SaveName())
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            StartCoroutine(ShowErrorMessage());
        }
    }

    IEnumerator ShowErrorMessage()
    {
        errorMessage.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        errorMessage.gameObject.SetActive(false);
    }

    public void SavePlayerData(float scoreValue)
    {
        SaveData data = new SaveData();
        data.score = scoreValue;
        data.name = _currentPlayerName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData loadData = JsonUtility.FromJson<SaveData>(json);

            _playerName = loadData.name;
            _playerScore = loadData.score;
        }
    }

    [System.Serializable]
    class SaveData
    {
        public string name;
        public float score;
    }

}