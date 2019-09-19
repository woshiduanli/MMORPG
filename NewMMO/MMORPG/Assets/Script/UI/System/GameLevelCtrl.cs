using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameLevelCtrl : SystemCtrlBase<GameLevelCtrl>, ISystemCtrl
{
    UIGameLevelMapView m_UIGameLevelMapView;
    UIGameLevelDetailView m_UIRoleInfoView;

    public void OpenView(WindowUIType type)
    {
        switch (type)
        {
            case WindowUIType.GameLevelDetail:
                break;
            case WindowUIType.GameLevelMap:
                OpenGameLevelMapView();
                break;
        }
    }


    void OpenGameLevelVictiryView()
    {
        UIViewUtil.Instance.OpenWindow(WindowUIType.GameLevelVictory);

    }

    void OpenGameLevelFailView()
    {
        UIViewUtil.Instance.OpenWindow(WindowUIType.GameLevelVictory);

    }

    void OpenGameLevelMapView()
    {
        m_UIGameLevelMapView = UIViewUtil.Instance.OpenWindow(WindowUIType.GameLevelMap).GetComponent<UIGameLevelMapView>();
        TransferData data = new TransferData();
        ChapterEntity entity = ChapterDBModel.Instance.Get(1);
        // ÕÂ½Ú
        data.SetValue<int>(ConstDefine.ChapterId, entity.Id);
        data.SetValue<string>(ConstDefine.ChapterName, entity.ChapterName);
        data.SetValue<string>(ConstDefine.ChapterBG, entity.BG_Pic);


        // ¹Ø¿¨
        List<GameLevelEntity> gameList = GameLevelDBModel.Instance.GetListByChapterId(entity.Id);
        if (gameList != null)
        {
            List<TransferData> lst = new List<TransferData>();
            for (int i = 0; i < gameList.Count; i++)
            {
                TransferData childData = new TransferData();
                childData.SetValue<int>(ConstDefine.GameLevelId, gameList[i].Id);
                childData.SetValue<string>(ConstDefine.GameLevelName, gameList[i].Name);
                childData.SetValue<Vector2>(ConstDefine.GameLevelPostion, gameList[i].Postion);
                childData.SetValue<string>(ConstDefine.GameLevelIco, gameList[i].Ico);
                childData.SetValue<int>(ConstDefine.GameLevelisBoss, gameList[i].isBoss);
                lst.Add(childData);
            }
            data.SetValue<List<TransferData>>(ConstDefine.GameLevelList, lst);
        }
        m_UIGameLevelMapView.SetUI(data, OnGameLevelItemClick);

    }

    private void OnGameLevelItemClick(int obj)
    {
        m_UIRoleInfoView = UIViewUtil.Instance.OpenWindow(WindowUIType.GameLevelDetail).GetComponent<UIGameLevelDetailView>();
        m_UIRoleInfoView.OnBtnGradeClick = OnGameLevelGradeClick;
        m_UIRoleInfoView.OnBtnEnterClick = OnEnterGameLevel;


        SetGameLevelDetailData(obj, GameLevelGrade.Normal);
    }

    private void OnEnterGameLevel(int gameLevelId, GameLevelGrade grade)
    {
        SceneMgr.Instance.LoadToGameLevel(gameLevelId, grade);
    }

    private void OnGameLevelGradeClick(int gameLevelId, GameLevelGrade grade)
    {
        SetGameLevelDetailData(gameLevelId, grade);
    }

    void SetGameLevelDetailData(int gameLevelId, GameLevelGrade grade)
    {
        GameLevelEntity gameLevelEntity = GameLevelDBModel.Instance.Get(gameLevelId);
        GameLevelGradeEntity gameLevelGradeDBModel = GameLevelGradeDBModel.Instance.GetEntityByGameLevelIdAndGrade(gameLevelId, grade);
        if (gameLevelEntity == null || gameLevelGradeDBModel == null) return;
        TransferData data = new TransferData();
        data.SetValue<int>(ConstDefine.GameLevelId, gameLevelEntity.Id);
        data.SetValue<string>(ConstDefine.GameLevelDlgPic, gameLevelEntity.DlgPic);
        data.SetValue<string>(ConstDefine.GameLevelName, gameLevelEntity.Name);

        data.SetValue<int>(ConstDefine.GameLevelExp, gameLevelGradeDBModel.Exp);
        data.SetValue<int>(ConstDefine.GameLevelGold, gameLevelGradeDBModel.Gold);
        data.SetValue<string>(ConstDefine.GameLevelDesc, gameLevelGradeDBModel.Desc);
        data.SetValue<string>(ConstDefine.GameLevelConditionDesc, gameLevelGradeDBModel.ConditionDesc);
        data.SetValue<int>(ConstDefine.GameLevelCommendFighting, gameLevelGradeDBModel.CommendFighting);


        List<TransferData> lstReward = new List<TransferData>();
        if (gameLevelGradeDBModel.EquipList.Count > 0)
        {
            gameLevelGradeDBModel.EquipList.Sort((e1, e2) =>
            {
                if (e1.Probability < e2.Probability) { return -1; }
                else if (e1.Probability == e2.Probability) return 0;
                else { return 1; }
            });

            GoodsEntity entity = gameLevelGradeDBModel.EquipList[0];
            TransferData equipReward = new TransferData();
            equipReward.SetValue<int>(ConstDefine.GoodsId, entity.Id);
            equipReward.SetValue<string>(ConstDefine.GoodsName, entity.Name);
            equipReward.SetValue<GoodsType>(ConstDefine.GoodsType, GoodsType.Equip);

            lstReward.Add(equipReward);
        }

        if (gameLevelGradeDBModel.ItemList.Count > 0)
        {
            gameLevelGradeDBModel.ItemList.Sort((e1, e2) =>
            {
                if (e1.Probability < e2.Probability) { return -1; }
                else if (e1.Probability == e2.Probability) return 0;
                else { return 1; }
            });

            GoodsEntity entity = gameLevelGradeDBModel.ItemList[0];
            TransferData ItemListReward = new TransferData();
            ItemListReward.SetValue<int>(ConstDefine.GoodsId, entity.Id);
            ItemListReward.SetValue<string>(ConstDefine.GoodsName, entity.Name);
            ItemListReward.SetValue<GoodsType>(ConstDefine.GoodsType, GoodsType.Item);

            lstReward.Add(ItemListReward);
        }


        if (gameLevelGradeDBModel.MaterialList.Count > 0)
        {
            gameLevelGradeDBModel.MaterialList.Sort((e1, e2) =>
            {
                if (e1.Probability < e2.Probability) { return -1; }
                else if (e1.Probability == e2.Probability) return 0;
                else { return 1; }
            });

            GoodsEntity entity = gameLevelGradeDBModel.MaterialList[0];
            TransferData MateriaReward = new TransferData();
            MateriaReward.SetValue<int>(ConstDefine.GoodsId, entity.Id);
            MateriaReward.SetValue<string>(ConstDefine.GoodsName, entity.Name);
            MateriaReward.SetValue<GoodsType>(ConstDefine.GoodsType, GoodsType.Material);

            lstReward.Add(MateriaReward);
        }



        data.SetValue<List<TransferData>>(ConstDefine.GameLevelReward, lstReward);
        m_UIRoleInfoView.SetUI(data);
    }

}
