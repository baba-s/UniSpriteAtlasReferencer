using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
	/// <summary>
	/// Preferences における設定を管理する ScriptableObject
	/// </summary>
	internal sealed class SpriteAtlasReferencerSettings : ScriptableObjectForPreferences<SpriteAtlasReferencerSettings>
	{
		//================================================================================
		// 定数
		//================================================================================
		private const bool   DEFAULT_ENABLED_LOG = false;
		private const string DEFAULT_LOG_FORMAT  = "[UniSpriteAtlasReferencer] シーンの保存にかかった時間：{0} 秒";

		//================================================================================
		// 変数(static)
		//================================================================================
		[SerializeField] private bool   m_enabledLog = DEFAULT_ENABLED_LOG;
		[SerializeField] private string m_logFormat  = DEFAULT_LOG_FORMAT;

		//================================================================================
		// プロパティ
		//================================================================================
		public bool   EnabledLog => m_enabledLog;
		public string LogFormat  => m_logFormat;

		//================================================================================
		// 関数(static)
		//================================================================================
		[SettingsProvider]
		private static SettingsProvider SettingsProvider()
		{
			return CreateSettingsProvider
			(
				settingsProviderPath: "Kogane/UnitSpriteAtlasReferencer",
				onGUIExtra: so =>
				{
					if ( !GUILayout.Button( "Reset to Default" ) ) return;

					so.FindProperty( nameof( m_enabledLog ) ).boolValue  = DEFAULT_ENABLED_LOG;
					so.FindProperty( nameof( m_logFormat ) ).stringValue = DEFAULT_LOG_FORMAT;
				}
			);
		}
	}
}