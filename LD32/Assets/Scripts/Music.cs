using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {


    private AudioSource _audio;

    IEnumerator fadeIn()
    {
        var t = 0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * 0.05f;
            _audio.volume = t;
            yield return null;
        }
    }

    void Start()
    {
        this._audio = GetComponent<AudioSource>();
        _audio.time = 10f;
        this.StartCoroutine("fadeIn");
    }


	// Update is called once per frame
	void Update () {

        var audio = _audio;
	    if (Input.GetKeyDown(KeyCode.M))
        {
            audio.enabled = !audio.enabled;
        }

        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            audio.volume = Mathf.Clamp(audio.volume - 0.05f, 0f, 1f);
        }
        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            audio.volume = Mathf.Clamp(audio.volume + 0.05f, 0f, 1f);
        }

	}
}
