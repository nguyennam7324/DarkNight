using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SpriteRenderer icon;
    public Text skillName;
    public Text skillDescription;
    public SimpleTooltip tooltip;
    public GameObject skillObject;
    private SkillItemSO skillItemSO;
    private Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void ActiveSkill()
    {
        player.UseSkill(skillItemSO);
        skillObject.SetActive(false);
        NPCSkill.instance.SelectedSkill(this);
    }

    public void SetSkill(SkillItemSO skillItemSO)
    {
        skillObject.SetActive(true);
        this.icon.sprite = skillItemSO.icon;
        //this.skillName.text = name;
        tooltip.infoLeft = skillItemSO.description;
        this.skillItemSO = skillItemSO;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ActiveSkill();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
            tooltip.HideTooltip();
        if (GameManager.instance.tooltip == null)
        {
            var tooltip = GameObject.FindGameObjectWithTag("tooltip");
            GameManager.instance.tooltip = tooltip;
        }
        GameManager.instance.tooltip.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip != null)
            tooltip.ShowTooltip();
    }
}
