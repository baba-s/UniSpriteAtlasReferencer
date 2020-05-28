using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Kogane.Internal
{
	/// <summary>
	/// シーンを保存した時に、そのシーンが参照している SpriteAtlas を
	/// SpriteAtlasReference コンポーネントに登録するエディタ拡張
	/// </summary>
	[InitializeOnLoad]
	internal static class SpriteAtlasReferencerInjector
	{
		//================================================================================
		// 関数(static)
		//================================================================================
		/// <summary>
		/// コンストラクタ
		/// </summary>
		static SpriteAtlasReferencerInjector()
		{
			EditorSceneManager.sceneSaving += OnSceneSaving;
		}

		/// <summary>
		/// シーンが保存される時に呼び出されます
		/// </summary>
		private static void OnSceneSaving( Scene scene, string path )
		{
			Inject( scene );
		}

		/// <summary>
		/// 指定されたシーンが参照している SpriteAtlas を
		/// SpriteAtlasReference コンポーネントに登録します
		/// </summary>
		private static void Inject( Scene scene )
		{
			var spriteAtlasReference = scene
					.GetRootGameObjects()
					.Select( x => x.GetComponentInChildren<SpriteAtlasReferencer>( true ) )
					.FirstOrDefault( x => x != null )
				;

			if ( spriteAtlasReference == null )
			{
				return;
			}

			var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>( scene.path );

			if ( sceneAsset == null )
			{
				return;
			}

			var spriteAtlasesInProject = AssetDatabase
					.FindAssets( $"t:{nameof( SpriteAtlas )}" )
					.Select( x => AssetDatabase.GUIDToAssetPath( x ) )
					.Where( x => x.EndsWith( ".spriteatlas" ) )
					.Select( x => AssetDatabase.LoadAssetAtPath<SpriteAtlas>( x ) )
					.Where( x => x != null )
					.ToArray()
				;

			/*
			 * シーンが参照している SpriteAtlas を確実に抽出したい場合は
			 * EditorUtility.CollectDependencies を使用するべきですが、
			 * シーンによっては抽出処理に 0.1 秒以上かかってしまうため、
			 *
			 * シーンに存在する Image と SpriteRenderer が参照している
			 * SpriteAtlas のみを抽出するようにしています
			 * こちらの方法だと抽出処理にかかる時間が 1/10 に抑えられます
			 */
			//var spritesInScene = EditorUtility
			//		.CollectDependencies( new Object[] { sceneAsset } )
			//		.OfType<Sprite>()
			//		.ToArray()
			//	;

			var spritesInSceneImage = scene
					.GetRootGameObjects()
					.SelectMany( x => x.GetComponentsInChildren<Image>( true ) )
					.Select( x => x.sprite )
					.ToArray()
				;

			var spritesInSceneSpriteRenderer = scene
					.GetRootGameObjects()
					.SelectMany( x => x.GetComponentsInChildren<SpriteRenderer>( true ) )
					.Select( x => x.sprite )
					.ToArray()
				;

			var spritesInScene = spritesInSceneImage
					.Concat( spritesInSceneSpriteRenderer )
					.Distinct()
					.ToArray()
				;

			var result = new List<SpriteAtlas>();

			for ( var i = 0; i < spriteAtlasesInProject.Length; i++ )
			{
				var spriteAtlas = spriteAtlasesInProject[ i ];
				var spritesInSpriteAtlas = new HashSet<Object>
				(
					EditorUtility.CollectDependencies( new Object[] { spriteAtlas } )
				);

				if ( spritesInScene.Any( x => spritesInSpriteAtlas.Contains( x ) ) )
				{
					result.Add( spriteAtlas );
				}
			}

			spriteAtlasReference.Set( result.OrderBy( x => x.name ).ToArray() );
		}
	}
}