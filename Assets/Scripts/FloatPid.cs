using UnityEngine;

namespace AsteroidBelt
{
    // Modified from http://answers.unity3d.com/questions/199055/addtorque-to-rotate-rigidbody-to-look-at-a-point.html
    // Implements a PID controller https://en.wikipedia.org/wiki/PID_controller
    public class FloatPid
    {
        private readonly float dTerm;
        private readonly float iTerm;
        private readonly float pTerm;
        private float integral;
        private float lastError;

        public FloatPid(float p, float i, float d)
        {
            pTerm = p;
            iTerm = i;
            dTerm = d;
        }

        public float Update(float error)
        {
            integral += error * Time.deltaTime;
            var derivative = (error - lastError) / Time.deltaTime;
            lastError = error;
            return error * pTerm + integral * iTerm + derivative * dTerm;
        }
    }
}