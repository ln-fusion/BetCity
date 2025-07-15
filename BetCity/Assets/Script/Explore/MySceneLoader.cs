using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneLoader : MonoBehaviour // 将类名从 SceneManager 修改为 MySceneLoader
{
    // 加载新场景并卸载当前场景
    public void LoadScene(string sceneName)
    {
        // 自动卸载当前场景并加载新场景
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
