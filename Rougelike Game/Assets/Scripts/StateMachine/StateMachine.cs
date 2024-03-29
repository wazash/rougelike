﻿using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StateMachine
{
    public abstract class StateMachine<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private List<State<T>> states;
        [ShowInInspector] private State<T> activeState;
        [ShowInInspector] private State<T> previousState;

        public List<State<T>> MachineStates { get => states; set => states = value; }
        public State<T> ActiveState { get => activeState; set => activeState = value; }
        public State<T> PreviousState { get => previousState; set => previousState = value; }

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

            previousState = activeState;
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

        protected State<T> GetStateByName(string stateName) => states.First(state => state.GetType().Name == stateName);
    }
}
