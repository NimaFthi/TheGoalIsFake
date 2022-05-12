using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int levelNum;
    public Animator animator;
    public Transform playerSpawnPos;
    public Transform fakeEnemySpawnPos;
    public Transform fakeGoalStartPos;
    public Transform fakeEnemyRunAwayPos;


    public void StartLevel()
    {
        this.gameObject.SetActive(true);
        if (this.levelNum != LevelManager.instance.startLevel)
            this.animator.SetTrigger("Enter");
    }

    public void ExitLevel()
    {
        this.animator.SetTrigger("Exit");
        StartCoroutine(DisableLevel());
    }

    private IEnumerator DisableLevel()
    {
        yield return new WaitForSeconds(2.01f);
        this.gameObject.SetActive(false);
        LevelManager.instance.navMeshSurface.BuildNavMesh();
    }

    public void Spawn()
    {
        LevelManager.instance.SpawnFakes();
        PlayerManager.instance.EnablePlayer();
    }
}