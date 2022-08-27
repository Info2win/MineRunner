using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOldLevels : MonoBehaviour
{
    void OnTriggerExit (Collider other)
    {
        Destroy(transform.parent.gameObject);
        //transform.parent.gameObject.SetActive(false);
    }
}
