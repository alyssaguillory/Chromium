using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHealth : Health
{
    [SerializeField] BossController boss;
    public override void Die()
    {
        boss.advanceGameStage();
        Instantiate(corpse, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public override IEnumerator DamageWithInvincible(float Iframes, float damage)
    {
        Damage(damage);
        StartCoroutine(DamageFlash(Iframes));
        yield return null;
    }
}
