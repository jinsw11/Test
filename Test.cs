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

// ===== 플레이어 - 마법 모드 =====
public class PlayerMagic : ISkillCast, IMove
{
    private MagicSkill currentSkill;
    private AchievementUnlock achievement;

    public PlayerMagic(MagicSkill skill, AchievementUnlock unlockSystem)
    {
        currentSkill = skill;
        achievement = unlockSystem;
    }

    public void Move() => Console.WriteLine("플레이어가 움직입니다");

    public void CastingSkill()
    {
        currentSkill.Cast();
        achievement.CheckCasting();
    }
}

// ===== 마법 스킬 종류 =====
public abstract class MagicSkill
{
    public abstract void Cast();
}

// 파이어볼
class FireBall : MagicSkill
{
    public override void Cast() => Console.WriteLine("파이어볼 발사!");
}
// 워터볼
class WaterBall : MagicSkill
{
    public override void Cast() => Console.WriteLine("워터볼 발사!");
}