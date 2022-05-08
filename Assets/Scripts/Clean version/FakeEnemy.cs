using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FakeEnemy : MonoBehaviour
{
    //components
    private NavMeshAgent agent;
    private Animator fakeEnemyAnim;

    //destinations 
    private Transform target;
    
    //stats
    [SerializeField] private float enemySpeed = 12f;
    [SerializeField] private float enemyAcceleration = 10f;
    [SerializeField] private float enemyAngularSpeed = 360f;
    
    [SerializeField] private float goalSpeed = 12f;
    [SerializeField] private float goalAcceleration = 10f;
    [SerializeField] private float goalAngularSpeed = 360f;
    
    [SerializeField] private float delayBeforeRunningAway = 0.5f;

    private bool isUsed;
    [HideInInspector] public bool isTransforming;

    private enum FollowState
    {
        Idle,
        FollowingPLayer,
        MovingBackToStart,
        RunningAway
    }

    private FollowState followState;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        fakeEnemyAnim = GetComponent<Animator>();
        
        SetAgentStats(enemySpeed,enemyAcceleration,enemyAngularSpeed);
    }

    #region Set Destination

    private void Update()
    {
        SetFollowState();
        GoToDestination();
    }

    private void SetFollowState()
    {
        if (isTransforming)
        {
            followState = FollowState.Idle;
            return;
        }

        if (PlayerManager.instance.isDead)
        {
            followState = FollowState.MovingBackToStart;
            return;
        }

        followState = isUsed ? FollowState.RunningAway : FollowState.FollowingPLayer;
    }

    private void GoToDestination()
    {
        switch (followState)
        {
            case FollowState.Idle:
                target = transform;
                break;
            case FollowState.FollowingPLayer:
                target = PlayerManager.instance.transform;
                break;
            case FollowState.MovingBackToStart:
                target = LevelManager.instance.levels[LevelManager.instance.currentLevel].fakeEnemySpawnPos;
                break;
            case FollowState.RunningAway:
                target = LevelManager.instance.levels[LevelManager.instance.currentLevel].fakeEnemyRunAwayPos;
                break;
        }

        agent.SetDestination(target.position);
    }

    #endregion

    #region Transform

    private void OnEnable()
    {
        PlayerManager.instance.OnPlayerDeath += OnPlayerDeathAction;
        PlayerManager.instance.OnFirstTouchToGoal += OnFirstTouchToGoalAction;
    }

    private void OnDisable()
    {
        PlayerManager.instance.OnPlayerDeath -= OnPlayerDeathAction;
        PlayerManager.instance.OnFirstTouchToGoal -= OnFirstTouchToGoalAction;
    }

    private void OnPlayerDeathAction()
    {
        TransformToEnemy();
    }

    private void OnFirstTouchToGoalAction()
    {
        StartCoroutine(TransformToGoal());
    }

    private void TransformToEnemy()
    {
        fakeEnemyAnim.SetTrigger("TransformToEnemy");
        isUsed = false;
        gameObject.tag = "Enemy";
        SetAgentStats(enemySpeed,enemyAcceleration,enemyAngularSpeed);
    }

    private IEnumerator TransformToGoal()
    {
        isTransforming = true;
        fakeEnemyAnim.SetTrigger("TransformToGoal");

        yield return new WaitForSeconds(delayBeforeRunningAway);
        
        gameObject.tag = "Goal";
        isUsed = true;
        SetAgentStats(goalSpeed,goalAcceleration,goalAngularSpeed);
        isTransforming = false;
    }

    private void SetAgentStats(float speed , float acceleration,float angularSpeed)
    {
        agent.speed = speed;
        agent.acceleration = acceleration;
        agent.angularSpeed = angularSpeed;
    }
    
    #endregion
}