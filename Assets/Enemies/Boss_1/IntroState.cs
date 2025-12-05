using UnityEngine;
using DG.Tweening;

public class IntroState : State
{
    [SerializeField] private int duration;
    private float timer;

    protected StateMachine stateMachine;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
    }

    public override void Enter()
    {
        timer = duration;
        transform.DOPunchScale(new Vector3(5, 5, 5), duration, 10, 1);
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            stateMachine.ChangeState<SpreadState>();
        }
    }
}

