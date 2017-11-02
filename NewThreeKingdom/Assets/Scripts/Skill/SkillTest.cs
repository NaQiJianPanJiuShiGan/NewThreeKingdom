using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkillTest : MonoBehaviour {
    private NavMeshAgent _NavAgent;
    private Animator _Animator;
    private float _Speed;
    public int _SkillId = 0;
    private SkillTest _Target;

    private Skill skill = null;
    private void Awake()
    {
        _NavAgent = gameObject.AddComponent<NavMeshAgent>();
        _NavAgent.speed = 0;
        _NavAgent.acceleration = 0;
        _NavAgent.angularSpeed = 0;

        _NavAgent.height = 2.0f;
        _NavAgent.radius = 1.5f;
        //距离目标位置位2米的时候停止
        _NavAgent.stoppingDistance = 2.0f;
        CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider>();
        collider.height = _NavAgent.height;
        collider.radius = _NavAgent.radius;
        collider.center = Vector3.up * (collider.height * 0.5f);
        _Animator = gameObject.GetComponent<Animator>();
    }
    // Use this for initialization
    void Start () {
        if (_SkillId>0)
        {
            
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void FightStateUpdate() {
        if (_Target==null)
        {
            //目标为空，开始查找目标
            _Target = FindTargetInradius();
        }
        if (_Target!=null)
        {
            //有攻击目标是，要开始尝试攻击
            bool moveToTarget = false;

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
    /// <summary>
    /// 停止移动站立
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
        _NavAgent.SetDestination(pos); //NavMesh移动
    }
    /// <summary>
    /// 准备攻击,处理战力和转向问题
    /// </summary>
    private void PrepareAttack()
    {
        if (_Target!=null)
        {
            RotateToTarget(_Target.transform.position);
        }
    }
    public bool IsIdleToUserSkill
    {
        get
        {
            int curSkill = _Animator.GetInteger("SkillId");
            AnimatorStateInfo statInfo = _Animator.GetCurrentAnimatorStateInfo(0);
            if (curSkill == 0 && statInfo.IsName("Base Layer.idle")&&_Animator.IsInTransition(0))
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
        if (skill!=null)
        {
            if (skill.CoolDown)//技能是否冷却
            {
                if (Vector3.Distance(transform.position,_Target.transform.position)>=skill.AttackDist)//距离大于技能攻击距离，
                {
                    moveToTarget = true;//移动向目标
                    return;
                }
                else
                {
                    PrepareAttack();
                }
                //如果是站立，就进行播放战斗技能动作
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, 100, 1 << LayerMask.NameToLayer("Warrior"));
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
