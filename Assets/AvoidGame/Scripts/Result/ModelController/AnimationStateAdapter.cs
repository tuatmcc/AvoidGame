using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AnimationStateAdapter : MonoBehaviour
{
    private Animator animator;
    [Inject] ResultSceneManager _sceneManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        var _data = _sceneManager.GetTimeData();
        ChangeAnimation(_data.timeList, _data.playerTime);
    }

    private void ChangeAnimation(List<long> timeList, long time)
    {
        animator?.SetInteger("Ranking", timeList.IndexOf(time) + 1);
    }

}
