using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("���ʳ]�w")]
    public float moveSpeed;     //�����t��
    public float runSpeed;      //�]�B�t��
    public float jumpForce;     //���D����
    public float jumpCooldown;  //���D���j
    public float groundDrag;

    [Header("����j�w")]
    public KeyCode jumpKey = KeyCode.Space;     //�w�]�ť��䬰���D
    public KeyCode RunKey = KeyCode.LeftShift;  //�w�]��Shift���]�B��

    [Header("�򥻳]�w")]
    public Transform PlayerCamera;   // ��v��

    [Header("�a�O�T�{")]
    public float checkHight;           // �]�w�ˬd�O�_�b�a��������
    public LayerMask whatIsGround;     // �]�w���@�ӹϼh�O�g�u�i�H���쪺
    public bool isground;              // ���L�ܼơG�O�_�b�a�W

    private bool isRun;             //�O�_���b�]�B
    private bool canJump;           // �]�w�O�_�i�H���D
    private float horizontalInput;  // ������V���O
    private float verticalInput;    // ������V���O

    private Vector3 moveDirection;   // ���ʤ�V

    private Rigidbody rbFirstPerson; // ���a������

    private void Start()
    {
        rbFirstPerson = GetComponent<Rigidbody>();
        rbFirstPerson.freezeRotation = true;         // �����Ӫ����H�N����(�����û��O�����T������)
        canJump = true;
        isRun = false;
    }

    private void Update()
    {
        MyInput();
        SpeedControl();   // �����t�סA�L�ִN��t

        // �g�X�@���ݤ��쪺�g�u�A�ӧP�_���S������a���H
        isground = Physics.Raycast(transform.position, Vector3.down, checkHight * 0.5f + 0.3f, whatIsGround);
        Debug.DrawRay(transform.position, new Vector3(0, -(checkHight * 0.5f + 0.3f), 0), Color.red); // �b���ն��q�N�g�u�]�w������u���A�Ӭݬݽu�����װ������H
        // �p�G�I��a�O�A�N�]�w�@�Ӥϧ@�ΤO(�o�ӥi�H�s�y�H�����ʪ���t�P)
        if (isground)
            rbFirstPerson.drag = groundDrag;
        else
            rbFirstPerson.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();     // �u�n�O���󲾰ʡA��ĳ�A���FixedUpdate()        
    }

    // ��k�G���o�ثe���a����V��W�U���k���ƭȡA������D�欰
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRun = true;
        }
        else
        isRun = false;

        // �p�G���U�]�w�����D����
        if (Input.GetKeyDown(jumpKey) && canJump && isground)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown); // �p�G���D�L��A�N�|�̷ӳ]�w������ɶ��˼ơA�ɶ���F�~�੹�W���D
        }
    }

    private void MovePlayer()
    {
        // �p�Ⲿ�ʤ�V(���N�O�p��X�b�PZ�b��Ӥ�V���O�q)
        moveDirection = PlayerCamera.forward * verticalInput + PlayerCamera.right * horizontalInput;

        // �p�G�b�a���A�h�i�H����
        if (isground)
            rbFirstPerson.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    // ��k�G�����t�רô�t
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rbFirstPerson.velocity.x, 0f, rbFirstPerson.velocity.z); // ���o��X�b�PZ�b�������t��

        if(isRun!=true)
        {
            // �p�G�����t�פj��w�]�t�׭ȡA�N�N���󪺳t�׭��w��w�]�t�׭�(�����t��)
            if (flatVel.magnitude > moveSpeed)
            {
               Vector3 limitedVel = flatVel.normalized * moveSpeed;
               rbFirstPerson.velocity = new Vector3(limitedVel.x, rbFirstPerson.velocity.y, limitedVel.z);
            }
        }
        else
        {
            //�p�G�����t�פj��w�]�t�׭ȡA�N�N���󪺳t�׭��w��w�]�t�׭�(�]�B�t��)
            Vector3 limitedVel = flatVel.normalized * runSpeed;
            rbFirstPerson.velocity = new Vector3(limitedVel.x, rbFirstPerson.velocity.y, limitedVel.z);
        }
    }

    // ��k�G���D
    private void Jump()
    {
        // ���s�]�wY�b�t��
        rbFirstPerson.velocity = new Vector3(rbFirstPerson.velocity.x, 0f, rbFirstPerson.velocity.z);
        // �ѤU���W���Ĥ@�H�٪���AForceMode.Impulse�i�H�����e���Ҧ����@�����A�|�󹳸��D���Pı
        rbFirstPerson.AddForce(transform.up * jumpForce, ForceMode.Impulse);//Impulse�j�����Pı�O�b�@����������ӤO
    }

    // ��k�G���s�]�w�ܼ�readyToJump��true����k
    private void ResetJump()
    {
        canJump = true;
    }
}