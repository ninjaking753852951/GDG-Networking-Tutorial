using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    NetworkList<int> scores;
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
        
        scores = new NetworkList<int>();
    }

    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += AddPlayerScore;
    }

    void AddPlayerScore(ulong @ulong)
    {
        if(IsServer)
            scores.Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(IsSpawned)
            UpdateUI();
    }

    void UpdateUI()
    {
        string scoreboardText = "";

        for (int i = 0; i < scores.Count; i++)
        {
            scoreboardText += "Player " + i + ": " + scores[i] + "\n";
        }
        scoreboard.text = scoreboardText;
    }
    
    public void TankDestroyed(int tankID)
    {
        int winnerTankID = (int)Mathf.Repeat(tankID + 1, playerCount);
        
        if (winnerTankID > scores.Count - 1)
        {
            Debug.LogError("Tank ID does NOT match valid score entry");
            return;
        }
        
        Debug.Log("hit " + winnerTankID);
        
        scores[winnerTankID]++;
        Invoke(nameof(ResetMatch), resetMatchDelay);
    }

    public void ResetMatch()
    {
        for (int i = 0; i < NetworkManager.Singleton.ConnectedClientsList.Count; i++)
        {
            TankController tank = NetworkManager.Singleton.ConnectedClientsList[i].PlayerObject.GetComponent<TankController>();
            tank.TeleportRPC(spawnPoints[i].position);
            tank.SetActiveRPC(true);
        }
    }
}
