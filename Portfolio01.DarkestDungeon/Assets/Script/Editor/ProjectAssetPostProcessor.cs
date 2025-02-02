using UnityEditor;

namespace Portfolio.Editor
{
    /// <summary>
    /// 프로젝트 내 에셋 폴더에서 파일의 변경사항을 감지하여
    /// 콜백 함수를 실행시키기 위해 만들어진 클래스.
    /// </summary>
    public class ProjectAssetPostProcessor : AssetPostprocessor
    {
        /// <summary>
        /// 클래스 설명에 있는 콜백 함수.
        /// </summary>
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetpaths)
        {
            StaticDataImporter.Import(importedAssets, deletedAssets, movedAssets, movedFromAssetpaths);
        }
    }
}
