using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SaveFile : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] string _saveFileName = "";
    public string SaveFileName
    {
        get { return _saveFileName; }
        set { _saveFileName = value; }
    }

    [SerializeField] string _saveDataName = "";
    public string SaveDataName
    {
        get { return _saveDataName; }
    }

    [Header("Content")]
    #region Content

    [SerializeField] Button _saveFileButton;
    public Button SaveFileButton
    {
        get { return _saveFileButton; }
    }
    [SerializeField] Button _deleteFileButton;
    public Button DeleteFileButton
    {
        get { return _deleteFileButton; }
    }

    [SerializeField] TMP_Text _saveFileNameTxt;
    [SerializeField] TMP_Text _saveFileLastPlayedTxt;
    [SerializeField] Image _saveFileImage;

    #endregion

    public void SetData(GameData data)
    {
        if (data == null)
        {
            _saveFileNameTxt.text = "Save file name A";
            _saveFileLastPlayedTxt.text = "Last played: A";
        }
        else
        {
            _saveFileName = data.saveFileName;
            _saveDataName = data.saveDataName;

            _saveFileNameTxt.text = _saveFileName + "_" + _saveDataName;
            _saveFileLastPlayedTxt.text = DateTime.FromBinary(data.lastUpdated).ToString();
        }
    }
}
