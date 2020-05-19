using UnityEngine;
using System.Collections;

public enum TargetType { 
    Pet,            //����
    Camera,         //����ͷ
}

namespace FirClient.Component
{
    [RequireComponent(typeof(CameraCulling))]
    public class SmoothFollow : CameraBase
    {
        private float dampTime = 0.2f;
        private float xOffset = 32;
        private float yOffset = 128;
        private float maxSpeed = 10000f;
        private Transform myTrans;
        private Transform mTarget;
        private Vector3 velocity = Vector2.one;

        public bool activeUse = false;
        public TargetType type = TargetType.Camera;

        Transform Trans
        {
            get
            {
                if (myTrans == null)
                {
                    myTrans = transform;
                }
                return myTrans;
            }
        }

        /// <summary>
        /// ���ö���
        /// </summary>
        public void SetActiveTarget(Transform target, float offsetX, float offsetY, float maxSpeed = 10000f)
        {
            this.activeUse = true;
            this.mTarget = target;
            this.xOffset = offsetX;
            this.yOffset = offsetY;
            this.maxSpeed = maxSpeed;
            this.SetCameraPos(true);
        }

        /// <summary>
        /// �������λ��
        /// </summary>
        /// <param name="isDirMove"></param>
        void SetCameraPos(bool isDirMove)
        {
            if (Trans == null || !activeUse)
            {
                return;
            }
            Vector3 newPos = mTarget.position;
            newPos.x += xOffset;
            newPos.y -= yOffset;
            newPos.x = (int)newPos.x;
            newPos.y = (int)newPos.y;
            if (isDirMove)
            {
                Trans.position = newPos;
            }
            else
            {
                Trans.position = SmoothDamp(newPos);
            }
        }

        // Update is called once per frame
        protected override void OnLateUpdate()
        {
            base.OnLateUpdate();
            if (mTarget != null)
            {
                Trans.position = SmoothDamp(mTarget.position);
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        Vector3 SmoothDamp(Vector3 destPos)
        {
            return Vector3.SmoothDamp(myTrans.position, destPos, ref velocity, dampTime, maxSpeed, Time.deltaTime);
        }
    }
}