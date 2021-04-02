using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _bones = new List<GameObject>();

    private int _boneIndex = 0;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            YeetFood();
        }
    }

    public void YeetFood()
    {
        Rigidbody2D rb2d = _bones[_boneIndex].GetComponent<Rigidbody2D>();

        _bones[_boneIndex].transform.position = this.transform.position;
        _bones[_boneIndex].transform.rotation = this.transform.rotation;

        if (rb2d != false)
        {
            rb2d.AddForce(new Vector2(-10, 10), ForceMode2D.Impulse);
        }

        _boneIndex++;

        if(_boneIndex == _bones.Count)
        {
            _boneIndex = 0;
        }
    }
}
