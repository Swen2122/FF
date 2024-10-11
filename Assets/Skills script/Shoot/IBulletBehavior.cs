using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public interface IBulletBehavior
{
    void SetBulletProperties(int damage);
    void SetSequence(Sequence sequence);
}
