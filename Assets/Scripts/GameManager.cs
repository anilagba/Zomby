using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : Simpleton<GameManager>
{
    public TowerController tower;

    public Transform zombyTarget;
    public Transform zombyDogTarget;

    public PlayerController[] players;
    List<PlayerController> playerList = new List<PlayerController>();

    public ZombyController[] zombies;
    List<ZombyController> zombyList = new List<ZombyController>();

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Button settingsButton;
    [SerializeField] CanvasRenderer settingsPanel;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        settingsButton.onClick.AddListener(PauseGame);
    }

    private void Update()
    {
        int second = Mathf.FloorToInt(Time.timeSinceLevelLoad) % 60;
        int minute = Mathf.FloorToInt(Time.timeSinceLevelLoad) / 60;
        int hour = Mathf.FloorToInt(Time.timeSinceLevelLoad) / 3600;

        timerText.text = hour.ToString() + ":" + minute.ToString() + ":" + second.ToString();
    }



    public void AddNewZombyToList(ZombyController zomby)
    {
        zombyList.Add(zomby);
        zombies = zombyList.ToArray();
    }


    public void RemoveZombyFromList(ZombyController zomby)
    {
        zombyList.Remove(zomby);
        zombies = zombyList.ToArray();
    }


    public void AddNewPlayerToTheList(PlayerController newPlayer)
    {
        playerList.Add(newPlayer);
        players = playerList.ToArray();
    }


    void PauseGame()
    {
        Time.timeScale = 0;
        settingsPanel.gameObject.SetActive(true);
    }
}