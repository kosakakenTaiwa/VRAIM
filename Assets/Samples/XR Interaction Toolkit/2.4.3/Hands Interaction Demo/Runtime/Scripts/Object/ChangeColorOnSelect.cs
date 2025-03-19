using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChangeColorOnSelect : MonoBehaviour
{
    public Material targetMaterial; // 色を変更するマテリアル
    private Color originalColor; // 元の色を保存

    private void Awake()
    {
        if (targetMaterial != null)
        {
            originalColor = targetMaterial.color; // 元の色を取得
        }
    }

    public void OnSelectEnter(SelectEnterEventArgs args)
    {
        if (targetMaterial != null)
        {
            // オブジェクトの色を変更
            targetMaterial.color = Color.red; 
        }
    }

    public void OnSelectExit(SelectExitEventArgs args)
    {
        if (targetMaterial != null)
        {
            // オブジェクトの色を元に戻す
            targetMaterial.color = originalColor; 
        }
    }
}
