using System.Collections.Generic;
using UnityEngine;


// Node.cs
public enum NodeType
{
    Normal,      // 普通节点
    RandomEvent, // 触发随机事件的节点
    FixedEvent,  // 触发固定事件的节点
    Battle       // 触发战斗的节点
    // ... 其他可能的节点类型可以在这里添加
}



public class Node : MonoBehaviour
{
    [Header("节点类型")]
    public NodeType nodeType = NodeType.Normal;

    [Header("固定事件场景索引 (仅当 NodeType 为 FixedEvent 时有效)")]
    public int fixedEventSceneIndex = -1; // 默认-1表示未设置

    [Header("节点连接")]
    public List<Node> connectedNodes = new List<Node>();



    private PlayerController playerController; // 添加对 PlayerController 的引用

    private void Start()
    {
        // 在游戏开始时找到场景中的 PlayerController 实例
        // 确保场景中只有一个 PlayerController 实例
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("场景中未找到 PlayerController 实例！请确保 PlayerController 脚本挂载在某个活跃的 GameObject 上。");
        }
    }

    // 当鼠标点击此节点的碰撞体时调用
    private void OnMouseDown()
    {
        if (playerController != null)
        {
            Debug.Log($"点击了节点: {this.name}");
            playerController.TryMoveToNode(this); // 调用 PlayerController 的移动方法
        }
    }

    // 可视化连接关系（编辑器中使用）
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (Node node in connectedNodes)
        {
            if (node != null)
                Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}
