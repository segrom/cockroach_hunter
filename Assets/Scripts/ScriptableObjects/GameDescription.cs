using UnityEngine;

namespace CockroachHunter.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Game", menuName = "Cockroach Hunter/Game Description", order = 0)]
    public class GameDescription : ScriptableObject
    {
        [Tooltip("Position of start zone")] public Vector2 startPosition;
        [Tooltip("Position of finish")] public Vector2 finishPosition;
        [Tooltip("Cockroaches number at start")] public int cockroachNumber;
    }
}