using UnityEngine;
/// <summary>
/// 
/// </summary>
public class UnitHealth : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public GameObject healthContainer;
    /// <summary>
    /// 
    /// </summary>
    public RectTransform healthFill;
    /// <summary>
    /// 
    /// </summary>
    private float maxSize;
    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        maxSize = healthFill.sizeDelta.x;
        healthContainer.SetActive(false);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="curHp"></param>
    /// <param name="maxHp"></param>
    public void UpdateHealthBar(int curHp, int maxHp)
    {
        healthContainer.SetActive(true);
        float healthPercentage = (float)curHp / (float)maxHp;
        healthFill.sizeDelta = new Vector2(maxSize * healthPercentage, healthFill.sizeDelta.y);
    }

}
