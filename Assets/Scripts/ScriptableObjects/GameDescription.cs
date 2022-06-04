using UnityEngine;

namespace CockroachHunter.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Game", menuName = "Cockroach Hunter/Game Description", order = 0)]
    public class GameDescription : ScriptableObject
    {
        public Vector2 startPosition;
        public Vector2 finishPosition;
        public int cockroachCount;
    }
}