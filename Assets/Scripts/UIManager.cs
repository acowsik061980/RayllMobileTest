using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject gameMenuUI;
    [SerializeField] private TextMeshProUGUI coinsCollectedDisplay;
    [SerializeField] private GameManager gameMgr;

    public GameObject backButton;

    private void Start()
    {
        ShowMainMenu(true);
    }

    public void ShowMainMenu(bool state)
    {
        mainMenuUI.SetActive(state);
        gameMenuUI.SetActive(!state);
        gameMgr.isGameRunning = !state;
    }

    public void LoadGameState(int slotIndex)
    {
        gameMgr.lastUsedSaveSlotIndex = slotIndex;
        gameMgr.saveMgr.LoadPlayerData(slotIndex);
    }

    public void UpdateCoinCollectedCount(int count)
    {
        coinsCollectedDisplay.text = string.Format("COINS COLLECTED : {0}", count);
    }
}
