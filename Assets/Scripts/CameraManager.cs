using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] CinemachineBasicMultiChannelPerlin perlin;

    [SerializeField] float shakeTime;
    [SerializeField] float shakeMagnitude;

    float timer;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        StopShake();
    }
    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if(timer < 0)
        {
            StopShake();
        }
    }
    public void ShakeCamera()
    {
        perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = shakeMagnitude;

        timer = shakeTime;
    }

    void StopShake()
    {
        perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = 0f;
        timer = 0.0f;
    }
}
