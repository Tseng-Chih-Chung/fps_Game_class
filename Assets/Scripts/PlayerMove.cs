using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed;     //走路速度
    public float runSpeed;      //跑步速度
    public float jumpForce;     //跳躍高度
    public float jumpCooldown;  //跳躍間隔
    public float groundDrag;

    [Header("按鍵綁定")]
    public KeyCode jumpKey = KeyCode.Space;     //預設空白鍵為跳躍
    public KeyCode RunKey = KeyCode.LeftShift;  //預設左Shift為跑步鍵

    [Header("基本設定")]
    public Transform PlayerCamera;   // 攝影機

    [Header("地板確認")]
    public float checkHight;           // 設定檢查是否在地面的高度
    public LayerMask whatIsGround;     // 設定哪一個圖層是射線可以打到的
    public bool isground;              // 布林變數：是否在地上

    private bool isRun;             //是否正在跑步
    private bool canJump;           // 設定是否可以跳躍
    private float horizontalInput;  // 水平方向的力
    private float verticalInput;    // 垂直方向的力

    private Vector3 moveDirection;   // 移動方向

    private Rigidbody rbFirstPerson; // 玩家的剛體

    private void Start()
    {
        rbFirstPerson = GetComponent<Rigidbody>();
        rbFirstPerson.freezeRotation = true;         // 不讓該物件隨意旋轉(讓它永遠保持正確的姿勢)
        canJump = true;
        isRun = false;
    }

    private void Update()
    {
        MyInput();
        SpeedControl();   // 偵測速度，過快就減速

        // 射出一條看不到的射線，來判斷有沒有打到地面？
        isground = Physics.Raycast(transform.position, Vector3.down, checkHight * 0.5f + 0.3f, whatIsGround);
        Debug.DrawRay(transform.position, new Vector3(0, -(checkHight * 0.5f + 0.3f), 0), Color.red); // 在測試階段將射線設定為紅色線條，來看看線條長度夠不夠？
        // 如果碰到地板，就設定一個反作用力(這個可以製造人物移動的減速感)
        if (isground)
            rbFirstPerson.drag = groundDrag;
        else
            rbFirstPerson.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();     // 只要是物件移動，建議你放到FixedUpdate()        
    }

    // 方法：取得目前玩家按方向鍵上下左右的數值，控制跳躍行為
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

        // 如果按下設定的跳躍按鍵
        if (Input.GetKeyDown(jumpKey) && canJump && isground)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown); // 如果跳躍過後，就會依照設定的限制時間倒數，時間到了才能往上跳躍
        }
    }

    private void MovePlayer()
    {
        // 計算移動方向(其實就是計算X軸與Z軸兩個方向的力量)
        moveDirection = PlayerCamera.forward * verticalInput + PlayerCamera.right * horizontalInput;

        // 如果在地面，則可以移動
        if (isground)
            rbFirstPerson.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    // 方法：偵測速度並減速
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rbFirstPerson.velocity.x, 0f, rbFirstPerson.velocity.z); // 取得僅X軸與Z軸的平面速度

        if(isRun!=true)
        {
            // 如果平面速度大於預設速度值，就將物件的速度限定於預設速度值(走路速度)
            if (flatVel.magnitude > moveSpeed)
            {
               Vector3 limitedVel = flatVel.normalized * moveSpeed;
               rbFirstPerson.velocity = new Vector3(limitedVel.x, rbFirstPerson.velocity.y, limitedVel.z);
            }
        }
        else
        {
            //如果平面速度大於預設速度值，就將物件的速度限定於預設速度值(跑步速度)
            Vector3 limitedVel = flatVel.normalized * runSpeed;
            rbFirstPerson.velocity = new Vector3(limitedVel.x, rbFirstPerson.velocity.y, limitedVel.z);
        }
    }

    // 方法：跳躍
    private void Jump()
    {
        // 重新設定Y軸速度
        rbFirstPerson.velocity = new Vector3(rbFirstPerson.velocity.x, 0f, rbFirstPerson.velocity.z);
        // 由下往上推第一人稱物件，ForceMode.Impulse可以讓推送的模式為一瞬間，會更像跳躍的感覺
        rbFirstPerson.AddForce(transform.up * jumpForce, ForceMode.Impulse);//Impulse大概的感覺是在一瞬間給完整個力
    }

    // 方法：重新設定變數readyToJump為true的方法
    private void ResetJump()
    {
        canJump = true;
    }
}