using UnityEngine;
using UnityEngine.UI;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;

    public Transform gunTip;
    public Transform player;
    public Image crosshairUI;

    private float maxDistance = 100f;
    private SpringJoint joint;
    private Rigidbody rb;

    public float jumpForce = 6f;

    private Vector3 currentGrapplePosition;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rb = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdateCrosshair();

        // 按 E 开始抓钩
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartGrapple();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            StopGrapple();
        }

        // 只有在抓钩激活时才能跳跃
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && IsGrappling())
        {
            Jump();
        }
    }

    void LateUpdate()
    {
        DrawRope();
    }

    void UpdateCrosshair()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, whatIsGrappleable))
        {
            crosshairUI.enabled = true;
        }
        else
        {
            crosshairUI.enabled = false;
        }
    }

    void StartGrapple()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;

            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 15f;
            joint.damper = 2f;
            joint.massScale = 5f;

            Vector3 dir = (grapplePoint - player.position).normalized;
            rb.AddForce(dir * 12f, ForceMode.VelocityChange);

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        if (joint != null) Destroy(joint);
    }

    void DrawRope()
    {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(player.position, Vector3.down, 1.1f);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
