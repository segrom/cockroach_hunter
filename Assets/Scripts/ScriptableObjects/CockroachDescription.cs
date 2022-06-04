using UnityEngine;

namespace CockroachHunter.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Cockroach", menuName = "Cockroach Hunter/Cockroach Description", order = 1)]
    public class CockroachDescription : ScriptableObject
    {
        public float saveMovingSpeed, saveRotationSpeed;
        public float dangerMovingSpeed, dangerRotationSpeed;
        public float triggerRadius, saveRadius;
    }
}