﻿using Extensions;
using UnityEngine;

namespace Tools.UI.Card
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(IMouseInput))]
    public class UiCardComponent : MonoBehaviour, IUiCard
    {
        //--------------------------------------------------------------------------------------------------------------

        #region Unity Callbacks

        void OnEnable()
        {
            //components

            MyTransform = transform;
            MyCollider = GetComponent<Collider>();
            MyRigidbody = GetComponent<Rigidbody>();
            MyInput = GetComponent<IMouseInput>();
            Hand = transform.parent.GetComponentInChildren<IUiPlayerHand>();
            MyRenderers = GetComponentsInChildren<SpriteRenderer>();
            MyRenderer = GetComponent<SpriteRenderer>();

            //transform
            Scale = new UiMotionScaleCard(this);
            Movement = new UiMotionMovementCard(this);
            Rotation = new UiMotionRotationCard(this);

            //fsm
            Fsm = new UiCardHandFsm(MainCamera, cardConfigsParameters, this);
            
        }

        void Update()
        {


            Fsm?.Update();
            Movement?.Update();
            Rotation?.Update();
            Scale?.Update();

        }

        #endregion

        //--------------------------------------------------------------------------------------------------------------

        #region Components

        SpriteRenderer[] IUiCardComponents.Renderers => MyRenderers;
        SpriteRenderer IUiCardComponents.Renderer => MyRenderer;
        Collider IUiCardComponents.Collider => MyCollider;
        Rigidbody IUiCardComponents.Rigidbody => MyRigidbody;
        IMouseInput IUiCardComponents.Input => MyInput;
        IUiPlayerHand IUiCard.Hand => Hand;

        #endregion

        #region Transform

        public UiMotionBaseCard Movement { get; private set; }
        public UiMotionBaseCard Rotation { get; private set; }
        public UiMotionBaseCard Scale { get; private set; }

        #endregion

        #region Properties

        public string Name => gameObject.name;
        [SerializeField] public UiCardParameters cardConfigsParameters;
        UiCardHandFsm Fsm { get; set; }
        Transform MyTransform { get; set; }
        Collider MyCollider { get; set; }
        SpriteRenderer[] MyRenderers { get; set; }
        SpriteRenderer MyRenderer { get; set; }
        Rigidbody MyRigidbody { get; set; }
        IMouseInput MyInput { get; set; }
        IUiPlayerHand Hand { get; set; }
        public MonoBehaviour MonoBehavior => this;
        public Camera MainCamera => Camera.main;
        public bool IsDragging => Fsm.IsCurrent<UiCardDrag>();
        public bool IsHovering => Fsm.IsCurrent<UiCardHover>();
        public bool IsDisabled => Fsm.IsCurrent<UiCardDisable>();
        public bool IsPlayer => transform.CloserEdge(MainCamera, Screen.width, Screen.height) == 1;

        public int daño;
        public int absorber;
        [SerializeField] AudioClip au_PasarClick, au_Discard;

        #endregion

        //--------------------------------------------------------------------------------------------------------------

        #region Transform

        public void RotateTo(Vector3 rotation, float speed) => Rotation.Execute(rotation, speed);

        public void MoveTo(Vector3 position, float speed, float delay) => Movement.Execute(position, speed, delay);

        public void MoveToWithZ(Vector3 position, float speed, float delay) =>
            Movement.Execute(position, speed, delay, true);

        public void ScaleTo(Vector3 scale, float speed, float delay) => Scale.Execute(scale, speed, delay);

        #endregion

        //--------------------------------------------------------------------------------------------------------------

        #region Operations

        public void Hover() 
        {
            AudioManager.Instance.Play(au_PasarClick);
            Fsm.Hover();
        } 

        public void Disable() => Fsm.Disable();

        public void Enable() => Fsm.Enable();

        public void Select()
        {
            // to avoid the player selecting enemy's cards
            if (!IsPlayer)
                return;

                Hand.SelectCard(this);
                Fsm.Select();
            
        }

        public void Unselect() => Fsm.Unselect();

        public void Draw() => Fsm.Draw();

        public void Discard()
        {
            
            AudioManager.Instance.Play(au_Discard);
            SistemaDeTurnos.Instance.DañoAlEnemigo(this.gameObject);
            Fsm.Discard();
            
        }  

        public void Set_Daño(int nuevoDaño, int nuevoAbs)
        {
            daño = nuevoDaño;
            absorber = nuevoAbs;
        }

        public int Get_Daño()
        {
            return daño;
        }

        public int Get_Absorber()
        {
            return absorber;
        }

        #endregion

        //--------------------------------------------------------------------------------------------------------------
    }
}