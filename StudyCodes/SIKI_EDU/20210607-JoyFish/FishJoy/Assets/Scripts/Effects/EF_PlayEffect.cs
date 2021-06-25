using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EF_PlayEffect : MonoBehaviour
{
    public GameObject[] effects;

    public void PlayEffect()
    {
        foreach (var effect in effects)
        {
            Instantiate(effect);
        }
    }
}
