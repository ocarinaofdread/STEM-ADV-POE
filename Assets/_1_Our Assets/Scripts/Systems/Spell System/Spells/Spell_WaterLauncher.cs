using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Spell_WaterLauncher : Spell
{
    [SerializeField] private Transform launchPoint;
    [SerializeField] private GameObject waterSpell;
    [SerializeField] private float spawnRate = 0.5f;

    private float _timer;
    
    protected override void RunCastAction()
    {
        launchPoint ??= transform;
        _timer = spawnRate;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            Instantiate(waterSpell, launchPoint.position, launchPoint.rotation);
            _timer = spawnRate;
        }
        
    }
    public override void End()
    {
        Destroy(gameObject);
    }
}
