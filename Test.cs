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

// ===== 스택 기반 상태 관리 =====
public class StateManager
{
    private Stack<IState> stateStack = new Stack<IState>();

    public void PushState(IState state)
    {
        if (stateStack.Count > 0)
            stateStack.Peek().Exit();

        stateStack.Push(state);
        state.Enter();
    }

    public void Update()
    {
        if (stateStack.Count > 0)
            stateStack.Peek().Update();
    }
}

// ===== 플레이어 - 물리 모드 =====
public class PlayerMeele : IAttack, IMove
{
    public void Move() => Console.WriteLine("플레이어가 움직입니다");
    public void Attack() => Console.WriteLine("일반 공격");
}
