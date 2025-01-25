using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Knitwit Studios/Weapon", order = 1)]
public class Weapon : ScriptableObject {
    public Sprite Sprite;
    public Vector2 HitboxSize;
    public float Knockback;
    public float AttackSpeed;
    public float ActiveTime;
    public float StunTime;
}