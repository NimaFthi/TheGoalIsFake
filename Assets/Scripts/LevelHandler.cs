using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    private static LevelHandler _instance;

    public static LevelHandler Instance
    {
        private set => _instance = value;
        get
        {
            if (_instance != null)
            {
                return _instance;
            }

            _instance = GameObject.FindObjectOfType<LevelHandler>();
            return _instance;
        }
    }

    [SerializeField] private Animator[] levelAnimators;
    [SerializeField] private Transform[] fakeEnemySpawnTransforms;
    [SerializeField] private Transform[] fakeEnemyRunAwayTransforms;
    [SerializeField] private Transform[] fakeGoalSpawnTransforms;
    [SerializeField] private GameObject fakeEnemyPrefab;
    [SerializeField] private GameObject fakeGoalPrefab;
    [SerializeField] private Transform playerTransform;

    private void Start()
    {
        SpawnFakeEnemy();
        SpawnFakeGoal();
    }

    public IEnumerator NextLevelTransition()
    {
        GameManager.Instance.isLevelChanging = true;
        levelAnimators[GameManager.Instance.level - 1].SetTrigger("PreExit");
        yield return new WaitForSeconds(0.5f);
        levelAnimators[GameManager.Instance.level].gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        levelAnimators[GameManager.Instance.level - 1].SetTrigger("Exit");
        
        StartCoroutine(GameManager.Instance.RebuildNavmesh());
        yield return new WaitForSeconds(1.02f);
        
        GameManager.Instance.isLevelChanging = false;
        
        SpawnFakeEnemy();
        
        SpawnFakeGoal();
    }

    private void SpawnFakeEnemy()
    {
        if(GameManager.Instance.isLastLevel) return;
        if (fakeEnemySpawnTransforms[GameManager.Instance.level] == null) return;

        GameObject fakeEnemy = Instantiate(fakeEnemyPrefab, fakeEnemySpawnTransforms[GameManager.Instance.level].position,Quaternion.identity);
        FakeEnemyController fakeEnemyController = fakeEnemy.GetComponent<FakeEnemyController>();
        fakeEnemyController.startTransform = fakeEnemySpawnTransforms[GameManager.Instance.level];
        fakeEnemyController.runAwayTransform = fakeEnemyRunAwayTransforms[GameManager.Instance.level];
        fakeEnemyController.playerTransform = playerTransform;
        GameManager.Instance.currentFakeEnemyController = fakeEnemyController;
    }

    private void SpawnFakeGoal()
    {
        GameObject fakeGoal = Instantiate(fakeGoalPrefab, fakeGoalSpawnTransforms[GameManager.Instance.level].position,Quaternion.identity);
        FakeGoalController fakeGoalController = fakeGoal.GetComponent<FakeGoalController>();
        fakeGoalController.startTransform = fakeGoalSpawnTransforms[GameManager.Instance.level];
        fakeGoalController.playerTransform = playerTransform;
        GameManager.Instance.currentFakeGoalController = fakeGoalController;
    }
}