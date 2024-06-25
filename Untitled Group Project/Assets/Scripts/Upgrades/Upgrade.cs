using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    public UpgradeType upgradeType;
    public float upgradeMultiplier;
}

public enum UpgradeType
{
    WEAPON, ARMOR
}
