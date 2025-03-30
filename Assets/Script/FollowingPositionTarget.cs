using UnityEngine;

public class FollowPositionTarget : MonoBehaviour
{
    [Tooltip("要跟踪的目标（例如球体）")]
    public Transform target;

    [Tooltip("跟踪时的偏移量")]
    public Vector3 offset = Vector3.zero;

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
