using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;

    public static PlayerManager instance
    {
        private set => _instance = value;

        get
        {
            if (_instance != null)
                return _instance;

            _instance = GameObject.FindObjectOfType<PlayerManager>();
            return _instance;
        }
    }

    //components
    [SerializeField] private Collider playerCol;
    [SerializeField] private GameObject playerGfx;
    [SerializeField] private GameObject dieVfx;
    [SerializeField] private GameObject spawnVfx;
    [SerializeField] private GameObject wallTrailVFX;
    [SerializeField] private EffectObjectPool wallShockVFX;

    //stats
    [SerializeField] private float reSpawnTime = 5f;
    public float delayBeforeCanMoveAgain = 1f;
    public float delayBeforeDetectingCollision = 0.1f;
    
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool canDetectCollision = true;
    [HideInInspector] public bool canMove = true;
    private bool isTouchedGoal;
    
    //events
    public event Action OnFirstTouchToGoal;
    public event Action OnSecondTouchToGoal;
    public event Action OnPlayerDeath;

    private void OnCollisionEnter(Collision other)
    {
        if(!canDetectCollision) return;
        if(isDead) return;
        
        if (other.gameObject.CompareTag("Trap"))
        {
            Die();
            StartCoroutine(ReSpawn());
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            Die();
            StartCoroutine(ReSpawn());
        }

        if (other.gameObject.CompareTag("Goal"))
        {
            if (!isTouchedGoal)
            {
                OnFirstTouchToGoal?.Invoke();
                isTouchedGoal = true;
            }
            else
            {
                OnSecondTouchToGoal?.Invoke();
                isTouchedGoal = false;
            }
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            print("khord");
            var wallVFX = wallShockVFX.GetObject();
            wallVFX.transform.position = other.contacts[0].point;
            var rot = Quaternion.LookRotation(other.contacts[0].normal);
            wallVFX.transform.rotation = rot;
            wallVFX.SetActive(true);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            print("traillll");
            wallTrailVFX.SetActive(true);
            wallTrailVFX.transform.position = other.contacts[0].point;
            
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            wallTrailVFX.SetActive(false);
        }
    }

    private IEnumerator ReSpawn()
    {
        isDead = true;
        canMove = false;
        playerGfx.SetActive(false);
        playerCol.enabled = false;
        wallTrailVFX.SetActive(false);
        
        transform.position = LevelManager.instance.levels[LevelManager.instance.currentLevel].playerSpawnPos.position;

        yield return new WaitForSeconds(reSpawnTime);
        
        spawnVfx.SetActive(true);
        playerGfx.SetActive(true);
        playerCol.enabled = true;
        canMove = true;
        isDead = false;
    }

    private void Die()
    {
        
        var dieEffect = Instantiate(dieVfx, transform.position, Quaternion.identity);
        Destroy(dieEffect, 7f);

        if (!isTouchedGoal) return;
        
        OnPlayerDeath?.Invoke();
        isTouchedGoal = false;
    }

    public void DisablePlayer()
    {
        canMove = false;
        playerGfx.SetActive(false);
        playerCol.enabled = false;
    }

    public void EnablePlayer()
    {
        isTouchedGoal = false;
        transform.position = LevelManager.instance.levels[LevelManager.instance.currentLevel].playerSpawnPos.position;
        canMove = true;
        wallTrailVFX.SetActive(false);
        spawnVfx.SetActive(true);
        playerGfx.SetActive(true);
        playerCol.enabled = true;
    }
}
