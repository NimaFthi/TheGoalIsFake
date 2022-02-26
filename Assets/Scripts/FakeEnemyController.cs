using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FakeEnemyController : MonoBehaviour
{
    [SerializeField] private float agentEnemySpeed = 10f;
    [SerializeField] private float agentGoalSpeed = 25f; 
    
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public Transform startTransform;
    [HideInInspector] public Transform runAwayTransform;

    private PlayerController playerController;
    private NavMeshAgent agent;
    private Animator fakeEnemyAnim;
    private bool isUsed;
    public bool isTransforming;
    

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        fakeEnemyAnim = GetComponent<Animator>();
        playerController = playerTransform.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (isTransforming)
        {
            agent.SetDestination(transform.position);
            return;
        }

        if (playerController.isDead)
        {
            agent.SetDestination(startTransform.position);
            return;
        }

        if (!isUsed)
        {
            agent.SetDestination(playerTransform.position);
        }
        else
        {
            agent.SetDestination(runAwayTransform.position);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !isTransforming)
        {
            if (!isUsed) return;
            if (GameManager.Instance.isLastLevel)
            {
                GameManager.Instance.LoadEndMenu();
                return;
            }
            GameManager.Instance.level++;
            GameManager.Instance.TransformPlayerToNextLevel();
            
            if(GameManager.Instance.currentFakeGoalController == null) return;
            GameManager.Instance.currentFakeGoalController.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public IEnumerator TransformToGoal()
    {
        isTransforming = true;
        fakeEnemyAnim.SetTrigger("TransformToGoal");
        gameObject.tag = "RealGoal";
        agent.speed = agentGoalSpeed;
        yield return new WaitForSeconds(1f);
        isUsed = true;
        isTransforming = false;
    }
    
    public IEnumerator TransformToEnemy()
    {
        isTransforming = true;
        fakeEnemyAnim.SetTrigger("TransformToEnemy");
        agent.speed = agentEnemySpeed;
        yield return new WaitForSeconds(1f);
        isUsed = false;
        gameObject.tag = "Enemy";
        isTransforming = false;
    }
}