using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunDissolve : MonoBehaviour
{
    Material _Mat;
    float _Percentage;
    float duration = 1;
    bool _Fired = false;
    private void Start()
    {
        if(TryGetComponent(out MeshRenderer renderer))
        {
            _Mat = renderer.material;
        } else
        {
            Debug.Log("Whoops");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_Fired)
        {
            _Percentage += Time.deltaTime / duration;
            _Mat.SetFloat("_DissolveProgress", _Percentage);
            if(_Percentage > 1)
            {
                _Percentage = 0;
                _Fired = false;
            }
        }


        if (Input.GetKeyDown("space"))
        {
            _Percentage = 0;
            _Fired = true;
        }
    }
}
