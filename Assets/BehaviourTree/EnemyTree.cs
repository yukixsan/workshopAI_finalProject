using UnityEngine;
using System.Collections.Generic;
using BehaviourTree;

public class EnemyTree : MonoBehaviour
{
    private Node root;
    [SerializeField] private Transform player;
    [SerializeField] private float detectRange = 10f;
    [SerializeField] private ParticleSystem spreadParticles;
    [SerializeField] private ParticleSystem rotateParticles;

    private void Start()
    {
        // Build the tree
        root = new SelectorNode(new List<Node>
        {
            new SequenceNode(new List<Node> // Attack Sequence
            {
                new IsPlayerInRange(transform, player, detectRange),
                new RandomSelector(new List<Node>
                {
                    new CooldownNode(new SpreadAttack(spreadParticles), 3f), // 2 sec cooldown
                    new CooldownNode(new RotateAttack(rotateParticles), 2f)
                })
            }),
            new PatrolNode(transform, new Vector3(5f, 0, 0), 2f) // Patrol if not in range
        });
    }

    private void Update()
    {
        root.Evaluate();
    }
}
