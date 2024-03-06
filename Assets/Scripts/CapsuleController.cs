using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CapsuleController : MonoBehaviour
{
    [SerializeField] private GameManager gameMgr;

    private Camera m_gameCam;
    private Ray m_Ray;
    private RaycastHit m_RaycastHit;
    private bool isMoving = false;
    private static readonly int _GROUND_LAYER = 1 << 6;

    public NavMeshAgent agent;
    public Vector3 targetPosn;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        m_gameCam = Camera.main;
    }

    public void InitController()
    {
        agent.ResetPath();
        agent.enabled = false;
        transform.localPosition = Vector3.zero;
        targetPosn = Camera.main.WorldToScreenPoint(Vector3.zero);
        agent.enabled = true;
        isMoving = false;
    }

    private void Update()
    {
        if (gameMgr != null)
        {
            if (!gameMgr.saveMgr.isSaveLoading && gameMgr.targetPointSet)
            {
                if (!isMoving)
                {
                    isMoving = true;
                    m_Ray = m_gameCam.ScreenPointToRay(targetPosn);
                    if (Physics.Raycast(m_Ray, out m_RaycastHit, _GROUND_LAYER))
                        agent.destination = m_RaycastHit.point;
                }

                if (!agent.pathPending)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                        {
                            gameMgr.targetPointSet = false;
                            gameMgr.UIMgr.backButton.SetActive(true);
                            isMoving = false;
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Coin coin = other.GetComponent<Coin>();
        if (coin != null) 
        {
            if (!gameMgr.coinIndexesCollected.Contains(coin.coinIndex))
            {
                gameMgr.coinIndexesCollected.Add(coin.coinIndex);
                gameMgr.coinsCollected++;
                gameMgr.UIMgr.UpdateCoinCollectedCount(gameMgr.coinsCollected);
                coin.gameObject.SetActive(false);
                FirebaseAnalytics.LogEvent("CoinCollProgress", "percentage", gameMgr.coinsCollected * 0.1f);
                Debug.Log("Total coins collected for save slot " + (gameMgr.lastUsedSaveSlotIndex + 1) + " is " + gameMgr.coinsCollected);
            }
        }
    }
}
