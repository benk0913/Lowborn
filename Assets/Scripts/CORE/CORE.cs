using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CORE : MonoBehaviour
{
    #region Parameters

    public static CORE Instance;

    [System.NonSerialized]
    public List<GameObject> DontDestroyMeOnLoadList = new List<GameObject>();

    [SerializeField]
    public GameDB Database;

    #endregion


    #region Init

    private void Awake()
    {
        Instance = this;

        Initialize();
    }

    void Initialize()
    {
        StartCoroutine(InitializeRoutine());

        AddListeners();
    }

    private void AddListeners()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    IEnumerator InitializeRoutine()
    {
        yield return 0;

        while (ResourcesLoader.Instance.m_bLoading)
        {
            yield return 0;
        }

        OnInitialize.Invoke();
    }

    #endregion


    #region Public Methods

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    #endregion


    #region Event Handlers

    [SerializeField]
    UnityEvent OnInitialize;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }

    #endregion
}
