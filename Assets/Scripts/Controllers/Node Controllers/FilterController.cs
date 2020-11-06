using System;
using UnityEngine;
using UnityEngine.UI;
using nullbloq.Noodles;

public class FilterController : NodeController, ISaveComponent
{
    public enum Filter
    {
        None,
        AfternoonFilter,
        NightFilter
    }

    public override Type NodeType { protected set; get; }

    [SerializeField] Image filter = null;
    [SerializeField] Sprite afternoonFilter = null;
    [SerializeField] Sprite nightFilter = null;

    public Filter CurrentFilter { private set; get; } = Filter.None;

    void Awake()
    {
        NodeType = typeof(CustomFilterNode);
    }

    void ApplyFilter(Filter appliedFilter)
    {
        switch (appliedFilter)
        {
            case Filter.AfternoonFilter:
                filter.gameObject.SetActive(true);
                filter.sprite = afternoonFilter;
                break;
            case Filter.NightFilter:
                filter.gameObject.SetActive(true);
                filter.sprite = nightFilter;
                break;
            case Filter.None:
            default:
                filter.gameObject.SetActive(false);
                break;
        }

        CurrentFilter = appliedFilter;
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomFilterNode;

        ApplyFilter(node.filter);

        CallNodeExecutionCompletion(0);
    }

    public void SetLoadedData(SaveData loadedData)
    {
        ApplyFilter(loadedData.currentFilter);
    }
}