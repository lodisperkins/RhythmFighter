using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RhythmBarBehavior : MonoBehaviour
{
    private Vector3 _downBeatEndPosition = new Vector3(814, 61, 0);

    void Awake()
    {
        StartCoroutine(Move(transform.position, _downBeatEndPosition, 2.5f));
        Invoke("Destroy", 2.5f);
    }

    private IEnumerator Move(Vector3 beginPos, Vector3 endPos, float time)
    {
        for (float t = 0; t < 1; t += Time.deltaTime / time)
        {
            transform.position = Vector3.Lerp(beginPos, endPos, t);
            yield return null;
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}

