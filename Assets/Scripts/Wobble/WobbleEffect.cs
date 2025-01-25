using System.Collections;
using UnityEngine;

public class WobbleEffect : MonoBehaviour
{
    [Header("Material")]
    public Material wobbleEffectMaterial;

    [Header("Wobble Effect")]
    [SerializeField] private bool  wobbleActive = false;
    [SerializeField] private float frequency = 4f;
    [SerializeField] private float shift = 0f;
    [SerializeField] private float amplitude = 0f;
    [SerializeField] private float maxAmplitude = 0.01f;
    [SerializeField] private float shiftSpeed = 5f;
    [SerializeField] private float amplitudeSpeed = 0.025f;

    //private void OnRenderImage(RenderTexture source, RenderTexture destination)
    //{
    //    Graphics.Blit(source, destination, wobbleEffectMaterial);
    //}

    private void SetFrequency(float frequency)
    {
        wobbleEffectMaterial.SetFloat("_frequency", frequency);
    }

    private void SetShift(float shift)
    {
        wobbleEffectMaterial.SetFloat("_shift", shift);
    }

    private void SetAmplitude(float amplitude)
    {
        wobbleEffectMaterial.SetFloat("_amplitude", amplitude);
    }

    public void StartWobble()
    {
        if (!wobbleActive) 
        {
            wobbleActive = true;
            StartCoroutine(WobbleCoroutine());
        }
    }

    public void StopWobble()
    {
        wobbleActive = false;
    }

    private IEnumerator WobbleCoroutine()
    {
        SetFrequency(frequency);

        while (amplitude < maxAmplitude) 
        {
            if(wobbleActive)
            {
                SetAmplitude(amplitude);
                SetShift(shift);

                amplitude += amplitudeSpeed * Time.deltaTime;
                shift += shiftSpeed * Time.deltaTime;
                shift %= Mathf.PI * 2f;

                yield return null;
            }
            else
            {
                break;
            }
        }

        if(wobbleActive)
        {
            amplitude = maxAmplitude;
            SetAmplitude(amplitude);
        }

        while (wobbleActive)
        {
            SetShift(shift);
            shift += shiftSpeed * Time.deltaTime;
            shift %= Mathf.PI * 2f;

            yield return null;
        }

        while (amplitude > 0f)
        {
            if(!wobbleActive)
            {
                SetAmplitude(amplitude);
                SetShift(shift);

                amplitude -= amplitudeSpeed * Time.deltaTime;
                shift += shiftSpeed * Time.deltaTime;
                shift %= Mathf.PI * 2f;

                yield return null;
            }
            else
            {
                break;
            }
        }

        if (!wobbleActive)
        {
            amplitude = 0f;
            shift = 0f;
            SetAmplitude(amplitude);
            SetShift(shift);
        }
    }
}