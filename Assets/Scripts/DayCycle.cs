using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    [SerializeField] private float _dayDurationInSec = 20f;
    [SerializeField][Range(0, 1)] private float _timeOfDay;
    [SerializeField] private Light _sun;
    [SerializeField] private Light _moon;
    [SerializeField] private AnimationCurve _sunIntensityCurve;
    [SerializeField] private AnimationCurve _moonIntensityCurve;
    [SerializeField] private AnimationCurve _skyboxCurve;
    [SerializeField] private Material _daySkybox;
    [SerializeField] private Material _nightSkybox;


    private float _sunIntensity;
    private float _moonIntensity;
    

    #region MONO
    private void Start()
    {
        _sunIntensity = _sun.intensity;
        _moonIntensity = _moon.intensity;
    }
    #endregion
    private void Update()
    {
        ChangeCurrentTime();
        MoveLighter(_sun);
        MoveLighter(_moon, 180f);
        ChangeIntensity();
        ChangePeriodState();
        
    }
    private void ChangeCurrentTime()
    {
        _timeOfDay += Time.deltaTime / _dayDurationInSec;
        if (_timeOfDay >= 1)
            _timeOfDay = 0;
    }
    private void MoveLighter(Light light,float offset = 0)
    {
        light.gameObject.transform.localRotation = Quaternion.Euler(_timeOfDay * 360f + offset, 180, 0);
    }
    private void ChangeIntensity()
    {
        _sun.intensity = _sunIntensityCurve.Evaluate(_timeOfDay) * _sunIntensity;
        _moon.intensity = _moonIntensityCurve.Evaluate(_timeOfDay) * _moonIntensity;
    }
    private void ChangePeriodState()
    {
        RenderSettings.skybox.Lerp(_nightSkybox, _daySkybox, _skyboxCurve.Evaluate(_timeOfDay));
        RenderSettings.sun = _skyboxCurve.Evaluate(_timeOfDay) > 0.1f ? _sun : _moon;
        DynamicGI.UpdateEnvironment();
    }

}
