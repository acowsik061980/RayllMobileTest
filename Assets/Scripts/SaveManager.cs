using Firebase.Analytics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private GameManager gameMgr;

    private PlayerData m_PlayerData;
    private string m_SaveSlot1Path;
    private string m_SaveSlot2Path;
    private string m_SaveSlot3Path;

    public bool isSaveLoading = false;

    private void Start()
    {
        m_PlayerData = new PlayerData();
        m_PlayerData.playerPosn = Vector3.zero;
        m_PlayerData.collectedCoinIndexes = gameMgr.coinIndexesCollected;

        m_SaveSlot1Path = Application.persistentDataPath + "/SaveSlot1.json";
        m_SaveSlot2Path = Application.persistentDataPath + "/SaveSlot2.json";
        m_SaveSlot3Path = Application.persistentDataPath + "/SaveSlot3.json";
    }

    public void SavePlayerData(PlayerData saveData, int slotIndex)
    {
        string savePlayerData = JsonUtility.ToJson(saveData);
        switch (slotIndex)
        {
            case 0:
                File.WriteAllText(m_SaveSlot1Path, savePlayerData);
                break;

            case 1:
                File.WriteAllText(m_SaveSlot2Path, savePlayerData);
                break;

            case 2:
                File.WriteAllText(m_SaveSlot3Path, savePlayerData);
                break;

            default:
                break;
        }
    }

    public void LoadPlayerData(int slotIndex)
    {
        isSaveLoading = true;
        switch (slotIndex)
        {
            case 0:
                if (File.Exists(m_SaveSlot1Path))
                {
                    string loadPlayerData = File.ReadAllText(m_SaveSlot1Path);
                    m_PlayerData = JsonUtility.FromJson<PlayerData>(loadPlayerData);
                    gameMgr.SetGameStateFromSavedData(m_PlayerData);
                    FirebaseAnalytics.LogEvent("GameLoad", "loadSlotNumber", slotIndex + 1);
                    Debug.Log("Player data loaded successfully from save slot " + (slotIndex + 1));
                }
                else
                {
                    Debug.Log("Player data not found for save slot " + (slotIndex + 1) + ". Starting new game");
                    gameMgr.InitGame();
                }

                gameMgr.UIMgr.ShowMainMenu(false);
                isSaveLoading = false;
                break;

            case 1:
                if (File.Exists(m_SaveSlot2Path))
                {
                    string loadPlayerData = File.ReadAllText(m_SaveSlot2Path);
                    m_PlayerData = JsonUtility.FromJson<PlayerData>(loadPlayerData);
                    gameMgr.SetGameStateFromSavedData(m_PlayerData);
                    FirebaseAnalytics.LogEvent("GameLoad", "loadSlotNumber", slotIndex + 1);
                    Debug.Log("Player data loaded successfully from save slot " + (slotIndex + 1));
                }
                else
                {
                    Debug.Log("Player data not found for save slot " + (slotIndex + 1) + ". Starting new game");
                    gameMgr.InitGame();
                }

                gameMgr.UIMgr.ShowMainMenu(false);
                isSaveLoading = false;
                break;

            case 2:
                if (File.Exists(m_SaveSlot3Path))
                {
                    string loadPlayerData = File.ReadAllText(m_SaveSlot3Path);
                    m_PlayerData = JsonUtility.FromJson<PlayerData>(loadPlayerData);
                    gameMgr.SetGameStateFromSavedData(m_PlayerData);
                    FirebaseAnalytics.LogEvent("GameLoad", "loadSlotNumber", slotIndex + 1);
                    Debug.Log("Player data loaded successfully from save slot " + (slotIndex + 1));
                }
                else
                {
                    Debug.Log("Player data not found for save slot " + (slotIndex + 1) + ". Starting new game");
                    gameMgr.InitGame();
                }

                gameMgr.UIMgr.ShowMainMenu(false);
                isSaveLoading = false;
                break;

            default:
                break;
        }
    }
}

public class PlayerData
{
    public Vector3 playerPosn;
    public List<int> collectedCoinIndexes;
}



