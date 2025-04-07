using System;
using System.Linq;
using System.Collections.Generic;

// ===== 사용 예시 =====
class Test
{
    static void Main()
    {
        var counter = new CastCounter();
        var achievement = new AchievementUnlock(counter);
        var selectedSkill = new FireBall(); // or WaterBall();
        var magicPlayer = new PlayerMagic(selectedSkill, achievement);

        var stateManager = new StateManager();

        // 물리모드 진입
        stateManager.PushState(new MeeleState());
        stateManager.Update(); // 이동, 공격

        // 마법모드 진입
        stateManager.PushState(new MagicState(magicPlayer));

        // 테스트 : 100회 시전
        for (int i = 0; i < 10; i++)
            stateManager.Update();
    }
}

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

// ===== 상태 구현 =====
public class MeeleState : IState
{
    private PlayerMeele player = new PlayerMeele();

    public void Enter() => Console.WriteLine("Meele Mod");
    public void Exit() => Console.WriteLine("Meele Mod Exit");
    public void Update()
    {
        player.Move();
        player.Attack();
    }
}

// MagicState
public class MagicState : IState
{
    private PlayerMagic player;

    public MagicState(PlayerMagic player)
    {
        this.player = player;
    }

    public void Enter() => Console.WriteLine("Magic Mod");
    public void Exit() => Console.WriteLine("Magic Mod Exit");
    public void Update()
    {
        // Console.WriteLine("Magic 로직 실행 중");
        player.Move();
        player.CastingSkill();
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

// ===== 업적 해금 시스템 =====
public interface ICounter
{
    int Count(int current);
}

public class CastCounter : ICounter
{
    public int Count(int current) => current + 1;
}

public class AchievementUnlock
{
    private ICounter counter;
    private int currentCount = 0;

    public AchievementUnlock(ICounter counter)
    {
        this.counter = counter;
    }

    public void CheckCasting()
    {
        currentCount = counter.Count(currentCount);
        if (currentCount == 100)
        {
            Console.WriteLine("업적 해금! 스킬 100회 사용!");
        }
    }
}