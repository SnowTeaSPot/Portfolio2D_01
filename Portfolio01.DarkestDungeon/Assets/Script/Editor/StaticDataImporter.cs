using System;
using System.Collections.Generic;
using Portfolio.Define;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace Portfolio.Editor
{
    /// <summary>
    /// 액셀 파일이 추가되었을 때 후처리를 진행할 클래스
    ///  -> excel 파일의 추가/변경을 감지하고, json으로 반환하기
    /// </summary>
    public class StaticDataImporter
    {
        /// <summary>
        /// 파일이 액셀 파일이면서 기획데이터인지 체크
        /// </summary>
        /// <param name="path">해당 파일 경로</param>
        /// <param name="isDeleted">삭제 이벤트</param>
        /// <returns></returns>
        private static bool IsStaticData(string path, bool isDeleted)
        {
            // xlsx 확장자가 아니라면 리턴.
            if (path.EndsWith(".xlsx") == false)
                return false;

            // 파일 존재여부 확인을 위해, 파일의 전체 경로를 구함.
            //  -> Application.dataPath : 드라이브로부터 프로젝트 에셋폴더의 경로까지의 주소.
            //                            파라미터로 전달받은 경로는 에셋폴더부터.
            var dstPath = Application.dataPath + path.Remove(0, "Assets".Length);

            // Assets/StaticData/Excel 폴더에 존재하는 액셀파일은 기획데이터 라고 자체적인 규칙을 만듭시다!
            // 따라서, 해당 경로에 존재하지 않는다면 기획데이터가 아님. ※꼭 해당 폴더에 넣읍시다!

            // 삭제이벤트 이거나 존재하는 파일이어야 하고, 경로는 excel 데이터 경로에 있어야 함.
            // return할 때 조건문을 건 것이고 반환 값은 bool임.
            return (isDeleted || File.Exists(dstPath)) && path.StartsWith(StaticData.SDExcelPath);
        }

        /// <summary>
        /// xlsx(엑셀 데이터 문서) -> Json파일로 변환하는 함수.
        /// </summary>
        /// <param name="assets"></param>
        /// <param name="isDeleted"></param>
        private static void ExcelToJson(string[] assets, bool isDeleted)
        {
            // 파라미터로 전달받은 assets 배열의 데이터 중
            // 엑셀 파일인 에셋의 경로만 담을 리스트
            List<string> staticDataAssets = new List<string>();

            // 에셋 경로에서 기획데이터인 엑셀파일만 걸러내기.
            foreach(var asset in assets)
            {
                if (IsStaticData(asset, isDeleted))
                    staticDataAssets.Add(asset);
            }

            foreach(var staticDataAsset in staticDataAssets)
            {
                try
                {
                    var rootPath = Application.dataPath;
                    // 문자열에서 마지막 / 가 존재하는 부분부터 뒤쪽 문자열을 전부 지움.
                    // 결과적으로 Assets 폴더 경로가 지워짐.
                    rootPath = rootPath.Remove(rootPath.LastIndexOf("/"));

                    // 절대 경로를 구하기.
                    var dstPath = $"{rootPath}/{staticDataAsset}";

                    // 변환 실행 및 결과를 변환받아 성공했는지 확인하기.
                    var converter = new ExcelToJsonConvert(dstPath, $"{rootPath}/{StaticData.SDJsonPath}");

                    if(converter.SaveJsonFiles() > 0)
                    {
                        // 경로에서 파일이름과 확장자만 남긴다.
                        var fileName = staticDataAsset.Substring(staticDataAsset.LastIndexOf("/") + 1);
                        // 확장자를 제거해서 파일이름만 남긴다.
                        fileName = fileName.Remove(fileName.LastIndexOf("."));
                        
                        // json을 파일을 생성하여 프로젝트 폴더 내에 위치시켰을 뿐임.
                        // 에디터 상에서 로드하여 인식시키는 작업은 하지 않았으므로
                        // 에디터에서 인식할 수 있도록 임포트하는 함수임.
                        AssetDatabase.ImportAsset($"{StaticData.SDJsonPath}/{fileName}.json");
                        Debug.Log($"##### Static Data {fileName} reimported");
                    }
                }
                catch(Exception e)  
                {
                    Debug.LogError(e);
                    Debug.LogErrorFormat("Coudln't convert assets = {0}", staticDataAsset);
                    // 창 띄우기
                    EditorUtility.DisplayDialog("Error Convert", string.Format("Coudln't convert assets = {0}", staticDataAsset), "OK");
                }
            }
        }

        /// <summary>
        /// 파일을 새로 불러오거나 수정했을 때.
        /// </summary>
        private static void ImportNewOrModified(string[] importedAssets)
        {
            ExcelToJson(importedAssets, false);
        }

        /// <summary>
        /// 파일을 삭제한 경우 실행할 기능.
        /// </summary>
        /// <param name="deletedAssets"></param>
        private static void Delete(string[] deletedAssets)
        {
            ExcelToJson(deletedAssets, true);
        }

        /// <summary>
        /// 파일이 이동 됐을 때.
        /// </summary>
        /// <param name="movedAssets"> 새로운 경로(이동 후)의 에셋 정보</param>
        /// <param name="movedFromAssetsPath">이전 경로(이동하기 전)의 에셋 정보</param>
        private static void Move(string[] movedAssets, string[] movedFromAssetsPath)
        {
            // 이전 경로 에셋 삭제
            Delete(movedFromAssetsPath);

            // 새로운 경로 에셋 수정.
            ImportNewOrModified(movedAssets);
        }

        /// <summary>
        /// 에셋 후처리기에서 파일 변경 감지 콜백이 실행될 시, 호출할 함수.
        /// </summary>
        public static void Import(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetsPath)
        {
            ImportNewOrModified(importedAssets);
            Delete(deletedAssets);
            Move(movedAssets, movedFromAssetsPath);
        }
    }
}
