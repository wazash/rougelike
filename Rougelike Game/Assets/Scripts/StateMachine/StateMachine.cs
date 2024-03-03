using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StateMachine
{
    public abstract class StateMachine<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private List<State<T>> states;
        private State<T> activeState;

        public State<T> ActiveState { get => activeState; set => activeState = value; }

        protected virtual void Awake()
        {
            SetState(states[0].GetType());
        }

        public void SetState(Type newStateType)
        {
            if(activeState != null)
            {
                activeState.Exit();
            }

            activeState = states.First(state => state.GetType() == newStateType);
            activeState.Enter(GetComponent<T>());
        }

        private void Update()
        {
            activeState.Tick();
            activeState.ChangeState();
        }

        private void FixedUpdate()
        {
            activeState.FixedTick();
        }
    }
}
