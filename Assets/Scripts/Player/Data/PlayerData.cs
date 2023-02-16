using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("General")]
    public float _playerHeight;

    [Header("Movement")]
    public float _movementVelocity;

    [Header("Dash")]
    public float _dashForce;
    public float _dashRange;
    public float _dashCooldown;

    [Header("Jump")]
    public float _jumpForce;

    [Header("Immune")]
    public float _immuneTimer;
}
