using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeCameraShake : MonoBehaviour
{
    public bool isShaking = false;

    public IEnumerator TimedShake(float duration, float xMagnitude, float yMagnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f) * xMagnitude;
            float y = Random.Range(-1f, 1f) * yMagnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public IEnumerator Shake(float xMagnitude, float yMagnitude)
    {
        Vector3 originalPos = transform.localPosition;

        while (isShaking)
        {
            float x = Random.Range(-1f, 1f) * xMagnitude;
            float y = Random.Range(-1f, 1f) * yMagnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public void Stop()
    {
        isShaking = false;
    }
}
