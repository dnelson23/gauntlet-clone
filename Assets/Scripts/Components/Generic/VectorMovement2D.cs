using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Components.Generic
{
    /// <summary>
    /// Simple 2D movement script using Vectors
    /// Add this script as a component in your controller and use SetMoveVector to move object in a direction
    /// </summary>
    public class VectorMovement2D : CustomComponentBase
    {
        public Vector3 moveVector { get; private set; }

        void FixedUpdate()
        {
            Move();
        }

        public override void Load(GameObject parent)
        {
            base.Load(parent);
            moveVector = _parent.position;
        }

        /// <summary>
        /// Sets the direction for the object to move
        /// </summary>
        /// <param name="vect"></param>
        public void SetMoveVector(Vector3 vect)
        {
            moveVector = vect;
        }

        void Move()
        {
            _parent.position = _parent.position + moveVector;
        }


        /// <summary>
        /// Rotate parent object in given direction
        /// </summary>
        /// <param name="dir"></param>
        public void Rotate(Vector3 dir)
        {
            if (dir == Vector3.zero) { return; }
            _parent.rotation = Quaternion.LookRotation(dir);
        }

        /// <summary>
        /// Rotate parent object in given direction at speed
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="speed"></param>
        public void Rotate(Vector3 dir, float speed)
        {
            float step = speed * Time.deltaTime;
            Vector3 newLookRot = Vector3.RotateTowards(_parent.forward, dir, step, 0.0F);
            _parent.rotation = Quaternion.LookRotation(newLookRot);
        }
    }
}
