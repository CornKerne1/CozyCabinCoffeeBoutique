using UnityEngine;
using UnityEngine.Playables;

public class SkeletonAlembic : MonoBehaviour
{
    private Animator _animator;

    public GameObject jumpIdle;
    private PlayableDirector _jumpIdleDirector;
    public GameObject jumpWalking;
    private PlayableDirector _jumpWalkingDirector;
    public GameObject jumpTalking;
    private PlayableDirector _jumpTalkingDirector;
    public GameObject jumpIntro;
    private PlayableDirector _jumpIntroDirector;

    private GameObject _currentObject;

    private void Start()
    {
        //animator = GetComponent<Animator>();
        //jumpIdleDirector = jumpIdle.GetComponent<PlayableDirector>();
        //jumpWalkingDirector = jumpWalking.GetComponent<PlayableDirector>();
        //jumpTalkingDirector = jumpTalking.GetComponent<PlayableDirector>();
        //jumpIntroDirector = jumpIntroDirector.GetComponent<PlayableDirector>();
    }

    private void Update()
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