using UnityEngine;
using UnityEngine.SceneManagement;

public class EventSceneController : MonoBehaviour
{
    // �����¼���������ʱ������ĳ��������������
    [SerializeField] private KeyCode returnKey = KeyCode.Escape;

    void Update()
    {
        if (Input.GetKeyDown(returnKey))
        {
            EndEvent();
        }
    }

    // ���¼�����ʱ���ô˷���
    public void EndEvent()
    {
        Debug.Log("�¼�������ж�ص�ǰ������");
        // ж�ص�ǰ�¼�����
        SceneManager.UnloadSceneAsync(gameObject.scene);
        // ע�⣺UnloadSceneAsync ���첽������PlayerController ���� OnSceneUnloaded ���������ÿ���
    }

    // �����ս��������������ս��ʤ����ʧ��ʱ���� EndEvent()
    // public void OnBattleEnd() { EndEvent(); }
}
