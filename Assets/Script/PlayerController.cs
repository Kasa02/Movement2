using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Camera cam;


    private Vector2 input;
    private Rigidbody _rb;
    private int count;
    public TextMeshProUGUI countText;
  

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();


        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    void Update()
    {
        UpdateInput();
    }

    private void FixedUpdate()
    {
        MoveWithAddTorque();
    }

    private void UpdateInput()
    {
        input = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) input.y += 1;
        if (Input.GetKey(KeyCode.S)) input.y -= 1;
        if (Input.GetKey(KeyCode.D)) input.x += 1;
        if (Input.GetKey(KeyCode.A)) input.x -= 1;

        input = input.normalized;
    }

    private Vector3 GetCameraRelativeDirection()
    {
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return (right * input.x + forward * input.y).normalized;
    }

    private void MoveWithAddTorque()
    {
        Vector3 moveDir = GetCameraRelativeDirection();

        Vector3 torque = Vector3.Cross(Vector3.up, moveDir);

        _rb.AddTorque(torque * moveSpeed, ForceMode.Force);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }

    }
    void SetCountText()
    {
        countText.text = "count: " + count.ToString();
    }
}
