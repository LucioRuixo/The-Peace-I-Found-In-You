using System;
#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEngine.UIElements;
#endif

namespace nullbloq.Noodles
{
	public class CustomMusicChangeNode : NoodlesNode
	{
		public MusicController.SongTitle songTitle;

		protected override void PreInit()
		{
			base.PreInit();
			title = "Music Change";
#if UNITY_EDITOR
			classNameString = typeof(CustomMusicChangeNodeVisual).AssemblyQualifiedName;
#endif
			width = 600;
			height = 500;
		}

		protected override void PostInit()
		{
			base.PostInit();
			NoodlesPort inputPort = new NoodlesPort(Guid.NewGuid().ToString(), GUID, "Input");
			inputPorts.Add(inputPort);
			NoodlesPort outputPort = new NoodlesPort(Guid.NewGuid().ToString(), GUID, "Output");
			outputPorts.Add(outputPort);
		}
	}

#if UNITY_EDITOR
	public class CustomMusicChangeNodeVisual : NoodlesNodeVisual
	{
		protected override void CreateVisualsBody()
		{
			base.CreateVisualsBody();
			CustomMusicChangeNode musicChangeNode = nodeData as CustomMusicChangeNode;

			title = musicChangeNode.title;

			var combo = new EnumField("Song Title", musicChangeNode.songTitle);
			combo.RegisterValueChangedCallback(evt => { musicChangeNode.songTitle = (MusicController.SongTitle)evt.newValue; });
			mainContainer.Add(combo);
		}
	}
#endif
}