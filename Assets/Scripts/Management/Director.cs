using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class Director
{
    private static bool _initialized;
    private static List<Manager> managers;

    static Director()
    {
        Initialize();
    }
    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
        if (_initialized) return;
        //TODO: initialization
        managers = new List<Manager>();
        SceneManager.LoadScene("Managers", LoadSceneMode.Additive);
        _initialized = true;
    }


    public static T GetManager<T>() where T : Manager
    {
        foreach (var manager in managers)
        {
            if (manager as T != null) return (T)manager;
        }
        Debug.LogWarning(string.Format("Manager of type {0} could not be found, returning null.", typeof(T).ToString()));
        return null;
    }

    public static void RegisterManager(Manager manager)
    {
        if (!managers.Contains(manager))
        {
            managers.Add(manager);
            Debug.Log("Manager registered: " + manager.name);
            Object.DontDestroyOnLoad(manager.gameObject);
        }
    }

}
