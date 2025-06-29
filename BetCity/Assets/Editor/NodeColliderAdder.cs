using UnityEngine;
using UnityEditor; // 编辑器脚本需要引用 UnityEditor 命名空间

public class NodeColliderAdder : EditorWindow
{
    // 在 Unity 菜单栏中添加一个入口
    [MenuItem("Tools/Add Sphere Collider to Nodes")]
    public static void ShowWindow()
    {
        GetWindow<NodeColliderAdder>("Node Collider Adder");
    }

    void OnGUI()
    {
        GUILayout.Label("为所有 Node GameObject 添加 Sphere Collider", EditorStyles.boldLabel);

        if (GUILayout.Button("添加碰撞体"))
        {
            AddCollidersToAllNodes();
        }
    }

    static void AddCollidersToAllNodes()
    {
        // 查找场景中所有带有 Node 组件的 GameObject
        Node[] allNodes = FindObjectsOfType<Node>();
        int collidersAdded = 0;

        foreach (Node node in allNodes)
        {
            // 检查是否已经有 SphereCollider，如果没有则添加
            if (node.GetComponent<SphereCollider>() == null)
            {
                SphereCollider collider = node.gameObject.AddComponent<SphereCollider>();
                // 默认半径设置为 0.5f，这是 Unity 默认球体的半径。
                // 如果您的节点视觉模型大小不同，可能需要手动调整。
                collider.radius = 0.5f;
                Debug.Log($"已为 {node.name} 添加 SphereCollider。");
                collidersAdded++;
            }
            else
            {
                Debug.Log($"{node.name} 已经有 SphereCollider。");
            }
        }

        if (collidersAdded > 0)
        {
            Debug.Log($"完成，共为 {collidersAdded} 个 Node GameObject 添加了 SphereCollider。");
        }
        else
        {
            Debug.Log("没有添加新的 SphereCollider。所有 Node GameObject 都已拥有碰撞体或场景中没有 Node。");
        }
    }
}
