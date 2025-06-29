using UnityEngine;
using UnityEditor; // �༭���ű���Ҫ���� UnityEditor �����ռ�

public class NodeColliderAdder : EditorWindow
{
    // �� Unity �˵��������һ�����
    [MenuItem("Tools/Add Sphere Collider to Nodes")]
    public static void ShowWindow()
    {
        GetWindow<NodeColliderAdder>("Node Collider Adder");
    }

    void OnGUI()
    {
        GUILayout.Label("Ϊ���� Node GameObject ��� Sphere Collider", EditorStyles.boldLabel);

        if (GUILayout.Button("�����ײ��"))
        {
            AddCollidersToAllNodes();
        }
    }

    static void AddCollidersToAllNodes()
    {
        // ���ҳ��������д��� Node ����� GameObject
        Node[] allNodes = FindObjectsOfType<Node>();
        int collidersAdded = 0;

        foreach (Node node in allNodes)
        {
            // ����Ƿ��Ѿ��� SphereCollider�����û�������
            if (node.GetComponent<SphereCollider>() == null)
            {
                SphereCollider collider = node.gameObject.AddComponent<SphereCollider>();
                // Ĭ�ϰ뾶����Ϊ 0.5f������ Unity Ĭ������İ뾶��
                // ������Ľڵ��Ӿ�ģ�ʹ�С��ͬ��������Ҫ�ֶ�������
                collider.radius = 0.5f;
                Debug.Log($"��Ϊ {node.name} ��� SphereCollider��");
                collidersAdded++;
            }
            else
            {
                Debug.Log($"{node.name} �Ѿ��� SphereCollider��");
            }
        }

        if (collidersAdded > 0)
        {
            Debug.Log($"��ɣ���Ϊ {collidersAdded} �� Node GameObject ����� SphereCollider��");
        }
        else
        {
            Debug.Log("û������µ� SphereCollider������ Node GameObject ����ӵ����ײ��򳡾���û�� Node��");
        }
    }
}
