using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoleAttackInfo  {

    public int Index;
    public int SkillId;

    // 角色在战斗场景中使用的
    public string EffectName;

#if DEBUG_ROLESTATE
    // 在测试环境下使用的
    public GameObject EffectObject;
#endif

    // 特效存在时间
    public float EffectLifeTime;

    public float PatrolRange = 0;

    public float AttackRange = 0;

    // 让对方受伤延迟
    public float HurtDelayTime = 0; 


}
