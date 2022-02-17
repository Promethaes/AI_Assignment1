//Anthony Smiderle
//100695532
//2022/02/07
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class FileIO : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TMPro.TMP_InputField field = null;
    string path = "";
    public string ReadInProbabilities()
    {
        path = field.text;
        string output = "";
        StreamReader sr = File.OpenText(path);
        string temp = "";
        while ((temp = sr.ReadLine()) != null)
        {
            if (temp.Contains("Hot"))
                continue;
            var o = temp.Split('\t');
            temp = "";
            foreach (var s in o)
            {
                temp += s;
            }
            temp = temp.Insert(3, " ");
            output += temp;
            output += "\n";
        }
        return output;
    }
}
