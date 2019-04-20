using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoleAttackInfo  {

    public int Index;
    public int SkillId;

    // ��ɫ��ս��������ʹ�õ�
    public string EffectName;

#if DEBUG_ROLESTATE
    // �ڲ��Ի�����ʹ�õ�
    public GameObject EffectObject;
#endif

    // ��Ч����ʱ��
    public float EffectLifeTime;

    public float PatrolRange = 0;

    public float AttackRange = 0;

    // �öԷ������ӳ�
    public float HurtDelayTime = 0; 


}
