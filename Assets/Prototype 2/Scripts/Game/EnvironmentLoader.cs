using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentLoader : MonoBehaviour
{
    GameObject Environment;
    
    public void LoadEnvironment(GameObject environment)
    {
        GameObject env = Instantiate(environment);
        env.transform.SetParent(transform, false);
        Environment = env;
    }
}
