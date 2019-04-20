using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleHurt
{
    RoleFSMMgr m_CurrRoleFSMMgr = null;
    // 当角色受伤的时候，
    public RoleHurt(RoleFSMMgr mgr)
    {
        m_CurrRoleFSMMgr = mgr;
    }

    public void ToHurt(int atackValue)
    {
        m_CurrRoleFSMMgr.ChangeState(RoleState.Hurt);
    }
}
