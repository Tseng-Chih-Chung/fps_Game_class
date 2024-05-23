using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [Header("�ѦҪ���")]
    public Camera PlayerCam;
    public Transform FirePos;
    public Rigidbody PlayerRB;

    [Header("�l�u�w�m����")]
    public GameObject bullet;

    [Header("�j�K�]�w")]
    public int magazineSize;        // �]�w�u���i�H��h�����l�u�H
    public int bulletsLeft;         // �l�u�٦��h�����H(�p�G�S���n���աA�A�i�H�]�w�� Private)
    public float reloadTime;        // �]�w���u���һݭn���ɶ�
    public float recoilForce;       // �ϧ@�ΤO

    float fireTime;                 // ����g�����j
    public float nextFireTime = 0.1f; // �g�����j�ɶ�

    bool reloading;                 // ���L�ܼơG�x�s�O���O���b���u�������A�HTrue�G���b���u���BFalse�G���u�����ʧ@�w����

    [Header("UI����")]
    public TextMeshProUGUI ammunitionDisplay; // �u�q���
    public TextMeshProUGUI reloadingDisplay;  // ��ܬO���O���b���u���H

    private void Start()
    {
        bulletsLeft = magazineSize;        // �C���@�}�l�u���]�w���������A
        reloadingDisplay.enabled = false;  // �N��ܥ��b���u�����r�����ð_��

        ShowAmmoDisplay();                 // ��s�u�q���

        fireTime = 0f; // ��l��fireTime
    }

    private void Update()
    {
        MyInput();
        if (fireTime > 0) fireTime -= Time.deltaTime;
    }

    // ��k�G�������a�ާ@���A
    private void MyInput()
    {
        // �P�_�G���S�����U����H
        if (Input.GetMouseButton(0))
        {
            // �p�G�٦��l�u�A�åB�S�����b���ˤl�u�A�N�i�H�g��
            if (bulletsLeft > 0 && !reloading)
            {
                if (fireTime <= 0)
                {
                    Shoot();
                    fireTime = nextFireTime; // ���mfireTime
                }
            }
        }

        // �P�_�G1.�����UR��B2.�l�u�ƶq�C��u�������u�q�B3.���O���u�������A�A�T�ӱ��󳣺����A�N�i�H���u��
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading ||bulletsLeft==0)
            Reload();
    }

    // ��k�G�g���Z��
    private void Shoot()
    {
        Ray ray = PlayerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));  // �q��v���g�X�@���g�u
        RaycastHit hit;  // �ŧi�@�Ӯg���I
        Vector3 targetPoint;  // �ŧi�@�Ӧ�m�I�ܼơA��ɭԦp�G������F��A�N�s��o���ܼ�

        // �p�G�g�u�������ƸI���骺����
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;         // �N���쪫�󪺦�m�I�s�i targetPoint
        else
            targetPoint = ray.GetPoint(75);  // �p�G�S�����쪫��A�N�H����75�������I���o�@���I�A�s�i targetPoint

        Debug.DrawRay(ray.origin, targetPoint - ray.origin, Color.red, 2f); // �e�X�o���g�u

        Vector3 shootingDirection = targetPoint - FirePos.position; // �H�_�I�P���I�������I��m�A�p��X�g�u����V
        GameObject currentBullet = Instantiate(bullet, FirePos.position, Quaternion.identity); // �b�����I�W�����ͤ@�Ӥl�u
        currentBullet.transform.forward = shootingDirection.normalized; // �N�l�u�����V�P�g�u��V�@�P

        currentBullet.GetComponent<Rigidbody>().AddForce(currentBullet.transform.forward * 20, ForceMode.Impulse); // �̾ڭ����V���e�l�u

        bulletsLeft--;    // �N�u�������l�u��@

        // ��y�O����
        PlayerRB.AddForce(-shootingDirection.normalized * recoilForce, ForceMode.Impulse);

        ShowAmmoDisplay();                 // ��s�u�q���
    }

    // ��k�G���u��������ɶ��]�w
    private void Reload()
    {
        reloading = true;                      // �����N���u�����A�]�w���G���b���u��
        reloadingDisplay.enabled = true;       // �N���b���u�����r����ܥX��
        Invoke("ReloadFinished", reloadTime);  // �̷�reloadTime�ҳ]�w�����u���ɶ��˼ơA�ɶ���0�ɰ���ReloadFinished��k
    }

    // ��k�G���u��
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;            // �N�l�u��
        reloading = false;                     // �N���u�����A�]�w���G�󴫼u������
        reloadingDisplay.enabled = false;      // �N���b���u�����r�����áA�������u�����ʧ@
        ShowAmmoDisplay();
    }

    // ��k�G��s�u�q���
    private void ShowAmmoDisplay()
    {
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText($"Ammo {bulletsLeft} / {magazineSize}");
    }
}