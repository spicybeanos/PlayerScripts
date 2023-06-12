using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public CharacterController cc;
    [SerializeField]
    public Transform Head;
    [Header("Acceleration due to gravity")]
    public float Gravity;
    [SerializeField]
    public Transform GroundCheck;
    [SerializeField]
    public LayerMask GroundLayerMask;
    public bool IsGrounded { get;private set; }
    public const float GroundCheckRadius = 0.5f;
    public float MovementSpeed;
    public float Sensitivity;
    public float ActionMultiplier = 1f;
    private float _m_jumpheight_;
    private float xrot = 0;
    public float JumpHeight
    {
        get
        {
            return _m_jumpheight_;
        }
        set
        {
            if(value >= 0)
            {
                _m_jumpheight_ = value;
            }
            else
            {
                _m_jumpheight_=0;
            }
        }
    }


    private void Awake()
    {
        xrot = Head.rotation.eulerAngles.x;
    }
    private void Start()
    {
        if(!TryGetComponent<CharacterController>(out cc))
        {
            Debug.LogWarning("No charecter controller found. Destroying this script.");
            Destroy(this); 
            return;
        }
        if(Head == null)
        {
            Debug.LogWarning($"{nameof(Head)} is null!!!");
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GroundCheck.position, GroundCheckRadius);
    }

    public void Initiate(MovementConfig config,Transform head)
    {
        Gravity = config.Gravity;
        MovementSpeed = config.MovementSpeed;
        Sensitivity = config.Sensitivity;
        Head = head;
    }
    Vector3 move = Vector3.zero;
    /// <summary>
    /// Moves and rotates the player using the axises
    /// by default, space is used as the jump key.
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="mouse"></param>
    public void Move(Vector2 axis, Vector2 mouse, KeyCode jump = KeyCode.Space)
    {
        IsGrounded = Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, GroundLayerMask);

        float x = axis.x * MovementSpeed * Time.deltaTime;
        float z = axis.y * MovementSpeed * Time.deltaTime;

        float mx = mouse.x * Sensitivity * Time.deltaTime * ActionMultiplier;
        float my = mouse.y * Sensitivity * Time.deltaTime * ActionMultiplier;

        xrot -= my;
        xrot = Mathf.Clamp(xrot, -90, 90);
        Head.localRotation = Quaternion.Euler(xrot, 0, 0);
        transform.Rotate(Vector3.up * mx);

        move = transform.forward * z * ActionMultiplier + transform.right * x * ActionMultiplier;

        if (Input.GetKeyDown(jump) && IsGrounded)
        {
            move.y = Mathf.Sqrt(Mathf.Abs(2*Gravity*JumpHeight)) * Time.deltaTime;
        }
        else if (IsGrounded)
        {
            move.y = 0;
        }
        else 
        {
            move.y += Gravity * Time.deltaTime;
        }
        cc.Move(move);
    }
}
[System.Serializable]
public class MovementConfig
{
    public float MovementSpeed = 10;
    public float Sensitivity = 5;
    public float Gravity = -9.8f;
    public float JumpHeight = 1;
}