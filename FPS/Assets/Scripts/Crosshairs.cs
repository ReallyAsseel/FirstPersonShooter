using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Crosshairs : MonoBehaviour {

    public Image crosshairV, crosshairV2, crosshairH, crosshairH2, circle;
    public float Radius;

	// Use this for initialization
	void Start () {
        crosshairV = GameObject.Find("CrosshairV").GetComponent<Image>();
        crosshairV2 = GameObject.Find("CrosshairV2").GetComponent<Image>();
        crosshairH = GameObject.Find("CrosshairH").GetComponent<Image>();
        crosshairH2 = GameObject.Find("CrosshairH2").GetComponent<Image>();
        circle = GameObject.Find("Radius").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update () {
        circle.rectTransform.sizeDelta = new Vector2(Radius, Radius);
        crosshairV.rectTransform.localPosition = new Vector3(0f, circle.rectTransform.sizeDelta.y - 2f, 0f);
        crosshairV2.rectTransform.localPosition = new Vector3(0f, -circle.rectTransform.sizeDelta.y + 2f, 0f);
        crosshairH.rectTransform.localPosition = new Vector3(circle.rectTransform.sizeDelta.x - 2f, 0f, 0f);
        crosshairH2.rectTransform.localPosition = new Vector3(-circle.rectTransform.sizeDelta.x + 2f, 0f, 0f);
    }

    public void ToggleCrossHairs(bool on)
    {
        for(int i=0; i < this.gameObject.GetComponentsInChildren<Image>().Length; i++)
        {
            this.gameObject.GetComponentsInChildren<Image>()[i].enabled = on;
        }
    }
}
