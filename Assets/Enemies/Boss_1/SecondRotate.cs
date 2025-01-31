using UnityEngine;
using DG.Tweening;

public class SecondRotate : State
{

    [SerializeField] private ParticleSystem _rotateParticle;
    [SerializeField] private int duration;

    protected StateMachine stateMachine;
    private float timer;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
    }

    public override void Enter()
    {
        timer = duration;
        print("Entering phase 2 rotate");
        if(_rotateParticle != null)
        {
            _rotateParticle.gameObject.SetActive(true);
            _rotateParticle.Play();           
        }
        transform.rotation = Quaternion.identity;

        transform.DORotate(new Vector3(0, 360, 0), timer, RotateMode.FastBeyond360).SetEase(Ease.Flash).SetLoops(1, LoopType.Incremental);
    }

    public override void Exit()
    {
        print("Exiting rotate 2");
        if (_rotateParticle != null)
        {
            
            _rotateParticle.Stop();
        }
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        
        if (timer <= 0f )
        {
            stateMachine.ChangeState<SecondBurst>();
        }
    }
}
