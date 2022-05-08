using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        private set => _instance = value;
        get
        {
            if (_instance != null)
            {
                return _instance;
            }

            _instance = GameObject.FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    [SerializeField] private int numberOfLevels;

    [Header("Components")] [SerializeField]
    private PlayerController playerController;

    [SerializeField] private NavMeshSurface navMeshSurface;


    [HideInInspector] public FakeEnemyController currentFakeEnemyController;
    [HideInInspector] public FakeGoalController currentFakeGoalController;
    [HideInInspector] public int level;
    [HideInInspector] public bool isLevelChanging;
    [HideInInspector] public bool isLastLevel;

    //camera
    public Animator camAnimator;
    public bool colorCamera;
    private void Start()
    {
        navMeshSurface.BuildNavMesh();
        SoundManager.Instance.BGPlayLight();
    }

    private void Update()
    {
        if (level != numberOfLevels - 1) return;
        isLastLevel = true;
    }

    public IEnumerator RebuildNavmesh()
    {
        yield return new WaitForSeconds(1.01f);
        navMeshSurface.BuildNavMesh();
    }

    public void TransformPlayerToNextLevel()
    {
        StartCoroutine(playerController.MoveToNextLevel());
        StartCoroutine(LevelHandler.Instance.NextLevelTransition());
        camAnimator.SetBool("Color",false);
        SoundManager.Instance.BGPlayLight();
    }

    public void LoadEndMenu()
    {
        isLevelChanging = true;
        playerController.Disable();
        currentFakeGoalController.enabled = false;
        Debug.Log("Game is Finished");
        StartCoroutine(EndMenu());
    }

    private IEnumerator EndMenu()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}