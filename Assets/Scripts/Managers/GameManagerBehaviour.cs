using Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerBehaviour : MonoBehaviour
{
    [SerializeField]
    private InputBehaviour _player1;
    [SerializeField]
    private InputBehaviour _player2;
    [SerializeField]
    private GameObject _endScreen;
    private HealthBehaviour _player1Health;
    private HealthBehaviour _player2Health;

    // Start is called before the first frame update
    void Start()
    {
        _player1Health = _player1.GetComponent<HealthBehaviour>();
        _player2Health = _player2.GetComponent<HealthBehaviour>();

        _player1Health.AddOnDeathAction(() => _endScreen.SetActive(true));
        _player2Health.AddOnDeathAction(() => _endScreen.SetActive(true));
    }

    public void RestartMatch()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
