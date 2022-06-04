using UnityEngine;

namespace CockroachHunter.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Cockroach", menuName = "Cockroach Hunter/Cockroach Description", order = 1)]
    public class CockroachDescription : ScriptableObject
    {
        public float saveMovingSpeed, saveRotationSpeed;
        public float dangerMinMovingSpeed, dangerMinRotationSpeed;
        public float dangerMaxMovingSpeed, dangerMaxRotationSpeed;
        [Tooltip("Radius of cursor trigger zone")]public float triggerRadius;
        [Tooltip("The radius where the cockroach must be to enter safe mode")]public float saveRadius;
    }
}