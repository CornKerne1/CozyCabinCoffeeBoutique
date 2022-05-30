using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class SkeletonAlembic : MonoBehaviour
{

    Animator animator;

    public GameObject jumpIdle;
    PlayableDirector jumpIdleDirector;
    public GameObject jumpWalking;
    PlayableDirector jumpWalkingDirector;
    public GameObject jumpTalking;
    PlayableDirector jumpTalkingDirector;
    public GameObject jumpIntro;
    PlayableDirector jumpIntroDirector;

    private GameObject currentObject;

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        //jumpIdleDirector = jumpIdle.GetComponent<PlayableDirector>();
        //jumpWalkingDirector = jumpWalking.GetComponent<PlayableDirector>();
        //jumpTalkingDirector = jumpTalking.GetComponent<PlayableDirector>();
        //jumpIntroDirector = jumpIntroDirector.GetComponent<PlayableDirector>();


    }

    // Update is called once per frame
    void Update()
    {
        //if(animator.speed >.1 && jumpWalkingDirector.time == 0)
        //{
        //    currentObject.SetActive(false);
        //    jumpWalking.SetActive(true);
        //    currentObject = jumpWalking;
        //    jumpWalkingDirector.Play();
        //}
        //if (animator.speed < .1 && jumpWalkingDirector.time == 0)
        //{
        //    currentObject.SetActive(false);
        //    jumpIdle.SetActive(true);
        //    currentObject = jumpIdle;
        //    jumpIdleDirector.Play();
        //}
    }
}
