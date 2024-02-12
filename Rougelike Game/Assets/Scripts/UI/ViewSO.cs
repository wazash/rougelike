using UnityEngine;

namespace UI
{
	[CreateAssetMenu(menuName = "CustomUI/ViewSO", fileName = "ViewSO")]
	public class ViewSO : ScriptableObject
	{
		public ThemeSO Theme;
		public RectOffset Padding;
		public float Spacing;
	} 
}
