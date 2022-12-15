using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{

    private int sortingOrderBase = 0;
    private Renderer _renderer;


    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }



    private void LateUpdate()
    {
        _renderer.sortingOrder = (int)(sortingOrderBase - transform.position.y);
    }
}
