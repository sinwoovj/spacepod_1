using UnityEngine;
using System;
using System.IO;

public class ErrorLogger : MonoBehaviour
{
    private string logFilePath;

    void Start()
    {
        // 로그 파일 경로 설정
        logFilePath = Application.persistentDataPath + "/error.log";

        // 에러 발생 시 이벤트에 오류 핸들러 등록
        Application.logMessageReceived += HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // 오류 타입이 에러인 경우만 처리
        if (type == LogType.Error || type == LogType.Exception)
        {
            // 오류 메시지와 스택 트레이스를 로그 파일에 기록
            WriteLogToFile(logString + "\n" + stackTrace);
        }
    }

    void WriteLogToFile(string log)
    {
        // 로그 파일에 오류 기록
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine("[" + DateTime.Now.ToString() + "] " + log);
        }
    }

    void OnDestroy()
    {
        // 오류 핸들러 제거
        Application.logMessageReceived -= HandleLog;
    }
}
