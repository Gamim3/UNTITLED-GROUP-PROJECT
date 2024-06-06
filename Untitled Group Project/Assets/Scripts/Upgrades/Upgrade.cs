using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    public UpgradeType upgradeType;
    public float upgradeAmount;
}

public enum UpgradeType
{
    WEAPON, ARMOR
}
