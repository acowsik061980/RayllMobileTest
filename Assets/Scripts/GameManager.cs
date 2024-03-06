using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private CoinData coinData;

    public InputManager inputMgr;
    public UIManager UIMgr;
    public SaveManager saveMgr;
    public CapsuleController controller;
    public FirebaseAnalyticsHandler analyticsMgr;

    public bool targetPointSet = false;
    public int coinsCollected = 0;
    public bool isGameRunning = false;
    public int lastUsedSaveSlotIndex = 0;
    public List<int> coinIndexesCollected = new List<int>();

    private List<GameObject> m_SpawnedCoins = new List<GameObject>();
    private List<GameObject> m_NewlyAddedCoins = new List<GameObject>();

    private void Start()
    {
        InitGame();
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart);
    }

    private void OnDestroy()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd);
    }

    public void InitGame()
    {
        coinsCollected = 0;
        InitCoins();
        coinIndexesCollected.Clear();
        controller.InitController();
    }

    private void InitCoins()
    {
        m_NewlyAddedCoins.Clear();
        for (var i = 0; i < coinData.coinPosns.Count; i++)
        {
            GameObject obj;
            if (m_SpawnedCoins.Count < (i + 1))
            {
                obj = Instantiate(coinPrefab, transform);
                m_NewlyAddedCoins.Add(obj);
            }
            else
                obj = m_SpawnedCoins[i];

            obj.SetActive(true);
            obj.transform.localPosition = coinData.coinPosns[i];
            Coin coin = obj.GetComponent<Coin>();
            coin.coinIndex = i;
        }

        m_SpawnedCoins.AddRange(m_NewlyAddedCoins);
        UIMgr.UpdateCoinCollectedCount(coinsCollected);
    }

    public void SetGameStateFromSavedData(PlayerData playerData)
    {
        InitCoins();
        coinIndexesCollected = playerData.collectedCoinIndexes;
        controller.agent.ResetPath();
        controller.agent.enabled = false;
        controller.transform.localPosition = playerData.playerPosn;
        controller.targetPosn = Camera.main.WorldToScreenPoint(transform.TransformPoint(playerData.playerPosn));
        controller.agent.enabled = true;
        HideCollectedCoins();
        coinsCollected = coinIndexesCollected.Count;
        UIMgr.UpdateCoinCollectedCount(coinsCollected);
    }

    public void HideCollectedCoins()
    {
        for (var i = 0; i < coinIndexesCollected.Count; i++)
        {
            m_SpawnedCoins[coinIndexesCollected[i]].gameObject.SetActive(false);
        }
    }

    public void SaveAndReturnToMenu()
    {
        PlayerData saveData = new PlayerData();
        saveData.playerPosn = controller.transform.localPosition;
        saveData.collectedCoinIndexes = coinIndexesCollected;
        saveMgr.SavePlayerData(saveData, lastUsedSaveSlotIndex);
        Debug.Log("Saving player data into save slot " + (lastUsedSaveSlotIndex + 1));
        UIMgr.ShowMainMenu(true);
        FirebaseAnalytics.LogEvent("GameSave", "saveSlotNumber", lastUsedSaveSlotIndex + 1);
    }
}
