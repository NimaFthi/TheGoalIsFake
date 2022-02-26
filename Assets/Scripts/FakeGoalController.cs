using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FakeGoalController : MonoBehaviour
{
    public Transform playerTransform;
    public Transform startTransform;

    private NavMeshAgent agent;
    private Animator fakeGoalAnim;
    public bool isUsed;
    public bool isTransforming;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        fakeGoalAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isTransforming)
        {
            agent.SetDestination(transform.position);
            return;
        }

        if (isUsed)
        {
            agent.SetDestination(playerTransform.position);
        }
        else
        {
            agent.SetDestination(startTransform.position);
        }
    }

    private IEnumerator TransformToEnemy()
    {
        StartCoroutine(GameManager.Instance.currentFakeEnemyController.TransformToGoal());

        isTransforming = true;
        fakeGoalAnim.SetTrigger("TransformToEnemy");
        if (GameManager.Instance.colorCam)
        {
            GameManager.Instance.camAnimator.SetBool("Color", true);
            SoundManager.Instance.BGPlayHard();
        }

        yield return new WaitForSeconds(1.5f);
        isUsed = true;
        gameObject.tag = "Enemy";
        isTransforming = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !isTransforming)
        {
            if (!isUsed)
            {
                StartCoroutine(TransformToEnemy());
            }
            else
            {
                if (GameManager.Instance.isLastLevel)
                {
                    GameManager.Instance.LoadEndMenu();
                    return;
                }

                StartCoroutine(TransformToGoal());
            }
        }
    }

    public IEnumerator TransformToGoal()
    {
        StartCoroutine(GameManager.Instance.currentFakeEnemyController.TransformToEnemy());

        isTransforming = true;
        fakeGoalAnim.SetTrigger("TransformToGoal");
        GameManager.Instance.camAnimator.SetBool("Color", false);
        SoundManager.Instance.BGPlayLight();
        yield return new WaitForSeconds(1f);
        isUsed = false;
        gameObject.tag = "Goal";
        isTransforming = false;
    }
}