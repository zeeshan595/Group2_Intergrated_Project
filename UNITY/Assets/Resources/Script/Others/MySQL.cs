using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class MySQL
{
    public static string regularExp = @"(\[([0-9]+)\] => (([\w\s\(\)\{\=\.\,\;\-\}]+)?[a-zA-Z0-9\}])[\s]+\[([a-zA-Z0-9]+)\] => (([\w\s\(\)\{\=\.\,\;\-\}]+)?[a-zA-Z0-9\}]))+";
    public List<QueryData> data = new List<QueryData>();

    public MySQL(string data)
    {
        if (!Regex.IsMatch(data, regularExp))
            throw new System.ArgumentException("Does not match expected data.");

        MatchCollection matches = Regex.Matches(data, regularExp);
        foreach (Match m in matches)
        {
            this.data.Add(new QueryData(m.Groups[5].ToString(), m.Groups[3].ToString()));
            Debug.Log(m.Groups[5].ToString() + "=" + m.Groups[3].ToString());
        }
    }

    public int Find(string name)
    {
        for (int x = 0; x < data.Count; x++)
        {
            if (data[x].name == name)
                return x;
        }
        return -1;
    }
}

public class QueryData
{
    public string name = "";
    public string data = "";

    public QueryData(string name, string data)
    {
        this.name = name;
        this.data = data;
    }
}