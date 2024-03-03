using UnityEngine;

namespace StateMachine
{
    public abstract class State<T> : ScriptableObject where T : MonoBehaviour
    {
        protected T machine;

        public virtual void Enter(T parent)
        {
            machine = parent;
        }

        public virtual void Tick() { }
        public virtual void FixedTick() {}
        public virtual void ChangeState() {}
        public virtual void Exit() {}
    }
}
