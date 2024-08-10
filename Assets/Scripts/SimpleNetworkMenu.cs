using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class SimpleNetworkMenu : MonoBehaviour
{
    NetworkManager networkManager;
    string ipAddress = "127.0.0.1";

    void Awake()
    {
        networkManager = FindObjectOfType<NetworkManager>();
    }

    void OnGUI()
    {
        if(networkManager.IsClient)
            return;
        
        // Set the GUI scale and style
        GUIStyle guiStyle = new GUIStyle(GUI.skin.button);
        guiStyle.fontSize = 20;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1.5f, 1.5f, 1));
        
        float buttonWidth = 200;
        float buttonHeight = 40;
        float padding = 10;
        float screenWidth = Screen.width / 1.5f; // Adjust for GUI matrix scaling
        float screenHeight = Screen.height / 1.5f; // Adjust for GUI matrix scaling

        float xPosition = padding; // Align to the left with padding
        float yPosition = screenHeight - (buttonHeight * 3) - (padding * 3); // Start from the bottom

        // Create a host game button in the bottom left
        if (GUI.Button(new Rect(xPosition, yPosition, buttonWidth, buttonHeight), "Host Game", guiStyle))
        {
            networkManager.StartHost();
        }

        // Create an input field below the host game button for the IP address
        ipAddress = GUI.TextField(new Rect(xPosition, yPosition + buttonHeight + padding, buttonWidth, buttonHeight), ipAddress, guiStyle);

        // Create a join game button below the input field
        if (GUI.Button(new Rect(xPosition, yPosition + (buttonHeight + padding) * 2, buttonWidth, buttonHeight), "Join Game", guiStyle))
        {
            networkManager.GetComponent<UnityTransport>().SetConnectionData(ipAddress, 7777);
            networkManager.StartClient();
        }
    }
}
