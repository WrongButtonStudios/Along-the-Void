using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffStateMachine : MonoBehaviour
{
    [Flags]
    public enum Debuffs
    {
        None = 0,
        Frozen = 1 << 0,
        Burning = 1 << 1,
        Poisoned = 1 << 2
    }
    private Debuffs _debuffs = Debuffs.None;
    private Coroutine _burning;
    private Coroutine _freezing;
    [SerializeField] private float _burnDuration = 3.0f;
    [SerializeField] private float _freezeDuration = 3.0f;

    private IEnumerator Debuff(Debuffs debuff,float duration) {
        yield return new WaitForSeconds(duration);
        RemoveDebuff(debuff);
        switch(debuff) {
            case Debuffs.Burning:
                _burning = null;
                break;
            case Debuffs.Frozen:
                _frozen == null;
                break;
            default:
                break;
        }
    }

    public void AddDebuff(Debuffs debuff, float duration) {
        switch(debuff) {
        case Debuffs.Burning:
                if(_burning != null) {
                    RemoveDebuff(Debuffs.Burning);
                }
                _burning = StartCoroutine(Debuff(debuff,duration));
                _debuffs |= Debuffs.Burning;
                break;
            case Debuffs.Frozen:
                if(_frozen != null) {
                    RemoveDebuff(Debuffs.Frozen);
                    _debuffs |= Debuffs.Frozen;
                }
                _frozen = StartCoroutine(Debuff(debuff,duration));
                break;
            default:
                break;
        }
    }

    public void RemoveDebuff(Debuffs debuff) {
        if(_burning != null) {
            _debuffs &= ~Debuffs.Burning;
            StopCoroutine(_burning);
            _burning = null;
        }
        if(_frozen != null) {
            _debuffs &= ~Debuffs.Frozen;
            StopCoroutine(_frozen);
            _frozen = null;
        }
    }

    public bool HasDebuff(Debuffs checkDebuff) {
        return (_debuffs & checkDebuff) == checkDebuff;
    }
}