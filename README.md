# UniSpriteAtlasReferencer

シーンを保存した時にそのシーンが使用しているすべての SpriteAtlas の参照を自動で保持するコンポーネント

## 使い方

![2020-06-13_205439](https://user-images.githubusercontent.com/6134875/84568134-52070180-adb8-11ea-8873-ddabce98a4b8.png)

* シーンに存在するゲームオブジェクトに「SpriteAtlasReferencer」します  
* これで、シーンを保存した時にそのシーンが使用しているすべての SpriteAtlas の参照が  
「SpriteAtlasReferencer」に自動で保持されるようになります  
* シーンをアセットバンドル化した時に SpriteAtlas の依存関係が  
正常に解決されない場合の対策として使用することを想定したコンポーネントです  
