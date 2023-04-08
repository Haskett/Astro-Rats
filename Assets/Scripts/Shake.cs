using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public IEnumerator CameraShake(float _duration, float _magnitude)
    {
        Vector3 _orignalPosition = transform.position;
        float _timeElapsed = 0f;

        while (_timeElapsed < _duration)
        {
            float x = Random.Range(-1f, 1f) * _magnitude;
            float y = Random.Range(-1f, 1f) * _magnitude;

            transform.position = new Vector3(x, y, -10f);
            _timeElapsed += Time.deltaTime;
            yield return 0;
        }
        transform.position = _orignalPosition;
    }
}
