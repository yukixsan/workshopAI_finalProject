using UnityEngine;
using DG.Tweening;

public class SecondIntroState : State
{
    protected BossTwoMachine stateMachine;
    [SerializeField] private int duration;
    private float timer;

    private void Awake()
    {
        stateMachine = GetComponent<BossTwoMachine>();
    }

    public override void Enter()
    {
        timer = duration;
        transform.DOPunchScale(new Vector3(5, 5, 5), duration, 5, 1);
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            stateMachine.ChangeState<CirclingState>();
        }
    }
}
