using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [Header("參考物件")]
    public Camera PlayerCam;
    public Transform FirePos;
    public Rigidbody PlayerRB;

    [Header("子彈預置物件")]
    public GameObject bullet;

    [Header("槍枝設定")]
    public int magazineSize;        // 設定彈夾可以放多少顆子彈？
    public int bulletsLeft;         // 子彈還有多少顆？(如果沒有要測試，你可以設定成 Private)
    public float reloadTime;        // 設定換彈夾所需要的時間
    public float recoilForce;       // 反作用力

    float fireTime;                 // 控制射擊間隔
    public float nextFireTime = 0.1f; // 射擊間隔時間

    bool reloading;                 // 布林變數：儲存是不是正在換彈夾的狀態？True：正在換彈夾、False：換彈夾的動作已結束

    [Header("UI物件")]
    public TextMeshProUGUI ammunitionDisplay; // 彈量顯示
    public TextMeshProUGUI reloadingDisplay;  // 顯示是不是正在換彈夾？

    private void Start()
    {
        bulletsLeft = magazineSize;        // 遊戲一開始彈夾設定為全滿狀態
        reloadingDisplay.enabled = false;  // 將顯示正在換彈夾的字幕隱藏起來

        ShowAmmoDisplay();                 // 更新彈量顯示

        fireTime = 0f; // 初始化fireTime
    }

    private void Update()
    {
        MyInput();
        if (fireTime > 0) fireTime -= Time.deltaTime;
    }

    // 方法：偵測玩家操作狀態
    private void MyInput()
    {
        // 判斷：有沒有按下左鍵？
        if (Input.GetMouseButton(0))
        {
            // 如果還有子彈，並且沒有正在重裝子彈，就可以射擊
            if (bulletsLeft > 0 && !reloading)
            {
                if (fireTime <= 0)
                {
                    Shoot();
                    fireTime = nextFireTime; // 重置fireTime
                }
            }
        }

        // 判斷：1.有按下R鍵、2.子彈數量低於彈夾內的彈量、3.不是換彈夾的狀態，三個條件都滿足，就可以換彈夾
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading ||bulletsLeft==0)
            Reload();
    }

    // 方法：射擊武器
    private void Shoot()
    {
        Ray ray = PlayerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));  // 從攝影機射出一條射線
        RaycastHit hit;  // 宣告一個射擊點
        Vector3 targetPoint;  // 宣告一個位置點變數，到時候如果有打到東西，就存到這個變數

        // 如果射線有打到具備碰撞體的物件
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;         // 將打到物件的位置點存進 targetPoint
        else
            targetPoint = ray.GetPoint(75);  // 如果沒有打到物件，就以長度75的末端點取得一個點，存進 targetPoint

        Debug.DrawRay(ray.origin, targetPoint - ray.origin, Color.red, 2f); // 畫出這條射線

        Vector3 shootingDirection = targetPoint - FirePos.position; // 以起點與終點之間兩點位置，計算出射線的方向
        GameObject currentBullet = Instantiate(bullet, FirePos.position, Quaternion.identity); // 在攻擊點上面產生一個子彈
        currentBullet.transform.forward = shootingDirection.normalized; // 將子彈飛行方向與射線方向一致

        currentBullet.GetComponent<Rigidbody>().AddForce(currentBullet.transform.forward * 20, ForceMode.Impulse); // 依據飛行方向推送子彈

        bulletsLeft--;    // 將彈夾中的子彈減一

        // 後座力模擬
        PlayerRB.AddForce(-shootingDirection.normalized * recoilForce, ForceMode.Impulse);

        ShowAmmoDisplay();                 // 更新彈量顯示
    }

    // 方法：換彈夾的延遲時間設定
    private void Reload()
    {
        reloading = true;                      // 首先將換彈夾狀態設定為：正在換彈夾
        reloadingDisplay.enabled = true;       // 將正在換彈夾的字幕顯示出來
        Invoke("ReloadFinished", reloadTime);  // 依照reloadTime所設定的換彈夾時間倒數，時間為0時執行ReloadFinished方法
    }

    // 方法：換彈夾
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;            // 將子彈填滿
        reloading = false;                     // 將換彈夾狀態設定為：更換彈夾結束
        reloadingDisplay.enabled = false;      // 將正在換彈夾的字幕隱藏，結束換彈夾的動作
        ShowAmmoDisplay();
    }

    // 方法：更新彈量顯示
    private void ShowAmmoDisplay()
    {
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText($"Ammo {bulletsLeft} / {magazineSize}");
    }
}