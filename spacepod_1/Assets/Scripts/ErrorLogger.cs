using UnityEngine;
using System;
using System.IO;

public class ErrorLogger : MonoBehaviour
{
    private string logFilePath;

    void Start()
    {
        // �α� ���� ��� ����
        logFilePath = Application.persistentDataPath + "/error.log";

        // ���� �߻� �� �̺�Ʈ�� ���� �ڵ鷯 ���
        Application.logMessageReceived += HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // ���� Ÿ���� ������ ��츸 ó��
        if (type == LogType.Error || type == LogType.Exception)
        {
            // ���� �޽����� ���� Ʈ���̽��� �α� ���Ͽ� ���
            WriteLogToFile(logString + "\n" + stackTrace);
        }
    }

    void WriteLogToFile(string log)
    {
        // �α� ���Ͽ� ���� ���
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine("[" + DateTime.Now.ToString() + "] " + log);
        }
    }

    void OnDestroy()
    {
        // ���� �ڵ鷯 ����
        Application.logMessageReceived -= HandleLog;
    }
}
