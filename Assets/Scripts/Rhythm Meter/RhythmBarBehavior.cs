using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmBarBehavior : MonoBehaviour
{
    private GameObject _downBeatEndObject;

    void Awake()
    {
        _downBeatEndObject = GameObject.Find("Center Visual");
        StartCoroutine(Move(transform.position, _downBeatEndObject, 2.5f));
        Invoke("Destroy", 2.5f);
    }

    
    private IEnumerator Move(Vector3 beginPos, GameObject endPos, float time)
    {
        for (float t = 0; t < 1; t += Time.deltaTime / time)
        {
            transform.position = Vector3.Lerp(beginPos, endPos.transform.position, t);
            yield return null;
        }
    }
    

    private void Destroy()
    {
        Destroy(gameObject);
    }
}

