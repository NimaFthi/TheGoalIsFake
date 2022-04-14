using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    public Transform enemyStartTransform;
    
    private NavMeshAgent agent;
    public bool canFollowPlayer;
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(GameManager.Instance.isLevelChanging) return;
        var playerControllerScript = target.GetComponent<PlayerController>();
        if(playerControllerScript== null) return;
        if (playerControllerScript.isDead)
        {
            agent.SetDestination(enemyStartTransform.position);
            return;
        }
        FollowPlayer();

        if(canFollowPlayer) return;
        agent.SetDestination(transform.position);
    }

    private void FollowPlayer()
    {
        if(!canFollowPlayer) return;

        agent.SetDestination(target.position);
    }
}
