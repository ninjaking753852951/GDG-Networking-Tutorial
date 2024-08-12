using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int[] scores;
    public List<Transform> spawnPoints;

    [Header("UI")]
    public TextMeshProUGUI scoreboard;
    
    const int playerCount = 2;
    const float resetMatchDelay = 2;
    List<GameObject> tanks;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        scores = new int[playerCount];
        tanks = GameObject.FindGameObjectsWithTag("Player").ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        string scoreboardText = "";

        for (int i = 0; i < scores.Length; i++)
        {
            scoreboardText += "Player " + i + ": " + scores[i] + "\n";
        }
        scoreboard.text = scoreboardText;
    }
    
    public void TankDestroyed(int tankID)
    {
        int winnerTankID = (int)Mathf.Repeat(tankID + 1, playerCount);
        
        if (winnerTankID > scores.Length - 1)
        {
            Debug.LogError("Tank ID does NOT match valid score entry");
            return;
        }

        scores[winnerTankID]++;
        Invoke(nameof(ResetMatch), resetMatchDelay);
    }

    public void ResetMatch()
    {
        for (int i = 0; i < tanks.Count; i++)
        {
            tanks[i].transform.position = spawnPoints[i].position;
            tanks[i].SetActive(true);
            Debug.Log(tanks[i].activeSelf);
        }
    }
}
