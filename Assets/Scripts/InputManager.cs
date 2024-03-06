using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameManager gameMgr;

    private Ray m_Ray;
    private RaycastHit m_RaycastHit;
    private static readonly int _GROUND_LAYER = 1 << 6;

    private void Awake()
    {
        Input.multiTouchEnabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gameMgr != null)
            {
                if (gameMgr.isGameRunning && !gameMgr.saveMgr.isSaveLoading)
                {
                    if (!gameMgr.targetPointSet)
                    {
                        m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(m_Ray, out m_RaycastHit, _GROUND_LAYER))
                        {
                            gameMgr.targetPointSet = true;
                            gameMgr.controller.targetPosn = Input.mousePosition;
                            gameMgr.UIMgr.backButton.SetActive(false);
                        }
                    }
                }
            }
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            if (gameMgr != null)
            {
                if (gameMgr.isGameRunning && !gameMgr.targetPointSet)
                    gameMgr.SaveAndReturnToMenu();
            }
        }
    }
}
