using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject playerGFX;
    [SerializeField] private Transform[] playerSpawnTransforms;
    [SerializeField] private GameObject dieEffectPrefab;

    [SerializeField] private float moveSpeed = 40f;
    [SerializeField] private float reSpawnTime = 5f;
    [SerializeField] private float levelTravelingTime = 5f;

    private Rigidbody rb;
    private Collider col;
    private Vector3 velocity;
    private bool isMovedToNextLevel;
    
    public bool isDead;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);
        velocity = moveDirection * moveSpeed + new Vector3(0f, rb.velocity.y, 0f);
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.currentFakeEnemyController.isTransforming || GameManager.Instance.isLevelChanging)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        if (isDead) return;
        rb.velocity = velocity * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(isDead) return;
        if (other.gameObject.CompareTag("Trap"))
        {
            GameObject dieEffect = Instantiate(dieEffectPrefab, transform.position, Quaternion.identity);
            Destroy(dieEffect,7f);
            StartCoroutine(ReSpawn());
            
            if(!GameManager.Instance.currentFakeGoalController.isUsed) return;
            StartCoroutine(GameManager.Instance.currentFakeGoalController.TransformToGoal());
        }
        else if(other.gameObject.CompareTag("Enemy"))
        {
            GameObject dieEffect = Instantiate(dieEffectPrefab, transform.position, Quaternion.identity);
            Destroy(dieEffect,7f);

            if (GameManager.Instance.isLastLevel)
            {
                col.enabled = false;
                playerGFX.SetActive(false);
                isDead = true;
                rb.velocity = Vector3.zero;
                return;
            }
            StartCoroutine(ReSpawn());
            
            if(!GameManager.Instance.currentFakeGoalController.isUsed) return;
            StartCoroutine(GameManager.Instance.currentFakeGoalController.TransformToGoal());
        }
    }

    private IEnumerator ReSpawn()
    {
        col.enabled = false;
        playerGFX.SetActive(false);
        isDead = true;
        transform.position = playerSpawnTransforms[GameManager.Instance.level].position;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(reSpawnTime);
        playerGFX.SetActive(true);
        col.enabled = true;
        isDead = false;
    }

    public IEnumerator MoveToNextLevel()
    {
        col.enabled = false;
        playerGFX.SetActive(false);
        transform.position = playerSpawnTransforms[GameManager.Instance.level].position;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(levelTravelingTime);
        playerGFX.SetActive(true);
        col.enabled = true;
        if (GameManager.Instance.level == playerSpawnTransforms.Length - 1)
        {
            GameManager.Instance.level++;
        }
    }

    public void Disable()
    {
        col.enabled = false;
        rb.velocity = Vector3.zero;
    }
}