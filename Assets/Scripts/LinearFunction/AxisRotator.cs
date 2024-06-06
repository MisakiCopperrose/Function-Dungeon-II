using UnityEngine;
using UnityEngine.Serialization;

namespace LinearFunction
{
    [ExecuteInEditMode]
    public class AxisRotator : MonoBehaviour
    {
        private enum Axis { X, Y, Z }

        [SerializeField] private Axis rotationAxis;
        [SerializeField] private bool invertRotation;
        [SerializeField] private Transform targetTransform;
        [FormerlySerializedAs("LinearFunctionData")] [SerializeField] private LinearFunctionData linearFunctionData;

        private float _rotationAngle;

        /// <summary>
        /// Rotates the target transform based on the linear function data.
        /// </summary>
        public void Rotate()
        {
            if (!targetTransform)
            {
                Debug.LogWarning("Target Transform is not set.");
                
                return;
            }

            _rotationAngle = LinearFunctionHelper.GetAngleOfFunction(linearFunctionData.Slope);

            if (invertRotation) _rotationAngle = -_rotationAngle;

            var rotationVector = rotationAxis switch
            {
                Axis.X => Vector3.right * _rotationAngle,
                Axis.Y => Vector3.up * _rotationAngle,
                Axis.Z => Vector3.forward * _rotationAngle,
                _ => Vector3.zero
            };

            targetTransform.Rotate(rotationVector);
        }
    }
}