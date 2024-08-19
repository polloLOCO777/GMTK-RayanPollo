using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineMachineMovimientoCamara : MonoBehaviour
{
    public static CineMachineMovimientoCamara Instance;

    CinemachineVirtualCamera cinemachineVirtualCamera;

    CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlin;

    float TiempoMovimiento;

    float tiempoMovimientoTotal;
    float intensidadInicial;

    void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void MoverCamara(float intensidad, float frecuencia, float tiempo)
    {
        CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensidad;
        CinemachineBasicMultiChannelPerlin.m_FrequencyGain = frecuencia;
        intensidadInicial = intensidad;
        tiempoMovimientoTotal = tiempo;
        TiempoMovimiento = tiempo;
    }

    void Update()
    {
        if (TiempoMovimiento > 0)
        {
            TiempoMovimiento -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(intensidadInicial, 0, 1 - (TiempoMovimiento / tiempoMovimientoTotal));
        }
    }
}
