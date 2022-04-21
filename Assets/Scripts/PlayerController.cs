using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private GameObject playerGFX;
    [SerializeField] private Transform[] playerSpawnTransforms;
    [SerializeField] private GameObject dieEffectPrefab;
    [SerializeField] private GameObject endingTxt;

    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private float reSpawnTime = 5f;
    [SerializeField] private float levelTravelingTime = 5f;

    private Rigidbody rb;
    private Collider col;
    private Vector3 input;
    private bool isMovedToNextLevel = true;

    public bool isDead;

    //art Work 
    [SerializeField] private GameObject spawnVFX;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        GatherInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GatherInput()
    {
        var horizontal = joystick.Horizontal;
        var vertical = joystick.Vertical;
        input = new Vector3(horizontal, 0, vertical);
    }

    private void Move()
    {
        if (GameManager.Instance.currentFakeEnemyController.isTransforming || GameManager.Instance.isLevelChanging)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        if (isDead) return;
        if (!isMovedToNextLevel) return;

        var relative = input.ToIso();
        
        rb.velocity = new Vector3(relative.x, 0, relative.z) * (moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isDead) return;
        if (other.gameObject.CompareTag("Trap"))
        {
            if (GameManager.Instance.currentFakeGoalController.isTransforming) return;
            GameObject dieEffect = Instantiate(dieEffectPrefab, transform.position, Quaternion.identity);
            Destroy(dieEffect, 7f);
            StartCoroutine(ReSpawn());

            if (!GameManager.Instance.currentFakeGoalController.isUsed) return;
            StartCoroutine(GameManager.Instance.currentFakeGoalController.TransformToGoal());
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject dieEffect = Instantiate(dieEffectPrefab, transform.position, Quaternion.identity);
            Destroy(dieEffect, 7f);

            if (GameManager.Instance.isLastLevel)
            {
                col.enabled = false;
                playerGFX.SetActive(false);
                isDead = true;
                rb.velocity = Vector3.zero;
                return;
            }

            StartCoroutine(ReSpawn());
            if (!GameManager.Instance.currentFakeGoalController.isUsed) return;
            StartCoroutine(GameManager.Instance.currentFakeGoalController.TransformToGoal());
        }
    }

    private IEnumerator ReSpawn()
    {
        col.enabled = false;
        playerGFX.SetActive(false);
        spawnVFX.SetActive(false);
        isDead = true;
        transform.position = playerSpawnTransforms[GameManager.Instance.level].position;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(reSpawnTime);
        spawnVFX.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        playerGFX.SetActive(true);
        col.enabled = true;
        isDead = false;
    }

    public IEnumerator MoveToNextLevel()
    {
        isMovedToNextLevel = false;
        col.enabled = false;
        playerGFX.SetActive(false);
        spawnVFX.SetActive(false);
        transform.position = playerSpawnTransforms[GameManager.Instance.level].position;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(levelTravelingTime);
        spawnVFX.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        playerGFX.SetActive(true);
        col.enabled = true;
        isMovedToNextLevel = true;

        if (GameManager.Instance.level == playerSpawnTransforms.Length - 1)
        {
            GameManager.Instance.level++;
            endingTxt.SetActive(true);
        }
    }

    public void Disable()
    {
        col.enabled = false;
        rb.velocity = Vector3.zero;
    }
}