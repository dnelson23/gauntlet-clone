using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Components
{
    /*
     * Simple 2D vector movement script.
     */

    public class VectorMovement2D : CustomComponentBase
    {
        public Vector3 moveVector { get; private set; }

        void Update()
        {
            Move();
        }

        public override void Load(GameObject parent)
        {
            base.Load(parent);
            moveVector = _parent.position;
        }

        public void SetMoveVector(Vector3 vect)
        {
            moveVector = vect;
        }

        public void Move()
        {
            _parent.position = _parent.position + moveVector;
        }

        public void Rotate(Vector3 dir, float speed)
        {
            float step = speed * Time.deltaTime;
            Vector3 newLookRot = Vector3.RotateTowards(_parent.forward, dir, step, 0.0F);
            _parent.rotation = Quaternion.LookRotation(newLookRot);
        }
    }
}
