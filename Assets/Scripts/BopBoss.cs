using System;
using System.Collections;
using UnityEngine;

public class BopBoss : MonoBehaviour
{
    [SerializeField] bool testBeat;
    [SerializeField] float bopSize = 0.85f;
    [SerializeField] float returnSpeed = 5f;
    private Vector2 startSize;

    private void Start()
    {
        startSize = transform.localScale;
        if (testBeat)
        {
            StartCoroutine(TestBeat());
        }
    }
    private void Update()
    {
        transform.localScale = Vector2.Lerp(transform.localScale, startSize, Time.deltaTime * returnSpeed);
    }
    public void Pulse()
    {
        transform.localScale = startSize * bopSize;
    }

    private IEnumerator TestBeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Pulse();
        }
    }
}
