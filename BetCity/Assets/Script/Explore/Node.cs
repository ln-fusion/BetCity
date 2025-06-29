using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("�ڵ�����")]
    public List<Node> connectedNodes = new List<Node>();

    private PlayerController playerController; // ��Ӷ� PlayerController ������

    private void Start()
    {
        // ����Ϸ��ʼʱ�ҵ������е� PlayerController ʵ��
        // ȷ��������ֻ��һ�� PlayerController ʵ��
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("������δ�ҵ� PlayerController ʵ������ȷ�� PlayerController �ű�������ĳ����Ծ�� GameObject �ϡ�");
        }
    }

    // ��������˽ڵ����ײ��ʱ����
    private void OnMouseDown()
    {
        if (playerController != null)
        {
            Debug.Log($"����˽ڵ�: {this.name}");
            playerController.TryMoveToNode(this); // ���� PlayerController ���ƶ�����
        }
    }

    // ���ӻ����ӹ�ϵ���༭����ʹ�ã�
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
