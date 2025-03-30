using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public GrapplingGun grappling;
    public float rotationSpeed = 5f;   
    private Quaternion desiredRotation;

    void Update()
    {
        if (!grappling.IsGrappling())
        {
            // 默认情况下，枪保持与父物体（通常是角色身体）一致的旋转
            desiredRotation = transform.parent.rotation;
        }
        else
        {
            Vector3 directionToTarget = grappling.GetGrapplePoint() - transform.position;

            // 可选：锁定 Y 方向防止枪上下乱跳
            // directionToTarget.y = 0f;

            // 只有当目标方向角度明显变化时才更新目标旋转（避免微抖）
            if (Vector3.Angle(transform.forward, directionToTarget) > 1f)
            {
                desiredRotation = Quaternion.LookRotation(directionToTarget);
            }
        }

        // 使用 Slerp 替代 Lerp，使旋转更平滑，防止“左右摇摆”
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }
}
