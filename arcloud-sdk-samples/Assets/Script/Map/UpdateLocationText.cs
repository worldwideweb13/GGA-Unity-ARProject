using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLocationText : MonoBehaviour
{
    public Text location;

    // Update is called once per frame
    void Update()
    {
        location.text = $"緯度: {MapLocationController.Instance.latitude}\n経度: {MapLocationController.Instance.longitude}\n高度: {MapLocationController.Instance.altitude}\n\nCount: {MapLocationController.Instance.gps_count}\nMessage:\n{MapLocationController.Instance.message}";        
    }
}
