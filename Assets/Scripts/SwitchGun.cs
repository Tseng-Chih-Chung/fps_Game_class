using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGun : MonoBehaviour
{
    [Header("�Z��")]
    public GameObject[] weaponObjects;        // �Z���M��

    int weaponNumber = 0;                     // �ثe��ܪZ�������ǽs��
    GameObject weaponInUse;                   // �ثe��ܪZ��

    void Start()
    {
        // ���éҦ��Z��
        foreach (GameObject item in weaponObjects)
        {
            item.SetActive(false);
        }

        // �C���@�}�l�]�w�Z������0�ӪZ��
        weaponNumber = 0;
        weaponInUse = weaponObjects[weaponNumber];
        weaponInUse.SetActive(true); // ��ܷ�e�Z��
    }

    // ��k�G�������a�ާ@���A
    private void MyInput()
    {
        // �P�_�G�u�ʷƹ��u��
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)      // ���e�u��
            SwitchWeapon(1);
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // ����u��
            SwitchWeapon(-1);
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    // ��k�G�Z�������A�Ѽ�_addNumber�B_weaponNumber
    private void SwitchWeapon(int _addNumber, int _weaponNumber = 0)
    {
        // ���éҦ��Z��
        foreach (GameObject item in weaponObjects)
        {
            item.SetActive(false);
        }

        // switch �P�_���G�H�Ѽ�_addNumber�P�_�n�������Z��
        switch (_addNumber)
        {
            case 0:                                                   // _addNumber == 0�A�N��Ϋ��䪽�����w�Z���}�C��}
                weaponNumber = _weaponNumber;
                break;
            case 1:                                                   // _addNumber == 1�A�N���W�u�ƹ��u��
                if (weaponNumber == weaponObjects.Length - 1)         // ��{�`���Ʀr�A���w�쥻���Z���}�C��}�w�g�O�̫�@�ӪZ���A�h�N�Z���}�C��}�]�w��0
                    weaponNumber = 0;
                else
                    weaponNumber += 1;
                break;
            case -1:                                                   // _addNumber == -1�A�N���U�u�ƹ��u��
                if (weaponNumber == 0)                                 // ��{�`���Ʀr�A���w�쥻���Z���}�C��}�O�Ĥ@�ӪZ���A�h�N�Z���}�C��}���M�檺�̫�@�Ӧ�}
                    weaponNumber = weaponObjects.Length - 1;
                else
                    weaponNumber -= 1;
                break;
        }

        weaponObjects[weaponNumber].SetActive(true);    // ��ܩҫ��w���Z��
        weaponInUse = weaponObjects[weaponNumber];      // �]�w�ثe�ҿ�ܪ��Z������(���ɥi�H�ΨӰ���Z���үS�w����k�A�U�@���`�|����)
    }
}