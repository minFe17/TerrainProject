using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] Image _hpBar;
    [SerializeField] TMP_Text _hpText;

    [SerializeField] Image _mpBar;
    [SerializeField] TMP_Text _mpText;

    [SerializeField] Image _expBar;
    [SerializeField] TMP_Text _expText;

    public void ShowHp(int curHp, int maxHp)
    {
        _hpBar.fillAmount = (float)curHp / maxHp;
        _hpText.text = $"{curHp} / {maxHp}";
    }

    public void ShowMp(int curMp, int maxMp)
    {
        _mpBar.fillAmount = (float)curMp / maxMp;
        _mpText.text = $"{curMp} / {maxMp}";
    }

    public void ShowExp()
    {

    }
}
