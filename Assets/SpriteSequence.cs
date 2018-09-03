using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SpriteSequence : MonoBehaviour
{
    public string FolderName;
    public int FramesPerSecond = 15;
    
    private List<Sprite> _sprites;
    private int _index;
    private float _timePerFrame;
    private float _timeSinceLastFrame;
    private SpriteRenderer _renderer;

    public void Start ()
	{
        _renderer = GetComponent<SpriteRenderer>();
        _sprites = Resources.LoadAll<Sprite>(FolderName).ToList();
	    _index = 0;
	}

	public void Update ()
	{
	    _timePerFrame = 1f / FramesPerSecond;

	    if (_timeSinceLastFrame > _timePerFrame)
	    {
	        _index = (_index + 1)%_sprites.Count;
	        _renderer.sprite = _sprites[_index];
	        _timeSinceLastFrame = 0;
	    }

	    _timeSinceLastFrame += Time.deltaTime;
	}
}
