using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_Text errorMessage;
    public string playerName;

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

    bool SaveName()
    {
        if(!String.IsNullOrEmpty(nameField.text))
        {
            playerName = nameField.text;
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

}