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
    public Transform playerTransform;
    public Transform startTransform;
    public Transform runAwayTransform;
    private Transform target;
    
    //stats
    [SerializeField] private float DelayBeforeRunningAway = 0.5f;

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
    }

    private IEnumerator TransformToGoal()
    {
        isTransforming = true;
        fakeEnemyAnim.SetTrigger("TransformToGoal");

        yield return new WaitForSeconds(DelayBeforeRunningAway);
        
        gameObject.tag = "Goal";
        isUsed = true;
        isTransforming = false;
    }
    
    #endregion
}