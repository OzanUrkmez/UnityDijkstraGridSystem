using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateExecutor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.InitializeLateInitialization();
    }

}
