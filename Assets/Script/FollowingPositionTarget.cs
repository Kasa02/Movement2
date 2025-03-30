using UnityEngine;

public class FollowPositionTarget : MonoBehaviour
{
    [Tooltip("Ҫ���ٵ�Ŀ�꣨�������壩")]
    public Transform target;

    [Tooltip("����ʱ��ƫ����")]
    public Vector3 offset = Vector3.zero;

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
