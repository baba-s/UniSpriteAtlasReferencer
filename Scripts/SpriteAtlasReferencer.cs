using UnityEngine;
using UnityEngine.U2D;

namespace Kogane
{
	/// <summary>
	/// SpriteAtlas の参照を保持するためのクラス
	/// シーンをアセットバンドル化してシーンを加算読み込みして、読み込み先のシーンから戻ってきた時に
	/// SpriteAtlas の参照を保持しておかないと、 意図しないタイミングで
	/// SpriteAtlas が含まれているアセットバンドルが解放されてしまい
	/// 戻り先のシーンでスプライトが正常に表示されなくなる不具合の対策として使用するクラスです
	///
	/// SpriteAtlas の参照はシーンを保存した時にエディタ拡張から自動で設定されます
	/// </summary>
	[DisallowMultipleComponent]
	public sealed class SpriteAtlasReferencer : MonoBehaviour
	{
		//================================================================================
		// 変数(SerializeField)
		//================================================================================
#pragma warning disable 0414
		[SerializeField] private SpriteAtlas[] m_list = default;
#pragma warning restore 0414
		
		//================================================================================
		// 関数
		//================================================================================
		/// <summary>
		/// 参照を保持する SpriteAtlas を設定します
		/// </summary>
		public void Set( params SpriteAtlas[] list )
		{
			m_list = list;
		}
	}
}