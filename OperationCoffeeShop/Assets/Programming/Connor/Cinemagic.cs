using UnityEngine;
using Cinemachine;
using UnityEngine.Serialization;

public class Cinemagic : MonoBehaviour
{
    [FormerlySerializedAs("vcam4")] public CinemachineVirtualCamera virtualCamera4;


    public void HoldCamera4()
    {
        virtualCamera4.Priority = 500;
    }
}