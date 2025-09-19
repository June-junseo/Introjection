using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public enum PlayerState
{
    IDLE,
    MOVE,
    ATTACK,
    DAMAGED,
    DEBUFF,
    DEATH,
    OTHER,
}

public class SPUM_Prefabs : MonoBehaviour
{
    public float _version;
    public bool EditChk;
    public string _code;
    public Animator _anim;
    public AnimatorOverrideController OverrideController;

    public string UnitType;
    public List<SpumPackage> spumPackages = new List<SpumPackage>();
    public List<PreviewMatchingElement> ImageElement = new();
    public List<SPUM_AnimationData> SpumAnimationData = new();

    public Dictionary<string, List<AnimationClip>> StateAnimationPairs = new();
    public List<AnimationClip> IDLE_List = new();
    public List<AnimationClip> MOVE_List = new();
    public List<AnimationClip> ATTACK_List = new();
    public List<AnimationClip> DAMAGED_List = new();
    public List<AnimationClip> DEBUFF_List = new();
    public List<AnimationClip> DEATH_List = new();
    public List<AnimationClip> OTHER_List = new();

    public void OverrideControllerInit()
    {
        Animator animator = _anim;

        // 이미 OverrideController가 연결된 경우 새로 만들지 않음
        if (animator.runtimeAnimatorController is AnimatorOverrideController existing)
        {
            OverrideController = existing;
        }
        else
        {
            // 원본 AnimatorController 기반으로 OverrideController 생성
            OverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = OverrideController;
        }

        // StateAnimationPairs 초기화
        foreach (PlayerState state in Enum.GetValues(typeof(PlayerState)))
        {
            var stateText = state.ToString();
            StateAnimationPairs[stateText] = new List<AnimationClip>();
            switch (stateText)
            {
                case "IDLE": StateAnimationPairs[stateText] = IDLE_List; break;
                case "MOVE": StateAnimationPairs[stateText] = MOVE_List; break;
                case "ATTACK": StateAnimationPairs[stateText] = ATTACK_List; break;
                case "DAMAGED": StateAnimationPairs[stateText] = DAMAGED_List; break;
                case "DEBUFF": StateAnimationPairs[stateText] = DEBUFF_List; break;
                case "DEATH": StateAnimationPairs[stateText] = DEATH_List; break;
                case "OTHER": StateAnimationPairs[stateText] = OTHER_List; break;
            }
        }
    }

    public bool allListsHaveItemsExist()
    {
        List<List<AnimationClip>> allLists = new List<List<AnimationClip>>()
        {
            IDLE_List, MOVE_List, ATTACK_List, DAMAGED_List, DEBUFF_List, DEATH_List, OTHER_List
        };

        return allLists.All(list => list.Count > 0);
    }

    [ContextMenu("PopulateAnimationLists")]
    public void PopulateAnimationLists()
    {
        IDLE_List = new();
        MOVE_List = new();
        ATTACK_List = new();
        DAMAGED_List = new();
        DEBUFF_List = new();
        DEATH_List = new();
        OTHER_List = new();

        var groupedClips = spumPackages
            .SelectMany(package => package.SpumAnimationData)
            .Where(spumClip => spumClip.HasData &&
                               spumClip.UnitType.Equals(UnitType) &&
                               spumClip.index > -1)
            .GroupBy(spumClip => spumClip.StateType)
            .ToDictionary(
                group => group.Key,
                group => group.OrderBy(clip => clip.index).ToList()
            );

        foreach (var kvp in groupedClips)
        {
            var stateType = kvp.Key;
            var orderedClips = kvp.Value;

            switch (stateType)
            {
                case "IDLE":
                    IDLE_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    break;
                case "MOVE":
                    MOVE_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    break;
                case "ATTACK":
                    ATTACK_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    break;
                case "DAMAGED":
                    DAMAGED_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    break;
                case "DEBUFF":
                    DEBUFF_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    break;
                case "DEATH":
                    DEATH_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    break;
                case "OTHER":
                    OTHER_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    break;
            }
        }

        Debug.Log("=== IDLE LIST ===");
        for (int i = 0; i < IDLE_List.Count; i++)
            Debug.Log($"IDLE[{i}] = {IDLE_List[i]?.name}");

        Debug.Log("=== MOVE LIST ===");
        for (int i = 0; i < MOVE_List.Count; i++)
            Debug.Log($"MOVE[{i}] = {MOVE_List[i]?.name}");
    }

    public void PlayAnimation(PlayerState PlayState, int index)
    {
        if (!StateAnimationPairs.ContainsKey(PlayState.ToString())) return;

        var animations = StateAnimationPairs[PlayState.ToString()];
        if (animations.Count == 0 || index < 0 || index >= animations.Count) return;

        OverrideController[PlayState.ToString()] = animations[index];

        string StateStr = PlayState.ToString();

        bool isMove = StateStr.Contains("MOVE");
        bool isDebuff = StateStr.Contains("DEBUFF");
        bool isDeath = StateStr.Contains("DEATH");

        _anim.SetBool("1_Move", isMove);
        _anim.SetBool("5_Debuff", isDebuff);
        _anim.SetBool("isDeath", isDeath);

        if (!isMove && !isDebuff)
        {
            AnimatorControllerParameter[] parameters = _anim.parameters;
            foreach (AnimatorControllerParameter parameter in parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Trigger)
                {
                    bool isTrigger = parameter.name.ToUpper().Contains(StateStr.ToUpper());
                    if (isTrigger)
                    {
                        Debug.Log($"Parameter: {parameter.name}, Type: {parameter.type}");
                        _anim.SetTrigger(parameter.name);
                    }
                }
            }
        }
    }

    AnimationClip LoadAnimationClip(string clipPath)
    {
        AnimationClip clip = Resources.Load<AnimationClip>(clipPath.Replace(".anim", ""));
        if (clip == null)
        {
            Debug.LogWarning($"Failed to load animation clip '{clipPath}'.");
        }
        return clip;
    }
}
