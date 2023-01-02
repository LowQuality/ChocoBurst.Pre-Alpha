using System.Collections.Generic;
using System.Linq;
using Character;
using Character.Skill;
using TMPro;
using UnityEngine;

namespace Management
{
    public class UpNDownGradeManager : MonoBehaviour
    {
        public float rate;
        public float beforeChange;
        public float afterChange;

        [SerializeField] private TextMeshProUGUI detail;
        [SerializeField] private TextMeshProUGUI update;
        private string _afterChangeColor;
        private string _beforeChangeColor;
        private readonly Dictionary<string, string> _bf = new();

        private readonly Dictionary<string, string> _field = new();
        private bool _isUpgrade;

        private void Awake()
        {
            if (transform.name.Contains("Upgrade")) _isUpgrade = true;
            if (_isUpgrade)
            {
                _beforeChangeColor = "ff0000";
                _afterChangeColor = "00ff00";
            }
            else
            {
                _beforeChangeColor = "00ff00";
                _afterChangeColor = "ff0000";
            }

            _field.Add("AttackSpeedUpgrade", $"플레이어의 공격 속도가 <color=#00ff00>{rate}%</color> 감소합니다.");
            _field.Add("AttackSpeedDowngrade", $"플레이어의 공격 속도가 <color=#ff0000>{rate}%</color> 증가합니다.");
            _field.Add("SkillAttackRangeUpgrade", $"플레이어의 <color=#ff0000>스킬 공격 범위</color>가 <color=#00ff00>{rate}%</color> 증가합니다.");
            _field.Add("SkillAttackRangeDowngrade", $"플레이어의 <color=#ff0000>스킬 공격 범위</color>가 <color=#ff0000>{rate}%</color> 감소합니다.");
            _field.Add("SkillAttackUpgrade(%)", $"스킬 사용시 주변의 적이 받는 데미지를 <color=#00ff00>{rate}%</color> 증가합니다.");
            _field.Add("SkillAttackDowngrade(%)", $"스킬 사용시 주변의 적이 받는 데미지를 <color=#ff0000>{rate}%</color> 감소합니다.");
            _field.Add("SkillAttackUpgrade(+)", "스킬 사용시 주변의 적이 받는 데미지를 <color=#00ff00>5</color> 만큼 증가합니다.");
            _field.Add("MoveSpeedUpgrade", $"플레이어의 이동 속도가 <color=#00ff00>{rate}%</color> 증가합니다.");
            _field.Add("MoveSpeedDowngrade", $"플레이어의 이동 속도가 <color=#ff0000>{rate}%</color> 감소합니다.");
            _field.Add("SkillDelayUpgrade", "스킬 재충전 시간을 <color=#00ff00>1초</color> 감소합니다.");
            _field.Add("SkillDelayDowngrade", "스킬 재충전 시간을 <color=#ff0000>1초</color> 증가합니다.");

            _bf.Add("AttackSpeed",
                $"공격 속도 : <color=#{_beforeChangeColor}>{beforeChange}</color> -> <color=#{_afterChangeColor}>{afterChange}</color>");
            _bf.Add("SkillAttackRange",
                $"범위 : <color=#{_beforeChangeColor}>{beforeChange}</color> -> <color=#{_afterChangeColor}>{afterChange}</color>");
            _bf.Add("SkillAttack",
                $"데미지 : <color=#{_beforeChangeColor}>{beforeChange}</color> -> <color=#{_afterChangeColor}>{afterChange}</color>");
            _bf.Add("MoveSpeed",
                $"이동 속도 : <color=#{_beforeChangeColor}>{beforeChange}</color> -> <color=#{_afterChangeColor}>{afterChange}</color>");
            _bf.Add("SkillDelay",
                $"딜레이 : <color=#{_beforeChangeColor}>{beforeChange}</color> -> <color=#{_afterChangeColor}>{afterChange}</color>");
        }

        private void Update()
        {
            detail.text = _field.FirstOrDefault(x => transform.name.Contains(x.Key)).Value;
            update.text = _bf.FirstOrDefault(x => transform.name.Contains(x.Key)).Value;
        }

        public void Apply()
        {
            if (transform.name.Contains("AttackSpeed"))
                GameObject.Find("Character").GetComponent<Attacker>().attackDelay = afterChange;
            else if (transform.name.Contains("SkillAttackRange"))
                GameObject.Find("Character").GetComponent<Flash>().flashAttackDistance = afterChange;
            else if (transform.name.Contains("SkillAttack"))
                GameObject.Find("Character").GetComponent<Flash>().flashAttackDamage = afterChange;
            else if (transform.name.Contains("MoveSpeed"))
                GameObject.Find("A").GetComponent<Movement>().moveSpeed = afterChange;
            else if (transform.name.Contains("SkillDelay"))
                GameObject.Find("Character").GetComponent<Flash>().rechargeTime = (int)afterChange;

            ScoreManager.Selected = true;
        }
    }
}