using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

// PackageManager와 nuget에서 다운 받음
using Newtonsoft.Json;
using ExcelDataReader;
using UnityEngine.UI;

namespace Portfolio.Editor
{
    /// <summary>
    /// 엑셀 파일을 Json 파일로 변환하는 클래스.
    /// </summary>
    public class ExcelToJsonConvert
    {
        // 읽어올 소스 파일을 담는 리스트
        private readonly List<FileInfo> srcFiles;
        // 사용 가능한 파일을 담는 리스트
        private readonly List<bool> isUseFiles;
        // 저장 경로
        private readonly string savePath;
        // 엑셀 문서의 시트 수
        private readonly int sheetCount;
        // 엑셀 문서의 열 시작지점.(헤더)
        private readonly int headerRows;

        /// <summary>
        /// 생성자를 만들어서 해당 클래스가 선언되자마자 전역 변수를 초기화 하는 작업을 실행.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="savePath"></param>
        public ExcelToJsonConvert(string filePath, string savePath)
        {
            // 소스 파일 초기화.
            srcFiles = new List<FileInfo>();
            srcFiles.Add(new FileInfo(filePath));

            // 사용 가능한 파일 초기화.
            isUseFiles = new List<bool>();
            isUseFiles.Add(true);

            // 데이터를 저장할 파일의 경로 지정.
            this.savePath = savePath;

            // 엑셀 데이터를 참조하기 위해 시트 수와 헤더값을 초기화.
            sheetCount = 1;
            headerRows = 2;
        }

        /// <summary>
        /// 가공이 끝난 Json파일을 저장하는 함수.
        /// </summary>
        /// <returns></returns>
        public int SaveJsonFiles()
        {
            return ReadAllTables(SaveSheetJson);
        }

        #region Read Table

        private int ReadAllTables(Func<DataTable, string, int> exportFunc)
        {
            // 소스 파일이 null 이거나, 리스트의 값이 0보다 작을 경우.
            if(srcFiles == null || srcFiles.Count <= 0)
            {
                // 오류 처리.
                Debug.LogError("에러 발생! 읽어올 엑셀 파일이 없습니다.");
                // 반환 값 -1
                return -1;
            }

            // 여기까지 왔으면 정상작동 중. 작업 ㄱㄱ

            // 결과값을 0으로 초기화.
            int result = 0;

            for (int i = 0; i < srcFiles.Count; i++) 
            {
                if (isUseFiles[i])
                {
                    var file = srcFiles[i];
                    // ReadTable함수를 사용해서 모든 엑셀 파일의 모든 문서를 읽어옴.
                    result += ReadTable(file.FullName, FileNameNoExit(file.Name), exportFunc);
                }
            }

            // 읽어온 모든 문서를 반환.
            return result;
        }

        /// <summary>
        /// 엑셀 파일 읽어오기.
        /// </summary>
        /// <param name="path">파일 경로</param>
        /// <param name="fileName">파일 이름</param>
        /// <param name="exportFunc">문서의 열을 읽어오기 위한 대리자</param>
        /// <returns></returns>
        private int ReadTable(string path, string fileName, Func<DataTable, string, int> exportFunc)
        {
            // 결과값을 0으로 초기화
            int result = 0;

            // using(리소스 타입 ㅣ 리소스를 담을 변수 = 사용할 리소스) : 사용한 리소스를 자동으로 정리 및 해제하는 문.

            // path로 들어온 파일을 열기.(기존 파일 열기, 읽기 전용)
            using(var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                //ExcelReaderFactory.CreateReader(stream) : xlsx 형태의 파일만 읽어오는 함수.
                using(var reader = ExcelReaderFactory.CreateReader(stream)) 
                {
                    // 읽어온 엑셀 파일의 시트 수를 가져오기.
                    int tableSheetNum = reader.ResultsCount;
                    
                    // 시트 수가 1보다 작으면??
                    if(tableSheetNum < 1)
                    {
                        // 읽어온 엑셀 파일이 비어있다는 뜻이기 때문에 오류 처리.
                        Debug.LogError("해당 경로의 엑셀 파일이 비어있습니다! : " + path);
                        // 반환값을 -1로 주고 함수 종료.
                        return -1;
                    }

                    // 여기로 넘어왔다면 데이터가 들어있는 엑셀 파일을 읽었다는 뜻임. 작업 ㄱㄱㄱㄱㄱㄱ

                    // 읽어온 엑셀 파일의 데이터를 불러오기. (확장 메서드)
                    var dataSet = reader.AsDataSet();

                    // sheetCount의 값이 0보다 작거나 같으면 tableSheetBum(읽어온 엑셀 파일의 시트 수)로, 크다면 sheetCount의 값을 대입함. ( 오류 방지 )
                    int checkCount = sheetCount <= 0 ? tableSheetNum : sheetCount;

                    for(int i = 0; i < checkCount; i++) 
                    {
                        if(i < tableSheetNum)
                        {
                            // DataSet.Tables[] : 데이터 베이스에 존재하는 행의 배열.
                            // DataSet.Tables[i].TableName : 해당 행의 이름.
                            string name = checkCount == 1 ? fileName : fileName + "_" + dataSet.Tables[i].TableName;

                            // 매개변수의 대리자를 이용해서 result에 행의 내용을 추가.
                            result += exportFunc(dataSet.Tables[i], name);
                        }
                    }

                }
            }

            // 결과값을 반환.
            return result;
        }

        #endregion

        #region Save Json Files

        /// <summary>
        /// 읽어온 문서를 Json으로 변환, 저장할 함수.
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private int SaveSheetJson(DataTable sheet, string fileName)
        {
            // 데이터 테이블의 행 수가 0보다 작거나 같을 경우.
            if(sheet.Rows.Count <= 0)
            {
                // 비어있는 엑셀 파일을 읽어왔기 때문에 오류 처리.
                Debug.LogError("문서의 시트가 비어있습니다. 테이블 이름 : " + sheet.TableName);
                //변환값을 -1로 설정하고 함수 종료.
                return -1;
            }

            // 여기로 넘어왔다면 정상적인 문서를 읽어왔다는 뜻. 작업 ㄱㄱㄱㄱㄱ

            // 기존의 행과 열의 수를 변수로 저장.
            int columns = sheet.Columns.Count;
            int rows = sheet.Rows.Count;

            // Dictionary 객체는 액셀에 하나의 row(행)을 나타내고 있다.
            // 하나의 행이란 결국 하나의 데이터셋을 의미함.
            // 그래서 tData리스트는 복수의 데이터셋(Dictionary)를 들고 있다는 뜻이 됨.
            List<Dictionary<string, object>> tData = new List<Dictionary<string, object>>();

            for(int i = headerRows; i < rows; i++)
            {
                // 열의 데이터를 담아둘 딕셔너리를 생성.
                Dictionary<string, object> rowData = new Dictionary<string, object>();

                for(int j = 0; j < columns; j++) 
                {
                    // 필드 이름을 읽어옴 (엑셀 문서의 첫번째 행)
                    string key = sheet.Rows[0][j].ToString();
                    // 데이터 타입을 읽어옴 ( 엑셀 문서의 두번째 행)
                    string type = sheet.Rows[1][j].ToString();
                    // 읽어온 데이터 타입에 맞게 파싱작업을 한 후, 딕셔너리에 담아두기.
                    rowData[key] = SetObjectFiled(type, sheet.Rows[i][j].ToString());
                }
                // 읽기가 끝났으니, tData에 추가.
                tData.Add(rowData);
            }

            // JsonConvert.SerializeObject(object, Formatting) : object를 Json으로 직렬화.
            string json = JsonConvert.SerializeObject(tData, Formatting.Indented);
            // 파일 저장
            string destFolder = savePath;
            
            // 지정된 경로에 폴더가 존재하는지 확인
            if(!Directory.Exists(destFolder))
            {
                // destFolder가 없다면 새로 만들기.
                Directory.CreateDirectory(destFolder);
            }

            // json 파일이 저장될 경로와 파일 이름 설정.
            string path = $"{destFolder}/{fileName}.json";
            // 해당 경로에 파일 생성. (새 파일, 수정 가능)
            using(FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                //????
                using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    // 만들어진 파일 안에 아까 Json
                    textWriter.Write(json);
                    // 저장된 경로에 파일 저장 완료!
                    Debug.Log("파일 저장 : " + path);
                    // 반환값을 1로 주고 함수 종료.
                    return 1;
                }
            }    
        }

        /// <summary>
        /// 해당 로컬함수 내에서 case로 명시된 타입만 현재 파싱 가능함.
        /// 나중에 필요한 데이터 타입이 있다면, 해당 타입을 case로 명시한 후에 파싱 작업을 진행하면 된다.
        /// </summary>
        /// <param name="type">데이터 타입의 이름</param>
        /// <param name="param">데이터 내용.</param>
        /// <returns></returns>
        private object SetObjectFiled(string type, string param)
        {
            object pObj = param;
            switch(type.ToLower())
            {
                case "string":
                    break;
                case "string[]":
                    pObj = param.Split(',');
                    break;
                case "byte":
                    pObj = byte.Parse(param);
                    break;
                case "int":
                    pObj = int.Parse(param);
                    break;
                case "int[]":
                    pObj = Array.ConvertAll(param.Split(','), element => int.Parse(element));
                    break;
                case "float":
                    pObj = float.Parse(param);
                    break;
                case "float[]":
                    pObj = Array.ConvertAll(param.Split(','), element => float.Parse(element));
                    break;
                default:
                    Assembly assembly = Assembly.Load("Assembly-CSharp");
                    var t = assembly.GetType(type);
                    if (t != null)
                    {
                        if(t.IsEnum)
                        {
                            pObj = Enum.Parse(t, param);
                        }
                    }
                    break;
            }

            return pObj;
        }

#endregion

        private string FileNameNoExit(string fileName)
        {
            int length;
            if ((length = fileName.LastIndexOf('.')) == -1)
                return fileName;

            return fileName.Substring(0, length);
        }


    }
}