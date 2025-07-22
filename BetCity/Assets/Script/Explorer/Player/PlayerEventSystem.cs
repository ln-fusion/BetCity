
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEventSystem : MonoBehaviour
{
    [Header("组件引用")]
    [SerializeField] private MySceneLoader mySceneLoader; // 场景加载器

    [Header("事件场景索引")]
    [SerializeField] private int[] eventSceneIndices = { 2, 3, 4, 5 }; // 事件场景的索引

    public bool IsInEvent { get; private set; } = false; // 标记是否在事件场景中

    private Vector3 savedCameraPosition; // 用于保存摄像机的初始位置

    private void Awake()
    {
        // 确保MySceneLoader实例已赋值
        if (mySceneLoader == null)
        {
            // 尝试在场景中查找MySceneLoader实例
            mySceneLoader = FindObjectOfType<MySceneLoader>();
            if (mySceneLoader == null)
            {
                Debug.LogError("未找到MySceneLoader组件！请确保场景中有一个挂载了MySceneLoader脚本的GameObject。");
            }
        }

        // 保存摄像机的位置
        if (Camera.main != null)
        {
            savedCameraPosition = Camera.main.transform.position;
        }
        else
        {
            Debug.LogError("未找到MainCamera，请确保场景中有一个标记为MainCamera的摄像机！");
        }
    }

    // 切换到事件场景
    public void LoadEventScene(int sceneIndex)
    {
        // 获取场景名称（字符串）
        string sceneName = GetSceneNameByIndex(sceneIndex);

        // 加载事件场景
        if (mySceneLoader != null)
        {
            mySceneLoader.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("MySceneLoader未引用，无法加载场景！");
        }
        IsInEvent = true; // 设置为在事件场景中

        // 恢复摄像机的位置
        if (Camera.main != null)
        {
            Camera.main.transform.position = savedCameraPosition;
        }
        else
        {
            Debug.LogError("没有找到MainCamera，无法恢复摄像机位置！");
        }
    }

    // 离开事件，返回探索场景
    public void EndEvent()
    {
        // 加载主探索场景
        if (mySceneLoader != null)
        {
            mySceneLoader.LoadScene("ExplorerMap");
        }
        else
        {
            Debug.LogError("MySceneLoader未引用，无法加载场景！");
        }
        IsInEvent = false; // 设置为不在事件场景中
    }

    // 获取场景名称
    private string GetSceneNameByIndex(int index)
    {
        switch (index)
        {
            case 0:
                return "Scenes/ExplorerMap"; // 探索地图
            case 1:
                return "Scenes/MainCityEvent1"; // 事件场景1
            case 2:
                return "Scenes/BattleScene"; // 战斗场景
            case 3:
                return "Scenes/Event1"; // 事件场景1
            case 4:
                return "Scenes/Event2"; // 事件场景2
            case 5:
                return "Scenes/Event3"; // 事件场景3
            default:
                return "Scenes/ExplorerMap"; // 默认返回探索场景
        }
    }

    // 触发随机事件
    public void TriggerRandomEvent()
    {
        int randomIndex = Random.Range(0, eventSceneIndices.Length);
        int sceneToLoad = eventSceneIndices[randomIndex];
        LoadEventScene(sceneToLoad);
    }

    // 触发战斗
    public void TriggerBattle()
    {
        int sceneToLoad = eventSceneIndices[0]; // 假设战斗场景在事件场景索引中
        LoadEventScene(sceneToLoad);
    }

    // 检查当前节点是否触发事件
    public void CheckForNodeEvent(Node node)
    {
        switch (node.nodeType)
        {
            case NodeType.RandomEvent:
                TriggerRandomEvent();
                break;
            case NodeType.FixedEvent:
                if (node.fixedEventSceneIndex != -1)
                {
                    Debug.Log($"触发固定事件，加载场景索引: {node.fixedEventSceneIndex}");
                    LoadEventScene(node.fixedEventSceneIndex);
                }
                else
                {
                    Debug.LogWarning($"节点 {node.name} 的NodeType是FixedEvent，但未指定fixedEventSceneIndex。");
                }
                break;
            case NodeType.Battle:
                TriggerBattle();
                break;
            case NodeType.Normal:
                Debug.Log("当前节点是普通节点，无事件触发。");
                break;
            default:
                Debug.LogWarning($"未处理的节点类型: {node.nodeType}");
                break;
        }
    }
}

