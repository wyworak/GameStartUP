using System;
using UnityEngine;
using Gameplay.Attribute;
using Gameplay.Unit.Attack;
using Gameplay.Unit.Movement;
using UnityEngine.Networking;

namespace Gameplay.Unit
{
    [RequireComponent(typeof(BaseMovement))]
    public class BaseUnit : NetworkBehaviour, IHitByBullet
    {
        private AttributePool attributePool;
        private BaseMovement baseMovement;
        protected HitInformation lastHitInformation;

        public AttributePool AttributePool
        {
            get { return attributePool; }
        }


        protected virtual void Awake()
        {
            baseMovement = GetComponent<BaseMovement>();
            attributePool = GetComponentInChildren<AttributePool>();
        }

        protected virtual void Start()
        {
            attributePool.GetAttribute(AttributeType.Health).OnAttributeOver += OnHealthOver;
            baseMovement.Initialize();
        }

        public override void OnStartLocalPlayer()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            attributePool.GetAttribute(AttributeType.MoveSpeed).Initialize(5, 10);
            attributePool.GetAttribute(AttributeType.Health).Initialize(100, 100);
        }
        public void Initialize(int targetHealth, int targetMoveSpeed)
        {
            attributePool.GetAttribute(AttributeType.MoveSpeed).Initialize(targetMoveSpeed, targetMoveSpeed);
            attributePool.GetAttribute(AttributeType.Health).Initialize(targetHealth, targetHealth);
        }

        protected virtual void OnDestroy()
        {
            attributePool.GetAttribute(AttributeType.Health).OnAttributeOver -= OnHealthOver;
        }

        private void OnHealthOver(float prevValue, float currentValue)
        {
            Die();
        }

        protected virtual void Die()
        {

        }



        public virtual void Hit(HitInformation hitInformation)
        {
            lastHitInformation = hitInformation;
            attributePool.GetAttribute(AttributeType.Health)
                         .ChangeValue(-lastHitInformation.Weapon.GetWeaponDefinition().GetDamage());
        }
    }
}
