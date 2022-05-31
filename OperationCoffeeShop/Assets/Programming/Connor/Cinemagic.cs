using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Cinemagic : MonoBehaviour
{
    public CinemachineVirtualCamera vcam4;



    public void holdCamera4()
    {
        vcam4.Priority = 500;
    }
}
