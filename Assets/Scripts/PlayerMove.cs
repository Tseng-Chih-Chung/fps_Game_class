using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed;

    [Header("按鍵綁定")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("基本設定")]
    public Transform PlayerCamera;   // 攝影機

    private float horizontalInput;   // 左右方向按鍵的數值(-1 <= X <= +1)
    private float verticalInput;     // 上下方向按鍵的數值(-1 <= Y <= +1)

    private Vector3 moveDirection;   // 移動方向

    private Rigidbody rbFirstPerson; // 第一人稱物件(膠囊體)的剛體

    private void Start()
    {
        rbFirstPerson = GetComponent<Rigidbody>();
        rbFirstPerson.freezeRotation = true;         // 鎖定第一人稱物件剛體旋轉，不讓膠囊體因為碰到物件就亂轉
    }

    private void Update()
    {
        MyInput();
    }

    private void FixedUpdate()
    {
        MovePlayer(); // 只要是物件移動，建議你放到FixedUpdate()
    }

    // 方法：取得目前玩家按方向鍵上下左右的數值
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // 計算移動方向(其實就是計算X軸與Z軸兩個方向的力量)
        moveDirection = PlayerCamera.forward * verticalInput + PlayerCamera.right * horizontalInput;
        // 推動第一人稱物件
        rbFirstPerson.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }
}
