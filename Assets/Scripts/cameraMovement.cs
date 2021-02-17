using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class cameraMovement : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCam;
    private CinemachineOrbitalTransposer transposer;
    public float cameraStep = 90;
    public float time = .5f;
    public float recenterTime;
    public float recenterStep;

    private void Start()
    {
        transposer = virtualCam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
            DOVirtual.Float(transposer.m_XAxis.Value, transposer.m_XAxis.Value + cameraStep, time, SetCameraAxis).SetEase(Ease.OutSine);
        if (Input.GetKey(KeyCode.Q))
            DOVirtual.Float(transposer.m_XAxis.Value, transposer.m_XAxis.Value - cameraStep, time, SetCameraAxis).SetEase(Ease.OutSine);
        if (Input.GetKeyDown(KeyCode.Tab))
            DOVirtual.Float(transposer.m_XAxis.Value, transposer.m_XAxis.Value = 0, recenterTime, SetCameraAxis).SetEase(Ease.OutSine);
    }

    void SetCameraAxis(float x)
    {
        transposer.m_XAxis.Value = x;
    }
}
