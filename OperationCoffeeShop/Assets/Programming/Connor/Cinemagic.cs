using UnityEngine;
using Cinemachine;

public class Cinemagic : MonoBehaviour
{
    public CinemachineVirtualCamera vcam4;


    public void HoldCamera4()
    {
        vcam4.Priority = 500;
    }
}