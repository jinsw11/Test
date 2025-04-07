using System;
using System.Linq;
using System.Collections.Generic;

// ===== 인터페이스 및 상태 =====
public interface IState
{
    void Enter();
    void Exit();
    void Update();
}

// ===== 일반 공격/스킬/이동 인터페이스 =====
public interface IAttack { void Attack(); }
public interface ISkillCast { void CastingSkill(); }
public interface IMove { void Move(); }