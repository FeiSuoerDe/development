using Godot;
using System;

public partial class Music : Node
{

	//播放音频
	public void Play(string path)
	{
		AudioStreamPlayer audioStreamPlayer = new AudioStreamPlayer();
		audioStreamPlayer.Stream = GD.Load<AudioStream>(path);
		audioStreamPlayer.Play();
		AddChild(audioStreamPlayer);
	}
}
