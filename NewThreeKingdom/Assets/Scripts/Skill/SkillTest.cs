using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkillTest : MonoBehaviour {
    private NavMeshAgent m_NavAgent;
    private Animator m_Animator;
    private float m_Speed;
    public int m_SkillId = 0;
    private SkillTest m_Target;

    private Skill m_Skill = null;
    private void Awake()
    {
        m_NavAgent = gameObject.AddComponent<NavMeshAgent>();
        m_NavAgent.speed = 0;
        m_NavAgent.acceleration = 0;
        m_NavAgent.angularSpeed = 0;

        m_NavAgent.height = 2.0f;
        m_NavAgent.radius = 1.5f;
        //距离目标位置位2米的时候停止
        m_NavAgent.stoppingDistance = 2.0f;
        CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider>();
        collider.height = m_NavAgent.height;
        collider.radius = m_NavAgent.radius;
        collider.center = Vector3.up * (collider.height * 0.5f);
        m_Animator = gameObject.GetComponent<Animator>();
    }
    // Use this for initialization
    void Start () {
        if (m_SkillId>0)
        {
            
        }
    }
	
	// Update is called once per frame
	void Update () {
        FightStateUpdate();
    }
    private void FightStateUpdate() {
        if (m_Target==null)
        {
            //目标为空，开始查找目标
            m_Target = FindTargetInradius();
        }
        if (m_Target!=null)
        {
            //有攻击目标是，要开始尝试攻击
            bool moveToTarget = false;
            TryAttack(out moveToTarget);
            if (moveToTarget)
            {
                MoveToTarget();
            }
        }
    }
    /// <summary>
    /// 转向至目标方向
    /// </summary>
    /// <param name="pos"></param>
    private void RotateToTarget(Vector3 pos)
    {
        Vector3 relative = transform.InverseTransformPoint(pos);//以自身建立坐标系，得到与pos位置的向量
        float angle = Mathf.Atan2(relative.x,relative.z)*Mathf.Rad2Deg;//根据投影得到角度
        transform.Rotate(Vector3.up*angle);
    }
    private void MoveToTarget()
    {
        SetDestination(m_Target.transform.position);
    }
    /// <summary>
    /// 停止移动,站立
    /// </summary>
    private void StopMove()
    {
        SetDestination(transform.position);
    }
    /// <summary>
    /// 设置目标位置点
    /// </summary>
    /// <param name="pos"></param>
    void SetDestination(Vector3 pos)
    {
        m_NavAgent.SetDestination(pos); //NavMesh移动
    }
    /// <summary>
    /// 准备攻击,处理站立和转向问题
    /// </summary>
    private void PrepareAttack()
    {
        if (m_Target!=null)
        {
            RotateToTarget(m_Target.transform.position);
        }
        StopMove();
    }
    public bool IsIdleToUserSkill
    {
        get
        {
            int curSkill = m_Animator.GetInteger("SkillId");//获取技能id.不等于0表示当前正在播放动画
            AnimatorStateInfo statInfo = m_Animator.GetCurrentAnimatorStateInfo(0);//获取当前这一层的动画信息

            if (curSkill == 0 && statInfo.IsName("Base Layer.idle")&&m_Animator.IsInTransition(0))//m_Animator.IsInTransition(0)当前动画不处于切换状态
            {
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// 尝试使用攻击，如果在攻击范围内就准备攻击
    /// 如果不再攻击范围内，就需要移动至目标
    /// </summary>
    /// <param name="moveToTarget"></param>
    public void TryAttack(out bool moveToTarget)
    {
        moveToTarget = false;
        if (m_Skill!=null)
        {
            if (m_Skill.CoolDown)//技能是否冷却
            {
                if (Vector3.Distance(transform.position,m_Target.transform.position)>=m_Skill.AttackDist)//距离大于技能攻击距离，
                {
                    moveToTarget = true;//移动向目标
                    return;
                }
                else
                {
                    PrepareAttack();
                }
                //如果是站立，就进行播放战斗技能动作
                if (IsIdleToUserSkill)
                {
                    m_Animator.SetInteger("SkillID",m_Skill.SkillId);
                }
            }
        }
    }

    /// <summary>
    /// 在一定范围查找带有SkillTest的攻击目标
    /// </summary>
    /// <returns></returns>
    private SkillTest FindTargetInradius()
    {
        //
        Collider[] colliders = Physics.OverlapSphere(transform.position, 100, 1 << LayerMask.NameToLayer("Warrior"));//半径范围内的目标
        foreach (var item in colliders)
        {
            SkillTest unit = item.gameObject.GetComponent<SkillTest>();
            if (unit == null)
            {
                continue;
            }
            if (unit == this)
            {
                continue;
            }
            return unit;//可以做的查找的条件（怪物类型，血量，距离最近，等）
        }
        return null;
    }
}
