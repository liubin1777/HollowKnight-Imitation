using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vc;
    // [SerializeField] CinemachineComposer cm;

    CinemachineFramingTransposer aa;

    private void Awake()
    {
        aa = vc.GetCinemachineComponent<CinemachineFramingTransposer>();
        aa.m_TrackedObjectOffset.y = 0f;
    }

    private void Update()
    {

    }

    private void LookUpDown()
    {
        aa = vc.GetCinemachineComponent<CinemachineFramingTransposer>();
        aa.m_TrackedObjectOffset.y = 50f;
        //cm.m_TrackedObjectOffset.y = 50f;
    }

    // TODO : �Ĵٺ��� ���� = inputDir�� ���� body.x�� ������ �Ͽ� �ٶ󺸴� �������� �� ���� ���̰Բ�, �� �Ʒ� ����Ű�� ������ 1������ �Ŀ� body.y�� �������Ͽ� �ٶ󺸰Բ�
    // ���� ���� �� �� Ground, Wall�� üũ�Ͽ� �� ���� �ִ°� �ƴ϶�� Player�� ȭ�� ����� ��ġ, mapũ�⿡ �´� polygon collier�� confiner2D�� �����Ͽ� �ذ� ���� �� �� ����
    // �ʸ��� ���� ũ�⿡ �´� polygon collier ���� ���
    // �Ĵٺ��°Ŵ� 0.3f, ī�޶� ������ 1f
}
