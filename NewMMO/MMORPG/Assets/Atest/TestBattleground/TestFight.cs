using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TestFight : MonoBehaviour
{

    public RoleCtrl TestRole;
    public RoleCtrl TestEnemy;

    GameObject TestEnemyObj;

    void Start()
    {



        TestEnemyObj = GameObject.Instantiate(TestEnemy.gameObject);
        TestEnemyObj.transform.position = TestRole.transform.position + new Vector3(0, 0, 10);

        TestEnemyObj.transform.LookAt(TestRole.transform);


    }

    public IEnumerator DoCameraShake(float delay = 0, float duration = 0.6f, float strength = 0.4f, int vibrat = 20)
    {
        yield return new WaitForSeconds(delay);
        came.transform.DOShakePosition(duration, strength, vibrat);
    }

    public Camera came;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {

            came.transform.DOShakePosition(0.6f, 0.4f, 20);

        }
    }
    void OnGUI()
    {
        if (TestRole == null)
        {
            return;
        }

        int posY = 0;
        if (GUI.Button(new Rect(1, posY, 60, 30), "ptIdel"))
        {
            TestRole.ToIdle();
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "fIdel"))
        {
            TestRole.ToIdle(RoleIdleState.IdelFight);
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "run"))
        {
            TestRole.ToRun();
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "Hurt"))
        {
            TestRole.ToHurt(20, 0.2f);
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "Die"))
        {
            TestRole.TestToDies();
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "Hurt"))
        {
            TestRole.ToHurt(20, 0.2f);
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "toXiYue"))
        {
            TestRole.ToSelectAni();
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "phy1"))
        {
            TestAttack(RoleAttackType.PhyAttack, 1);
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "phy2"))
        {
            TestAttack(RoleAttackType.PhyAttack, 2);
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "phy3"))
        {
            TestAttack(RoleAttackType.PhyAttack, 3);
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "Skill1"))
        {
            TestAttack(RoleAttackType.SkillAttack, 1);
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "Skill2"))
        {
            TestAttack(RoleAttackType.SkillAttack, 2);
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "Skill3"))
        {
            TestAttack(RoleAttackType.SkillAttack, 3);
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "Skill4"))
        {
            TestAttack(RoleAttackType.SkillAttack, 4);
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "Skill5"))
        {
            TestAttack(RoleAttackType.SkillAttack, 5);
        }

        posY += 30;
        if (GUI.Button(new Rect(1, posY, 60, 30), "Skill6"))
        {
            TestAttack(RoleAttackType.SkillAttack, 6);

        }


    }

    void TestAttack(RoleAttackType type = RoleAttackType.PhyAttack, int index = 0)
    {
#if DEBUG_ROLESTATE
        TestRole.ToAttack(type, index);
        if (TestEnemyObj != null)
        {
            TestEnemyObj.transform.position = TestRole.transform.position + new Vector3(0, 0, TestRole.CurrAttackRange);
            TestEnemyObj.GetComponent<RoleCtrl>().ToHurt(1, TestRole.roleAttackInfo.HurtDelayTime);
        }
        if (TestRole.roleAttackInfo.IsDoCameraShake)
        {
            StartCoroutine(DoCameraShake(TestRole.roleAttackInfo.CameraShakeDelay));
        }
#endif
    }
}
