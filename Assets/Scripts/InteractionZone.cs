using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    TPSControllerDani _controller;

    void Awake()
    {
        _controller = GetComponentInParent<TPSControllerDani>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Recogible")
        {
            _controller.objectToGrab = collider.gameObject;
        }
    }

        void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "Recogible")
        {
            _controller.objectToGrab = null;
        }
    }
}
