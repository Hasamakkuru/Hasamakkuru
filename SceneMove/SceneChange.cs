using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneChange{
    private static AsyncOperation async;
	public static void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public static string SceneName
    {
        get { return SceneManager.GetActiveScene().name; }
    }

    public static IEnumerator LoadScene(string sceneName)
    {
        async = SceneManager.LoadSceneAsync(sceneName);
        while(!async.isDone)
            yield return null;
    }
}
