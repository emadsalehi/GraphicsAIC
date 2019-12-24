using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class Map
{
    public Map()
	{

	}

    public int Row { get; set; }
	public int Col { get; set; }


	public List<King> kings { get; set; }

    public List<MapPath> paths { get; set; }
}

public class King
{
    public King()
	{

	}

    public int row { get; set; }
    public int col { get; set; }
    public int id { get; set; }
    public string name { get; set; }
    
}

public class MapPath
{
    public MapPath()
	{

	}

    public int[,] cells { get; set; }
    public int id { get; set; }
}

public class createMap : MonoBehaviour
{
	// Start is called before the first frame update

	Map map;
    void Start()
    {
		using (StreamReader r = new StreamReader("Assets/Scripts/Log/map.json"))
		{
			string json = r.ReadToEnd();
			//JObject jObject = JObject.Parse(json);
			//var JSONObj = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(example1);
			map = JsonConvert.DeserializeObject<Map>(json);
            Debug.Log(map.Col);

		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
