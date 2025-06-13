using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Side { Israel, Palestine };

public class GameManager : MonoBehaviour
{
    // Bieżący wybór strony przez gracza
    public Side side;

    // Singleton – jedyny GameManager w grze
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Przejście do menu głównego
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Rozpoczęcie nowej gry (np. wybór postaci)
    public void NewGame()
    {
        SceneManager.LoadScene("CharacterSelection");
    }

    // Wyjście z gry
    public void ExitGame()
    {
        Application.Quit();
    }

    // Rozpoczęcie gry właściwej
    public void StartGame(Side s)
    {
        side = s;

        if (s == Side.Palestine)
        {
            SceneManager.LoadScene("SampleScene2");
        }
        else
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
