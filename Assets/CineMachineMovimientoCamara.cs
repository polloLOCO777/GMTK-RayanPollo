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
        => BlockGone.OnProxyActionEventHandler += HandleProxyAction;

    private void OnDisable()
        => BlockGone.OnProxyActionEventHandler -= HandleProxyAction;


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

    void HandleProxyAction(object sender, BlockGone.ProxyEventArgs e)
    {
        if (e.action == BlockGone.ProxyEventArgs.ActionType.Disappear)
            MoverCamara(1, 1, 0.5f);
    }
}
