using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cmVC;
    CinemachineFramingTransposer cmFT;
    PlayerMover playerMover;
    PlayerAttacker playerAttacker;
    PlayerMover.UpDown upDown;
    Vector2 curDir;


    private void Awake()
    {
        playerMover = GetComponent<PlayerMover>();
        playerAttacker = GetComponent<PlayerAttacker>();
        curDir = playerMover.LookDir();
        cmFT = cmVC.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void FixedUpdate()
    { 
        HorizonCameraMoving();
        VerticalCameraMoving();
    }

    private void VerticalCameraMoving()
    {
        if (playerAttacker.IsAttack())
        {
            cmFT.m_TrackedObjectOffset.y = 0f;
            return;
        }

        switch (playerMover.LookingUpDown())
        {
            case PlayerMover.UpDown.Up:
                cmFT.m_TrackedObjectOffset.y = 15f;
                break;
            case PlayerMover.UpDown.Down:
                cmFT.m_TrackedObjectOffset.y = -15f;
                break;
            case PlayerMover.UpDown.None:
                cmFT.m_TrackedObjectOffset.y = 0f;
                break;
        }
    }

    private void HorizonCameraMoving()
    {
        if(playerMover.LookDir().x > 0)
        {
            cmFT.m_TrackedObjectOffset.x = 10f;
        }
        else if(playerMover.LookDir().x < 0)
        {
            cmFT.m_TrackedObjectOffset.x = -10f;
        }
    }

    // TODO : �Ĵٺ��� ���� = inputDir�� ���� body.x�� ������ �Ͽ� �ٶ󺸴� �������� �� ���� ���̰Բ�, �� �Ʒ� ����Ű�� ������ 1������ �Ŀ� body.y�� �������Ͽ� �ٶ󺸰Բ�
    // ���� ���� �� �� Ground, Wall�� üũ�Ͽ� �� ���� �ִ°� �ƴ϶�� Player�� ȭ�� ����� ��ġ, mapũ�⿡ �´� polygon collier�� confiner2D�� �����Ͽ� �ذ� ���� �� �� ����
    // �ʸ��� ���� ũ�⿡ �´� polygon collier ���� ���
    // �Ĵٺ��°Ŵ� �ð� 0.3f, ī�޶� ������ 1f
}
