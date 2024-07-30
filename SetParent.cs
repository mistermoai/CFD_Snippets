using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParent : MonoBehaviour
{
    public GameObject[] detailedBuildings;
    public GameObject[] simplifiedMeshes;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < detailedBuildings.Length; i++)
            Debug.Log(i);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
