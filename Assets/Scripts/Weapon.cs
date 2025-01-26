using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Knitwit Studios/Weapon", order = 1)]
public class Weapon : ScriptableObject {
    public Sprite Sprite;
    public float Knockback;
    public float AttackSpeed;
    public float StunTime;
    public AudioClip Sound;
}