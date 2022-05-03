using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject playerGfx;
    [SerializeField] private GameObject dieVfx;
    //[SerializeField] private GameObject spawnVfx;
    //[SerializeField] private Collider playerCol;
    
    //stats
    [SerializeField] private float reSpawnTime = 5f;
    private bool isDead;

    private void OnCollisionEnter(Collision other)
    {
        if(isDead) return;
        
        if (other.gameObject.CompareTag("Trap"))
        {
            StartCoroutine(ReSpawn());
        }
    }

    private IEnumerator ReSpawn()
    {
        isDead = true;
        playerMovement.canMove = false;
        playerGfx.SetActive(false);
        Die();
        
        transform.position = LevelManager.instance.playerSpawnPos[LevelManager.instance.currentLevel].position;
        
        yield return new WaitForSeconds(reSpawnTime);
        
        playerGfx.SetActive(true);
        playerMovement.canMove = true;
        isDead = false;
    }

    private void Die()
    {
        GameObject dieEffect = Instantiate(dieVfx, transform.position, Quaternion.identity);
        Destroy(dieEffect, 7f);
    }
}
