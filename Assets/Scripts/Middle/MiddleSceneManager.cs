using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject playButton;

    void Update()
    {
        playButton.transform.Rotate(0, (float)(24 * Time.deltaTime), 0);
    }
}
