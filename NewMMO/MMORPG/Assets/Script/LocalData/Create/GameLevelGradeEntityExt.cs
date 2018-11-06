
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class GameLevelGradeEntity
{
    /// <summary>
    /// 当前的难度等级
    /// </summary>
    public GameLevelGrade CurrGrade
    {
        get { return (GameLevelGrade)Grade; }
    }

    #region 奖励的装备
    private List<GoodsEntity> m_EquipList;

    public List<GoodsEntity> EquipList
    {
        get
        {
            if (m_EquipList == null)
            {
                m_EquipList = new List<GoodsEntity>();

                string[] arr = Equip.Split('|');
                if (arr.Length > 0)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string[] arr2 = arr[i].Split('_');
                        if (arr2.Length >= 3)
                        {
                            GoodsEntity entity = new GoodsEntity();
                            int goodsId = 0;
                            int.TryParse(arr2[0], out goodsId);

                            int probability = 0;
                            int.TryParse(arr2[1], out probability);

                            int Count = 0;
                            int.TryParse(arr2[2], out Count);

                            string name = string.Empty;
                            EquipEntity equipEntity = EquipDBModel.Instance.Get(goodsId);
                            if (equipEntity != null)
                            {
                                name = equipEntity.Name;
                            }

                            entity.Id = goodsId;
                            entity.Name = name;
                            entity.Probability = probability;
                            entity.Count = Count;

                            m_EquipList.Add(entity);
                        }
                    }
                }
            }
            return m_EquipList;
        }
    }
    #endregion

    #region 奖励的道具
    private List<GoodsEntity> m_ItemList;

    public List<GoodsEntity> ItemList
    {
        get
        {
            if (m_ItemList == null)
            {
                m_ItemList = new List<GoodsEntity>();

                string[] arr = Item.Split('|');
                if (arr.Length > 0)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string[] arr2 = arr[i].Split('_');
                        if (arr2.Length >= 3)
                        {
                            GoodsEntity entity = new GoodsEntity();
                            int goodsId = 0;
                            int.TryParse(arr2[0], out goodsId);

                            int probability = 0;
                            int.TryParse(arr2[1], out probability);

                            int Count = 0;
                            int.TryParse(arr2[2], out Count);

                            string name = string.Empty;
                            ItemEntity itemEntity = ItemDBModel.Instance.Get(goodsId);
                            if (itemEntity != null)
                            {
                                name = itemEntity.Name;
                            }

                            entity.Id = goodsId;
                            entity.Name = name;
                            entity.Probability = probability;
                            entity.Count = Count;

                            m_ItemList.Add(entity);
                        }
                    }
                }
            }
            return m_ItemList;
        }
    }
    #endregion

    #region 奖励的材料
    private List<GoodsEntity> m_MaterialList;

    public List<GoodsEntity> MaterialList
    {
        get
        {
            if (m_MaterialList == null)
            {
                m_MaterialList = new List<GoodsEntity>();

                string[] arr = Material.Split('|');
                if (arr.Length > 0)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string[] arr2 = arr[i].Split('_');
                        if (arr2.Length >= 3)
                        {
                            GoodsEntity entity = new GoodsEntity();
                            int goodsId = 0;
                            int.TryParse(arr2[0], out goodsId);

                            int probability = 0;
                            int.TryParse(arr2[1], out probability);

                            int Count = 0;
                            int.TryParse(arr2[2], out Count);

                            string name = string.Empty;
                            MaterialEntity equipEntity = MaterialDBModel.Instance.Get(goodsId);
                            if (equipEntity != null)
                            {
                                name = equipEntity.Name;
                            }

                            entity.Id = goodsId;
                            entity.Name = name;
                            entity.Probability = probability;
                            entity.Count = Count;

                            m_MaterialList.Add(entity);
                        }
                    }
                }
            }
            return m_MaterialList;
        }
    }
    #endregion
}