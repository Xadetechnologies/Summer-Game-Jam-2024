using Pelumi.Juicer;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRevealer : MonoBehaviour
{
    [Header("Movement Effect")]
    [SerializeField] private Transform _effector;
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] Ease _movementEaseType = Ease.Spring;

    private Collider _collider;
    private JuicerRuntimeCore<Vector3> _rotateEffect;
    private bool _canToggleReveal = true;

    private void Awake()
    {
        _effector.transform.eulerAngles = new Vector3(0, 0, 90);
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        _rotateEffect = _effector.JuicyRotate(new Vector3(0, 0, 0), _duration);
    }

    public void ToggleReveal(bool state)
    {
        _rotateEffect.SetDestination(state ? new Vector3(0, 0, 0) : new Vector3(0, 0, 90));
        _rotateEffect.Start();
    }
}
