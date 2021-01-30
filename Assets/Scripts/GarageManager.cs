using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageManager : MonoBehaviour, IStateManaged
{
    [SerializeField] private Camera mainCam;

    [SerializeField] private LayerMask boxLayer;
    [SerializeField] private LayerMask itemLayer;
    private LayerMask currentLayer;

    private StateMachine stateMachine = new StateMachine();
    public static GarageManager instance;

    private void Awake() {
        instance = this;
        stateMachine.ChangeState(new GarageState(this));
    }

    private void Update() {
        stateMachine.Update();
    }

    private void CheckMouseInput() {
        if (Input.GetMouseButtonDown(0))
            CheckClickables();
    }

    private void CheckClickables()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, currentLayer))
        {
            if (stateMachine.currentState is GarageState) {
                BoxManager.instance?.SetActiveBox(hit.collider.GetComponent<Box>());
                RequestState(new BoxState(this));
            }
            else if (stateMachine.currentState is BoxState) {
                BoxManager.instance?.SetActiveItem(hit.collider.GetComponent<Item>());
                RequestState(new ItemState(this));
            }
        }
    }

    private void SetLayer(LayerMask layer) => currentLayer = layer;

    public void ReturnBox() {
        if (stateMachine.currentState is ItemState) {
            BoxManager.instance?.ReturnItem();
            RequestState(new BoxState(this));
        }
        else if (stateMachine.currentState is BoxState) {
            BoxManager.instance?.ReturnBox();
            RequestState(new GarageState(this));
        }
    }

    #region States
    public void RequestState(IState requestedState) {
        if (requestedState is GarageState) {
            if (stateMachine.currentState is BoxState)
                stateMachine.ChangeState(requestedState);
        }

        if (requestedState is BoxState)
            stateMachine.ChangeState(requestedState);

        if (requestedState is ItemState) {
            if (stateMachine.currentState is BoxState)
                stateMachine.ChangeState(requestedState);
        }
    }

    private class GarageState : IState
    {
        public GarageManager manager;

        public GarageState(GarageManager manager) { this.manager = manager; }

        public void Enter() { 
            manager.SetLayer(manager.boxLayer);
        }

        public void Execute() {
            manager.CheckMouseInput();
        }

        public void Exit() { }
    }

    private class BoxState : IState
    {
        public GarageManager manager;
        public BoxState(GarageManager manager) { this.manager = manager; }

        public void Enter() {
            manager.SetLayer(manager.itemLayer);
        }
        public void Execute() {
            manager.CheckMouseInput();
        }

        public void Exit() { }
    }

    private class ItemState : IState
    {
        public GarageManager manager;
        public ItemState(GarageManager manager) { this.manager = manager; }

        public void Enter() { }
        public void Execute() { }
        public void Exit() { }
    }
    #endregion
}
