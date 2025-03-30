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
            // Ĭ������£�ǹ�����븸���壨ͨ���ǽ�ɫ���壩һ�µ���ת
            desiredRotation = transform.parent.rotation;
        }
        else
        {
            Vector3 directionToTarget = grappling.GetGrapplePoint() - transform.position;

            // ��ѡ������ Y �����ֹǹ��������
            // directionToTarget.y = 0f;

            // ֻ�е�Ŀ�귽��Ƕ����Ա仯ʱ�Ÿ���Ŀ����ת������΢����
            if (Vector3.Angle(transform.forward, directionToTarget) > 1f)
            {
                desiredRotation = Quaternion.LookRotation(directionToTarget);
            }
        }

        // ʹ�� Slerp ��� Lerp��ʹ��ת��ƽ������ֹ������ҡ�ڡ�
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }
}
