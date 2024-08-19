using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineMachineMovimientoCamara : MonoBehaviour
{
    CinemachineVirtualCamera cinemachineVirtualCamera;

    CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlin;

    float TiempoMovimiento;

    float tiempoMovimientoTotal;
    float intensidadInicial;

    private void OnEnable()
        => BlockGone.OnProxyDisappearEventHandler += HandleBlockDissapear;
    
    private void OnDisable()
        => BlockGone.OnProxyDisappearEventHandler -= HandleBlockDissapear;
    

    void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (TiempoMovimiento > 0)
        {
            TiempoMovimiento -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(intensidadInicial, 0, 1 - (TiempoMovimiento / tiempoMovimientoTotal));
        }
    }

    public void MoverCamara(float intensidad, float frecuencia, float tiempo)
    {
        CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensidad;
        CinemachineBasicMultiChannelPerlin.m_FrequencyGain = frecuencia;
        intensidadInicial = intensidad;
        tiempoMovimientoTotal = tiempo;
        TiempoMovimiento = tiempo;
    }

    void HandleBlockDissapear(object sender, BlockGone.ProxyDisappearEventArgs e)
        => MoverCamara(1, 1, 0.5f);
}
