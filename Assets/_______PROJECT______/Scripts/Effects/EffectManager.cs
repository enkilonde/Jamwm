using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private DamageEffect _damageEffectPrefab;

    [Button]
    private void Do5Damage()
    {
        DoDamageEffectOn(5,Vector3.zero);
    }
    [Button]
    private void Do10Damage()
    {
        DoDamageEffectOn(10,Vector3.zero);
    }
    [Button]
    private void Do50Damage()
    {
        DoDamageEffectOn(50,Vector3.zero);
    }
    
    [Button]
    private void Do80Damage()
    {
        DoDamageEffectOn(80,Vector3.zero);
    }
    [Button]
    private void Do100Damage()
    {
        DoDamageEffectOn(100,Vector3.zero);
    }


    [Button]
    public void DoDamageEffectOn(int amount, Vector3 position)
    {
        var damageEffect = Instantiate(_damageEffectPrefab);
        damageEffect.Init(amount, position);
    }
}
