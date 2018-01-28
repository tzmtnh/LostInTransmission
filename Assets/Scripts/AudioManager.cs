using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AudioManager : MonoBehaviour {

	[System.Serializable]
	public class AudioElement {
		public string name;
		public float defaultVolume = 1;
		public float defaultPitch = 1;
		public AudioClip clip;
	}

	public static AudioManager inst;

	public AudioElement[] audioElements;

	Stack<AudioSource> _freeSources = new Stack<AudioSource>(8);
	List<AudioSource> _usedSources = new List<AudioSource>(8);
	Dictionary<string, AudioElement> _elementByName = new Dictionary<string, AudioElement>();

	AudioSource getSource() {
		AudioSource source;
		if (_freeSources.Count > 0) {
			source = _freeSources.Pop();
		} else {
			GameObject obj = new GameObject();
			obj.transform.SetParent(transform);
			source = obj.AddComponent<AudioSource>();
		}

		_usedSources.Add(source);
		return source;
	}

	public AudioSource playSound(string name, float volume = -1, float pitch = -1, bool loop = false) {
		if (_elementByName.ContainsKey(name) == false) {
			Debug.LogError("Unable to locate sound " + name);
			return null;
		}

		AudioElement element = _elementByName[name];
		AudioSource source = getSource();

		if (volume < 0)
			volume = element.defaultVolume;
		if (pitch < 0)
			pitch = element.defaultPitch;

		source.clip = element.clip;
		source.volume = volume;
		source.pitch = pitch;
		source.loop = loop;
		source.Play();
		return source;
	}

	public void stopSound(AudioSource source) {
		if (source.isPlaying == false)
			return;

		Assert.IsTrue(_usedSources.Contains(source));
		source.Stop();
		source.clip = null;
		_usedSources.Remove(source);
		_freeSources.Push(source);
	}

	void Awake() {
		Assert.IsNull(inst, "There can be only one instance!");
		inst = this;

		foreach (AudioElement element in audioElements) {
			_elementByName.Add(element.name, element);
		}

		playSound("Theme");
	}

	void Update() {
		for (int i = _usedSources.Count - 1; i >= 0; i--) {
			AudioSource source = _usedSources[i];
			if (source.isPlaying == false) {
				source.clip = null;
				_freeSources.Push(source);
				_usedSources.RemoveAt(i);
			}
		}
	}
}
