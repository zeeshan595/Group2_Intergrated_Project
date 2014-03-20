using UnityEngine;
using System.Collections;

public class MySQL
{
    private WWW w;
    private WWWForm form;

    public MySQL(string query, string url, string[] variableValues, string[] varibleNames)
    {
        if (varibleNames.Length != variableValues.Length)
            throw new System.Exception("Variable Names and values length do not match.");

        form = new WWWForm();

        for (int x = 0; x < variableValues.Length; x++)
        {
            form.AddField(varibleNames[x], variableValues[x]);
        }

        w = new WWW(url, form);
    }

    public string Responce()
    {
        if (w.isDone)
        {
            if (w.error == null)
            {
                return w.text;
            }
            else
            {
                return w.error;
            }
        }
        else
        {
            return null;
        }
    }
}