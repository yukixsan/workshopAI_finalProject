using UnityEngine;


namespace BehaviourTree
{
   
    public abstract class Node 
    {
        public enum NodeState
        {
            Running,
            Success,
            Failure
        }

        protected NodeState state;
        public NodeState CurrentState => state;

        public abstract NodeState Evaluate();


    }
}


