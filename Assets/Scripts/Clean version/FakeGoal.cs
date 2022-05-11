using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FakeGoal : MonoBehaviour
{
    //components
    private NavMeshAgent agent;
    private Animator fakeGoalAnim;
    public GameObject goalCanvas;

    //destinations
    private Transform target;

    [HideInInspector] public bool isUsed;
    [HideInInspector] public bool isTransforming;

    //stats
    [SerializeField] private float delayBeforeChasingPlayer = 1f;

    private enum FollowState
    {
        Idle,
        FollowingPLayer,
        MovingBackToStart
    }

    private FollowState followState;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        fakeGoalAnim = GetComponent<Animator>();
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

        followState = isUsed ? FollowState.FollowingPLayer : FollowState.MovingBackToStart;
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
                target = LevelManager.instance.levels[LevelManager.instance.currentLevel].fakeGoalStartPos;
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
        StartCoroutine(TransformToGoal());
    }

    private void OnFirstTouchToGoalAction()
    {
        StartCoroutine(TransformToEnemy());
        goalCanvas.SetActive(false);
    }

    private IEnumerator TransformToEnemy()
    {
        isTransforming = true;
        fakeGoalAnim.SetTrigger("TransformToEnemy");
        PlayerManager.instance.canMove = false;
        PlayerManager.instance.canDetectCollision = false;

        yield return new WaitForSeconds(PlayerManager.instance.delayBeforeCanMoveAgain);

        PlayerManager.instance.canMove = true;

        yield return new WaitForSeconds(delayBeforeChasingPlayer);

        isUsed = true;
        gameObject.tag = "Enemy";
        PlayerManager.instance.canDetectCollision = true;
        isTransforming = false;
    }

    private IEnumerator TransformToGoal()
    {
        fakeGoalAnim.SetTrigger("TransformToGoal");

        yield return new WaitForSeconds(PlayerManager.instance.delayBeforeDetectingCollision);

        isUsed = false;
        gameObject.tag = "Goal";
    }

    #endregion
    
}