using Character;
using Character.Skill;
using TMPro;
using UnityEngine;

namespace Management
{
    public class ValueManager : MonoBehaviour
    {
        public TextMeshProUGUI valueText;

        private void Update()
        {
            valueText.text = $"AttackSpeed : {GameObject.Find("Character").GetComponent<Attacker>().attackDelay}\n" +
                             $"SkillAttackRange : {GameObject.Find("Character").GetComponent<Flash>().flashAttackDistance}\n" +
                             $"SkillAttack : {GameObject.Find("Character").GetComponent<Flash>().flashAttackDamage}\n" +
                             $"MoveSpeed : {GameObject.Find("A").GetComponent<Movement>().moveSpeed}\n" +
                             $"SkillDelay : {GameObject.Find("Character").GetComponent<Flash>().rechargeTime}";
        }
    }
}