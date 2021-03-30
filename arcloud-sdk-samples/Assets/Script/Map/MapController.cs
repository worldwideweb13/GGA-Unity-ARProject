using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MapLocationController : MonoBehaviour
{
    // Start is called before the first frame update
    public static MapLocationController Instance { set; get; }
    public float latitude;
    public float longitude;
    public float altitude;
    public int gps_count = 0;
    public string message;

    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("GPS not enabled");
            message = "GPS not enabled";
            yield break;
        }

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait >0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if(maxWait <= 0)
        {
            Debug.Log("Timed out");
            message = "Timed out";
            yield break;
        }

        // Connection has failed
        if(Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("nable to determine device location");
            message = "Unable to determine device location";
            yield break;
        }

        // Set locational infomations
        while(true) {
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            altitude = Input.location.lastData.altitude;
            gps_count++;
            yield return new WaitForSeconds(10);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
